using UnityEngine;

/// <summary>
/// Class responsible for breakable objects.
/// </summary>
public class BreakableBox : MonoBehaviour, IBreakable
{
    [SerializeField] private GameObject brokenObject;

    private ISpawnItemBehaviour spawnItemsBehaviour;

    private void Awake()
    {
        spawnItemsBehaviour = GetComponent<SpawnItemBehaviour>();
    }

    /// <summary>
    /// Method that defines what happens when something collides with this object.
    /// </summary>
    public void Execute()
    {
        Instantiate(brokenObject, transform.position, Quaternion.identity);

        spawnItemsBehaviour?.ExecuteBehaviour();

        Destroy(gameObject);
    }
}
