using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for handling items in general.
/// </summary>
public class ItemControl : MonoBehaviour
{
    // Components
    private PlayerInputCustom input;

    // Items control
    private IList<IUsableItems> allItemsInventory;
    public IUsableItems CurrentItem { get; private set; }
    public GameObject CurrentItemObject { get; private set; }

    // List of items
    [SerializeField] private ItemBehaviour firebombKunai;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
    }

    private void Start()
    {
        allItemsInventory = new List<IUsableItems>()
        {
            firebombKunai,
        };

        CurrentItem = firebombKunai;
        CurrentItemObject = firebombKunai.gameObject;
    }

    private void OnEnable()
    {
        input.ItemChange += HandleItemSwitch;
    }

    private void OnDisable()
    {
        input.ItemChange -= HandleItemSwitch;
    }

    private void HandleItemSwitch(LeftOrRight direction)
    {
        // Switches current activated item
 
    }
}
