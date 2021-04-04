using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Scriptable object responsible for controlling enemy movement state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Senshi Defense State")]
public class EnemySenshiDefenseState : EnemyStateWithVision
{
    [Header("Kunai to spawn")]
    [SerializeField] private GameObject kunai;

    [Header("Kunai spawn delay")]
    [SerializeField] private float kunaiDelay;

    [Header("Rotation smooth time")]
    [SerializeField] private float turnSmooth;
    private float smoothTimeRotation;

    /// <summary>
    /// Runs once on start and when the player spawns.
    /// </summary>
    /// <param name="enemy">Enemy to get variables from.</param>
    public override void Initialize(Enemy enemy)
    {
        // Gets enemy target and player target
        base.Initialize(enemy);
    }

    public override IEnemyState Execute(Enemy enemy)
    {
        RotateEnemy(enemy.transform);

        // Search for player every searchCheckDelay seconds inside a vision cone
        if (Time.time - lastTimeChecked > kunaiDelay)
        {
            // If it found the player throws a kunai
            if (PlayerInRange())
            {
                ThrowKunai(enemy);
            }
            else
            {
                enemy.PlayerLastKnownPosition = playerTarget.position;
                return enemy.LostPlayerState;
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

    private void RotateEnemy(Transform enemy)
    {
        // Rotates the enemy towards the player
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
