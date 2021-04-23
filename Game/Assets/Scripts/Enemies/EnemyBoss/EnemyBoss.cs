using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class responsible for handling boss enemy.
/// </summary>
public sealed class EnemyBoss : EnemyBase
{
    [Header("Enemy Boss States")]
    [SerializeField] private EnemyAbstractState aggressiveStateOriginal;
    [SerializeField] private EnemyAbstractState rangedStateOriginal;
    [SerializeField] private EnemyAbstractState reinforcementsStateOriginal;

    // State getters
    public IState AggressiveState { get; private set; }
    public IState RangedState { get; private set; }
    public IState ReinforcementsState { get; private set; }

    private new void Awake()
    {
        base.Awake();

        if (aggressiveStateOriginal != null)
            AggressiveState = Instantiate(aggressiveStateOriginal);

        if (rangedStateOriginal != null)
            RangedState = Instantiate(rangedStateOriginal);

        if (reinforcementsStateOriginal != null)
            ReinforcementsState = Instantiate(reinforcementsStateOriginal);

        states = new List<IState>
        {
            AggressiveState,
            RangedState,
            ReinforcementsState,
            DeathState,
        };

        stateMachine = new StateMachine(states, this);
    }
}
