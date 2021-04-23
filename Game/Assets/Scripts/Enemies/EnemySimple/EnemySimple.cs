using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class responsible for handling a simple enemy script.
/// </summary>
[RequireComponent(typeof(SpawnItemBehaviour))]
public sealed class EnemySimple : EnemyBase
{
    [Header("Simple enemy states")]
    [SerializeField] private EnemyAbstractState patrolStateOriginal;
    [SerializeField] private EnemyAbstractState defenseStateOriginal;
    [SerializeField] private EnemyAbstractState lostPlayerStateOriginal;
    [SerializeField] private EnemyAbstractState aggressiveStateOriginal;
    [SerializeField] private EnemyAbstractState temporaryBlindnessStateOriginal;

    // State getters
    public IState PatrolState { get; private set; }
    public IState DefenseState { get; private set; }
    public IState LostPlayerState { get; private set; }
    public IState AggressiveState { get; private set; }
    public IState TemporaryBlindnessState { get; private set; }

    [Header("Enemy patrol path (order is important)")]
    [SerializeField] private EnemyPatrolPoint[] patrolPoints;
    public EnemyPatrolPoint[] PatrolPoints => patrolPoints;

    [Header("Cone mesh")]
    [SerializeField] private GameObject visionCone;
    public GameObject VisionCone => visionCone;

    public Vector3 PlayerLastKnownPosition { get; set; }

    public VisionCone EnemyVisionCone { get; set; }

    private new void Awake()
    {
        base.Awake();

        if (patrolStateOriginal != null)
            PatrolState = Instantiate(patrolStateOriginal);

        if (defenseStateOriginal != null)
            DefenseState = Instantiate(defenseStateOriginal);

        if (lostPlayerStateOriginal != null)
            LostPlayerState = Instantiate(lostPlayerStateOriginal);

        if (aggressiveStateOriginal != null)
            AggressiveState = Instantiate(aggressiveStateOriginal);

        if (temporaryBlindnessStateOriginal != null)
            TemporaryBlindnessState =
                Instantiate(temporaryBlindnessStateOriginal);

        states = new List<IState>
        {
            PatrolState,
            DefenseState,
            LostPlayerState,
            AggressiveState,
            TemporaryBlindnessState,
            DeathState,
        };

        stateMachine = new StateMachine(states, this);
    }

    /// <summary>
    /// Runs once on start. Initializes states.
    /// </summary>
    private new void Start()
    {
        PlayerLastKnownPosition = default;
        base.Start();
    }

    /// <summary>
    /// Method that changes current state to TemporaryBlindnessState.
    /// </summary>
    public void BlindEnemy()
    {
        if (TemporaryBlindnessState != null)
            stateMachine?.SwitchToNewState(TemporaryBlindnessState);
    }

    /// <summary>
    /// Method called from AlertSurroundings, called from enemy states,
    /// in order to invoke Alert event.
    /// </summary>
    protected override void OnAlert() => 
        Alert?.Invoke();

    /// <summary>
    /// Invokes CollisionWithPlayer event.
    /// </summary>
    private void OnCollisionWithPlayer() => 
        CollisionWithPlayer?.Invoke();

    /// <summary>
    /// On trigger enter it invokes CollisionWithPlayer event.
    /// </summary>
    /// <param name="other">Other collider.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (Player != null)
            if (other.gameObject.layer == Player.gameObject.layer)
                OnCollisionWithPlayer();
    }

    /// <summary>
    /// Happens when the enemy collides with player.
    /// Event registered on PatrolState and LostPlayerState.
    /// </summary>
    public event Action CollisionWithPlayer;

    /// <summary>
    /// Event registered on enemy states in order to alert all enemies.
    /// </summary>
    public event Action Alert;

    #region Gizmos with patrol points
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 offset = new Vector3(0, 0.15f, 0);
        foreach (EnemyPatrolPoint patrolPoint in PatrolPoints)
        {
            Gizmos.DrawSphere(patrolPoint.transform.position + offset, 0.25f);
            Gizmos.DrawLine(patrolPoint.transform.position + offset,
                patrolPoint.transform.position + offset +
                patrolPoint.transform.forward);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 offset = new Vector3(0, 0.15f, 0);
        foreach (EnemyPatrolPoint patrolPoint in PatrolPoints)
        {
            Gizmos.DrawSphere(patrolPoint.transform.position + offset, 0.25f);
            Gizmos.DrawLine(patrolPoint.transform.position + offset,
                patrolPoint.transform.position + offset +
                patrolPoint.transform.forward);
        }  
    }
    #endregion
}
