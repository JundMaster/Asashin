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
    private PlayerValues values;

    // Gravity
    private Vector3 verticalVelocity;
    public Vector3 VerticalVelocity => verticalVelocity;

    // Jumping variables
    public bool CanJump { get; set; }

    // Ground variables
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInputCustom>();
        roll = GetComponent<PlayerRoll>();
        values = GetComponent<Player>().Values;
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

    }

    public void ComponentFixedUpdate()
    {
        // Gravity
        if (IsGrounded() && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -1f;
        }

        verticalVelocity.y += values.Gravity * Time.fixedUnscaledDeltaTime;
        controller.Move(verticalVelocity * Time.fixedUnscaledDeltaTime);
    }

    /// <summary>
    /// Handles jump.
    /// </summary>
    private void HandleJump()
    {
        if (IsGrounded() && CanJump)
        {
            verticalVelocity.y = Mathf.Sqrt(values.JumpForce * values.Gravity);
            roll.CanRoll = false;
        }
    }

    /// <summary>
    /// Checks if the character is grounded.
    /// </summary>
    public bool IsGrounded()
    {
        Collider[] isGrounded =
            Physics.OverlapSphere(
                transform.position, values.IsGroundedCheckSize, groundLayer);

        if (isGrounded.Length > 0)
            return true;

        return false;
    }
}
