using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Abstract Scriptable object responsible for controlling enemy states.
/// </summary>
public abstract class EnemyAbstractState : StateBase
{
    // Movement
    [Range(0.1f, 5f)] [SerializeField] protected float walkingSpeed;
    [Range(0.1f, 5f)] [SerializeField] protected float runningSpeed;

    [Header("Distance that the enemy travels back after being hit")]
    [Range(0.1f, 1f)] [SerializeField] protected float timeToTravelAfterHit;
    [Range(0.1f, 3f)] [SerializeField] protected float takeDamageDistancePower;

    protected EnemyStats stats;
    protected Transform myTarget;
    protected NavMeshAgent agent;
    protected Transform playerTarget;

    private EnemyBase enemyBase;

    /// <summary>
    /// Method that defines what happens when this state is initialized.
    /// </summary>
    /// <param name="en">Parent object of this state.</param>
    public override void Initialize(object en)
    {
        enemyBase = en as EnemyBase;

        myTarget = enemyBase.MyTarget;
        playerTarget = enemyBase.PlayerTarget;
        agent = enemyBase.Agent;
        stats = enemyBase.GetComponent<EnemyStats>();
    }

    /// <summary>
    /// Runs every time the state machine enters this state.
    /// Finds playerTarget in case it's null.
    /// Registers to events.
    /// </summary>
    public override void OnEnter()
    {
        if (playerTarget == null) playerTarget = enemyBase.PlayerTarget;

        stats.AnyDamageOnEnemy += TakeImpact;
    }

    /// <summary>
    /// Method that defines what this state does. Runs on fixed update.
    /// </summary>
    /// <returns>Returns an IState.</returns>
    public override IState FixedUpdate()
    {
        if (playerTarget == null) playerTarget = enemyBase.PlayerTarget;
        return null;
    }

    /// <summary>
    /// Runs every time the state machine leaves this state.
    /// </summary>
    public override void OnExit()
    {
        stats.AnyDamageOnEnemy -= TakeImpact;
    }

    /// <summary>
    /// Starts ImapctToBackCoroutine.
    /// Pushes enemy back.
    /// </summary>
    protected virtual void TakeImpact() =>
        enemyBase.StartCoroutine(ImpactToBack());

    /// <summary>
    /// Happens after enemy being hit. Enemy is pushed it back.
    /// </summary>
    /// <returns>Null.</returns>
    protected virtual IEnumerator ImpactToBack()
    {
        YieldInstruction wffu = new WaitForFixedUpdate();
        float timeEntered = Time.time;

        // Direction from player to enemy
        Vector3 dir =
            myTarget.position.Direction(playerTarget.position);

        // Waits for fixed update to check if the enemy died meanwhile
        yield return wffu;

        while (Time.time - timeEntered < timeToTravelAfterHit)
        {
            agent.isStopped = true;

            // Pushes enemy back
            enemyBase.transform.position +=
                -(dir) *
                Time.fixedDeltaTime *
                takeDamageDistancePower;

            yield return wffu;
        }
        agent.isStopped = false;
    }
}
