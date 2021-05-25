using System.Collections;
using UnityEngine;

/// <summary>
/// Abstract Scriptable object responsible for controlling boss enemy states.
/// </summary>
public abstract class EnemyBossAbstractState : EnemyAbstractState
{
    [Header("Boss vanish smoke")]
    [SerializeField] private GameObject smokePrefab;

    [Header("Time to wait on teleport")]
    [Range(0.1f, 3f)][SerializeField] private float timeToWait;
    protected YieldInstruction wfs;

    protected Transform[] limitPositions;
    protected IEnumerator teleportBoss;

    protected EnemyBoss enemy;

    /// <summary>
    /// Method that defines what happens when this state is initialized.
    /// </summary>
    /// <param name="en">Parent object of this state.</param>
    public override void Initialize(object en)
    {
        base.Initialize(en);
        enemy = en as EnemyBoss;
    }

    /// <summary>
    /// Runs once on start. Limits boss area.
    /// </summary>
    public override void Start()
    {
        base.Start();
        limitPositions = enemy.Corners;
        wfs = new WaitForSeconds(timeToWait);
    }

    /// <summary>
    /// Runs once on exit. Sets teleportBoss IEnumerator to null.
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();
        teleportBoss = null;
    }

    /// <summary>
    /// Checks if the enemy is close to the player.
    /// </summary>
    /// <param name="distance">Distance from player.</param>
    /// <returns>Returns true if it's near the player.</returns>
    protected virtual bool IsCloseToPlayer(float distance) => false;

    /// <summary>
    /// Makes boss vanish, waits for seconds, teleports boss to a random 
    /// position inside its area. Alerts enemies.
    /// </summary>
    /// <returns>Returns wait for seconds.</returns>
    protected IEnumerator TeleportBoss()
    {
        enemy.CineTarget.AutomaticallyFindTargetCall(100f);

        agent.SetDestination(myTarget.position);
        agent.enabled = false;

        Vector3 offset = new Vector3(0, 0.5f, 0);
        Instantiate(
            smokePrefab, enemy.transform.position + offset, Quaternion.identity);

        // Makes the enemy disappear for wfs
        enemy.transform.position = new Vector3(100000, enemy.transform.position.y, 100000);
        yield return wfs;

        // Teleports enemy to a random position and stops him
        Vector2 teleportTo = Custom.RandomPositionInSquare(limitPositions);
        enemy.transform.position = new Vector3(teleportTo.x, 0, teleportTo.y);
        agent.enabled = true;
        agent.SetDestination(myTarget.position);
        enemy.AlertSurroundings();

        teleportBoss = null;
    }
}
