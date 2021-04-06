using UnityEngine;

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
    private Transform[] patrolPoints;
    private byte patrolIndex;

    private float pathTimer;

    /// <summary>
    /// Runs once on start.
    /// Sets agent's initial destination.
    /// </summary>
    public override void Start()
    {
        patrolPoints = enemy.PatrolPoints;

        patrolIndex = 0;
        agent.SetDestination(patrolPoints[0].transform.position);
    }

    /// <summary>
    /// Runs when entering this state. Turns back agent's movement.
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();

        agent.isStopped = false;
    }

    /// <summary>
    /// Searches for player in a vision cone.
    /// Executes enemy's movement. Runs on fixed update.
    /// </summary>
    /// <returns>Returns an IState.</returns>
    public override IState FixedUpdate()
    {
        if (playerTarget == null) playerTarget = enemy.PlayerTarget;

        // Moves the agent
        Movement();

        // Search for player every searchCheckDelay seconds inside a vision cone
        if (Time.time - lastTimeChecked > searchCheckDelay)
        {
            // If it found the player, triggers defense state
            if (PlayerInRange())
            {
                return enemy.DefenseState;
            }
        }
        return enemy.PatrolState;
    }

    /// <summary>
    /// Runs once when leaving this state. Stops agent.
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();
        agent.isStopped = true;
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

                agent.SetDestination(
                    patrolPoints[patrolIndex].transform.position);
            } 
        }
    }
}
