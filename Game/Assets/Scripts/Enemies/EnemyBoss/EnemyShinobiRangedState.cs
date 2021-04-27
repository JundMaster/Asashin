using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Shinobi Ranged State")]
public sealed class EnemyShinobiRangedState : EnemyBossAbstractState
{
    [Header("Minimum distance to be close to player")]
    [Range(1, 10)] [SerializeField] private float closeToPlayerRange;

    [Header("Time to wait for animation to end")]
    [Range(0.01f, 1)] [SerializeField] private float smokeGrenadeAnimationTime;
    private Transform[] limitPositions;
    private IEnumerator teleportEnemy;

    [Header("Kunai to spawn")]
    [SerializeField] private GameObject kunai;

    [Header("Kunai attack spawn delay")]
    [Range(1f, 10f)] [SerializeField] private float kunaiDelay;
    [Header("Waits this time in order to spawn kunai in the right time")]
    [Range(0f, 5f)] [SerializeField] private float kunaiSpawnAfterAnimation;
    private IEnumerator kunaiCoroutine;
    private const byte KUNAILAYER = 15;
    private float usedKunaiTime;

    private int minionsAlive;

    /// <summary>
    /// Runs once on start. Gets enemy variables.
    /// </summary>
    public override void Start()
    {
        base.Start();
        limitPositions = enemy.Corners;
    }

    /// <summary>
    /// Runs every time the enemy enters this state. Registers to event.
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();

        kunaiCoroutine = null;
        usedKunaiTime = Time.time;
        minionsAlive = enemy.SpawnedMinions.Length;

        foreach (GameObject minion in enemy.SpawnedMinions)
        {
            EnemyBase minionSpawned = 
                minion.GetComponentInChildren<EnemyBase>();

            minionSpawned.Die += () => minionsAlive--;
        }
    }

    /// <summary>
    /// Runs on fixed update.
    /// </summary>
    /// <returns></returns>
    public override IState FixedUpdate()
    {
        base.FixedUpdate();

        if (die)
            return enemy.DeathState;

        if (minionsAlive == 0)
            return enemy.AggressiveState;

        enemy.transform.RotateTo(playerTarget.position);

        float currentDistanceFromPlayer =
            Vector3.Distance(playerTarget.position, myTarget.position);

        // If the player is close to the enemy, the enemy will teleport
        if (IsCloseToPlayer(currentDistanceFromPlayer) && teleportEnemy == null)
        {
            if (kunaiCoroutine != null)
            {
                enemy.StopCoroutine(kunaiCoroutine);
                kunaiCoroutine = null;
            }

            usedKunaiTime = Time.time;
            teleportEnemy = TeleportEnemy();
            enemy.StartCoroutine(teleportEnemy);
        }

        // If kunai delat is over, the enemy will throw a kunai
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

        foreach (GameObject minion in enemy.SpawnedMinions)
        {
            EnemySimple minionSpawned = minion.GetComponent<EnemySimple>();
            if (minionSpawned != null)
                minionSpawned.Die -= () => minionsAlive--;
        }
    }

    /// <summary>
    /// Teleports enemy to a random position inside its area.
    /// </summary>
    /// <returns></returns>
    private IEnumerator TeleportEnemy()
    {
        YieldInstruction wfs = new WaitForSeconds(2);

        enemy.CineTarget.CancelCurrentTarget();

        agent.SetDestination(myTarget.position);
        agent.enabled = false;

        // Makes the enemy disappear for wfs
        enemy.transform.position = new Vector3(100000, 100000, 100000);
        yield return wfs;

        // Teleports enemy to a random position and stops him
        Vector2 teleportTo = Custom.RandomPositionInSquare(limitPositions);
        enemy.transform.position = new Vector3(teleportTo.x, 0, teleportTo.y);
        agent.enabled = true;
        agent.SetDestination(myTarget.position);

        teleportEnemy = null;
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
