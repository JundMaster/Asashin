using UnityEngine;

/// <summary>
/// Abstract scriptable object base class for defense states.
/// </summary>
public abstract class EnemyAbstractDefenseState : EnemyAbstractStateWithVision
{
    [Header("Rotation smooth time")]
    [Range(0.1f, 1.5f)] [SerializeField] protected float turnSpeed;
    protected float smoothTimeRotation;

    [Header("Distance from player. X => min. distance, Y => max. distance")]
    [SerializeField] protected Vector2 randomDistanceFromPlayer;

    // Movement variables
    protected float randomDistance;

    /// <summary>
    /// Happens once on start. Sets a random distance to mantain while defending.
    /// </summary>
    public override void Start()
    {
        base.Start();

        if (randomDistanceFromPlayer.y < randomDistanceFromPlayer.x)
            randomDistanceFromPlayer.y = randomDistanceFromPlayer.x;
        if (randomDistanceFromPlayer.x > randomDistanceFromPlayer.y)
            randomDistanceFromPlayer.x = randomDistanceFromPlayer.y;

        randomDistance = Random.Range(
            randomDistanceFromPlayer.x, randomDistanceFromPlayer.y);
    }

    /// <summary>
    /// Happens once when this state is enabled. Sets a kunai timer.
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();

        agent.isStopped = false;

        enemy.VisionCone.SetActive(false);
    }

    /// <summary>
    /// Runs on fixed update.
    /// </summary>
    /// <returns>Null.</returns>
    public override IState FixedUpdate()
    {
        base.FixedUpdate();
        return null;
    }

    /// <summary>
    /// Happens once when leaving this state.
    /// Sets player's last known position.
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();
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
