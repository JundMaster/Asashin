using UnityEngine;
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

    private bool lookForPlayerCoroutine;
    private bool breakState;

    /// <summary>
    /// Happens once when this state is enabled.
    /// Sets a path to player's last known position.
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        lookForPlayerCoroutine = false;
        breakState = false;
        agent.isStopped = false;

        agent.SetDestination(enemy.PlayerLastKnownPosition);
    }

    /// <summary>
    /// Runs on fixed update.
    /// Moves the enemy towards player's last known position. When the enemy
    /// reaches that position, it starts a coroutine to look for the player.
    /// </summary>
    /// <returns>An IState.</returns>
    public override IState FixedUpdate()
    {
        if (playerTarget == null) playerTarget = enemy.PlayerTarget;

        // If enemy is in range, it stops looking for player coroutine
        if (PlayerInRange())
            return enemy.DefenseState;

        // If the enemy reached the player last known position
        // starts looking for him
        if (ReachedLastKnownPosition())
        {
            if (lookForPlayerCoroutine == false)
            {
                lookForPlayerCoroutine = true;
                enemy.StartCoroutine(LookForPlayer());
            }
        }

        // Breaks from this state back to patrol state
        // Happens if the enemy didn't find the player
        if (breakState)
            return enemy.PatrolState;

        return enemy.LostPlayerState;
    }

    /// <summary>
    /// Happens when leaving this state.
    /// 
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();
        // Cancels rotation coroutine
        lookForPlayerCoroutine = false;
        agent.isStopped = false;
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
    /// <returns>Returns null.</returns>
    private IEnumerator LookForPlayer()
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
