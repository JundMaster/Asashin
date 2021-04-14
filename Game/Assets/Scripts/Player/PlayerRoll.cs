using UnityEngine;
using System;

public class PlayerRoll : MonoBehaviour, IAction
{
    // Components
    private PlayerInputCustom input;
    private PlayerMeleeAttack attack;
    private PlayerUseItem useItem;
    private Animator anim;
    private PlayerBlock block;
    private PlayerWallHug wallHug;
    private PlayerMovement movement;

    public bool Performing { get; private set; }
    public float PerformingTime { get; private set; }

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        attack = GetComponent<PlayerMeleeAttack>();
        anim = GetComponent<Animator>();
        useItem = GetComponent<PlayerUseItem>();
        block = GetComponent<PlayerBlock>();
        wallHug = GetComponent<PlayerWallHug>();
        movement = GetComponent<PlayerMovement>();
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
        if (Performing)
            PerformingTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        else
            PerformingTime = 0;
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
            movement.IsGrounded() && useItem.Performing == false &&
            block.Performing == false && wallHug.Performing == false)
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

    /// <summary>
    /// Invokes Roll event.
    /// </summary>
    protected virtual void OnRoll() => Roll?.Invoke();

    /// <summary>
    /// Event registered on PlayerAnimations.
    /// Event registered on PlayerMovement.
    /// </summary>
    public event Action Roll;

    /// <summary>
    /// Invokes Dodge event.
    /// </summary>
    public virtual void OnDodge() => Dodge?.Invoke();

    /// <summary>
    /// Event registered on SlowMotionBehaviour.
    /// </summary>
    public event Action Dodge;
}
