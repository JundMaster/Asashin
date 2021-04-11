using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class responsible for handling enemy script.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(SpawnItemBehaviour))]
[RequireComponent(typeof(CapsuleCollider))]
public class Enemy : MonoBehaviour, IFindPlayer
{
    [Header("Enemy Target")]
    [SerializeField] private Transform myTarget;
    public Transform MyTarget => myTarget;

    [Header("Enemy Patrol path (order is important)")]
    [SerializeField] private Transform[] patrolPoints;
    public Transform[] PatrolPoints => patrolPoints;

    [Header("Cone Mesh")]
    [SerializeField] private GameObject visionCone;
    public GameObject VisionCone => visionCone;

    [Header("Enemy melee weapon")]
    [SerializeField] private SphereCollider weaponCollider;
    public SphereCollider WeaponCollider => weaponCollider;

    [Header("Enemy animator")]
    [SerializeField] private Animator anim;
    public Animator Anim => anim;

    public CinemachineTarget CineTarget { get; private set; }

    // Player variables
    public Player Player { get; private set; }
    public bool PlayerCurrentlyFighting
    {
        get => Player.PlayerCurrentlyFighting;
        set => Player.PlayerCurrentlyFighting = value;
    }
    public Transform PlayerTarget { get; private set; }
    public Vector3 PlayerLastKnownPosition { get; set; }

    [Header("EnemyStates")]
    [SerializeField] private EnemyState patrolStateOriginal;
    [SerializeField] private EnemyState defenseStateOriginal;
    [SerializeField] private EnemyState lostPlayerStateOriginal;
    [SerializeField] private EnemyState aggressiveStateOriginal;
    [SerializeField] private EnemyState temporaryBlindnessStateOriginal;
    [SerializeField] private EnemyState deathStateOriginal;

    // State getters
    public IState PatrolState { get; private set; }
    public IState DefenseState { get; private set; }
    public IState LostPlayerState { get; private set; }
    public IState AggressiveState { get; private set; }
    public IState TemporaryBlindnessState { get; private set; }
    public IState DeathState { get; private set; }

    private IEnumerable<IState> states;
    private StateMachine stateMachine;

    // Components
    public NavMeshAgent Agent { get; private set; }
    public VisionCone EnemyVisionCone { get; set; }
    private Stats enemyStats;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        CineTarget = FindObjectOfType<CinemachineTarget>();
        enemyStats = GetComponent<Stats>();

        PlayerLastKnownPosition = default;
        if (Player != null) PlayerCurrentlyFighting = false;

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

        if (deathStateOriginal != null)
            DeathState = Instantiate(deathStateOriginal);

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

    private void OnEnable()
    {
        enemyStats.Die += TriggerDeath;
    }

    private void OnDisable()
    {
        enemyStats.Die -= TriggerDeath;
    }

    private void FixedUpdate()
    {
        stateMachine?.FixedUpdate();
    }

    /// <summary>
    /// Finds Player when the Player spawns.
    /// Initializes values for all states.
    /// </summary>
    public void FindPlayer()
    {
        PlayerTarget = 
            GameObject.FindGameObjectWithTag("playerTarget").transform;

        Player = FindObjectOfType<Player>();
    }

    /// <summary>
    /// Turns PlayerTarget to null when the Player disappears.
    /// </summary>
    public void PlayerLost()
    {
        PlayerTarget = null;
        PlayerCurrentlyFighting = false;
    }

    /// <summary>
    /// Method that changes current state to TemporaryBlindnessState.
    /// </summary>
    public void BlindEnemy()
    {
        if (TemporaryBlindnessState != null) 
            stateMachine.CurrentState = TemporaryBlindnessState;
    }
        
    /// <summary>
    /// Method that triggers DeathState.
    /// </summary>
    private void TriggerDeath()
    {
        if (DeathState != null)
            stateMachine.CurrentState = DeathState;
    }   
}
