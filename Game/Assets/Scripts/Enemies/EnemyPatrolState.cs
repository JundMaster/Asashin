using UnityEngine;
using System.Collections;

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
    private bool breakState;

    /// <summary>
    /// Runs once on start.
    /// Sets agent's initial destination and starts movement coroutine.
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
        breakState = false;
        patrolPoints = enemy.PatrolPoints;
        patrolIndex = 0;
        enemy.StartCoroutine(MovementCoroutine());
    }

    /// <summary>
    /// Runs when entering this state. Turns back agent's movement and starts
    /// movement coroutine.
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        breakState = false;
        agent.isStopped = false;
        enemy.VisionCone.SetActive(true);
        enemy.StartCoroutine(MovementCoroutine());
    }

    /// <summary>
    /// Searches for player in a vision cone.
    /// Executes enemy's movement. Runs on fixed update.
    /// </summary>
    /// <returns>Returns an IState.</returns>
    public override IState FixedUpdate()
    {
        base.FixedUpdate();

        // Calculates vision cone if the player isn't too far
        if (Vector3.Distance(myTarget.position, playerTarget.position) < 50)
        {
            if (!enemy.VisionCone.activeSelf) enemy.VisionCone.SetActive(true);
            visionCone?.Calculate();
        }

        // Search for player every searchCheckDelay seconds inside a vision cone
        if (Time.time - lastTimeChecked > searchCheckDelay)
        {
            // If it found the player, triggers defense state
            if (PlayerInRange())
            {
                if (enemy.DefenseState != null) return enemy.DefenseState;
                return enemy.AggressiveState;
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
        breakState = true;

        // Instantiates an exclamation mark
        GameObject exclMark = Instantiate(
            exclamationMarkPrefab, 
            enemy.transform.position + offset, 
            Quaternion.identity);
        exclMark.transform.parent = enemy.transform;
    }

    /// <summary>
    /// Sets initial destination. If agent reached the destination or is stopped
    /// it stops the agent and waits for a delay. After the delay is over it
    /// increments the patrol points value and sets the next destination.
    /// </summary>
    /// <returns>Null.</returns>
    private IEnumerator MovementCoroutine()
    {
        YieldInstruction wffu = new WaitForFixedUpdate();
        YieldInstruction wfs = new WaitForSeconds(waitingDelay);

        agent.SetDestination(patrolPoints[0].transform.position);

        yield return wfs;

        while (breakState == false)
        {
            Debug.DrawRay(myTarget.position, patrolPoints[patrolIndex].position - enemy.transform.position);

            // If agent reached the destination or is stopped
            if (agent.remainingDistance <= 0.1f || agent.velocity.magnitude < 0.1f)
            {
                // Sets destination to where the agent is
                agent.SetDestination(enemy.transform.position);

                // Waits for the delay
                yield return wfs;

                // Increments the patrol point
                if (patrolIndex + 1 > patrolPoints.Length - 1) patrolIndex = 0;
                else patrolIndex++;

                // Sets the next destination
                agent.SetDestination(
                    patrolPoints[patrolIndex].transform.position);

                yield return wfs;
            }
            yield return wffu;
        }
    }
}
