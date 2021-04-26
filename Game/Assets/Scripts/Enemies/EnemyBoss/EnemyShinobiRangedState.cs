using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Shinobi Ranged State")]
public sealed class EnemyShinobiRangedState : EnemyBossAbstractState
{
    [Header("Minimum distance to be close to player")]
    [Range(0.01f, 5)] [SerializeField] private float closeToPlayerRange;

    [Header("Time to wait for animation to end")]
    [Range(0.01f, 1)] [SerializeField] private float smokeGrenadeAnimationTime;
    private Transform[] limitPositions;

    /// <summary>
    /// Runs once on start. Gets enemy variables.
    /// </summary>
    public override void Start()
    {
        base.Start();
        limitPositions = enemy.Corners;
    }

    /// <summary>
    /// Runs on fixed update.
    /// </summary>
    /// <returns></returns>
    public override IState FixedUpdate()
    {
        base.FixedUpdate();

        Debug.Log(enemy.SpawnedMinions[1]);

        if (die)
            return enemy.DeathState;

        float currentDistanceFromPlayer =
            Vector3.Distance(playerTarget.position, myTarget.position);

        return enemy.RangedState;
    }

    private IEnumerator TeleportEnemy()
    {
        YieldInstruction wffu = new WaitForFixedUpdate();

        agent.SetDestination(myTarget.position);
        anim.SetTrigger("SmokeGrenade");

        // Waits for the current animation before smoke grenade to pass
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
        {
            yield return wffu;
        }
        // Waits for the current animation snome grenade to end
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime <
            smokeGrenadeAnimationTime)
        {
            yield return wffu;
        }

        // Teleports enemy to a random position and stops him
        Vector2 teleportTo = Custom.RandomPlanePosition(limitPositions);
        enemy.transform.position = new Vector3(teleportTo.x, 0, teleportTo.y);
        agent.SetDestination(myTarget.position);
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
