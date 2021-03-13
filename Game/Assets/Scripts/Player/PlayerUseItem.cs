using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for handling usage of items.
/// </summary>
public class PlayerUseItem : MonoBehaviour, IAction
{
    [SerializeField] private Transform kunaiItemPosition;
    public Transform KunaiItemPosition => kunaiItemPosition;
    [Range(0, 3)][SerializeField] private int delayAfterUse;

    // Components
    private ItemControl itemControl;
    private PlayerInputCustom input;
    private PlayerAnimations playerAnims;
    private PlayerMeleeAttack attack;
    private PlayerJump jump;
    private PlayerRoll roll;
    private Animator anim;
    private PlayerMovement movement;
    private CinemachineTarget target;

    private float timeItemWasUsed;
    private bool canUseItemDelayOver;
    public bool CanUseItem { get; set; }

    public bool Performing { get; private set; }

    private void Awake()
    {
        itemControl = FindObjectOfType<ItemControl>();
        input = GetComponent<PlayerInputCustom>();
        playerAnims = GetComponent<PlayerAnimations>();
        attack = GetComponent<PlayerMeleeAttack>();
        jump = GetComponent<PlayerJump>();
        roll = GetComponent<PlayerRoll>();
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        target = FindObjectOfType<CinemachineTarget>();
    }

    private void Start()
    {
        timeItemWasUsed = 0f;
        canUseItemDelayOver = true;

        CanUseItem = true;
    }

    private void OnEnable()
    {
        input.ItemUse += HandleItemUse;
    }

    private void OnDisable()
    {
        input.ItemUse -= HandleItemUse;
    }

    /// <summary>
    /// Uses the item and starts it's delay.
    /// Class PlayerItemAnimationEvents complements this class, controlling what
    /// happens after the current animation is set in motion.
    /// </summary>
    private void HandleItemUse()
    {
        if (canUseItemDelayOver && attack.Performing == false && jump.Performing == false && roll.Performing == false)
        {
            if (target.Targeting)
            {
                transform.LookAt(target.CurrentTarget);
                transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z);
            }

            // If the player is pressing any direction
            // rotates the character instantly to roll in that direction
            else if (target.Targeting == false && movement.Direction != Vector3.zero)
            {
                // Finds angle
                float targetAngle = Mathf.Atan2(movement.Direction.x, movement.Direction.z) *
                        Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;

                // Rotates to that angle
                transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            }
            

            switch (itemControl.CurrentItem.ItemType)
            {
                case ListOfItems.FirebombKunai:
                    playerAnims.TriggerKunaiAnimation();
                    break;
            }

            timeItemWasUsed = Time.time;
            canUseItemDelayOver = false;
        }
    }

    /// <summary>
    /// Called on item use animation events.
    /// </summary>
    private void PerformingItemUseToTrue()
    {
        anim.applyRootMotion = true;
        Performing = true;
    }

    /// <summary>
    /// Called on item use animation events.
    /// </summary>
    private void PerformingItemUseToFalse()
    {
        anim.applyRootMotion = false;
        Performing = false;
    }

    public void ComponentUpdate()
    {
        // Only possible to use an item after delay is over
        if (Time.time - timeItemWasUsed > delayAfterUse) canUseItemDelayOver = true;
    }

    public void ComponentFixedUpdate()
    {
        //
    }
}
