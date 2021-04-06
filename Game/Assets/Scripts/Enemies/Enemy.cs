using System.Collections.Generic;
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

    [Header("Cone Mesh")]
    [SerializeField] private GameObject visionCone;
    public GameObject VisionCone => visionCone;

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
    public IState PatrolState { get; private set; }
    public IState DefenseState { get; private set; }
    public IState LostPlayerState { get; private set; }
    public IState AggressiveState { get; private set; }


    private IEnumerable<IState> states;
    private StateMachine stateMachine;
    public StateMachine StateMachineGet => stateMachine;

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

        states = new List<IState>
        {
            PatrolState,
            DefenseState,
            LostPlayerState,
            AggressiveState,
        };

        stateMachine = new StateMachine(states, this);
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
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
