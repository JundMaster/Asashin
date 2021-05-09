using UnityEngine;

/// <summary>
/// Abstract scriptable object base class for defense states.
/// </summary>
public abstract class EnemySimpleAbstractDefenseState : 
    EnemySimpleAbstractStateWithVision
{
    [Header("Distance from player. X => min. distance, Y => max. distance")]
    [SerializeField] protected CustomVector2 randomDistanceFromPlayer;

    // Movement variables
    protected float randomDistance;
    protected bool runningBack;

    [Header("Min and Max time to walk sideways after reaching destination")]
    [SerializeField] protected CustomVector2 timeToWalkSidewaysFromPlayer;
    protected float timeAfterReachingPos;
    protected float timeToWalkSideways;
    protected Direction directionToWalk;
    protected bool walkingSideways;
    protected bool walkingLeft;
    protected bool walkingRight;

    protected float smoothRotation;
    protected readonly float ROTATIONSPEED = 0.01f;

    protected readonly float MINDISTANCEFROMWALL = 3f;

    /// <summary>
    /// Happens once on start. Sets a random distance to mantain while defending.
    /// </summary>
    public override void Start()
    {
        base.Start();

        // Sets values in case they are not valid
        if (randomDistanceFromPlayer.y < randomDistanceFromPlayer.x)
            randomDistanceFromPlayer.y = randomDistanceFromPlayer.x;
        if (randomDistanceFromPlayer.x > randomDistanceFromPlayer.y)
            randomDistanceFromPlayer.x = randomDistanceFromPlayer.y;

        if (timeToWalkSidewaysFromPlayer.y < timeToWalkSidewaysFromPlayer.x)
            timeToWalkSidewaysFromPlayer.y = timeToWalkSidewaysFromPlayer.x;
        if (timeToWalkSidewaysFromPlayer.x > timeToWalkSidewaysFromPlayer.y)
            timeToWalkSidewaysFromPlayer.x = timeToWalkSidewaysFromPlayer.y;

        randomDistance = Random.Range(
            randomDistanceFromPlayer.x, randomDistanceFromPlayer.y);

        timeToWalkSideways = Random.Range(
            timeToWalkSidewaysFromPlayer.x, timeToWalkSidewaysFromPlayer.y);

        // Sets a random direction to walk intialially
        switch (Random.Range(0, 2))
        {
            case 0:
                directionToWalk = Direction.Left;
                break;
            case 1:
                directionToWalk = Direction.Right;
                break;
        }

        runningBack = false;
    }

    /// <summary>
    /// Happens once when this state is enabled. Sets a kunai timer.
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();

        enemy.InCombat = true;

        agent.isStopped = false;

        runningBack = false;

        enemy.VisionConeGameObject.SetActive(false);
    }

    /// <summary>
    /// Runs on fixed update.
    /// </summary>
    /// <returns>Null.</returns>
    public override IState FixedUpdate()
    {
        base.FixedUpdate();
        anim.SetBool("RunningBack", runningBack);
        anim.SetBool("WalkingLeft", walkingLeft);
        anim.SetBool("WalkingRight", walkingRight);

        return null;
    }

    /// <summary>
    /// Happens once when leaving this state.
    /// Sets player's last known position.
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();
        runningBack = false;
        walkingLeft = false;
        walkingRight = false;
        anim.SetBool("RunningBack", runningBack);
        anim.SetBool("WalkingLeft", walkingLeft);
        anim.SetBool("WalkingRight", walkingRight);
        agent.speed = runningSpeed;
    }

    /// <summary>
    /// Moves the enemy towards the desired defense position.
    /// </summary>
    /// <returns>Returns true if it needs to move. 
    /// Returns false if it's in the desired position.</returns>
    protected abstract bool MoveToDefensiveRange();

    /// <summary>
    /// Checks if the enemy is facing the player.
    /// </summary>
    /// <returns>Returns true if the enemy is facing the player.</returns>
    protected bool FacingPlayer()
    {
        if (myTarget.IsLookingTowards(playerTarget.position))
            return true;

        return false;
    }
}
