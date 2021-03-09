using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for handling player jump.
/// </summary>
public class PlayerJump : MonoBehaviour, IAction
{
    // Components
    private CharacterController controller;
    private PlayerInputCustom input;
    private PlayerRoll roll;

    // Gravity
    public float Gravity { get; set; } = -9.81f;
    private Vector3 verticalVelocity;
    public Vector3 VerticalVelocity => verticalVelocity;

    // Jumping variables
    [SerializeField] private float jumpForce = -2f;
    public float JumpForce { get => jumpForce; set => jumpForce = value; }
    public bool CanJump { get; set; }

    // Ground variables
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInputCustom>();
        roll = GetComponent<PlayerRoll>();
    }

    private void Start()
    {
        CanJump = true;
    }

    private void OnEnable()
    {
        input.Jump += HandleJump;
    }

    private void OnDisable()
    {
        input.Jump -= HandleJump;
    }

    public void ComponentUpdate()
    {
        if (roll.Rolling) CanJump = false;
        else CanJump = true;
    }

    public void ComponentFixedUpdate()
    {
        // Gravity
        if (IsGrounded() && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -1f;
        }

        verticalVelocity.y += Gravity * Time.fixedUnscaledDeltaTime;
        controller.Move(verticalVelocity * Time.fixedUnscaledDeltaTime);
    }

    /// <summary>
    /// Handles jump.
    /// </summary>
    private void HandleJump()
    {
        if (IsGrounded() && CanJump)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpForce * Gravity);
            roll.CanRoll = false;
        }
    }

    /// <summary>
    /// Checks if the character is grounded.
    /// </summary>
    public bool IsGrounded()
    {
        Collider[] isGrounded =
            Physics.OverlapSphere(transform.position, 0.1f, groundLayer);

        if (isGrounded.Length > 0)
            return true;

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}
