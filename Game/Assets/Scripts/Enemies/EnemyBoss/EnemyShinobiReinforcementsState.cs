using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Shinobi Reinforcements State")]
public sealed class EnemyShinobiReinforcementsState : EnemyBossAbstractState
{
    public override void Start()
    {
        //
    }

    public override IState FixedUpdate()
    {
        base.FixedUpdate();

        return enemy.ReinforcementsState;
    }
}
