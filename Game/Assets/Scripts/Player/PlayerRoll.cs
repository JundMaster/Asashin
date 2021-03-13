using UnityEngine;
using System;
using System.Collections;

public class PlayerRoll : MonoBehaviour, IAction
{
    // Components
    private PlayerMovement movement;
    private PlayerInputCustom input;
    private PlayerJump jump;
    private PlayerMeleeAttack attack;
    private PlayerUseItem useItem;
    private Animator anim;

    public bool Performing { get; private set; }

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        input = GetComponent<PlayerInputCustom>();
        jump = GetComponent<PlayerJump>();
        attack = GetComponent<PlayerMeleeAttack>();
        anim = GetComponent<Animator>();
        useItem = GetComponent<PlayerUseItem>();
    }

    private void Start()
    {
        Performing = false;
    }

    private void OnEnable()
    {
        input.Roll += HandleRoll;
    }

    private void OnDisable()
    {
        input.Roll -= HandleRoll;
    }

    public void ComponentUpdate()
    {

    }

    public void ComponentFixedUpdate()
    {
        
    }

    /// <summary>
    /// Handles rolling.
    /// </summary>
    private void HandleRoll()
    {
        if (Performing == false && attack.Performing == false &&
            jump.IsGrounded() && useItem.Performing == false)
        {
            // If the player is pressing any direction
            // rotates the character instantly to roll in that direction
            if (movement.Direction != Vector3.zero)
            {
                // Finds angle
                float targetAngle = Mathf.Atan2(movement.Direction.x, movement.Direction.z) *
                        Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;

                // Rotates to that angle
                transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            }

            //////////////
            
            anim.applyRootMotion = true;
            Performing = true;
            OnRoll();
        }
    }

    /// <summary>
    /// Called with animation event.
    /// </summary>
    private void RollingToFalse()
    {
        Performing = false;
        anim.applyRootMotion = false;
    }

    protected virtual void OnRoll() => Roll?.Invoke();

    /// <summary>
    /// Event registered on SlowMotionBehaviour and PlayerAnimations..
    /// </summary>
    public event Action Roll;
}
