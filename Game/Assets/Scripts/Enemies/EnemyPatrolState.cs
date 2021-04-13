﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Scriptable object responsible for controlling enemy patrol state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Common Patrol State")]
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
    private IEnumerator movementCoroutine;

    /// <summary>
    /// * INITIAL ENEMY STATE * OnEnter won't run on the first state.
    /// Runs once on start.
    /// Sets agent's initial destination and starts movement coroutine.
    /// </summary>
    public override void Start()
    {
        base.Start();

        // Vision cone setup
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

        movementCoroutine = MovementCoroutine();
        enemy.StartCoroutine(movementCoroutine);

        // Must be on start aswell because OnEnter doesn't run on first state
        stats.MeleeDamageOnEnemy += CheckForInstantKill;
        stats.AnyDamageOnEnemy += TakeImpact;
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

        movementCoroutine = MovementCoroutine();
        enemy.StartCoroutine(movementCoroutine);
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

        // Calculates vision cone if the player isn't too far
        if (Vector3.Distance(myTarget.position, playerTarget.position) < 50)
        {
            if (!enemy.VisionCone.activeSelf) enemy.VisionCone.SetActive(true);
            visionCone?.Calculate();
        }
        else
        {
            if (enemy.VisionCone.activeSelf) enemy.VisionCone.SetActive(false);
        }

        // Search for player every searchCheckDelay seconds inside a vision cone
        if (Time.time - lastTimeChecked > searchCheckDelay)
        {
            // If it found the player, triggers defense state
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

        // Cancels all movement
        agent.SetDestination(myTarget.position);
        agent.isStopped = true;

        enemy.VisionCone.SetActive(false);

        breakState = false;
        if (movementCoroutine != null)
            enemy.StopCoroutine(movementCoroutine);

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
            // If agent reached the destination or is stopped
            if (agent.remainingDistance <= 0.1f || 
                agent.velocity.magnitude < 0.1f)
            {
                // Sets destination to where the agent is
                agent.SetDestination(enemy.transform.position);

                // Waits for the delay
                yield return wfs;

                // Increments the patrol point
                if (patrolIndex + 1 > patrolPoints.Length - 1) patrolIndex = 0;
                else patrolIndex++;

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
