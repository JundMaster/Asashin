using UnityEngine;
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
    protected Transform myTarget;
    protected NavMeshAgent agent;

    protected Transform playerTarget;

    protected bool instantKill;

    /// <summary>
    /// Method that defines what happens when this state is initialized.
    /// </summary>
    /// <param name="obj">Parent object of this state.</param>
    public override void Initialize(object obj)
    {
        enemy = obj as Enemy;
        stats = enemy.GetComponent<EnemyStats>();
        myTarget = enemy.MyTarget;
        playerTarget = enemy.PlayerTarget != null? enemy.PlayerTarget : myTarget;
        agent = enemy.Agent;
    }

    /// <summary>
    /// Runs once on start.
    /// </summary>
    public override void Start()
    {
        instantKill = false;
    }

    /// <summary>
    /// Runs every time the state machine enters this state.
    /// Finds playerTarget in case it's null.
    /// Registers to events to check for instant kill or take impact after
    /// being hit.
    /// </summary>
    public override void OnEnter()
    {
        if (playerTarget == null) playerTarget = enemy.PlayerTarget;

        stats.MeleeDamageOnEnemy += CheckForInstantKill;
        stats.AnyDamageOnEnemy += TakeImpact;
    }

    /// <summary>
    /// Runs every time the state machine leaves this state.
    /// Sets player's last known position.
    /// </summary>
    public override void OnExit()
    {
        enemy.PlayerLastKnownPosition = playerTarget.position;
        stats.MeleeDamageOnEnemy -= CheckForInstantKill;
        stats.AnyDamageOnEnemy -= TakeImpact;
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
    protected virtual void TakeImpact() => 
        enemy.StartCoroutine(ImpactToBack());

    /// <summary>
    /// Happens after enemy being hit. Rotates enemy and pushes it back.
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

        enemy.transform.RotateTo(playerTarget.position);

        while (Time.time - timeEntered < timeToTravelAfterHit &&
            instantKill == false)
        {
            agent.isStopped = true;

            // Pushes enemy back
            enemy.transform.position +=
                -(dir) *
                Time.fixedDeltaTime *
                takeDamageDistancePower;

            yield return wffu;
        }
        agent.isStopped = false;
    }

    /// <summary>
    /// If the enemy has his back turned and the player is sneaking,
    /// the enemy dies instantly.
    /// </summary>
    protected void CheckForInstantKill()
    {
        PlayerMovement playerMovement = 
            enemy.Player.GetComponent<PlayerMovement>();

        if (playerMovement != null)
        {
            // Checks if enemy has his back turned to the player
            // If the player forward is similiar to the enemy's forward
            // This means the player successfully instantly killed the enemy
            // Only happens if player is sneaking.
            if (Vector3.Dot(
                enemy.transform.forward, playerTarget.forward) >
                0.5f &&
                playerMovement.Walking)
            {
                SwitchToDeathState();
            }
        }
    }

    /// <summary>
    /// Instantly switches to DeathState.
    /// </summary>
    protected void SwitchToDeathState() =>
        instantKill = true;
}
