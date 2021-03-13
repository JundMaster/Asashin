using UnityEngine;

/// <summary>
/// Class responsible for handling animation events after using an item.
/// Uses a version of sandbox pattern.
/// </summary>
public class PlayerItemUseAnimationEvents : MonoBehaviour
{
    // Components
    private ItemControl itemControl;
    private PlayerUseItem playerUseItem;

    private void Awake()
    {
        itemControl = FindObjectOfType<ItemControl>();
        playerUseItem = GetComponent<PlayerUseItem>();
    }

    /// <summary>
    /// Called on animation event.
    /// </summary>
    public void AnimationEventThrowKunai()
    {
        Instantiate(itemControl.CurrentItemObject, playerUseItem.KunaiItemPosition.position, Quaternion.identity);
    }

    public void AnimationEventUseHealthFlask()
    {
        itemControl.GetComponent<IItem>().Execute();
    }

    public void AnimationEventUseSmokeGrenade()
    {
        itemControl.GetComponent<IItem>().Execute();
    }
}
