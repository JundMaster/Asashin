﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Base class for all enemies. Implements IFindPlayer to update enemy variables
/// in case the player was lost and spawned.
/// </summary>
/// [RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]

public abstract class EnemyBase : MonoBehaviour, IFindPlayer
{
    // Fields and their getters
    [SerializeField] protected LayerMask myLayer;

    [Header("Enemy target")]
    [SerializeField] protected Transform myTarget;
    
    [Header("Size to alert other enemies when the enemy finds the player")]
    [SerializeField] private float sizeOfAlert;

    [Header("Enemy melee weapon")]
    [SerializeField] protected SphereCollider weaponCollider;
    ////////////////////////////////////////////////////////////////////////////

    // State machine variables
    protected IEnumerable<IState> states;
    protected StateMachine stateMachine;

    // Common state for every enemies
    [Header("Enemy death state")]
    [SerializeField] private EnemyAbstractState deathStateOriginal;
    public IState DeathState { get; private set; }

    // Player variables
    public Player Player { get; private set; }
    public byte PlayerCurrentlyFighting
    {
        get => Player.PlayerCurrentlyFighting;
        set
        {
            if (Player != null)
                Player.PlayerCurrentlyFighting = value;
        }
    }

    // Components
    public CinemachineTarget CineTarget { get; private set; }
    public Transform PlayerTarget { get; private set; }
    public Animator Anim { get; private set; }
    public EnemyStats Stats { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public Transform MyTarget => myTarget;
    public SphereCollider WeaponCollider => weaponCollider;

    protected void Awake()
    {
        CineTarget = FindObjectOfType<CinemachineTarget>();
        Anim = GetComponentInChildren<Animator>();
        Stats = GetComponent<EnemyStats>();
        Agent = GetComponent<NavMeshAgent>();

        if (deathStateOriginal != null)
            DeathState = Instantiate(deathStateOriginal);
    }

    /// <summary>
    /// Runs on state machine states.
    /// </summary>
    private void FixedUpdate()
    {
        stateMachine?.FixedUpdate();
    }

    /// <summary>
    /// In case this enemy finds the player, it alerts the surrounding enemies.
    /// </summary>
    public void AlertSurroundings()
    {
        Collider[] enemiesAround =
            Physics.OverlapSphere(myTarget.position, sizeOfAlert, myLayer);

        if (enemiesAround.Length > 0)
        {
            foreach (Collider enemyCollider in enemiesAround)
            {
                if (enemyCollider.TryGetComponent(out EnemySimple otherEnemy))
                {
                    if (otherEnemy.gameObject != gameObject)
                    {
                        otherEnemy.OnAlert();
                    }
                }
            }
        }
    }

    protected virtual void OnAlert()
    {
        // Left brank on purpose
    }

    /// <summary>
    /// Finds Player when the Player spawns.
    /// </summary>
    public void FindPlayer()
    {
        Player = FindObjectOfType<Player>();
        if (Player != null)
        {
            PlayerTarget =
                GameObject.FindGameObjectWithTag("playerTarget").transform;
        }
    }

    /// <summary>
    /// Turns PlayerTarget to null when the Player disappears.
    /// </summary>
    public void PlayerLost()
    {
        PlayerTarget = myTarget;
        PlayerCurrentlyFighting = 0;
    }

    #region Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sizeOfAlert);
    }
    #endregion
}
