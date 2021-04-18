using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

/// <summary>
/// Class responsible for handling enemy script.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(SpawnItemBehaviour))]
[RequireComponent(typeof(CapsuleCollider))]
public class Enemy : MonoBehaviour, IFindPlayer
{
    [SerializeField] private LayerMask myLayer;
    [SerializeField] private float sizeOfAlert;

    [Header("Enemy Target")]
    [SerializeField] private Transform myTarget;
    public Transform MyTarget => myTarget;

    [Header("Enemy Patrol path (order is important)")]
    [SerializeField] private EnemyPatrolPoint[] patrolPoints;
    public EnemyPatrolPoint[] PatrolPoints => patrolPoints;

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
        set
        {
            if (Player != null)
                Player.PlayerCurrentlyFighting = value;
        }
    }
    public Transform PlayerTarget { get; private set; }
    public Vector3 PlayerLastKnownPosition { get; set; }

    [Header("EnemyStates")]
    [SerializeField] private EnemyAbstractState patrolStateOriginal;
    [SerializeField] private EnemyAbstractState defenseStateOriginal;
    [SerializeField] private EnemyAbstractState lostPlayerStateOriginal;
    [SerializeField] private EnemyAbstractState aggressiveStateOriginal;
    [SerializeField] private EnemyAbstractState temporaryBlindnessStateOriginal;
    [SerializeField] private EnemyAbstractState deathStateOriginal;

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
    private EnemyAnimationEvents animationEvents;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        CineTarget = FindObjectOfType<CinemachineTarget>();
        enemyStats = GetComponent<Stats>();
        animationEvents = GetComponentInChildren<EnemyAnimationEvents>();

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

    /// <summary>
    /// Happens once on enable, registers to events.
    /// </summary>
    private void OnEnable()
    {
        enemyStats.Die += OnDeath;
        animationEvents.Hit += OnWeaponHit;
    }

    /// <summary>
    /// Happens once on disable, unregisters from events.
    /// </summary>
    private void OnDisable()
    {
        enemyStats.Die -= OnDeath;
        animationEvents.Hit -= OnWeaponHit;
    }

    /// <summary>
    /// Runs once on start. Initializes states.
    /// </summary>
    private void Start()
    {
        PlayerLastKnownPosition = default;
        PlayerCurrentlyFighting = false;

        stateMachine?.Initialize();
    }

    /// <summary>
    /// Runs on state machine states.
    /// </summary>
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
        Player = FindObjectOfType<Player>();
        if (Player != null)
        {
            PlayerTarget =
                GameObject.FindGameObjectWithTag("playerTarget").transform;
            PlayerCurrentlyFighting = false;
        }
    }

    /// <summary>
    /// Turns PlayerTarget to null when the Player disappears.
    /// </summary>
    public void PlayerLost()
    {
        PlayerTarget = myTarget;
        PlayerCurrentlyFighting = false;
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
    /// Method that triggers DeathState.
    /// Is triggered when enemy's health reaches 0.
    /// </summary>
    private void OnDeath()
    {
        if (DeathState != null)
            stateMachine?.SwitchToNewState(DeathState);
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
                if (enemyCollider.TryGetComponent(out Enemy otherEnemy))
                {
                    if (otherEnemy.gameObject != gameObject)
                    {
                        otherEnemy.OnAlert();
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 offset = new Vector3(0, 0.15f, 0);
        foreach(EnemyPatrolPoint patrolPoint in PatrolPoints)
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sizeOfAlert);
    }

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
    /// Invokes CollisionWithPlayer event.
    /// </summary>
    protected virtual void OnCollisionWithPlayer() => 
        CollisionWithPlayer?.Invoke();

    /// <summary>
    /// Method called from enemy states in order to invoke Alert event.
    /// </summary>
    public virtual void OnAlert() => Alert?.Invoke();

    /// <summary>
    /// Invokes WeaponHit event.
    /// </summary>
    protected virtual void OnWeaponHit() => WeaponHit?.Invoke();

    /// <summary>
    /// Happens when the enemy collides with player.
    /// Event registered on PatrolState and LostPlayerState.
    /// </summary>
    public event Action CollisionWithPlayer;

    /// <summary>
    /// Event registered on enemy states in order to alert all enemies.
    /// </summary>
    public event Action Alert;

    /// <summary>
    /// Event registered on Aggressive State.
    /// Is triggered after the enemy atacks.
    /// </summary>
    public event Action WeaponHit;
}
