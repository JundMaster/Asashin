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
    [SerializeField] private CustomVector2 timeToWalkSidewaysFromPlayer;
    private float timeToWalkSideways;
    private Direction directionToWalk;
    private float timeAfterReachingPos;
    private bool walkingLeft;
    private bool walkingRight;

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
    /// Runs on update.
    /// </summary>
    /// <returns>Null.</returns>
    public override IState Update()
    {
        base.Update();
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
        CancelWalkSideWaysVariables();
        anim.SetBool("RunningBack", runningBack);
        anim.SetBool("WalkingLeft", walkingLeft);
        anim.SetBool("WalkingRight", walkingRight);
        agent.speed = runningSpeed;
    }

    /// <summary>
    /// Moves the enemy towards the desired defense position.
    /// If the enemy is already in the desired position, it will starting
    /// to walk in circles around the player.
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

    /// <summary>
    /// Makes the enemy walk sideways.
    /// </summary>
    protected void WalkSideways()
    {
        // While the time passed is lower than the random walking time
        if (Time.time - timeAfterReachingPos >
            timeToWalkSideways * 2)
        {
            directionToWalk = Direction.Default;
            CancelWalkSideWaysVariables();

            // Gets new time for the next time walking sideways
            timeToWalkSideways = Random.Range(
                timeToWalkSidewaysFromPlayer.x,
                timeToWalkSidewaysFromPlayer.y);

            return;
        }

        if (Time.time - timeAfterReachingPos > timeToWalkSideways)
        {
            agent.speed = walkingSpeed;

            // Sets a new direction
            if (directionToWalk == Direction.Default)
            {
                int randomNum = Random.Range(0, 2);
                switch (randomNum)
                {
                    case 0:
                        directionToWalk = Direction.Left;
                        break;
                    case 1:
                        directionToWalk = Direction.Right;
                        break;
                }
            }

            // Starts walking Right
            if (directionToWalk == Direction.Right)
            {
                agent.SetDestination(
                    enemy.transform.position + enemy.transform.right * 2);

                walkingLeft = false;
                walkingRight = true;
            }
            // Starts walking left
            else
            {
                agent.SetDestination(
                    enemy.transform.position - enemy.transform.right * 2);

                walkingLeft = true;
                walkingRight = false;
            }
        }
    }

    /// <summary>
    /// Cancels walking sideways variables.
    /// </summary>
    protected void CancelWalkSideWaysVariables()
    {
        timeAfterReachingPos = Time.time;
        walkingLeft = false;
        walkingRight = false;
    }
}
