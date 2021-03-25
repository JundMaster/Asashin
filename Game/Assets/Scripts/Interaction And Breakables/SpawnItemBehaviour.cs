using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for handling item spawning behaviour.
/// </summary>
public class SpawnItemBehaviour : MonoBehaviour, ISpawnItemBehaviour
{
    [SerializeField] private GameObject[] objectToSpawn;
    private int randomSpawnNumber;
    private Vector3 spawnPosition;
    [Tooltip("This is the chance of dropping the first item.")]
    [Range(0, 100)][SerializeField] private float firstItemSpawnChance;
    [Tooltip("The item needs a chance higher than this in order to be spawned.")]
    [Range(0, 100)] [SerializeField] private float spawningChance;

    private void Start()
    {
        spawnPosition =
            new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        if (firstItemSpawnChance < spawningChance)
        {
            firstItemSpawnChance = spawningChance;
        }
    }

    public void ExecuteBehaviour()
    {
        // Spawns at least one item
        SpawnItem(ref firstItemSpawnChance);
    }

    /// <summary>
    /// Spawns a random item in a random direction.
    /// </summary>
    /// <param name="probability">Probability of spawning the item.</param>
    private void SpawnItem(ref float probability)
    {
        while (probability >= spawningChance)
        {
            randomSpawnNumber = Random.Range(0, objectToSpawn.Length);

            // Instantiates the item
            GameObject spawnedObject = Instantiate(
                objectToSpawn[randomSpawnNumber], spawnPosition, Quaternion.identity);

            spawnedObject.GetComponent<Pickable>().TypeOfDrop = TypeOfDropEnum.Treasure;
            Rigidbody spawnedObjectRB = spawnedObject.GetComponent<Rigidbody>();

            // Gives it a random force
            spawnedObjectRB.AddForce(
                Random.Range(-75f, 75f), 90f, Random.Range(-75f, 75f), ForceMode.Impulse);

            // Starts the coroutine again with a random chance of spawning an item
            probability = Random.Range(0f, 100f);
        }
    }
}
