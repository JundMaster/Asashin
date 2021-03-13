using UnityEngine;

/// <summary>
/// Abstract class responsbile for handling item behaviours.
/// Uses sanbox pattern.
/// </summary>
public abstract class ItemBehaviour : MonoBehaviour, IUsableItems
{
    // Components
    protected PlayerStats playerStats;

    /// <summary>
    /// Getter for which time of item this is.
    /// </summary>
    public abstract ListOfItems ItemType { get; }

    private void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>();
    }

    /// <summary>
    /// Method that defines what happens when the item is used.
    /// </summary>
    public abstract void Execute();
}
