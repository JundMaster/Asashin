using UnityEngine;
using System.Collections;

/// <summary>
/// Scriptable object responsible for controlling tutorial enemy state after dying.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Tutorial Death State")]
public class EnemyTutorialDeathState : EnemyTutorialAbstractState
{
    [Header("Particles to spawn when the enemy dies")]
    [SerializeField] private GameObject smokeParticles;

    /// <summary>
    /// Happens once when the enemy enters this state. Starts death coroutine.
    /// </summary>
    public override void OnEnter()
    {
        if (enemy.InCombat == true)
            enemy.InCombat = false;

        // Variables to make sure the is no collision while the enemy is dying
        enemy.Agent.enabled = false;
        enemy.GetComponent<CapsuleCollider>().enabled = false;
        enemy.gameObject.layer = 31; // Changes to ignore layer

        enemy.StopAllCoroutines();
        enemy.StartCoroutine(Die());

        enemy.OnTutorialDie(TypeOfTutorial.EnemyDie);
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
        enemy.CineTarget.CancelCurrentTargetOnDeath();

        enemy.CineTarget.AutomaticallyFindTargetCall(
            enemy.CineTarget.FindTargetSize);

        Instantiate(
            smokeParticles,
            new Vector3(
                enemy.transform.position.x,
                enemy.transform.position.y + 0.5f,
                enemy.transform.position.z),
            Quaternion.identity);

        enemy.OnDie();
        Destroy(enemy.gameObject);
    }
}
