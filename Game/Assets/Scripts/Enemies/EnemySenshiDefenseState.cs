using System.Collections;
using UnityEngine;

/// <summary>
/// Scriptable object responsible for controlling enemy movement state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Senshi Defense State")]
public class EnemySenshiDefenseState : EnemyStateWithVision
{
    private const byte KUNAILAYER = 15;

    [Header("Kunai to spawn")]
    [SerializeField] private GameObject kunai;

    [Header("Kunai spawn delay")]
    [Range(1f, 10f)][SerializeField] private float kunaiDelay;
    [Range(0f, 5f)] [SerializeField] private float kunaiSpawnAfterAnimation;
    private bool kunaiCoroutine;
    private bool whileThrowingKunai;

    [Header("Rotation smooth time")]
    [Range(0.1f,1.5f)][SerializeField] private float turnSmooth;
    private float smoothTimeRotation;

    [Header("Distance from player. X => min. distance, Y => max. distance")]
    [SerializeField] private Vector2 randomDistanceFromPlayer;

    // Movement variables
    private float randomDistance;

    private Animator anim;

    /// <summary>
    /// Happens once on start. Sets a random distance to mantain while defending.
    /// </summary>
    public override void Start()
    {
        base.Start();
        kunaiCoroutine = false;
        whileThrowingKunai = false;
        anim = enemy.Anim;

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
        base.OnEnter();

        kunaiCoroutine = false;

        whileThrowingKunai = false;

        agent.isStopped = false;

        enemy.VisionCone.SetActive(false);
    }

    /// <summary>
    /// Goes to defense position. If the player is fighting an enemy,
    /// it keeps throwing kunais.
    /// </summary>
    /// <returns>An IState.</returns>
    public override IState FixedUpdate()
    {
        base.FixedUpdate();

        if (instantKill)
            return enemy.DeathState;

        // If the enemy is not moving towards the end position
        if (MoveToDefensiveRange() == false)
        {
            // If the enemy can see and is facing the player
            if (PlayerInRange() && FacingPlayer())
            {
                if (kunaiCoroutine == false)
                {
                    kunaiCoroutine = true;
                    enemy.StartCoroutine(ThrowKunaiCoroutine());
                }
            }
            // If the enemy can NOT see and is facing the player
            else if (PlayerInRange() == false && FacingPlayer())
            {
                if (kunaiCoroutine == false)
                    return enemy.LostPlayerState;
            }

            // Keeps rotating the enemy towards the player
            RotateEnemy(true);
        }
        // Else it moves to the enemy without rotating towards the player

        // Only if the player isn't fighting an enemy yet
        if (enemy.PlayerCurrentlyFighting == false)
        {
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
        kunaiCoroutine = false;
        whileThrowingKunai = false;
    }

    /// <summary>
    /// Moves the enemy towards the desired defense position.
    /// </summary>
    /// <returns>Returns true if it needs to move. 
    /// Returns false if it's in the desired position.</returns>
    private bool MoveToDefensiveRange()
    {
        float distance = 
            Vector3.Distance(myTarget.position, playerTarget.position);

        // If the enemy is NOT in the desired position
        if (distance > randomDistance + 2 || 
            distance < randomDistance - 2)
        {
            agent.isStopped = false;

            // Direction from player to enemy.
            Vector3 desiredDirection =
            (playerTarget.position - myTarget.position).normalized;

            // Ray from player to final destination
            Ray finalPosition = 
                new Ray(
                    playerTarget.position,
                    -desiredDirection * randomDistance);

            // If there isn't any wall in the way
            if (Physics.Raycast(
                finalPosition, randomDistance, collisionLayers) == false)
            {
                // Moves the enemy in order to keep a random distance 
                // from the player
                // Only happens if the enemy is not throwing a kunai
                if (whileThrowingKunai == false)
                {
                    agent.SetDestination(
                        playerTarget.position - desiredDirection *
                        randomDistance);
                    return true;
                }
                // Stops enemy, only happens if the enemy is throwing a kunai
                else
                {
                    agent.SetDestination(myTarget.position);
                }
            }
            // Else if there is a wall
            else
            {
                // Keeps the enemy in the same place and final destination.
                agent.SetDestination(myTarget.position);
                agent.isStopped = true;
                return false;
            }
        }
        // Else if the enemy is in the final destination
        agent.isStopped = true;
        return false;
    }

    /// <summary>
    /// Throws a kunai towards the player future position.
    /// </summary>
    private IEnumerator ThrowKunaiCoroutine()
    {
        YieldInstruction wfd = new WaitForSeconds(kunaiDelay);
        YieldInstruction wfks = new WaitForSeconds(kunaiSpawnAfterAnimation);

        while(kunaiCoroutine)
        {
            yield return wfd;

            whileThrowingKunai = true;

            RotateEnemy(false);
            anim.SetTrigger("ThrowKunai");

            yield return wfks;

            // Spawns a kunai
            GameObject thisKunai = Instantiate(
                kunai,
                myTarget.position + myTarget.forward,
                Quaternion.identity);

            // Sets layer and parent enemy of the kunai
            thisKunai.layer = KUNAILAYER;
            thisKunai.GetComponent<Kunai>().Behaviour.ParentEnemy = enemy;

            // Waits until throw kunai animation ends
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                yield return null;
            }

            whileThrowingKunai = false;
            kunaiCoroutine = false;
        }
    }

    /// <summary>
    /// Rotates the enemy. Can rotate smooth or instantly.
    /// </summary>
    /// <param name="smooth">True if smooth turn.</param>
    private void RotateEnemy(bool smooth)
    {
        Vector3 dir = playerTarget.transform.position - myTarget.position;
        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(
                enemy.transform.eulerAngles.y,
                targetAngle,
                ref smoothTimeRotation,
                turnSmooth);

        if (smooth)
            enemy.transform.rotation = Quaternion.Euler(0f, angle, 0f);
        else
            enemy.transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
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
