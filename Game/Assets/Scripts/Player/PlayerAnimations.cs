﻿using UnityEngine;

/// <summary>
/// Class responsible for playing player's animations, uses a kind of sandbox pattern.
/// This class also handles most of the animation events.
/// </summary>
public class PlayerAnimations : MonoBehaviour
{
    // Components
    private Animator anim;
    private PlayerMovement movement;
    private PlayerJump jump;
    private PlayerRoll roll;
    private PlayerMeleeAttack attack;
    private PauseSystem pauseSystem;
    private PlayerBlock block;
    private PlayerDeathBehaviour death;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        jump = GetComponent<PlayerJump>();
        roll = GetComponent<PlayerRoll>();
        attack = GetComponent<PlayerMeleeAttack>();
        pauseSystem = FindObjectOfType<PauseSystem>();
        block = GetComponent<PlayerBlock>();
        death = GetComponent<PlayerDeathBehaviour>();
    }

    private void OnEnable()
    {
        roll.Roll += TriggerRollAnimation;
        attack.LightMeleeAttack += TriggerLightMeleeAttack;
        attack.StrongMeleeAttack += TriggerStrongMeleeAttack;
        attack.AirAttack += TriggerAirAttack;
        pauseSystem.GamePaused += PauseSystemAnimator;
        death.PlayerDied += TriggerDeath;
    }

    private void OnDisable()
    {
        roll.Roll -= TriggerRollAnimation;
        attack.LightMeleeAttack -= TriggerLightMeleeAttack;
        attack.StrongMeleeAttack -= TriggerStrongMeleeAttack;
        attack.AirAttack -= TriggerAirAttack;
        pauseSystem.GamePaused -= PauseSystemAnimator;
        death.PlayerDied -= TriggerDeath;
    }

    private void Update()
    {
        anim.SetFloat("MovementSpeed", movement.MovementSpeed);
        anim.SetFloat("VerticalVelocity", jump.VerticalVelocity.y);
        anim.SetBool("IsGrounded", jump.IsGrounded());
        anim.SetBool("Block", block.Performing);
        anim.SetBool("Walking", movement.Walking);
        anim.SetBool("Sprinting", movement.Sprinting);
    }

    /// <summary>
    /// This methid is called with an animation event.
    /// The method is triggered on the final of the animation.
    /// </summary>
    private void AnimationEventDeathBehaviour()
    {

    }

    private void TriggerDeath() => anim.SetTrigger("Death");

    public void TriggerBlockReflect() => anim.SetTrigger("BlockReflect");

    private void TriggerRollAnimation() => anim.SetTrigger("Rolling");

    private void TriggerAirAttack() => anim.SetTrigger("AirAttack");

    private void TriggerLightMeleeAttack()
    {
        anim.SetTrigger("MeleeLightAttack");
        anim.ResetTrigger("MeleeStrongAttack");
    }

    private void TriggerStrongMeleeAttack()
    {
        anim.SetTrigger("MeleeStrongAttack");
        anim.ResetTrigger("MeleeLightAttack");
    }

    /// <summary>
    /// The action of this item is called on PlayerAnimationEvents class.
    /// Its behaviour is called with animation events with a method on that class.
    /// </summary>
    public void TriggerKunaiAnimation() => anim.SetTrigger("ThrowKunai");

    /// <summary>
    /// The action of this item is called on PlayerAnimationEvents class.
    /// Its behaviour is called with animation events with a method on that class.
    /// </summary>
    public void TriggerFirebombKunaiAnimation() => anim.SetTrigger("ThrowFirebombKunai");

    /// <summary>
    /// The action of this item is called on PlayerAnimationEvents class.
    /// Its behaviour is called with animation events with a method on that class.
    /// </summary>
    public void TriggerHealthFlaskAnimation() => anim.SetTrigger("UseHealthFlask");

    /// <summary>
    /// The action of this item is called on PlayerAnimationEvents class.
    /// Its behaviour is called with animation events with a method on that class.
    /// </summary>
    public void TriggerSmokeGrenadeAnimation() => anim.SetTrigger("UseSmokeGrenade");

    /// <summary>
    /// Sets animator update mode to scaled or unscaled when the game is paused.
    /// </summary>
    /// <param name="">Parameter that checks if the game is paused or unpaused.</param>
    private void PauseSystemAnimator(PauseSystemEnum pauseSystem)
    {
        if (pauseSystem == PauseSystemEnum.Paused)
        {
            anim.updateMode = AnimatorUpdateMode.Normal;
        }
        else
        {
            anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
    }
}
