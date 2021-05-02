using UnityEngine;

/// <summary>
/// Class responsible for handling item spawning behaviour.
/// </summary>
public class SpawnItemBehaviour : MonoBehaviour, ISpawnItemBehaviour
{
    [SerializeField] private SpawnItemScriptableObj itemsScriptableObj;
    [SerializeField] private TypeOfDropEnum typeOfDrop;
    private Vector3 spawnPosition;
    private float firstItemSpawnChance;
    private float nextItemSpawnChance;
    private LootItem[] objectsToSpawn;

    public void ExecuteBehaviour()
    {
        spawnPosition = new Vector3(
                transform.position.x, 
                transform.position.y + 0.5f, 
                transform.position.z);

        switch(typeOfDrop)
        {
            case TypeOfDropEnum.Enemy:
                firstItemSpawnChance = itemsScriptableObj.EnemyFirstItemSpawnChance;
                nextItemSpawnChance = itemsScriptableObj.EnemyNextItemsSpawningChance;
                objectsToSpawn = itemsScriptableObj.EnemyLoot;
                break;
            case TypeOfDropEnum.Treasure:
                firstItemSpawnChance = itemsScriptableObj.TreasureFirstItemSpawnChance;
                nextItemSpawnChance = itemsScriptableObj.TreasureNextItemsSpawningChance;
                objectsToSpawn = itemsScriptableObj.TreasureLoot;
                break;
            case TypeOfDropEnum.Lootbox:
                firstItemSpawnChance = itemsScriptableObj.WoodenBoxFirstItemSpawnChance;
                nextItemSpawnChance = itemsScriptableObj.WoodenBoxNextItemsSpawningChance;
                objectsToSpawn = itemsScriptableObj.WoodenBoxLoot;
                break;
        }

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
            // Spawns the next items with nextItemsSpawningChance
            while (nextItemSpawnChance >= probability)
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
        int randomSpawnNumber = Random.Range(0, objectsToSpawn.Length);

        LootItem itemToSpawn = objectsToSpawn[randomSpawnNumber];

        // Instantiates the item
        GameObject spawnedObject = Instantiate(
            itemToSpawn.Prefab, spawnPosition, Quaternion.identity);

        // Determines the type of drop and its quantity
        spawnedObject.GetComponent<Pickable>().Quantity = itemToSpawn.Quantity;

        Rigidbody spawnedObjectRB = spawnedObject.GetComponent<Rigidbody>();

        // Gives it a random force
        spawnedObjectRB.AddForce(
            Random.Range(-75f, 75f), 
            90f, 
            Random.Range(-75f, 75f), 
            ForceMode.Impulse);
    }
}
