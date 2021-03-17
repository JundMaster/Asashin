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
    private PlayerValuesScriptableObj values;
    private PlayerUseItem useItem;
    private PlayerMeleeAttack attack;
    private PlayerBlock block;

    // Gravity
    private Vector3 verticalVelocity;
    public Vector3 VerticalVelocity => verticalVelocity;

    // Ground variables
    [SerializeField] private LayerMask groundLayer;

    public bool Performing { get; private set; }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInputCustom>();
        roll = GetComponent<PlayerRoll>();
        values = GetComponent<Player>().Values;
        attack = GetComponent<PlayerMeleeAttack>();
        useItem = GetComponent<PlayerUseItem>();
        block = GetComponent<PlayerBlock>();
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
        if (IsGrounded() && attack.Performing == false &&
            roll.Performing == false && useItem.Performing == false &&
            block.Performing == false)
        {
            verticalVelocity.y = Mathf.Sqrt(values.JumpForce * values.Gravity);
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
