using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Class responsible for handling item's UI behaviour.
/// </summary>
public class ItemUI : MonoBehaviour, IItemUI, IFindPlayer
{
    [SerializeField] private ListOfItems itemType;
    public ListOfItems ItemType => itemType;

    // Text with quantity of this item
    [SerializeField] private TextMeshProUGUI quantityText;

    // Panel with the item's image
    [SerializeField] private Image image;

    // Components
    private PlayerStats playerStats;
    private PlayerUseItem useItem;
    private ItemControl itemControl;

    // All itemsUI.
    private IItemUI[] allItemsUI;

    private void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        useItem = FindObjectOfType<PlayerUseItem>();
        itemControl = FindObjectOfType<ItemControl>();
        allItemsUI = FindObjectsOfType<ItemUI>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForFixedUpdate();
        GiveTransparencyToAllItemUIs();
        HighlightSelectedItem();
    }

    private void OnEnable()
    {
        itemControl.ChangedCurrentItem += HighlightSelectedItem;
    }

    private void OnDisable()
    {
        itemControl.ChangedCurrentItem -= HighlightSelectedItem;
    }

    /// <summary>
    /// Gives transparency too al item uis.
    /// </summary>
    private void GiveTransparencyToAllItemUIs()
    {
        foreach (ItemUI itemUI in allItemsUI)
        {
            itemUI.image.color = new Color(
                itemUI.image.color.r,
                itemUI.image.color.g,
                itemUI.image.color.b,
                0.25f);
        }
    }

    /// <summary>
    /// Highlights selected item and gives transparency to all other items.
    /// </summary>
    private void HighlightSelectedItem()
    {
        GiveTransparencyToAllItemUIs();

        if (itemControl.CurrentItem != null)
        {
            switch (itemControl.CurrentItem.ItemType)
            {
                case ListOfItems.Kunai:
                    foreach (ItemUI itemUI in allItemsUI)
                    {
                        if (itemUI.ItemType == ListOfItems.Kunai)
                        {
                            itemUI.image.color = new Color(
                                itemUI.image.color.r,
                                itemUI.image.color.g,
                                itemUI.image.color.b,
                                1f);
                        }
                    }
                    break;
                case ListOfItems.FirebombKunai:
                    foreach (ItemUI itemUI in allItemsUI)
                    {
                        if (itemUI.ItemType == ListOfItems.FirebombKunai)
                        {
                            itemUI.image.color = new Color(
                                itemUI.image.color.r,
                                itemUI.image.color.g,
                                itemUI.image.color.b,
                                1f);
                        }
                    }
                    break;
                case ListOfItems.HealthFlask:
                    foreach (ItemUI itemUI in allItemsUI)
                    {
                        if (itemUI.ItemType == ListOfItems.HealthFlask)
                        {
                            itemUI.image.color = new Color(
                                itemUI.image.color.r,
                                itemUI.image.color.g,
                                itemUI.image.color.b,
                                1f);
                        }
                    }
                    break;
                case ListOfItems.SmokeGrenade:
                    foreach (ItemUI itemUI in allItemsUI)
                    {
                        if (itemUI.ItemType == ListOfItems.SmokeGrenade)
                        {
                            itemUI.image.color = new Color(
                                itemUI.image.color.r,
                                itemUI.image.color.g,
                                itemUI.image.color.b,
                                1f);
                        }
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Updates current quantity on UI.
    /// </summary>
    public void UpdateValue()
    {
        if (playerStats != null)
        {
            switch (itemType)
            {
                case ListOfItems.Kunai:
                    quantityText.text = playerStats.Kunais.ToString();
                    break;
                case ListOfItems.FirebombKunai:
                    quantityText.text = playerStats.FirebombKunais.ToString();
                    break;
                case ListOfItems.HealthFlask:
                    quantityText.text = playerStats.HealthFlasks.ToString();
                    break;
                case ListOfItems.SmokeGrenade:
                    quantityText.text = playerStats.SmokeGrenades.ToString();
                    break;
            }
        }
    }

    /// <summary>
    /// Updates UI delay.
    /// </summary>
    /// <returns></returns>
    public IEnumerator UpdateDelay()
    {
        image.fillAmount = 0;

        while (image.fillAmount < 1)
        {
            float delayTimePassed = Time.time - useItem.TimeItemWasUsed;
            float fillAmount = delayTimePassed / useItem.Delay;
            image.fillAmount = fillAmount;

            yield return null;
        }       
    }

    public void FindPlayer()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        useItem = FindObjectOfType<PlayerUseItem>();
    }

    public void PlayerLost()
    {
        //
    }
}
