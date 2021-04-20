using UnityEngine;

/// <summary>
/// Scriptable object responsible for keeping item drop chances.
/// </summary>
[CreateAssetMenu(fileName = "Items Spawn Chances")]
public class SpawnItemScriptableObj : ScriptableObject
{
    [Header("Enemy Loot")]
    [SerializeField] private LootItem[] enemyLoot;
    [Tooltip("Probability of dropping the first item.")]
    [Range(0, 100)] [SerializeField] private float enemyFirstItemSpawnChance;
    [Tooltip("Probability of dropping items after the first one.")]
    [Range(0, 100)] [SerializeField] private float enemyNextItemsSpawningChance;
    public LootItem[] EnemyLoot => enemyLoot;
    public float EnemyFirstItemSpawnChance => enemyFirstItemSpawnChance;
    public float EnemyNextItemsSpawningChance => enemyNextItemsSpawningChance;
 
    ////////////////////////////////////////////////////////////////////////////
    [Header("Treasure Loot")]
    [SerializeField] private LootItem[] treasureLoot;
    [Tooltip("Probability of dropping the first item.")]
    [Range(0, 100)] [SerializeField] private float treasureFirstItemSpawnChance;
    [Tooltip("Probability of dropping items after the first one.")]
    [Range(0, 100)] [SerializeField] private float treasureNextItemsSpawningChance;
    public LootItem[] TreasureLoot => treasureLoot;
    public float TreasureFirstItemSpawnChance => treasureFirstItemSpawnChance;
    public float TreasureNextItemsSpawningChance => treasureNextItemsSpawningChance;

    ////////////////////////////////////////////////////////////////////////////
    [Header("Wooden Box Loot")]
    [SerializeField] private LootItem[] woodenBoxLoot;
    [Tooltip("Probability of dropping the first item.")]
    [Range(0, 100)] [SerializeField] private float woodenBoxFirstItemSpawnChance;
    [Tooltip("Probability of dropping items after the first one.")]
    [Range(0, 100)] [SerializeField] private float woodenBoxNextItemsSpawningChance;
    public LootItem[] WoodenBoxLoot => woodenBoxLoot;
    public float WoodenBoxFirstItemSpawnChance => woodenBoxFirstItemSpawnChance;
    public float WoodenBoxNextItemsSpawningChance => woodenBoxNextItemsSpawningChance;

    ////////////////////////////////////////////////////////////////////////////
    private void OnEnable()
    {
        if (enemyFirstItemSpawnChance < enemyNextItemsSpawningChance)
        {
            enemyFirstItemSpawnChance = enemyNextItemsSpawningChance;
        }
        if (treasureFirstItemSpawnChance < treasureNextItemsSpawningChance)
        {
            treasureFirstItemSpawnChance = treasureNextItemsSpawningChance;
        }
        if (woodenBoxFirstItemSpawnChance < woodenBoxNextItemsSpawningChance)
        {
            woodenBoxFirstItemSpawnChance = woodenBoxNextItemsSpawningChance;
        }
    }
}
