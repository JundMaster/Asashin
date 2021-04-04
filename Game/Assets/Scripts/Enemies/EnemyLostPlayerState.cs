using UnityEngine;
using UnityEngine.AI;
using System.Collections;

/// <summary>
/// Scriptable object responsible for controlling enemy state after losing the player.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Lost Player State")]
public class EnemyLostPlayerState : EnemyStateWithVision
{
    [Header("Time the enemy will spend looking for player")]
    [SerializeField] private float timeToLookForPlayer;
    [Header("Rotation speed while looking for player")]
    [Range(1,5)][SerializeField] private float rotationSpeed;

    // Components
    private NavMeshAgent agent;

    private bool lookForPlayerCoroutine;
    private bool breakState;

    /// <summary>
    /// Runs once on start and when the player spawns.
    /// </summary>
    /// <param name="enemy">Enemy to get variables from.</param>
    public override void Initialize(Enemy enemy)
    {
        // Gets enemy target and player target
        base.Initialize(enemy);
        agent = enemy.Agent;

        lookForPlayerCoroutine = false;
        breakState = false;
    }

    public override IEnemyState Execute(Enemy enemy)
    {
        GoToLastKnownPosition(enemy);

        // If enemy is in range, it stops looking for player coroutine
        if (PlayerInRange())
        {
            lookForPlayerCoroutine = false;
            breakState = false;
            agent.isStopped = true;
            return enemy.DefenseState;
        }

        // If the enemy reached the player last known position
        // starts looking for him
        if (ReachedLastKnownPosition())
        {
            if (lookForPlayerCoroutine == false)
            {
                lookForPlayerCoroutine = true;
                enemy.StartCoroutine(LookForPlayer(enemy));
            }
        }

        // Breaks from this state back to patrol state
        if (breakState)
        {
            lookForPlayerCoroutine = false;
            breakState = false;
            return enemy.PatrolState;
        }

        return enemy.LostPlayerState;
    }

    /// <summary>
    /// Method that sets agent's path to player's last known position.
    /// </summary>
    /// <param name="enemy"></param>
    private void GoToLastKnownPosition(Enemy enemy)
    {
        if (agent.isStopped)
        {
            lookForPlayerCoroutine = false;
            breakState = false;
            agent.SetDestination(enemy.PlayerLastKnownPosition);
            agent.isStopped = false;
        }
    }

    /// <summary>
    /// Method to check if the enemy is at player's last known position.
    /// </summary>
    /// <returns>True if it is.</returns>
    private bool ReachedLastKnownPosition()
    {
        if (agent.remainingDistance < 0.1f) return true;
        return false;
    }

    /// <summary>
    /// Rotates every x seconds, looking for the player.
    /// </summary>
    /// <param name="enemy">Enemy to rotate.</param>
    /// <returns>Returns null.</returns>
    private IEnumerator LookForPlayer(Enemy enemy)
    {
        YieldInstruction wffu = new WaitForFixedUpdate();
        YieldInstruction wfs = new WaitForSeconds(Random.Range(1f, 3f));
        float timePassed = Time.time;

        float yRotationCurrent = Mathf.Abs(enemy.transform.eulerAngles.y);
        float yRotationMax = yRotationCurrent + 45f;
        float yRotationMin = Mathf.Clamp(yRotationCurrent - 45f, 0, yRotationMax);
        float multiplier = 1;

        // While the enemy can't see the player or while the time is less than
        // the time allowed searching for the player
        while (PlayerInRange() == false && 
            Time.time - timePassed < timeToLookForPlayer &&
            lookForPlayerCoroutine == true)
        {
            // Triggers rotation to right
            if (enemy.transform.eulerAngles.y >= yRotationMax)
            {
                yRotationMax = enemy.transform.eulerAngles.y;
                multiplier *= -1;
                yield return wfs;
            }

            // Triggers rotation to left
            else if (enemy.transform.eulerAngles.y <= yRotationMin)
            {
                yRotationMin = enemy.transform.eulerAngles.y;
                multiplier *= -1;
                yield return wfs;
            }

            // Rotates
            enemy.transform.eulerAngles += 
                new Vector3(0, rotationSpeed * multiplier, 0);

            yield return wffu;
        }
        breakState = true;
        lookForPlayerCoroutine = false;
    }
}
