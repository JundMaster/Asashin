using UnityEngine;
using System.Collections;

/// <summary>
/// Scriptable object responsible for controlling enemy state after dying.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Death State")]
public class EnemyDeathState : EnemyState
{
    [SerializeField] private GameObject smokeParticles;
    private ISpawnItemBehaviour spawnItemBehaviour;

    public override void Start()
    {
        spawnItemBehaviour = enemy.GetComponent<ISpawnItemBehaviour>();
    }

    public override void OnEnter()
    {
        enemy.StartCoroutine(Die());
    }

    public override IState FixedUpdate()
    {
        base.FixedUpdate();
        return enemy.DeathState;
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(3f);

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
