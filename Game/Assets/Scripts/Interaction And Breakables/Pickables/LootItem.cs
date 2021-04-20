using UnityEngine;
using System;

/// <summary>
/// Struct for loot items.
/// </summary>
[Serializable]
public struct LootItem
{
    [Tooltip("Prefab to spawn")]
    public GameObject Prefab;
    [Tooltip("Minimum and maximum quantity of this item")]
    public CustomVector2 Quantity;
}
