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
    protected bool followSound;

    /// <summary>
    /// Enum with possible punctuation marks.
    /// </summary>
    protected enum TypeOfMark { Interrogation, Exclamation , None};
    private TypeOfMark currentPunctuationMark;
    // Stops punctuation mark from spawning with delay
    private float punctuationMarkCurrentTimer;

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
        punctuationMarkCurrentTimer = 0;

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
            if (die) 
                break;

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
        enemy.PositionOfSoundListened = position;
    }

    /// <summary>
    /// Sets alert variable to true, so this enemy is alerted.
    /// </summary>
    protected void AlertEnemies() => 
        alert = true;

    /// <summary>
    /// Checks if enemy is near player.
    /// </summary>
    /// <returns>Returns true if the enemy is really close to the 
    /// player.</returns>
    protected bool NearPlayer()
    {
        if (enemy.Player != null)
            if (Vector3.Distance(
                myTarget.position, playerTarget.position) < 1.3f)
                return true;
        
        return false;
    }

    /// <summary>
    /// Instantiates an interrogation or exclamation mark.
    /// </summary>
    protected void SpawnPunctuationMark(TypeOfMark type)
    {
        float punctuationMarkDelay = 1;

        // Instantiates an exclamation mark
        if (type == TypeOfMark.Interrogation)
        {
            // Interrogation mark has a delay so it will be prevented from
            // spawning constantly while the enemy is reacting
            if (Time.time - punctuationMarkCurrentTimer > punctuationMarkDelay)
            {
                if (currentPunctuationMark == TypeOfMark.Exclamation)
                    enemy.ExclamationMark.SetActive(false);

                enemy.InterrogationMark.SetActive(true);

                punctuationMarkCurrentTimer = Time.time;
                currentPunctuationMark = TypeOfMark.Interrogation;
            }
        }
        else if (type == TypeOfMark.Exclamation)
        {
            if (currentPunctuationMark == TypeOfMark.Interrogation)
                enemy.InterrogationMark.SetActive(false);

            enemy.ExclamationMark.SetActive(true);
            currentPunctuationMark = TypeOfMark.Exclamation;
        }
        else
        {
            enemy.InterrogationMark.SetActive(false);
            enemy.ExclamationMark.SetActive(false);
            currentPunctuationMark = TypeOfMark.None;
        }
    }
}
