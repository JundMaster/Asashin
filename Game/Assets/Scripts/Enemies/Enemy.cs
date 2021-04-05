using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class responsible for handling enemy script.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(DeathBehaviour))]
public class Enemy : MonoBehaviour, IFindPlayer
{
    [Header("Enemy Target")]
    [SerializeField] private Transform myTarget;
    public Transform MyTarget => myTarget;

    [Header("Enemy Patrol path (order is important)")]
    [SerializeField] private Transform[] patrolPoints;
    public Transform[] PatrolPoints => patrolPoints;

    [Header("Enemy animator")]
    [SerializeField] private Animator anim;
    public Animator Anim => anim;

    // Player variables
    private Player player;
    public bool PlayerCurrentlyFighting
    {
        get => player.PlayerCurrentlyFighting;
        set => player.PlayerCurrentlyFighting = value;
    }
    public Transform PlayerTarget { get; private set; }
    public Vector3 PlayerLastKnownPosition { get; set; }

    [Header("EnemyStates")]
    [SerializeField] private EnemyState patrolStateOriginal;
    [SerializeField] private EnemyState defenseStateOriginal;
    [SerializeField] private EnemyState lostPlayerStateOriginal;
    [SerializeField] private EnemyState aggressiveStateOriginal;

    // State getters
    public IEnemyState PatrolState { get; private set; }
    public IEnemyState DefenseState { get; private set; }
    public IEnemyState LostPlayerState { get; private set; }
    public IEnemyState AggressiveState { get; private set; }

    private IEnemyState currentBehaviourState;

    // Components
    public NavMeshAgent Agent { get; private set; }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();

        PlayerLastKnownPosition = default;
        if (player != null) PlayerCurrentlyFighting = false;

        if (patrolStateOriginal != null)
            PatrolState = Instantiate(patrolStateOriginal);

        if (defenseStateOriginal != null)
            DefenseState = Instantiate(defenseStateOriginal);

        if (lostPlayerStateOriginal != null)
            LostPlayerState = Instantiate(lostPlayerStateOriginal);

        if (aggressiveStateOriginal != null)
            AggressiveState = Instantiate(aggressiveStateOriginal);
    }

    private void Start()
    {
        // States are also initialized once the player spawns.
        InitializeStates();

        currentBehaviourState = PatrolState;
    }

    private void FixedUpdate()
    {
        currentBehaviourState = currentBehaviourState?.Execute(this);
    }

    /// <summary>
    /// Spawns all states.
    /// </summary>
    private void InitializeStates()
    {
        PatrolState?.Initialize(this);
        DefenseState?.Initialize(this);
        LostPlayerState?.Initialize(this);
        AggressiveState?.Initialize(this);
    }

    /// <summary>
    /// Finds player when the player spawns.
    /// Initializes values for all states.
    /// </summary>
    public void FindPlayer()
    {
        PlayerTarget = 
            GameObject.FindGameObjectWithTag("playerTarget").transform;

        player = FindObjectOfType<Player>();

        InitializeStates();
    }

    /// <summary>
    /// Turns playerTarget to null when the player disappears.
    /// </summary>
    public void PlayerLost()
    {
        PlayerTarget = null;
        PlayerCurrentlyFighting = false;
    }
}
