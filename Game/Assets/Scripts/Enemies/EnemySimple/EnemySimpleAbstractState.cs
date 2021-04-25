using UnityEngine;
using System.Collections;

/// <summary>
/// Abstract Scriptable object responsible for controlling simple enemy states.
/// </summary>
public abstract class EnemySimpleAbstractState : EnemyAbstractState
{
    [Range(0.1f, 5f)] [SerializeField] protected float walkingSpeed;

    [Header("Rotation speed after being hit (less means faster)")]
    [Range(0.1f, 1f)] [SerializeField] private float turnSpeedAfterBeingHit;
    private float smoothTimeRotationAfterBeingHit;
    
    protected EnemySimple enemy;

    protected bool alert;

    /// <summary>
    /// Method that defines what happens when this state is initialized.
    /// </summary>
    /// <param name="en">Parent object of this state.</param>
    public override void Initialize(object en)
    {
        base.Initialize(en);

        enemy = en as EnemySimple;
    }

    /// <summary>
    /// Runs once on start.
    /// </summary>
    public override void Start()
    {
        base.Start();
        alert = false;
    }

    /// <summary>
    /// Runs every time the state machine enters this state.
    /// Finds playerTarget in case it's null.
    /// Registers to events to check for instant kill or take impact after
    /// being hit.
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        alert = false;

        stats.MeleeDamageOnEnemy += CheckForInstantKill;
        enemy.Alert += AlertEnemies;
    }

    /// <summary>
    /// Runs every time the state machine leaves this state.
    /// Sets player's last known position.
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();

        if (playerTarget != null)
            enemy.PlayerLastKnownPosition = playerTarget.position;

        enemy.Alert -= AlertEnemies;
        stats.MeleeDamageOnEnemy -= CheckForInstantKill;
    }

    /// <summary>
    /// Happens after enemy being hit. Rotates enemy and pushes it back.
    /// </summary>
    /// <returns>Null.</returns>
    protected override IEnumerator ImpactToBack()
    {
        YieldInstruction wffu = new WaitForFixedUpdate();
        float timeEntered = Time.time;

        // Direction from player to enemy
        Vector3 dir =
            myTarget.position.Direction(playerTarget.position);

        // Waits for fixed update to check if the enemy died meanwhile
        yield return wffu;

        while (Time.time - timeEntered < timeToTravelAfterHit &&
            die == false)
        {
            // To be sure the coroutine doesn't run while the enemy is dying
            if (die) break;

            enemy.transform.RotateToSmoothly(playerTarget.position,
                ref smoothTimeRotationAfterBeingHit, turnSpeedAfterBeingHit);

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
                enemy.transform.forward, playerMovement.transform.forward) > 0 
                && playerMovement.Walking)
            {
                SwitchToDeathState();
            }
        }
    }

    /// <summary>
    /// Sets alert variable to true, so this enemy is alerted.
    /// </summary>
    protected void AlertEnemies() => 
        alert = true;
}
