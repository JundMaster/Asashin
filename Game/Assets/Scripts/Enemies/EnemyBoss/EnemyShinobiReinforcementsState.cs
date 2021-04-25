using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Shinobi Reinforcements State")]
public sealed class EnemyShinobiReinforcementsState : EnemyBossAbstractState
{
    public override IState FixedUpdate()
    {
        base.FixedUpdate();

        if (die)
            return enemy.DeathState;

        return enemy.ReinforcementsState;
    }
}
