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
    [Tooltip("The item needs a chance higher than this in order to be spawned")]
    [SerializeField] private float spawningChance;

    private void Start()
    {
        spawnPosition =
            new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    public void ExecuteBehaviour()
    {
        // Spawns at least one item
        SpawnItem(100f);
    }

    /// <summary>
    /// Spawns a random item in a random direction.
    /// </summary>
    /// <param name="probability">Probability of spawning</param>
    /// <returns>Wait for seconds in realtime.</returns>
    private void SpawnItem(float probability)
    {
        //yield return new WaitForSecondsRealtime(0.5f);

        if (probability > spawningChance)
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
            SpawnItem(Random.Range(0f, 100f));
        }
    }
}
