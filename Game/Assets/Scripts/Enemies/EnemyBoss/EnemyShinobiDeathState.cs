using UnityEngine;
using System.Collections;

/// <summary>
/// Scriptable object responsible for handling shinobi's death state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Shinobi Death State")]
public sealed class EnemyShinobiDeathState : EnemyBossAbstractState
{
    public override void Start()
    {
        // Left blank on purpose
    }

    public override void OnEnter()
    {
        // Variables to make sure the is no collision while the enemy is dying
        enemy.GetComponent<CapsuleCollider>().enabled = false;
        enemy.gameObject.layer = 0; // Changes to default layer
        agent.isStopped = true;

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

        anim.SetTrigger("Death");
    }
}