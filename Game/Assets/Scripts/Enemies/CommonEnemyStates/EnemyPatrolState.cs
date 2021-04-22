using UnityEngine;
using System.Collections;

/// <summary>
/// Scriptable object responsible for controlling enemy patrol state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Common Patrol State")]
public class EnemyPatrolState : EnemyAbstractStateWithVision
{
    [Header("Checks if player is in cone range every X seconds")]
    [Range(0.01f,2f)][SerializeField] private float searchCheckDelay;

    [Header("Enemy cone render variables")]
    [Range(0, 255)][SerializeField] private byte amountOfVertices;
    [SerializeField] private Material coneMaterial;
    private VisionCone visionCone;

    [Header("Exclamation mark prefab")]
    [SerializeField] private GameObject exclamationMarkPrefab;
    [SerializeField] private Vector3 offset;

    [Header("Rotation speed after reaching final point (less means faster)")]
    [Range(0.1f, 1f)] [SerializeField] private float turnSpeed;
    private float smoothTimeRotation;

    // Movement
    private EnemyPatrolPoint[] patrolPoints;
    private byte patrolIndex;
    private bool breakState;
    private IEnumerator movementCoroutine;

    /// <summary>
    /// Runs once on start.
    /// </summary>
    public override void Start()
    {
        base.Start();

        // Vision cone setup
        if (amountOfVertices > desiredConeAngle) 
            amountOfVertices = desiredConeAngle;

        MeshFilter meshFilter = enemy.VisionCone.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = 
            enemy.VisionCone.GetComponent<MeshRenderer>();

        visionCone = new VisionCone(
            meshFilter, meshRenderer, coneMaterial, amountOfVertices,
            desiredConeAngle, coneRange, collisionLayers, enemy.transform);

        enemy.EnemyVisionCone = visionCone;

        // Agent destination setup
        breakState = false;
        patrolPoints = enemy.PatrolPoints;
        patrolIndex = 0;
    }

    /// <summary>
    /// Runs when entering this state. Turns back agent's movement and starts
    /// movement coroutine.
    /// Sets agent's initial destination and starts movement coroutine.
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        breakState = false;
        agent.isStopped = false;
        enemy.VisionCone.SetActive(true);

        // Only starts movement coroutine if the enemy has more than 1 patroi
        // point (meaning the enemy is not static)
        if (enemy.PatrolPoints.Length > 1)
        {
            movementCoroutine = MovementCoroutine();
            enemy.StartCoroutine(movementCoroutine);
        }
        else
        {
            agent.SetDestination(patrolPoints[patrolIndex].transform.position);
        }

        agent.speed = walkingSpeed;

        enemy.CollisionWithPlayer += TakeImpact;
    }

    /// <summary>
    /// Searches for player in a vision cone.
    /// Executes enemy's movement. Runs on fixed update.
    /// </summary>
    /// <returns>Returns an IState.</returns>
    public override IState FixedUpdate()
    {
        base.FixedUpdate();

        if (instantKill)
            return enemy.DeathState;

        if (alert)
            return enemy.DefenseState;

        // Calculates vision cone if the player isn't too far
        if (playerTarget != null)
        {
            if (Vector3.Distance(myTarget.position, playerTarget.position) < 75)
            {
                if (!enemy.VisionCone.activeSelf) 
                    enemy.VisionCone.SetActive(true);

                visionCone?.Calculate();
            }
            else
            {
                if (enemy.VisionCone.activeSelf) 
                    enemy.VisionCone.SetActive(false);
            }
        }

        // Rotates enemy towards patrol point forward when it reaches its 
        // final destination
        if (agent.remainingDistance <= 0.1f || agent.velocity.magnitude < 0.1f)
        {
            // Rotates towards the current point's forward
            if (patrolIndex + 1 > patrolPoints.Length - 1)
            {
                enemy.transform.RotateToSmoothly(
                    patrolPoints[0].transform.position +
                    patrolPoints[0].transform.forward,
                    ref smoothTimeRotation, turnSpeed);
            }
            else
            {
                enemy.transform.RotateToSmoothly(
                    patrolPoints[patrolIndex + 1].transform.position +
                    patrolPoints[patrolIndex + 1].transform.forward,
                    ref smoothTimeRotation, turnSpeed);
            }
        }

        // Search for player every searchCheckDelay seconds inside a vision cone
        if (Time.time - lastTimeChecked > searchCheckDelay)
        {
            // If it found the player, triggers defense or aggressive state
            // in case the enemy doesn't have defense
            if (PlayerInRange())
            {
                return 
                    enemy.DefenseState ?? 
                    enemy.AggressiveState ?? 
                    enemy.PatrolState;
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

        breakState = false;

        // Cancels all movement
        agent.SetDestination(myTarget.position);
        agent.isStopped = true;

        agent.speed = runningSpeed;

        enemy.VisionCone.SetActive(false);

        if (movementCoroutine != null)
            enemy.StopCoroutine(movementCoroutine);

        // Instantiates an exclamation mark
        GameObject exclMark = Instantiate(
            exclamationMarkPrefab, 
            enemy.transform.position + offset, 
            Quaternion.identity);
        exclMark.transform.parent = enemy.transform;

        agent.speed = runningSpeed;

        enemy.CollisionWithPlayer -= TakeImpact;
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
        YieldInstruction wfs = new WaitForSeconds(2f);

        agent.SetDestination(patrolPoints[patrolIndex].transform.position);

        yield return wfs;

        while (breakState == false)
        {
            // If agent reached the destination or is stopped
            if (agent.remainingDistance <= 0.1f || 
                agent.velocity.magnitude < 0.1f)
            {
                // Sets destination to where the agent is
                agent.SetDestination(enemy.transform.position);

                YieldInstruction wfpd = 
                    new WaitForSeconds(patrolPoints[patrolIndex].WaitingTime);

                // Increments the patrol point
                if (patrolIndex + 1 > patrolPoints.Length - 1)
                    patrolIndex = 0;
                else
                    patrolIndex++;

                // Waits for the delay of the current patrol point
                yield return wfpd;

                // If not in this state anymore
                if (breakState)
                    break;

                // Sets the next destination
                agent.SetDestination(
                    patrolPoints[patrolIndex].transform.position);

                yield return wfs;
            }
            yield return wffu;
        }
    }

    /// <summary>
    /// Starts ImpactToBack coroutine.
    /// </summary>
    protected override void TakeImpact()
    {
        breakState = false;
        base.TakeImpact();
    }
}
