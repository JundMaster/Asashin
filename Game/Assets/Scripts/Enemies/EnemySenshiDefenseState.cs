using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Scriptable object responsible for controlling enemy movement state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Senshi Defense State")]
public class EnemySenshiDefenseState : EnemyStateWithVision
{
    [Header("Kunai to spawn")]
    [SerializeField] private GameObject kunai;

    [Header("Kunai spawn delay")]
    [SerializeField] private float kunaiDelay;
    private float kunaiLastTimeChecked;

    [Header("Search for player delay")]
    [SerializeField] private float checkDelay;

    [Header("Rotation smooth time")]
    [SerializeField] private float turnSmooth;
    private float smoothTimeRotation;

    // Components
    private NavMeshAgent agent;

    // Movement variables
    private float randomDistance;

    /// <summary>
    /// Runs once on start and when the player spawns.
    /// </summary>
    /// <param name="enemy">Enemy to get variables from.</param>
    public override void Initialize(Enemy enemy)
    {
        // Gets enemy target and player target
        base.Initialize(enemy);

        kunaiLastTimeChecked = 0f;
        agent = enemy.Agent;

        randomDistance = Random.Range(6f, 8f);
    }

    public override IEnemyState Execute(Enemy enemy)
    {
        // Only if the player isn't fighting an enemy yet
        // Changes state to aggresive state
        if (enemy.PlayerCurrentlyFighting == false)
        {
            enemy.PlayerCurrentlyFighting = true;
            return enemy.AggressiveState;
        }

        // If the enemy is not moving towards the end position
        if (MoveToDefensiveRange() == false)
        {
            // If the enemy can see and is facing the player
            if (PlayerInRange() && FacingPlayer())
            {
                ThrowKunai(enemy);
            }
            // If the enemy can NOT see and is facing the player
            else if (PlayerInRange() == false && FacingPlayer())
            {
                // If the enemy can't see the player
                // Changes the state to lost player state
                enemy.PlayerLastKnownPosition = playerTarget.position;
                return enemy.LostPlayerState;
            }

            // Keeps rotating the enemy towards the player
            RotateEnemy(enemy.transform);
        }
        // Else it moves to the enemy without rotating towards the player

        return enemy.DefenseState;
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
    /// <param name="enemy">This enemy (kunai's enemy parent).</param>
    private void ThrowKunai(Enemy enemy)
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
    /// Rotates enemy towards the player
    /// </summary>
    /// <param name="enemy"></param>
    private void RotateEnemy(Transform enemy)
    {
        // Rotates the enemy towards the player
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
    /// Checks if the enemy is facing the player
    /// </summary>
    /// <returns>Returns true if the enemy is facing the player,</returns>
    private bool FacingPlayer()
    {
        Vector3 dir = playerTarget.position - myTarget.position;

        if (Vector3.Angle(dir, myTarget.forward) < 10)
            return true;
        return false;

    }
}
