using System.Collections;
using UnityEngine;

/// <summary>
/// Scriptable object responsible for handing shinobi's reinforcements state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Shinobi Reinforcements State")]
public sealed class EnemyShinobiReinforcementsState : EnemyBossAbstractState
{
    [Header("Time to wait for animation to end")]
    [Range(0.01f, 1)] [SerializeField] private float smokeGrenadeAnimationTime;

    private Transform[] limitPositions;
    private GameObject[] minionsPrefabs;

    private byte timesEnteredThisState;
    private bool breakState;

    /// <summary>
    /// Runs once on start. Gets enemy variables.
    /// </summary>
    public override void Start()
    {
        base.Start();
        limitPositions = enemy.Corners;
        minionsPrefabs = enemy.MinionsPrefabs;
        timesEnteredThisState = 0;
        
    }

    /// <summary>
    /// Runs every time the enemy enters this state. Starts a coroutine to
    /// spawn minions and teleport the boss.
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        breakState = false;

        //enemy.SpawnedMinions = new GameObject[++timesEnteredThisState];
        enemy.SpawnedMinions = new GameObject[50];

        enemy.StartCoroutine(EnteredState());
    }

    /// <summary>
    /// Runs on fixed update.
    /// </summary>
    /// <returns>IState.</returns>
    public override IState FixedUpdate()
    {
        base.FixedUpdate();

        if (die)
            return enemy.DeathState;

        if (breakState)
            return enemy.RangedState;

        return enemy.ReinforcementsState;
    }

    /// <summary>
    /// Spawns minions and teleports the boss to a random position inside its 
    /// area.
    /// </summary>
    private IEnumerator EnteredState()
    {
        YieldInstruction wffu = new WaitForFixedUpdate();

        agent.SetDestination(myTarget.position);

        Vector2 spawnPos;
        
        // Spawns minions and sets them as boss' minions
        for (int i = 0; i < 50; i++)
        {
            spawnPos = Custom.RandomPositionInSquare(limitPositions);

            byte enemyPrefab;
            if (i % 2 == 0) enemyPrefab = 0;
            else enemyPrefab = 1;

            enemy.SpawnedMinions[i] = Instantiate(
            minionsPrefabs[enemyPrefab], new Vector3(spawnPos.x, 0, spawnPos.y),
            enemy.transform.RotateTo(playerTarget.position));
        }

        yield return wffu;
        enemy.AlertSurroundings();

        enemy.CineTarget.CancelCurrentTarget();

        // Teleports boss enemy to a random position and stops him
        spawnPos = Custom.RandomPositionInSquare(limitPositions);
        enemy.transform.position = new Vector3(spawnPos.x, 0, spawnPos.y);
        agent.SetDestination(myTarget.position);

        breakState = true;
    }
}
