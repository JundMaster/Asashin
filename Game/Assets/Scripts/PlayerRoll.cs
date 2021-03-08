﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoll : MonoBehaviour, IComponent
{
    // Components
    private PlayerMovement movement;
    private PlayerInputCustom input;
    private PlayerJump jump;
    private Animator anim;

    // Roll Variables
    public bool CanRoll { get; set; }
    public bool Rolling { get; private set; }

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        input = GetComponent<PlayerInputCustom>();
        jump = GetComponent<PlayerJump>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        CanRoll = true;
        Rolling = false;
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
        if (jump.VerticalVelocity.y > 0) CanRoll = false;
        else CanRoll = true;
    }

    public void ComponentFixedUpdate()
    {
        
    }

    private void HandleRoll()
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
        
        movement.CanMove = false;
        anim.applyRootMotion = true;
        Rolling = true;
    }

    /// <summary>
    /// Called with animation event.
    /// </summary>
    public void RollingToFalse()
    {
        movement.CanMove = true;
        anim.applyRootMotion = false;
        Rolling = false;
    }
}
