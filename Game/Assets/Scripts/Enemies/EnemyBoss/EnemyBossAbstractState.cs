using UnityEngine;

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

    public override void Start()
    {
        //
    }
}
