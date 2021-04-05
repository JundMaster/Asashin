using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Scriptable object for controlling enemy aggressive state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Aggressive State")]
public class EnemyAggressiveState : EnemyState
{
    [SerializeField] private float sphereRange;
    [SerializeField] private LayerMask playerLayer;

    // Components
    private NavMeshAgent agent;
    private Transform myTarget;
    private Transform playerTarget;

    public override void Initialize(Enemy enemy)
    {
        agent = enemy.Agent;
        myTarget = enemy.MyTarget;
        playerTarget = enemy.PlayerTarget;
    }

    public override IEnemyState Execute(Enemy enemy)
    {
        if (ApproachPlayer(enemy))
        {
            //Debug.Log("fighting");
        }
        else
        {
            // Else it chases the player
        }

        return enemy.AggressiveState;
    }

    /// <summary>
    /// Moves towards the player.
    /// </summary>
    /// <param name="enemy">Enemy to approach.</param>
    /// <returns>Returns true if it's near the player.
    /// Returns false if it's still moving towards the player.</returns>
    private bool ApproachPlayer(Enemy enemy)
    {
        bool nearPlayer = true;

        //Collider[] playerCollider = null;
        Collider[] playerCollider =
            Physics.OverlapSphere(myTarget.position, sphereRange, playerLayer);

        // Only if it's close to the player
        if (playerCollider.Length == 0)
        {
            enemy.PlayerCurrentlyFighting = true;
            agent.isStopped = false;
            
            agent.SetDestination(playerTarget.transform.position);
            nearPlayer = false;
        }

        return nearPlayer;
    }
}
