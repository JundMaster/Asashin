using UnityEngine;

/// <summary>
/// Scriptable object for controlling enemy aggressive state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Aggressive State")]
public class EnemyAggressiveState : EnemyState
{
    [Header("Player check ranges")]
    [Range(1,30)][SerializeField] private float checkForPlayerRange;
    [Range(0.5f, 2)] [SerializeField] private float closeToPlayerRange;
    [SerializeField] private LayerMask playerLayer;

    [Header("Rotation smooth time")]
    [Range(0.1f, 1)][SerializeField] private float turnSmooth;
    private float smoothTimeRotation;

    private float currentDistanceFromPlayer;

    public override void OnEnter()
    {
        base.OnEnter();

        agent.isStopped = false;
        if (playerTarget != null ) 
            agent.SetDestination(playerTarget.transform.position);
    }

    public override IState FixedUpdate()
    {
        base.FixedUpdate();

        currentDistanceFromPlayer =
            Vector3.Distance(playerTarget.position, myTarget.position);

        if (IsPlayerInMyRange(currentDistanceFromPlayer))
        {
            if(IsCloseToPlayer(currentDistanceFromPlayer))
            {
                RotateEnemy();
                AttackPlayer();
            }
            return enemy.AggressiveState;
        }
        return enemy.LostPlayerState;    
    }

    /// <summary>
    /// Happens once when the enemy leaves this state.
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();
        enemy.PlayerLastKnownPosition = playerTarget.position;
        enemy.PlayerCurrentlyFighting = false;
        agent.isStopped = false;
    }

    private void AttackPlayer()
    {
        Debug.Log("fighting");
    }

    /// <summary>
    /// Checks if player is in enemy's range.
    /// </summary>
    /// <param name="distance">Distance from player.</param>
    /// <returns>Returns true if player is still in enemy's range,
    /// else returns false.</returns>
    private bool IsPlayerInMyRange(float distance)
    {
        if (distance <= checkForPlayerRange)
            return true;
 
        return false;
    }

    /// <summary>
    /// Moves towards the player.
    /// </summary>
    /// <param name="distance">Distance from player.</param>
    /// <returns>Returns true if it's near the player.
    /// Returns false if it's still moving towards the player.</returns>
    private bool IsCloseToPlayer(float distance)
    {
        bool nearPlayer;

        // If the enemy is not close to the player
        if (distance >= closeToPlayerRange)
        {
            enemy.PlayerCurrentlyFighting = true;
            agent.isStopped = false;

            agent.SetDestination(playerTarget.position);

            nearPlayer = false;
        }
        // Else if the enemy is close to the player
        else
        {
            agent.SetDestination(myTarget.position);
            agent.isStopped = true;
            nearPlayer = true;
        }

        return nearPlayer;
    }

    /// <summary>
    /// Rotates enemy towards the player.
    /// </summary>
    private void RotateEnemy()
    {
        Vector3 dir = playerTarget.transform.position - myTarget.position;
        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(
                enemy.transform.eulerAngles.y,
                targetAngle,
                ref smoothTimeRotation,
                turnSmooth);
        enemy.transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
}
