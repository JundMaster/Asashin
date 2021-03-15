using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class responsible for handling player's block.
/// </summary>
public class PlayerBlock : MonoBehaviour, IAction
{
    // Components
    private PlayerInputCustom input;
    private Animator anim;
    private PlayerMeleeAttack attack;
    private PlayerJump jump;
    private PlayerRoll roll;
    private PlayerUseItem useItem;
    private PlayerMovement movement;

    public bool Performing { get; private set; }

    private void Awake()
    {
        input = GetComponent<PlayerInputCustom>();
        anim = GetComponent<Animator>();
        attack = GetComponent<PlayerMeleeAttack>();
        jump = GetComponent<PlayerJump>();
        roll = GetComponent<PlayerRoll>();
        useItem = GetComponent<PlayerUseItem>();
        movement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        input.Block += Block;
    }

    private void OnDisable()
    {
        input.Block -= Block;
    }

    private void Block(YesOrNo condition)
    {
        if (condition == YesOrNo.Yes)
        {
            if (attack.Performing == false && jump.Performing == false &&
                roll.Performing == false && useItem.Performing == false)
            {
                Performing = true;
                anim.applyRootMotion = true;
            }
        }
        else
        {
            Performing = false;
            anim.applyRootMotion = false;
        }
    }

    public void ComponentFixedUpdate()
    {
        
    }

    public void ComponentUpdate()
    {
        if (Performing)
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
        }
    }
}
