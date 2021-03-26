using UnityEngine;

/// <summary>
/// Class responsible for handling item spawning behaviour.
/// </summary>
public class SpawnItemBehaviour : MonoBehaviour, ISpawnItemBehaviour
{
    [SerializeField] private GameObject[] objectToSpawn;
    private int randomSpawnNumber;
    private Vector3 spawnPosition;
    [Tooltip("Probability of dropping the first item.")]
    [Range(0, 100)][SerializeField] private float firstItemSpawnChance;
    [Tooltip("Probability of dropping items after the first one.")]
    [Range(0, 100)][SerializeField] private float spawningChance;
    [SerializeField] private TypeOfDropEnum typeOfDrop;

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
    /// <param name="probability">First item spawn chance probabilty.</param>
    private void SpawnItem(ref float probability)
    {
        float random = Random.Range(0f, 100f);
        // Only happens once for the first time spawn chance
        // Spawns the first item with firstItemSpawnChance
        if (probability >= random)
        {
            SpawnObject();

            // Only happens if the first item was dropped
            probability = Random.Range(0f, 100f);
            // This loop is for the rest of the items after the first one was spawned
            // Spawns the next items with spawningChance
            while (spawningChance >= probability)
            {
                // Spawns an item, gives it a random force
                SpawnObject();

                // Attributes new value to probability to continue the loop
                probability = Random.Range(0f, 100f);
            }
        }       
    }

    /// <summary>
    /// Spawns a random item from possible loot array with random force and direction.
    /// </summary>
    private void SpawnObject()
    {
        randomSpawnNumber = Random.Range(0, objectToSpawn.Length);

        // Instantiates the item
        GameObject spawnedObject = Instantiate(
            objectToSpawn[randomSpawnNumber], spawnPosition, Quaternion.identity);

        // Determins what kind of object is dropping this loot
        spawnedObject.GetComponent<Pickable>().TypeOfDrop = typeOfDrop;

        Rigidbody spawnedObjectRB = spawnedObject.GetComponent<Rigidbody>();
        // Gives it a random force
        spawnedObjectRB.AddForce(
            Random.Range(-75f, 75f), 90f, Random.Range(-75f, 75f), ForceMode.Impulse);
    }
}
