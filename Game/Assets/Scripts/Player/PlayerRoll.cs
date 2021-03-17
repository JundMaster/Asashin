using UnityEngine;
using System;

public class PlayerRoll : MonoBehaviour, IAction
{
    // Components
    private PlayerMovement movement;
    private PlayerInputCustom input;
    private PlayerJump jump;
    private PlayerMeleeAttack attack;
    private PlayerUseItem useItem;
    private Animator anim;
    private PlayerBlock block;

    public bool Performing { get; private set; }

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        input = GetComponent<PlayerInputCustom>();
        jump = GetComponent<PlayerJump>();
        attack = GetComponent<PlayerMeleeAttack>();
        anim = GetComponent<Animator>();
        useItem = GetComponent<PlayerUseItem>();
        block = GetComponent<PlayerBlock>();
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
            jump.IsGrounded() && useItem.Performing == false &&
            block.Performing == false)
        {
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
    /// Event registered on SlowMotionBehaviour, PlayerAnimations.
    /// Event registered on PlayerMovement.
    /// </summary>
    public event Action Roll;
}
