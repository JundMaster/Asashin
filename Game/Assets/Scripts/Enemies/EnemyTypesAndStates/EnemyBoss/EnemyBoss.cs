﻿using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class responsible for handling boss enemy.
/// </summary>
public sealed class EnemyBoss : EnemyBase
{
    [SerializeField] private bool TESTVARIABLETOTESTBOSSOUTSIDEAREA;

    [Header("Enemy Boss States")]
    [SerializeField] private EnemyAbstractState aggressiveStateOriginal;
    [SerializeField] private EnemyAbstractState rangedStateOriginal;
    [SerializeField] private EnemyAbstractState reinforcementsStateOriginal;

    // State getters
    public IState AggressiveState { get; private set; }
    public IState RangedState { get; private set; }
    public IState ReinforcementsState { get; private set; }

    [Header("Boss area corner limits, 0 must be on the oposite corner of 3")]
    [SerializeField] private Transform[] corners;
    public Transform[] Corners => corners;
    [SerializeField] private Transform centerPosition;
    public Transform CenterPosition => centerPosition;

    [Header("Prefabs to spawn when calling for reinforcements")]
    [SerializeField] private GameObject[] minionsPrefabs;
    public GameObject[] MinionsPrefabs => minionsPrefabs;
    public GameObject[] SpawnedMinions { get; set; }

    private new void Awake()
    {
        base.Awake();

        if (TESTVARIABLETOTESTBOSSOUTSIDEAREA)
            InitializeStateMachine();

        SpawnedMinions = new GameObject[0];
    }

    /// <summary>
    /// "Connects" this boss to boss ui health bar.
    /// </summary>
    private void Start()
    {
        UIBossHealthBar myHpBar = FindObjectOfType<UIBossHealthBar>();
        myHpBar.EnemyStats = base.Stats;
        myHpBar.RegisterEvents();
    }

    /// <summary>
    /// Initializes states, state machine and calls base awake.
    /// </summary>
    public void InitializeStateMachine()
    {
        FindPlayer();

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

        stateMachine?.Initialize();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (Transform corner in corners)
            Gizmos.DrawSphere(corner.position, 0.25f);
    }
}
