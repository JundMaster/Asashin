using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class responsible for handling enemy script.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
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

    // State variables
    [SerializeField] private EnemyPatrolState patrolStateOriginal;
    [SerializeField] private EnemyDefenseState defenseStateOriginal;
    public IEnemyState PatrolState { get; private set; }
    public IEnemyState DefenseState { get; private set; }
    private IEnemyState currentState;
    private IEnumerable<IEnemyState> allStates;

    // Components
    public NavMeshAgent Agent { get; private set; }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();

        PatrolState = Instantiate(patrolStateOriginal);
        DefenseState = Instantiate(defenseStateOriginal);
        allStates = new List<IEnemyState>
        {
            PatrolState,
            DefenseState,
        }; 
    }

    private void Start()
    {
        foreach (IEnemyState state in allStates)
            state.Initialize(this);

        currentState = PatrolState;
    }

    private void FixedUpdate()
    {
        currentState = currentState.Execute(this);
    }

    /// <summary>
    /// Finds player target transform when the player spawns.
    /// </summary>
    public void FindPlayer()
    {
        PlayerTarget = GameObject.FindGameObjectWithTag("playerTarget").transform;
    }

    /// <summary>
    /// Turns playerTarget to null when the player disappears.
    /// </summary>
    public void PlayerLost()
    {
        PlayerTarget = null;
    }





    // TEMP
    private IEnumerator ThrowKunaiTemporary()
    {
        yield return new WaitForSeconds(1f);
        /*
        yield return new WaitForSeconds(1f);
        Player player = FindObjectOfType<Player>();
        EnemyVisionCone cone = GetComponent<EnemyVisionCone>();
        while (player != null)
        {
            if (kunai && cone.Performing)
            {
                GameObject thisKunai = Instantiate(kunai, MyTarget.position + transform.forward, Quaternion.identity);
                thisKunai.layer = 15;
                thisKunai.GetComponent<Kunai>().Behaviour.ParentEnemy = this;
            }
            yield return new WaitForSeconds(2.5f);
        }
        */
    }
}
