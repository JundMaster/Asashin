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

    [Header("How many enemies to spawn")]
    [Range(0, 10)][SerializeField] private byte numberOfEnemies;

    private Transform[] limitPositions;
    private GameObject[] minionsPrefabs;

    private bool breakState;

    /// <summary>
    /// Runs once on start. Gets enemy variables.
    /// </summary>
    public override void Start()
    {
        base.Start();
        limitPositions = enemy.Corners;
        minionsPrefabs = enemy.MinionsPrefabs;
        enemy.SpawnedMinions = new GameObject[numberOfEnemies];
    }

    /// <summary>
    /// Runs every time the enemy enters this state. Starts a coroutine to
    /// spawn minions and teleport the boss.
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        breakState = false;
        enemy.StartCoroutine(EnteredStateCoroutine());
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
    /// Triggers smoke grenade animation, spawns minions and teleports the boss
    /// to a random position inside its area.
    /// </summary>
    /// <returns>Wait for fixed update.</returns>
    private IEnumerator EnteredStateCoroutine()
    {
        YieldInstruction wffu = new WaitForFixedUpdate();
        
        agent.SetDestination(myTarget.position);
        anim.SetTrigger("SmokeGrenade");

        // Waits for the current animation before smoke grenade to pass
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
        {
            yield return wffu;
        }
        // Waits for the current animation snome grenade to end
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 
            smokeGrenadeAnimationTime)
        {
            yield return wffu;
        }

        Vector2 spawnPos;
        // Spawns minions and sets them as boss' minions
        for (int i = 0; i < numberOfEnemies; i++)
        {
            spawnPos = Custom.RandomPlanePosition(limitPositions);

            byte enemyToSpawn;
            if (i % 2 == 0) enemyToSpawn = 0;
            else enemyToSpawn = 1;

            enemy.SpawnedMinions[i] = Instantiate(
            minionsPrefabs[enemyToSpawn], new Vector3(spawnPos.x, 0, spawnPos.y),
            enemy.transform.RotateTo(playerTarget.position));
        }

        // Teleports boss enemy to a random position and stops him
        spawnPos = Custom.RandomPlanePosition(limitPositions);
        enemy.transform.position = new Vector3(spawnPos.x, 0, spawnPos.y);
        agent.SetDestination(myTarget.position);

        breakState = true;
    }
}
