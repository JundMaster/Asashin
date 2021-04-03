using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Scriptable object responsible for controlling enemy movement state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Patrol State")]
public class EnemyPatrolState : ScriptableObject, IEnemyState
{
    // Vision
    [Header("Vision Cone Attributes")]
    [SerializeField] private float coneRange;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask collisionLayers;
    [Header("Checks if player is in cone range every X seconds")]
    [SerializeField] private float searchCheckDelay;
    private float lastTimeSearch;

    // Movement
    private NavMeshAgent agent;
    private Transform[] patrolPoints;
    private byte patrolIndex;

    // Targets
    private Transform playerTarget;
    private Transform myTarget;

    
    
    /// <summary>
    /// Runs once on start.
    /// </summary>
    /// <param name="enemy">Enemy to get variables from.</param>
    public void Initialize(Enemy enemy)
    {
        agent = enemy.Agent;
        myTarget = enemy.MyTarget;
        playerTarget = enemy.PlayerTarget;
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
    public IEnemyState Execute(Enemy enemy)
    {
        if (playerTarget == null) playerTarget = enemy?.PlayerTarget;

        // Moves the agent
        Movement();

        // Search for player every searchCheckDelay seconds inside a vision cone
        if (Time.time - lastTimeSearch > searchCheckDelay)
        {
            // If it found the player, triggers defense state
            if (SearchForPlayer())
            {
                agent.isStopped = true;
                return enemy.DefenseState;
            }
        }
        return enemy.PatrolState;
    }

    private void Movement()
    {
        if (agent.remainingDistance < 0.25f)
        {
            if (patrolIndex + 1 > patrolPoints.Length - 1) patrolIndex = 0;
            else patrolIndex++;
            agent.SetDestination(patrolPoints[patrolIndex].transform.position);
        }
    }

    /// <summary>
    /// Search for player every searchCheckDelay seconds inside a cone vision.
    /// </summary>
    private bool SearchForPlayer()
    {
        bool playerFound = false;

        Collider[] playerCollider = 
            Physics.OverlapSphere(myTarget.position, coneRange, playerLayer);

        // If player is in this collider
        if (playerCollider.Length > 0)
        {
            if (playerTarget != null)
            {
                Vector3 direction = playerTarget.position - myTarget.position;
                Ray rayToPlayer = new Ray(myTarget.position, direction);

                // If player is in the cone range
                if (Vector3.Angle(direction, myTarget.forward) < 45)
                {
                    if (Physics.Raycast(
                        rayToPlayer, 
                        out RaycastHit hit, 
                        coneRange, 
                        collisionLayers))
                    {
                        // If it's player layer
                        if (hit.collider.gameObject.layer == 11)
                        {
                            playerFound = true;
                        }
                        else
                        {
                            playerFound = false;
                        }
                    }
                }
                else
                {
                    playerFound = false;
                }
            }
        }

        lastTimeSearch = Time.time;

        return playerFound;
    }
}
