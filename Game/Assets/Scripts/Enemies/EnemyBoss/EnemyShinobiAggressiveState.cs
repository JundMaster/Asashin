using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Shinobi Aggressive State")]
public sealed class EnemyShinobiAggressiveState : EnemyBossAbstractState
{
    public override void Start()
    {
        //
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override IState FixedUpdate()
    {
        base.FixedUpdate();

        return enemy.AggressiveState;
    }
}