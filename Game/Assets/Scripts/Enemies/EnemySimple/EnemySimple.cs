using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for handling a simple enemy.
/// </summary>
public class EnemySimple : EnemyBase, IHearSound
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
    [SerializeField] private GameObject visionConeGameObject;
    public GameObject VisionConeGameObject => visionConeGameObject;

    [Header("Options")]
    [SerializeField] private OptionsScriptableObj options;
    public OptionsScriptableObj Options => options;

    [Header("Punctuation Marks")]
    [SerializeField] private GameObject interrogationMark;
    [SerializeField] private GameObject exclamationMark;
    public GameObject InterrogationMark => interrogationMark;
    public GameObject ExclamationMark => exclamationMark;

    /// <summary>
    /// Get setter for current reaction of the enemy, meaning he's been hit or 
    /// is following a sound.
    /// </summary>
    public TypeOfReaction CurrentReaction { get; set; } 

    public Vector3 PositionOfSoundListened { get; set; }

    public VisionCone VisionConeScript { get; set; }

    private MusicControlAndTransition combatMusic;

    private bool inCombat;

    /// <summary>
    /// Needs to switch music here because it's better than the script
    /// finding all enemies.
    /// </summary>
    public bool InCombat
    {
        get => inCombat;
        set
        {
            inCombat = value;
            if (inCombat == false)
                if (combatMusic != null)
                    combatMusic.SwitchToBackgroundMusic();
        }
    }

    /// <summary>
    /// Instantiates states.
    /// </summary>
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
    private IEnumerator Start()
    {
        CurrentReaction = TypeOfReaction.None;
        PositionOfSoundListened = transform.position;

        yield return new WaitForFixedUpdate();
        // Finds combat music after singleton destroys the one that exists
        // on this scene
        combatMusic = FindObjectOfType<MusicControlAndTransition>();

        // Initializes states after options singleton takes effect
        stateMachine?.Initialize();
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
    public void OnAlert() => 
        Alert?.Invoke();

    /// <summary>
    /// Invokes CollisionWithPlayer event.
    /// </summary>
    private void OnCollisionWithPlayer() => 
        CollisionWithPlayer?.Invoke();

    /// <summary>
    /// Invokes InstantDeath event.
    /// </summary>
    public void OnInstanteDeath() =>
        InstantDeath?.Invoke();

    /// <summary>
    /// Invokes ReactToSound event. 
    /// Called by gameobjects with IDoSound interfaces.
    /// </summary>
    /// <param name="positionOfSound">Position of the sound.</param>
    public void OnReactToSound(Vector3 positionOfSound) =>
        ReactToSound?.Invoke(positionOfSound);

    /// <summary>
    /// Happens when the enemy collides with player.
    /// Event registered on PatrolState and LostPlayerState.
    /// Event registered on Tutorial.
    /// </summary>
    public event Action CollisionWithPlayer;

    /// <summary>
    /// Event registered on enemy states in order to alert all enemies.
    /// </summary>
    public event Action Alert;

    /// <summary>
    /// Triggered when PlayerMeleeAttack performs stealth kill.
    /// Event registered by enemy abstract state.
    /// </summary>
    public event Action InstantDeath;

    /// <summary>
    /// Event triggered by enemy states that react to sound.
    /// </summary>
    public event Action<Vector3> ReactToSound;

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
