using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Scriptable object responsible for controlling enemy patrol state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Patrol State")]
public class EnemyPatrolState : EnemyStateWithVision
{
    [Header("Checks if player is in cone range every X seconds")]
    [SerializeField] private float searchCheckDelay;
    [SerializeField] private float waitingDelay;

    // Movement
    private NavMeshAgent agent;
    private Transform[] patrolPoints;
    private byte patrolIndex;

    private float pathTimer;

    /// <summary>
    /// Runs once on start and when the player spawns.
    /// Sets agent's initial destination.
    /// </summary>
    /// <param name="enemy">Enemy to get variables from.</param>
    public override void Initialize(Enemy enemy)
    {
        // Gets enemy target and player target
        base.Initialize(enemy);

        agent = enemy.Agent;
        patrolPoints = enemy.PatrolPoints;

        patrolIndex = 0;
        agent.SetDestination(patrolPoints[0].transform.position);
    }

    /// <summary>
    /// Searches for player in a vision cone.
    /// Executes enemy's movement. Runs on fixed update.
    /// </summary>
    /// <param name="enemy">Enemy to move.</param>
    /// <returns>Returns IEnemy state.</returns>
    public override IEnemyState Execute(Enemy enemy)
    {
        // Moves the agent
        Movement();

        // Search for player every searchCheckDelay seconds inside a vision cone
        if (Time.time - lastTimeChecked > searchCheckDelay)
        {
            // If it found the player, triggers defense state
            if (PlayerInRange())
            {
                agent.isStopped = true;
                return enemy.DefenseState;
            }
        }
        return enemy.PatrolState;
    }

    /// <summary>
    /// Moves the agent through patrol points.
    /// </summary>
    private void Movement()
    {
        if (agent.remainingDistance > 0.1f && agent.remainingDistance < 0.2f)
            pathTimer = Time.time;

        if (agent.remainingDistance <= 0.1f && agent.pathPending == false)
        {
            if (Time.time - pathTimer >= waitingDelay)
            {
                if (patrolIndex + 1 > patrolPoints.Length - 1) patrolIndex = 0;
                else patrolIndex++;
                agent.SetDestination(patrolPoints[patrolIndex].transform.position);
            } 
        }
    }
}
