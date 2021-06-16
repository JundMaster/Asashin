using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Shinobi Ranged State")]
public sealed class EnemyShinobiRangedState : EnemyBossAbstractState
{
    [Header("Minimum distance to be close to player")]
    [Range(1, 10)] [SerializeField] private float closeToPlayerRange;

    [Header("Kunai to spawn")]
    [SerializeField] private GameObject kunai;

    [Header("Kunai attack spawn delay")]
    [Range(1f, 10f)] [SerializeField] private float kunaiDelay;
    [Header("Waits this time in order to spawn kunai in the right time")]
    [Range(0f, 5f)] [SerializeField] private float kunaiSpawnAfterAnimation;
    private IEnumerator kunaiCoroutine;
    private const byte KUNAILAYER = 15;
    private float usedKunaiTime;

    [Range(1f, 20f)][Header("Max time to stay on this state")]
    [SerializeField] private float maxTimeToChangeState;
    private float timeWhileOnCurrentState;
    private int minionsAlive;

    /// <summary>
    /// Runs every time the enemy enters this state. Registers to event.
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();

        kunaiCoroutine = null;
        usedKunaiTime = Time.time;
        timeWhileOnCurrentState = Time.time;
        minionsAlive = enemy.SpawnedMinions.Length;

        foreach (GameObject minion in enemy.SpawnedMinions)
        {
            EnemyBase minionSpawned = 
                minion.GetComponentInChildren<EnemyBase>();

            minionSpawned.Die += () => minionsAlive--;
        }
    }

    /// <summary>
    /// Runs on update.
    /// </summary>
    /// <returns></returns>
    public override IState Update()
    {
        base.Update();

        if (die)
            return enemy.DeathState;

        // If counter reaches its limit, all minions will die and boss will
        // subsequently go back to agressive state.
        if (Time.time - timeWhileOnCurrentState > maxTimeToChangeState)
        {
            for (int i = 0; i < enemy.SpawnedMinions.Length; i++)
            {
                if (enemy.SpawnedMinions[i] != null)
                {
                    EnemySimple minionSpawned =
                        enemy.SpawnedMinions[i].
                        GetComponentInChildren<EnemySimple>();

                    if (minionSpawned != null)
                        minionSpawned.OnInstanteDeath();
                }

                if (i == enemy.SpawnedMinions.Length - 1)
                    return enemy.AggressiveState;
            }
        }

        if (minionsAlive == 0)
            return enemy.AggressiveState;

        enemy.transform.RotateTo(playerTarget.position);

        float currentDistanceFromPlayer =
            Vector3.Distance(playerTarget.position, myTarget.position);

        // If the player is close to the enemy, the enemy will teleport
        if (IsCloseToPlayer(currentDistanceFromPlayer) && teleportBoss == null)
        {
            // If throwing a kunai, it will stop throwing
            if (kunaiCoroutine != null)
            {
                enemy.StopCoroutine(kunaiCoroutine);
                kunaiCoroutine = null;
            }

            usedKunaiTime = Time.time;

            // If it can teleport the boss, it will teleport him
            if (teleportBoss == null)
            {
                teleportBoss = TeleportBoss();
                enemy.StartCoroutine(teleportBoss);
            }
        }

        // If kunai delay is over, the enemy will throw a kunai
        if (Time.time - usedKunaiTime > kunaiDelay && kunaiCoroutine == null)
        {
            kunaiCoroutine = ThrowKunaiCoroutine();
            enemy.StartCoroutine(kunaiCoroutine);
            usedKunaiTime = Time.time;
        }

        return enemy.RangedState;
    }

    /// <summary>
    /// Happens once when the enemy leaves this state. Unregisters from events.
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();

        // If throwing a kunai, it will stop throwing
        if (kunaiCoroutine != null)
        {
            enemy.StopCoroutine(kunaiCoroutine);
            kunaiCoroutine = null;
        }

        foreach (GameObject minion in enemy.SpawnedMinions)
        {
            EnemySimple minionSpawned = minion.GetComponent<EnemySimple>();
            if (minionSpawned != null)
                minionSpawned.Die -= () => minionsAlive--;
        }
    }

    /// <summary>
    /// Throws a kunai towards the player future position.
    /// </summary>
    private IEnumerator ThrowKunaiCoroutine()
    {
        YieldInstruction wfks = new WaitForSeconds(kunaiSpawnAfterAnimation);

        anim.SetTrigger("ThrowKunai");

        // Waits this time in order to spawn kunai in the right time
        // inside the kunai animation
        yield return wfks;

        // Spawns a kunai
        GameObject thisKunai = Instantiate(
            kunai,
            myTarget.position + myTarget.forward,
            Quaternion.identity);

        // Sets layer and parent enemy of the kunai
        thisKunai.layer = KUNAILAYER;
        thisKunai.GetComponent<Kunai>().Behaviour.ParentEnemy = enemy;

        kunaiCoroutine = null;
    }

    /// <summary>
    /// Moves towards the player.
    /// </summary>
    /// <param name="distance">Distance from player.</param>
    /// <returns>Returns true if it's near the player.
    /// Returns false if it's still moving towards the player.</returns>
    protected override bool IsCloseToPlayer(float distance)
    {
        // If the enemy is not close to the player
        if (distance > closeToPlayerRange)
            return false;

        // Else if the enemy is close to the player
        return true;
    }
}
