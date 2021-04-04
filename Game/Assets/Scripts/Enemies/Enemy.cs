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

    [Header("Enemy Patrol path")]
    [SerializeField] private Transform[] patrolPoints;
    public Transform[] PatrolPoints => patrolPoints;

    // Player variables
    public Transform PlayerTarget { get; private set; }
    public Vector3 PlayerLastKnownPosition { get; set; }

    // State variables
    [SerializeField] private EnemyState patrolStateOriginal;
    [SerializeField] private EnemyState defenseStateOriginal;
    [SerializeField] private EnemyState lostPlayerStateOriginal;

    public IEnemyState PatrolState { get; private set; }
    public IEnemyState DefenseState { get; private set; }
    public IEnemyState LostPlayerState { get; private set; }
    private IEnemyState currentBehaviourState;

    // Components
    public NavMeshAgent Agent { get; private set; }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();

        PatrolState = Instantiate(patrolStateOriginal);
        DefenseState = Instantiate(defenseStateOriginal);
        LostPlayerState = Instantiate(lostPlayerStateOriginal);
    }

    private void Start()
    {
        // States are also initialized once the player spawns.
        InitializeStates();

        currentBehaviourState = PatrolState;
    }

    private void FixedUpdate()
    {
        currentBehaviourState = currentBehaviourState.Execute(this);
    }

    /// <summary>
    /// Spawns all states.
    /// </summary>
    private void InitializeStates()
    {
        PatrolState.Initialize(this);
        DefenseState.Initialize(this);
        LostPlayerState.Initialize(this);
    }

    /// <summary>
    /// Finds player target transform when the player spawns.
    /// </summary>
    public void FindPlayer()
    {
        PlayerTarget = 
            GameObject.FindGameObjectWithTag("playerTarget").transform;

        InitializeStates();
    }

    /// <summary>
    /// Turns playerTarget to null when the player disappears.
    /// </summary>
    public void PlayerLost()
    {
        PlayerTarget = null;
    }
}
