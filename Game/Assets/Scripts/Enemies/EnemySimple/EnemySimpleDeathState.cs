using UnityEngine;
using System.Collections;

/// <summary>
/// Scriptable object responsible for controlling enemy state after dying.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Common Death State")]
public class EnemySimpleDeathState : EnemyAbstractState
{
    [Header("Particles to spawn when the enemy dies")]
    [SerializeField] private GameObject smokeParticles;
    private ISpawnItemBehaviour spawnItemBehaviour;

    /// <summary>
    /// Happens once. Gets ISpawnItemBehaviour of this enemy.
    /// </summary>
    public override void Start()
    {
        spawnItemBehaviour = enemy.GetComponent<ISpawnItemBehaviour>();
    }

    /// <summary>
    /// Happens once when the enemy enters this state. Starts death coroutine.
    /// </summary>
    public override void OnEnter()
    {
        enemy.gameObject.layer = 0; // Changes to default layer
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
        spawnItemBehaviour.ExecuteBehaviour();

        Instantiate(
            smokeParticles,
            new Vector3(
                enemy.transform.position.x,
                enemy.transform.position.y + 1.5f,
                enemy.transform.position.z),
            Quaternion.identity);

        Destroy(enemy.gameObject);
    }
}
