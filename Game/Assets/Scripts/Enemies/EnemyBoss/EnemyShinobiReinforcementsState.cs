using System.Collections;
using UnityEngine;

/// <summary>
/// Scriptable object responsible for handing shinobi's reinforcements state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Shinobi Reinforcements State")]
public sealed class EnemyShinobiReinforcementsState : EnemyBossAbstractState
{
    private GameObject[] minionsPrefabs;

    private byte timesEnteredThisState;
    private bool breakState;
    private IEnumerator enteredState;

    /// <summary>
    /// Runs once on start. Gets enemy variables.
    /// </summary>
    public override void Start()
    {
        base.Start();
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
       
        enemy.SpawnedMinions = new GameObject[++timesEnteredThisState];

        enteredState = EnteredState();
        enemy.StartCoroutine(enteredState);
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
    /// Runs once on exit. Stops coroutine, if playing.
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();

        if (enteredState != null)
        {
            enemy.StopCoroutine(enteredState);
            enteredState = null;
        }
    }

    /// <summary>
    /// Spawns minions and teleports the boss to a random position inside its 
    /// area.
    /// </summary>
    private IEnumerator EnteredState()
    {
        Vector2 spawnPos;
        
        // Spawns minions and sets them as boss' minions
        for (int i = 0; i < timesEnteredThisState; i++)
        {
            spawnPos = Custom.RandomPositionInSquare(limitPositions);

            byte enemyPrefab;
            if (i % 2 == 0) enemyPrefab = 0;
            else enemyPrefab = 1;

            enemy.SpawnedMinions[i] = Instantiate(
                minionsPrefabs[enemyPrefab], 
                new Vector3(spawnPos.x, 0, spawnPos.y),
                enemy.transform.RotateTo(playerTarget.position));
        }

        if (teleportBoss == null)
        {
            teleportBoss = TeleportBoss();
            enemy.StartCoroutine(teleportBoss);
        }

        yield return wfs;

        breakState = true;
    }
}
