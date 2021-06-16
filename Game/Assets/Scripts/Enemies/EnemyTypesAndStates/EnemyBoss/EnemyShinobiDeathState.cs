using UnityEngine;
using System.Collections;

/// <summary>
/// Scriptable object responsible for handling shinobi's death state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Shinobi Death State")]
public sealed class EnemyShinobiDeathState : EnemyBossAbstractState
{
    public override void OnEnter()
    {
        // Variables to make sure the is no collision while the enemy is dying
        enemy.GetComponent<CapsuleCollider>().enabled = false;
        enemy.gameObject.layer = 0; // Changes to default layer
        agent.isStopped = true;

        for (int i = 0; i < enemy.SpawnedMinions.Length; i++)
        {
            if (enemy.SpawnedMinions[i] != null)
            {
                EnemySimple minionSpawned =
                    enemy.SpawnedMinions[i].
                    GetComponentInChildren<EnemySimple>();

                if (minionSpawned != null)
                    minionSpawned.OnInstanteDeath();
            }
        }

        enemy.StopAllCoroutines();
        enemy.StartCoroutine(Die());
    }

    /// <summary>
    /// Happens when the enemy dies. Cancels cinemachine target, spawns loot
    /// and destroys the enemy.
    /// </summary>
    /// <returns>Wait for fixed update.</returns>
    private IEnumerator Die()
    {
        SpawnSmokeOnBoss();

        yield return new WaitForFixedUpdate();

        enemy.transform.position = new Vector3(
            enemy.CenterPosition.position.x, 
            0,
            enemy.CenterPosition.position.z);

        SpawnSmokeOnBoss();

        // If the player is targetting this enemy and if there are more enemies 
        // around, it changes target to next enemy
        enemy.CineTarget.CancelCurrentTarget();

        anim.SetTrigger("Death");
        enemy.OnDie();
    }
}