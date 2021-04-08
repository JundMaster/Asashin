using UnityEngine;

/// <summary>
/// Scriptable object responsible for controlling enemy patrol state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Patrol State")]
public class EnemyPatrolState : EnemyStateWithVision
{
    [Header("Checks if player is in cone range every X seconds")]
    [Range(0.01f,2f)][SerializeField] private float searchCheckDelay;
    [Tooltip("Time to search for the player after reaching final destination")]
    [Range(0.01f, 10f)] [SerializeField] private float waitingDelay;

    [Header("Enemy cone render variables")]
    [Range(0,255)][SerializeField] private byte amountOfVertices;
    [SerializeField] private Material coneMaterial;
    private VisionCone visionCone;

    [Header("Exclamation mark prefab")]
    [SerializeField] private GameObject exclamationMarkPrefab;
    [SerializeField] private Vector3 offset;

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
        // Vision cone setup
        MeshFilter meshFilter = enemy.VisionCone.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = enemy.VisionCone.GetComponent<MeshRenderer>();
        visionCone = new VisionCone(
            meshFilter, meshRenderer, coneMaterial, amountOfVertices,
            desiredConeAngle, coneRange, collisionLayers, enemy.transform);
        enemy.EnemyVisionCone = visionCone;

        // Agent destination setup
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
        enemy.VisionCone.SetActive(true);
    }

    /// <summary>
    /// Searches for player in a vision cone.
    /// Executes enemy's movement. Runs on fixed update.
    /// </summary>
    /// <returns>Returns an IState.</returns>
    public override IState FixedUpdate()
    {
        base.FixedUpdate();

        visionCone.Calculate();
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
        enemy.VisionCone.SetActive(false);

        // Instantiates an exclamation mark
        GameObject exclMark = Instantiate(
            exclamationMarkPrefab, 
            enemy.transform.position + offset, 
            Quaternion.identity);
        exclMark.transform.parent = enemy.transform;
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
