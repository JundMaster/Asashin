using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    // Components
    private Animator anim;
    private PlayerMovement movement;
    private PlayerJump jump;
    private PlayerRoll roll;
    private PlayerMeleeAttack attack;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        jump = GetComponent<PlayerJump>();
        roll = GetComponent<PlayerRoll>();
        attack = GetComponent<PlayerMeleeAttack>();
    }

    private void OnEnable()
    {
        roll.Roll += TriggerRollAnimation;
        attack.LightMeleeAttack += TriggerLightMeleeAttack;
        attack.StrongMeleeAttack += TriggerStrongMeleeAttack;
    }

    private void OnDisable()
    {
        roll.Roll -= TriggerRollAnimation;
        attack.LightMeleeAttack -= TriggerLightMeleeAttack;
        attack.StrongMeleeAttack -= TriggerStrongMeleeAttack;
    }

    private void Update()
    {
        anim.SetFloat("Movement", movement.Direction.magnitude);
        anim.SetFloat("VerticalVelocity", jump.VerticalVelocity.y);
        anim.SetBool("IsGrounded", jump.IsGrounded());
    }

    private void TriggerRollAnimation() => anim.SetTrigger("Rolling");

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

    public void TriggerKunaiAnimation() => anim.SetTrigger("ThrowKunai");
}
