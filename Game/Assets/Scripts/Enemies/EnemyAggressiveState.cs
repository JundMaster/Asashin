using UnityEngine;

/// <summary>
/// Scriptable object for controlling enemy aggressive state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Aggressive State")]
public class EnemyAggressiveState : EnemyState
{
    [SerializeField] private float sphereRange;
    [SerializeField] private LayerMask playerLayer;


    public override void OnEnter()
    {
        base.OnEnter();

        if (playerTarget == null) playerTarget = enemy.PlayerTarget;
        if (playerTarget != null ) 
            agent.SetDestination(playerTarget.transform.position);
    }

    public override IState FixedUpdate()
    {
        if (playerTarget == null) playerTarget = enemy.PlayerTarget;

        if (ApproachPlayer())
        {
            Debug.Log("fighting");
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
    /// <returns>Returns true if it's near the player.
    /// Returns false if it's still moving towards the player.</returns>
    private bool ApproachPlayer()
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
