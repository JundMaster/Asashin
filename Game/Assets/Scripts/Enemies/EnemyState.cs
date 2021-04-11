﻿using UnityEngine;
using UnityEngine.AI;
using System.Collections;

/// <summary>
/// Abstract Scriptable object responsible for controlling enemy states.
/// </summary>
public abstract class EnemyState : StateBase
{
    [Header("Distance that the enemy travels back after being hit")]
    [Range(0.1f,1f)][SerializeField] protected float timeToTravelAfterHit;
    [Range(0.1f,3f)][SerializeField] protected float takeDamageDistancePower;
    protected Enemy enemy;
    protected EnemyStats stats;
    protected Transform playerTarget;
    protected Transform myTarget;
    protected NavMeshAgent agent;

    /// <summary>
    /// Method that defines what happens when this state is initialized.
    /// </summary>
    /// <param name="obj">Parent object of this state.</param>
    public override void Initialize(object obj)
    {
        enemy = obj as Enemy;
        stats = enemy.GetComponent<EnemyStats>();
        myTarget = enemy.MyTarget;
        playerTarget = enemy.PlayerTarget;
        agent = enemy.Agent;
    }

    /// <summary>
    /// Runs once on start.
    /// </summary>
    public override void Start()
    {
        // Left brank on purpose
    }

    /// <summary>
    /// Runs every time the state machine enters this state.
    /// Finds playerTarget in case it's null.
    /// </summary>
    public override void OnEnter()
    {
        if (playerTarget == null) playerTarget = enemy.PlayerTarget;
    }

    /// <summary>
    /// Runs every time the state machine leaves this state.
    /// Sets player's last known position.
    /// </summary>
    public override void OnExit()
    {
        enemy.PlayerLastKnownPosition = playerTarget.position;
    }

    /// <summary>
    /// Method that defines what this state does. Runs on fixed update.
    /// </summary>
    /// <returns>Returns an IState.</returns>
    public override IState FixedUpdate()
    {
        if (playerTarget == null) playerTarget = enemy.PlayerTarget;
        return null;
    }

    /// <summary>
    /// Starts ImapctToBackCoroutine.
    /// Pushes enemy back.
    /// </summary>
    protected virtual void TakeImpact()
    {
        enemy.StartCoroutine(ImpactToBack());
    }

    /// <summary>
    /// Happens after enemy being hit. Rotates enemy and pushes it back.
    /// </summary>
    /// <returns>Null.</returns>
    private IEnumerator ImpactToBack()
    {
        YieldInstruction wffu = new WaitForFixedUpdate();
        float timeEntered = Time.time;

        Vector3 dir =
            (playerTarget.transform.position - myTarget.position).normalized;
        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        enemy.transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

        while (Time.time - timeEntered < timeToTravelAfterHit)
        {
            agent.isStopped = true;

            enemy.transform.position +=
                -(dir) *
                Time.fixedDeltaTime *
                takeDamageDistancePower;

            yield return wffu;
        }
        agent.isStopped = false;
    }
}
