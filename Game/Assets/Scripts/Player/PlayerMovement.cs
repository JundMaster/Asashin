﻿using UnityEngine;

/// <summary>
/// Class responsible for handling player movement.
/// </summary>
public class PlayerMovement : MonoBehaviour, IAction
{
    // Components
    private CharacterController controller;
    private PlayerInputCustom input;
    private Transform mainCamera;
    private SlowMotionBehaviour slowMotion;
    private PlayerValues values;

    // Movement Variables
    public Vector3 Direction { get; private set; }
    private float hVel;
    private float vVel;
    private Vector3 moveDirection;
    public bool CanMove { get; set; }

    // Rotation Variables
    public float TurnSmooth { get; set; }
    private float smoothTimeRotation;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInputCustom>();
        mainCamera = Camera.main.transform;
        slowMotion = FindObjectOfType<SlowMotionBehaviour>();
        values = GetComponent<Player>().Values;
    }

    private void Start()
    {
        CanMove = true;
    }
    
    public void ComponentUpdate()
    {
        // Updates movement direction variable
        if (CanMove && slowMotion.SlowMotionOn == false)
        {
            Direction = new Vector3(
                Mathf.SmoothDamp(
                    Direction.x, 
                    input.Movement.x, 
                    ref hVel, 
                    values.SmoothTimeVelocity), 
                0f, 
                Mathf.SmoothDamp(
                    Direction.z, 
                    input.Movement.y, 
                    ref vVel, 
                    values.SmoothTimeVelocity));
        }
        else if (CanMove && slowMotion.SlowMotionOn)
        {
            Direction = new Vector3(input.Movement.x, 0f, input.Movement.y);                
        }
        else
        {
            Direction = Vector3.zero;
        }
    }

    public void ComponentFixedUpdate()
    {
        Movement();
        Rotation();
    }

    /// <summary>
    /// Handles movement.
    /// </summary>
    private void Movement()
    {
        if (Direction.magnitude > 0.01f)
        {
            // Moves controllers towards the moveDirection set on Rotation()
            controller.Move(
                moveDirection.normalized * values.Speed * Time.fixedUnscaledDeltaTime);
        }
    }

    /// <summary>
    /// Handles rotation.
    /// </summary>
    private void Rotation()
    {
        if (Direction.magnitude > 0.01f)
        {
            // Finds out angle
            float targetAngle = Mathf.Atan2(Direction.x, Direction.z) * 
                Mathf.Rad2Deg + mainCamera.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y, 
                targetAngle, 
                ref smoothTimeRotation, 
                TurnSmooth);

            // Rotates to that angle
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Sets moving Direction
            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) *
                Vector3.forward;
        }
    }
}