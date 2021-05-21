using UnityEngine;

/// <summary>
/// Abstract class responsbile for handling item behaviours.
/// </summary>
public abstract class ItemBehaviour : MonoBehaviour, IUsableItem, IFindPlayer
{
    // Components
    protected PlayerStats playerStats;
    private ItemUIParent itemUIParent;

    [SerializeField] protected ListOfItems itemType;
    /// <summary>
    /// Getter for which time of item this is.
    /// </summary>
    public ListOfItems ItemType { get => itemType; }

    private void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        itemUIParent = FindObjectOfType<ItemUIParent>();
    }

    /// <summary>
    /// Method that defines what happens when the item is used.
    /// </summary>
    public virtual void Execute()
    {
        // Updates UI of all items.
        if (itemUIParent != null)
            itemUIParent.UpdateAllItemUI();
    }

    public void FindPlayer() =>
        playerStats = FindObjectOfType<PlayerStats>();

    public void PlayerLost()
    {
        //
    }
}
