using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Shinobi Ranged State")]
public sealed class EnemyShinobiRangedState : EnemyBossAbstractState
{
    public override IState FixedUpdate()
    {
        base.FixedUpdate();

        if (die)
            return enemy.DeathState;

        return enemy.RangedState;
    }
}
