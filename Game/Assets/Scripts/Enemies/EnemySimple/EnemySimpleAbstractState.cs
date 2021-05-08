using System.Collections;
using UnityEngine;

/// <summary>
/// Abstract Scriptable object responsible for controlling simple enemy states.
/// </summary>
public abstract class EnemySimpleAbstractState : EnemyAbstractState
{
    [Header("Rotation speed after being hit (less means faster)")]
    [Range(0.1f, 1f)] [SerializeField] private float turnSpeedAfterBeingHit;
    private float smoothTimeRotationAfterBeingHit;
    
    protected EnemySimple enemy;

    protected bool alert;
    protected bool hitFromBehind;

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
        hitFromBehind = false;
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

        enemy.InstantDeath += SwitchToDeathState;
        enemy.Alert += AlertEnemies;
    }

    /// <summary>
    /// Runs every time the state machine leaves this state.
    /// Sets player's last known position.
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();

        hitFromBehind = false;

        if (playerTarget != null)
            enemy.PlayerLastKnownPosition = playerTarget.position;

        enemy.Alert -= AlertEnemies;
        enemy.InstantDeath -= SwitchToDeathState;
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

        anim.SetTrigger("TakeHit");

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

        // Triggers hit from behind and sets it to false OnExit
        // (after the state reacts to hit)
        hitFromBehind = true;
        agent.isStopped = false;
    }

    /// <summary>
    /// Sets alert variable to true, so this enemy is alerted.
    /// </summary>
    protected void AlertEnemies() => 
        alert = true;
}
