using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for handling player attack.
/// </summary>
public class PlayerMeleeAttack : MonoBehaviour, IAction
{
    // Components
    private PlayerInputCustom input;
    private Animator anim;
    private PlayerMovement movement;
    private PlayerJump jump;
    private PlayerRoll roll;

    private void Awake()
    {
        input = GetComponent<PlayerInputCustom>();
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        jump = GetComponent<PlayerJump>();
        roll = GetComponent<PlayerRoll>();
    }

    private void OnEnable()
    {
        input.MeleeLightAttack += MeleeLightAttack;
        input.MeleeStrongAttack += MeleeStrongAttack;
    }

    private void OnDisable()
    {
        input.MeleeLightAttack -= MeleeLightAttack;
        input.MeleeStrongAttack -= MeleeStrongAttack;
    }

    public void ComponentUpdate()
    {
        
    }

    public void ComponentFixedUpdate()
    {
        
    }

    private void MeleeLightAttack()
    {
        movement.CanMove = false;
        jump.CanJump = false;
        roll.CanRoll = false;

        anim.applyRootMotion = true;
        anim.speed = 2f;
        anim.SetTrigger("MeleeLightAttack");
        anim.ResetTrigger("MeleeStrongAttack");
    }

    private void MeleeStrongAttack()
    {
        movement.CanMove = false;
        jump.CanJump = false;
        roll.CanRoll = false;

        anim.applyRootMotion = true;
        anim.speed = 1.5f;
        anim.SetTrigger("MeleeStrongAttack");
        anim.ResetTrigger("MeleeLightAttack");
    }

    /// <summary>
    /// Turns of root motion. Runs on animation event.
    /// </summary>
    private void TurnOffRootMotion()
    {
        anim.ResetTrigger("MeleeLightAttack");
        movement.CanMove = true;
        jump.CanJump = true;
        roll.CanRoll = true;

        anim.speed = 1f;
        anim.applyRootMotion = false;
    }
}
