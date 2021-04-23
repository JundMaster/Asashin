using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Shinobi Ranged State")]
public sealed class EnemyShinobiRangedState : EnemyBossAbstractState
{
    public override void Start()
    {
        //
    }

    public override IState FixedUpdate()
    {
        base.FixedUpdate();

        return enemy.RangedState;
    }
}
