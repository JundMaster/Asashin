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

    [Header("Enemy cone render variables")]
    [SerializeField] private byte amountOfVertices;
    private Quaternion startingAngle;
    private Quaternion stepAngle;
    // Cone Mesh components
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

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

        // Mesh setup
        mesh = new Mesh();
        enemy.EnemyMeshFilter.mesh = mesh;
        vertices = new Vector3[amountOfVertices];
        triangles = new int[vertices.Length * 3];

        // Cone angles setup
        startingAngle = Quaternion.AngleAxis(-desiredConeAngle, Vector3.up);
        stepAngle = Quaternion.AngleAxis(
            (desiredConeAngle + desiredConeAngle) /
            vertices.Length,
            Vector3.up);

        // Agent destination setup
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
        if (playerTarget == null) playerTarget = enemy.PlayerTarget;

        CastVisionCone();
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

    /// <summary>
    /// Calculates vision cone's mesh vertices.
    /// </summary>
    private void CastVisionCone()
    {
        Quaternion angle = enemy.transform.rotation * startingAngle;
        Vector3 direction = angle * Vector3.forward;
        Vector3 currentPos = enemy.transform.position;
        Vector3 offset = new Vector3(0, 0.5f, 0);

        vertices[0] = Vector3.zero;

        for (var i = 0; i < vertices.Length; i++)
        {
            // If it hits something
            if (Physics.Raycast(currentPos, direction, out RaycastHit hit,
                coneRange, collisionLayers))
            {
                if (i != 0)
                {
                    vertices[i] =
                        enemy.transform.InverseTransformPoint(hit.point);
                }

            }
            // Else if it doesn't hit anything
            else
            {
                if (i != 0)
                {
                    vertices[i] =
                        enemy.transform.InverseTransformPoint(
                            currentPos + direction * coneRange);
                }
            }
            direction = stepAngle * direction;

            if (i < vertices.Length - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
