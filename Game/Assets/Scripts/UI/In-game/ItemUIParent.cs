using UnityEngine;

/// <summary>
/// Class responsible for controling ItemUI classes.
/// This script is all ItemUI parent.
/// </summary>
public class ItemUIParent : MonoBehaviour, IFindPlayer
{
    // Components
    private IItemUI[] allItemsUI;

    private PlayerUseItem useItem;

    private void Awake()
    {
        allItemsUI = GetComponentsInChildren<IItemUI>();
        useItem = FindObjectOfType<PlayerUseItem>();
    }

    private void OnEnable()
    {
        if (useItem != null) useItem.UsedItemDelay += UpdateAllItemUIDelay;
    }

    private void OnDisable()
    {
        useItem.UsedItemDelay -= UpdateAllItemUIDelay;
    }

    private void UpdateAllItemUIDelay()
    {
        foreach (IItemUI itemUI in allItemsUI)
        {
            StartCoroutine(itemUI.UpdateDelay());
        }
    }

    /// <summary>
    /// Updates all ItemUI scripts. Method called by ItemUIParent.
    /// </summary>
    public void UpdateAllItemUI()
    {
        foreach (IItemUI itemUI in allItemsUI)
        {
            itemUI.UpdateValue();   
        }
    }

    public void FindPlayer()
    {
        useItem = FindObjectOfType<PlayerUseItem>();
        useItem.UsedItemDelay += UpdateAllItemUIDelay;
    }

    public void PlayerLost()
    {
        useItem.UsedItemDelay -= UpdateAllItemUIDelay;
    }
}
