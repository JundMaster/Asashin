using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Enemy Shinobi Death State")]
public sealed class EnemyShinobiDeathState : EnemyBossAbstractState
{
    public override void Start()
    {
        //
    }

    public override void OnEnter()
    {
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
        yield return new WaitForFixedUpdate();

        // If the player is targetting this enemy and if there are more enemies 
        // around, it changes target to next enemy
        enemy.CineTarget.CancelCurrentTargetAutomaticallyCall();
        enemy.CineTarget.AutomaticallyFindTargetCall();

        // Random chance of spawning items.
        /*
        spawnItemBehaviour.ExecuteBehaviour();

        Instantiate(
            smokeParticles,
            new Vector3(
                enemy.transform.position.x,
                enemy.transform.position.y + 1.5f,
                enemy.transform.position.z),
            Quaternion.identity);
        */

        Destroy(enemy.gameObject);
    }
}