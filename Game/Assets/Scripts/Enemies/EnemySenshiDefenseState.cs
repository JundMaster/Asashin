﻿using UnityEngine;

/// <summary>
/// Scriptable object responsible for controlling enemy movement state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Senshi Defense State")]
public class EnemySenshiDefenseState : EnemyStateWithVision
{
    [Header("Kunai to spawn")]
    [SerializeField] private GameObject kunai;

    [Header("Kunai spawn delay")]
    [Range(1f, 10f)][SerializeField] private float kunaiDelay;
    private float kunaiLastTimeChecked;

    [Header("Rotation smooth time")]
    [Range(0.1f,1)][SerializeField] private float turnSmooth;
    private float smoothTimeRotation;

    [Header("Distance from player. X => min. distance, Y => max. distance")]
    [SerializeField] private Vector2 randomDistanceFromPlayer;

    // Movement variables
    private float randomDistance;

    /// <summary>
    /// Happens once on start. Sets a random distance to mantain while defending.
    /// </summary>
    public override void Start()
    {
        if (randomDistanceFromPlayer.y < randomDistanceFromPlayer.x)
            randomDistanceFromPlayer.y = randomDistanceFromPlayer.x;
        if (randomDistanceFromPlayer.x > randomDistanceFromPlayer.y)
            randomDistanceFromPlayer.x = randomDistanceFromPlayer.y;

        randomDistance = Random.Range(
            randomDistanceFromPlayer.x, randomDistanceFromPlayer.y);
    }

    /// <summary>
    /// Happens once when this state is enabled. Sets a kunai timer.
    /// </summary>
    public override void OnEnter()
    {
        kunaiLastTimeChecked = Time.time;
        agent.isStopped = false;
    }

    /// <summary>
    /// Goes to defense position. If the player is fighting an enemy,
    /// it keeps throwing kunais.
    /// </summary>
    /// <returns>An IState.</returns>
    public override IState FixedUpdate()
    {
        base.FixedUpdate();

        // If the enemy is not moving towards the end position
        if (MoveToDefensiveRange() == false)
        {
            // If the enemy can see and is facing the player
            if (PlayerInRange() && FacingPlayer())
            {
                ThrowKunai();
            }
            // If the enemy can NOT see and is facing the player
            else if (PlayerInRange() == false && FacingPlayer())
            {
                return enemy.LostPlayerState;
            }

            // Keeps rotating the enemy towards the player
            RotateEnemy();
        }
        // Else it moves to the enemy without rotating towards the player

        // Only if the player isn't fighting an enemy yet
        if (enemy.PlayerCurrentlyFighting == false)
        {
            enemy.PlayerCurrentlyFighting = true;
            return enemy.AggressiveState;
        }

        return enemy.DefenseState;
    }

    /// <summary>
    /// Happens once when leaving this state.
    /// Sets player's last known position.
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();
    }

    /// <summary>
    /// Moves the enemy towards the desired defense position.
    /// </summary>
    /// <returns>Returns true if it needs to move. 
    /// Returns false if it's in the desired position.</returns>
    private bool MoveToDefensiveRange()
    {
        bool inFinalDestination;
        
        float distance = 
            Vector3.Distance(myTarget.position, playerTarget.position);

        // If the enemy is NOT in the desired position
        if (distance > randomDistance + 0.5f || 
            distance < randomDistance - 0.5f)
        {
            agent.isStopped = false;

            Vector3 desiredDirection =
            (playerTarget.position - myTarget.position).normalized;

            agent.SetDestination(
                playerTarget.position - desiredDirection * randomDistance);

            inFinalDestination = true;
        }
        else
        {
            agent.isStopped = true;
            inFinalDestination = false;
        }

        return inFinalDestination;
    }

    /// <summary>
    /// Throws a kunai towards the player future position.
    /// </summary>
    private void ThrowKunai()
    {
        if (playerTarget != null)
        {
            if (Time.time - kunaiLastTimeChecked > kunaiDelay)
            {
                // Spawns a kunai
                GameObject thisKunai = Instantiate(
                    kunai,
                    myTarget.position + myTarget.forward,
                    Quaternion.identity);

                // Sets layer and parent enemy of the kunai
                thisKunai.layer = 15;
                thisKunai.GetComponent<Kunai>().Behaviour.ParentEnemy = enemy;

                kunaiLastTimeChecked = Time.time;
            }
        }
    }

    /// <summary>
    /// Rotates enemy towards the player.
    /// </summary>
    private void RotateEnemy()
    {
        Vector3 dir = playerTarget.transform.position - myTarget.position;
        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(
                enemy.transform.eulerAngles.y,
                targetAngle,
                ref smoothTimeRotation,
                turnSmooth);
        enemy.transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    /// <summary>
    /// Checks if the enemy is facing the player.
    /// </summary>
    /// <returns>Returns true if the enemy is facing the player.</returns>
    private bool FacingPlayer()
    {
        Vector3 dir = playerTarget.position - myTarget.position;

        if (Vector3.Angle(dir, myTarget.forward) < 10)
            return true;
        return false;
    }
}