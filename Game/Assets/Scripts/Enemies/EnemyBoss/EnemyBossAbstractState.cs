/// <summary>
/// Abstract Scriptable object responsible for controlling boss enemy states.
/// </summary>
public abstract class EnemyBossAbstractState : EnemyAbstractState
{
    protected EnemyBoss enemy;

    /// <summary>
    /// Method that defines what happens when this state is initialized.
    /// </summary>
    /// <param name="en">Parent object of this state.</param>
    public override void Initialize(object en)
    {
        base.Initialize(en);

        enemy = en as EnemyBoss;
    }

    /// <summary>
    /// Checks if the enemy is close to the player.
    /// </summary>
    /// <param name="distance">Distance from player.</param>
    /// <returns>Returns true if it's near the player.</returns>
    protected virtual bool IsCloseToPlayer(float distance) => false;
}
