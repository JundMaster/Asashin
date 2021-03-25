using UnityEngine;

/// <summary>
/// Handles Kenshusei death behaviour.
/// </summary>
public class KenshuseiDeathBehaviour : DeathBehaviour
{
    // Components
    private CinemachineTarget cinemachineTarget;

    [SerializeField] private GameObject smokeParticles;

    private ISpawnItemBehaviour spawnItemBehaviour;

    private void Awake()
    {
        cinemachineTarget = FindObjectOfType<CinemachineTarget>();
        spawnItemBehaviour = GetComponent<SpawnItemBehaviour>();
    }

    public override void Die()
    {
        // If the player is targetting and if there are more enemies around,
        // changes target to next enemy
        cinemachineTarget.CancelCurrentTargetAutomatically();
        cinemachineTarget.AutomaticallyFindTargetCall();

        // Random chance of spawning items.
        spawnItemBehaviour.ExecuteBehaviour();

        Instantiate(
            smokeParticles,
            new Vector3(
                transform.position.x,
                transform.position.y + 1.5f,
                transform.position.z),
            Quaternion.identity);

        Destroy(gameObject);
    }
}
