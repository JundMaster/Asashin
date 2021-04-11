using UnityEngine;
using System.Collections;

/// <summary>
/// Scriptable object for controlling enemy aggressive state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Aggressive State")]
public class EnemyAggressiveState : EnemyState
{
    [Header("Player check ranges")]
    [Range(1,30)][SerializeField] private float checkForPlayerRange;
    [Range(1f, 3)][SerializeField] private float closeToPlayerRange;
    [Tooltip("Distance to stay from player while atacking")]
    [Range(1f, 1.5f)][SerializeField] private float distanceFromPlayer;
    [SerializeField] private LayerMask playerLayer;

    private float currentDistanceFromPlayer;

    public override void OnEnter()
    {
        base.OnEnter();

        agent.isStopped = false;
        if (playerTarget != null ) 
            agent.SetDestination(playerTarget.position);

        enemy.PlayerCurrentlyFighting = true;

        stats.TookDamage += TakeImpact;
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
        enemy.PlayerCurrentlyFighting = false;
        agent.isStopped = false;
        stats.TookDamage -= TakeImpact;
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
        // If the enemy is not close to the player
        if (distance > closeToPlayerRange)
        {
            Vector3 dir = (myTarget.position - playerTarget.position).normalized;

            agent.SetDestination(
                playerTarget.position + dir * distanceFromPlayer);

            return false;
        }
        // Else if the enemy is close to the player
        return true;
    }

    /// <summary>
    /// Rotates enemy towards the player.
    /// </summary>
    private void RotateEnemy()
    {
        Vector3 dir = playerTarget.transform.position - myTarget.position;
        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        enemy.transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
    }

    /// <summary>
    /// Starts ImpactToBack coroutine.
    /// </summary>
    protected override void TakeImpact()
    {
        base.TakeImpact();
    }

    /// <summary>
    /// Happens after enemy being hit. Rotates enemy and pushes it back.
    /// </summary>
    /// <returns>Null.</returns>
    protected override IEnumerator ImpactToBack()
    {
        YieldInstruction wffu = new WaitForFixedUpdate();
        float timeEntered = Time.time;

        Vector3 dir =
            (playerTarget.transform.position - myTarget.position).normalized;
        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        enemy.transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

        while (Time.time - timeEntered < timeToTravelAfterHit)
        {
            agent.isStopped = true;

            enemy.transform.position +=
                -(dir) *
                Time.fixedDeltaTime *
                takeDamageDistancePower;

            yield return wffu;
        }
        agent.isStopped = false;
    }
}
