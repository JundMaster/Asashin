using UnityEngine;

/// <summary>
/// Class responsible for handling animation events after using an item.
/// Uses a version of sandbox pattern.
/// </summary>
public class PlayerAnimationEvents : MonoBehaviour
{
    // Components
    private ItemControl itemControl;
    private PlayerUseItem playerUseItem;
    private PlayerRoll roll;
    private PlayerUseItem useItem;
    private PlayerMeleeAttack attack;

    private void Awake()
    {
        itemControl = FindObjectOfType<ItemControl>();
        playerUseItem = GetComponentInParent<PlayerUseItem>();
        roll = GetComponentInParent<PlayerRoll>();
        useItem = GetComponentInParent<PlayerUseItem>();
        attack = GetComponentInParent<PlayerMeleeAttack>();
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
