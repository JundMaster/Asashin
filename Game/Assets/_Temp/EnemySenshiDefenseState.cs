using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Scriptable object responsible for controlling enemy movement state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Senshi Defense State")]
public class EnemySenshiDefenseState : EnemyDefenseState, IEnemyState
{
    [Header("Kunai to spawn")]
    [SerializeField] private GameObject kunai;

    [Header("Checks if player is in cone range every X seconds")]
    [SerializeField] private float searchCheckDelay;

    [Header("Sphere range to look for the player")]
    [SerializeField] private float sphereRange;
    [SerializeField] private LayerMask playerLayer;

    private NavMeshAgent agent;
    private Transform playerTarget;
    private Transform myTarget;

    private float lastTimeSearch;

    /// <summary>
    /// Runs once on start.
    /// </summary>
    /// <param name="enemy">Enemy to get variables from.</param>
    public override void Initialize(Enemy enemy)
    {
        agent = enemy.Agent;
        myTarget = enemy.MyTarget;
        playerTarget = enemy.PlayerTarget;
    }

    public override IEnemyState Execute(Enemy enemy)
    {
        if (playerTarget == null) playerTarget = enemy.PlayerTarget;


        // Search for player every searchCheckDelay seconds inside a vision cone
        if (Time.time - lastTimeSearch > searchCheckDelay)
        {
            // If it found the player throws a kunai
            if (SearchForPlayer())
            {
                ThrowKunai(enemy); ;
            }
        }

        return enemy.DefenseState;
    }

    /// <summary>
    /// Throws a kunai towards the player future position.
    /// </summary>
    /// <param name="enemy">This enemy (kunai's enemy parent).</param>
    private void ThrowKunai(Enemy enemy)
    {
        if (playerTarget != null)
        {
            // Spawns a kunai
            GameObject thisKunai = Instantiate(
                kunai, 
                myTarget.position + myTarget.forward, 
                Quaternion.identity);

            // Sets layer and parent enemy of the kunai
            thisKunai.layer = 15;
            thisKunai.GetComponent<Kunai>().Behaviour.ParentEnemy = enemy;
        }
    }

    /// <summary>
    /// Search for player every searchCheckDelay seconds inside a sphere.
    /// </summary>
    private bool SearchForPlayer()
    {
        bool playerFound = false;

        Collider[] playerCollider =
            Physics.OverlapSphere(myTarget.position, sphereRange, playerLayer);

        // If player is in this collider
        if (playerCollider.Length > 0)
        {
            playerFound = true;
        }

        lastTimeSearch = Time.time;

        return playerFound;
    }
}
