using UnityEngine;
using System.Collections;

/// <summary>
/// Scriptable object responsible for controlling enemy state after losing the player.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Common Lost Player State")]
public class EnemyLostPlayerState : EnemyAbstractStateWithVision
{
    [Header("Time the enemy will spend looking for player")]
    [Range(0.1f,15)][SerializeField] private float timeToLookForPlayer;
    [Header("Rotation speed while looking for player")]
    [Range(0.1f,1)][SerializeField] private float rotationSpeed;

    // State variables
    private IEnumerator lookForPlayerCoroutine;
    private bool breakState;

    // Components
    private VisionCone visionCone;

    /// <summary>
    /// Runs once on start. Sets vision cone to be the same as the enemy's one.
    /// </summary>
    public override void Start()
    {
        base.Start();
        visionCone = enemy.EnemyVisionCone;
    }

    /// <summary>
    /// Happens once when this state is enabled.
    /// Sets a path to player's last known position.
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();

        lookForPlayerCoroutine = null;

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
        base.FixedUpdate();

        if (instantKill)
            return enemy.DeathState;

        if (alert)
            return enemy.DefenseState;

        // If enemy is in range, it stops looking for player coroutine
        if (PlayerInRange())
            return enemy.DefenseState ?? enemy.PatrolState;

        // If the enemy reached the player last known position
        // starts looking for him
        if (ReachedLastKnownPosition())
        {
            if (lookForPlayerCoroutine == null)
            {
                lookForPlayerCoroutine = LookForPlayer();
                enemy.StartCoroutine(lookForPlayerCoroutine);
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
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();

        // Resets variables
        breakState = false;
        if (lookForPlayerCoroutine != null)
        {
            enemy.StopCoroutine(lookForPlayerCoroutine);
            lookForPlayerCoroutine = null;
        }

        agent.isStopped = false;
        enemy.VisionCone.SetActive(false);
    }


    /// <summary>
    /// Method to check if the enemy is at player's last known position.
    /// </summary>
    /// <returns>True if it is.</returns>
    private bool ReachedLastKnownPosition()
    {
        if (agent.remainingDistance < 1f) return true;
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

        float yRotationCurrent = enemy.transform.eulerAngles.y;
        float yRotationMax = Mathf.Clamp(yRotationCurrent + 45f, 1, 359);
        float yRotationMin = Mathf.Clamp(yRotationCurrent - 45f, 1, 359);
        float multiplier = 1;

        // While the enemy can't see the player or while the time is less than
        // the time allowed searching for the player.
        // As soon as the enemy leaves this state, is this coroutine is turned
        // to false.
        while (PlayerInRange() == false && 
            Time.time - timePassed < timeToLookForPlayer)
        {
            // Triggers rotation to right
            if (enemy.transform.eulerAngles.y >= yRotationMax)
            {
                yRotationMax = 
                    Mathf.Clamp(enemy.transform.eulerAngles.y, 1, 359);
                multiplier *= -1;
                yield return wfs;
            }

            // Triggers rotation to left
            else if (enemy.transform.eulerAngles.y <= yRotationMin)
            {
                yRotationMin =
                    Mathf.Clamp(enemy.transform.eulerAngles.y, 1, 359);
                multiplier *= -1;
                yield return wfs;
            }

            // Rotates
            enemy.transform.eulerAngles += 
                new Vector3(0, rotationSpeed * multiplier, 0);

            // Activates and calculates vision cone
            if (enemy.VisionCone.activeSelf == false)
                enemy.VisionCone.SetActive(true);

            visionCone.Calculate();

            yield return wffu;
        }
        breakState = true;
    }
}
