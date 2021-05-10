using System.Collections;
using UnityEngine;

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
    protected bool hitFromBehind;

    // Sound variables
    protected bool followSound;
    protected Vector3 positionOfSound;

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
        followSound = false;
        positionOfSound = enemy.transform.position;
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
        enemy.ReactToSound += SetPositionOfSound;
    }

    /// <summary>
    /// Runs every time the state machine leaves this state.
    /// Sets player's last known position.
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();

        // Every time the enemy leaves a state, it will have a position
        // in case it's lost.
        if (playerTarget != null)
        {
            // Sets target to sound if the enemy heard something and
            // didn't get hit
            if (followSound && hitFromBehind == false)
                enemy.PositionOnLostPlayerState = positionOfSound;
            else
                enemy.PositionOnLostPlayerState = playerTarget.position;
        }

        enemy.ReactToSound -= SetPositionOfSound;
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

        // Triggers hit from behind and sets it to false OnExit
        // (after the state reacts to hit)
        hitFromBehind = true;

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

        agent.isStopped = false;
    }

    /// <summary>
    /// Sets position of sound the enemy heard.
    /// </summary>
    /// <param name="position">Position to set sound to.</param>
    private void SetPositionOfSound(Vector3 position)
    {
        followSound = true;
        positionOfSound = position;
    }

    /// <summary>
    /// Sets alert variable to true, so this enemy is alerted.
    /// </summary>
    protected void AlertEnemies() => 
        alert = true;
}
