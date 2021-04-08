using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class responsible for handling items in general.
/// </summary>
public class ItemControl : MonoBehaviour
{
    // Components
    private PlayerInputCustom input;

    // Items control
    private IList<IUsableItem> allItemsInventory;
    public IUsableItem CurrentItem { get; private set; }
    public GameObject CurrentItemObject { get; private set; }
    private int index;

    // List of items
    [SerializeField] private ItemBehaviour kunai;
    [SerializeField] private ItemBehaviour firebombKunai;
    [SerializeField] private ItemBehaviour healthFlask;
    [SerializeField] private ItemBehaviour smokeGrenade;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
    }

    private void Start()
    {
        allItemsInventory = new List<IUsableItem>()
        {
            kunai,
            firebombKunai,
            healthFlask,
            smokeGrenade,
        };

        CurrentItem = kunai;
        CurrentItemObject = kunai.gameObject;

        index = 0;
    }

    private void OnEnable()
    {
        if (input != null) input.ItemChange += HandleItemSwitch;
    }

    private void OnDisable()
    {
        input.ItemChange -= HandleItemSwitch;
    }

    /// <summary>
    /// Switches to the next item or to the item before.
    /// </summary>
    /// <param name="direction">Right or left item.</param>
    private void HandleItemSwitch(Direction direction)
    {
        // Switches current activated item to the next one
        if (direction == Direction.Right)
        {
            if (index < allItemsInventory.Count - 1)
            {
                index++;
            }
            else
            {
                index = 0;
            }
        }
        else // Switches current activated item to the one on the left
        {
            if (index > 0)
            {
                index--;
            }
            else
            {
                index = allItemsInventory.Count - 1;
            }
        }

        // Updates current selected item
        CurrentItem = allItemsInventory[index];

        switch (CurrentItem.ItemType)
        {
            case ListOfItems.Kunai:
                CurrentItemObject = kunai.gameObject;
                break;
            case ListOfItems.FirebombKunai:
                CurrentItemObject = firebombKunai.gameObject;
                break;
            case ListOfItems.HealthFlask:
                CurrentItemObject = healthFlask.gameObject;
                break;
            case ListOfItems.SmokeGrenade:
                CurrentItemObject = smokeGrenade.gameObject;
                break;
        }

        OnChangedCurrentItem();
    }

    protected virtual void OnChangedCurrentItem() => ChangedCurrentItem?.Invoke();

    /// <summary>
    /// Event registered on ItemUIParent class.
    /// </summary>
    public event Action ChangedCurrentItem;
}
