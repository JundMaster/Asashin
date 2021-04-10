using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for handling wall hug behaviour.
/// </summary>
public class PlayerWallHug : MonoBehaviour, IAction
{
    [SerializeField] private LayerMask walls;

    [Header("Colliders to control wallhug")]
    [SerializeField] private SphereCollider mainCol;
    [SerializeField] private SphereCollider rightCol;
    [SerializeField] private SphereCollider leftCol;

    // Components
    private PlayerInputCustom input;
    private PlayerJump jump;
    private PlayerMeleeAttack attack;
    private PlayerUseItem useItem;
    private PlayerRoll roll;
    private Animator anim;
    private PlayerBlock block;
    private CharacterController controller;

    private Collider[] wallsColliders;
    private Collider[] wallsCollidersRight;
    private Collider[] wallsCollidersLeft;

    public bool Performing { get; private set; }

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        jump = GetComponent<PlayerJump>();
        attack = GetComponent<PlayerMeleeAttack>();
        anim = GetComponent<Animator>();
        useItem = GetComponent<PlayerUseItem>();
        roll = GetComponent<PlayerRoll>();
        block = GetComponent<PlayerBlock>();
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Performing = false;
    }

    private void OnEnable()
    {
        input.WallHug += HandleWallHug;
    }

    private void OnDisable()
    {
        input.WallHug -= HandleWallHug;
    }

    /// <summary>
    /// Handles wall hug.
    /// </summary>
    private void HandleWallHug()
    {
        if (Performing == false)
        {
            // If the player isn't performing any of these actions
            if (attack.Performing == false &&
                jump.IsGrounded() &&
                useItem.Performing == false &&
                block.Performing == false &&
                roll.Performing == false)
            {
                if (wallsColliders.Length > 0)
                {
                    // Finds closest point between collisions
                    Vector3 closesestPoint
                        = wallsColliders[0].ClosestPoint(transform.position);

                    // Gets direction with that same closest point
                    Vector3 contactDirection = 
                        transform.position - closesestPoint;

                    // Rotation angle with that direction
                    float targetAngle =
                        Mathf.Atan2(contactDirection.x, contactDirection.z) *
                        Mathf.Rad2Deg;

                    // Rotates player agaisnt that direction
                    transform.rotation =
                        Quaternion.Euler(0f, targetAngle, 0f);

                    Performing = true;  
                }
            }
        }
        else
            Performing = false;
    }

    public void ComponentUpdate()
    {
        wallsColliders =
            Physics.OverlapSphere(
                    mainCol.transform.position, mainCol.radius, walls);

        wallsCollidersLeft =
                Physics.OverlapSphere(
                    leftCol.transform.position, leftCol.radius, walls);

        wallsCollidersRight =
                Physics.OverlapSphere(
                    rightCol.transform.position, rightCol.radius, walls);
    }

    public void ComponentFixedUpdate()
    {
        if (Performing)
        {
            controller.radius = 0.3f;

            // Move direction perpendicular to player's back
            Vector3 moveDirection =
                Quaternion.Euler(0f, -90, 0f) * transform.forward;

            if ((input.Movement.x > 0 && wallsCollidersLeft.Length > 0) ||
                (input.Movement.x < 0 && wallsCollidersRight.Length > 0))
            {
                controller.Move(
                    input.Movement.x *
                    moveDirection *
                    Time.fixedUnscaledDeltaTime);
            }

            // If the player leaves the wall, cancels wall hug
            if (wallsColliders.Length == 0)
            {
                anim.applyRootMotion = false;
                Performing = false;
            }
        }
        else
        {
            controller.radius = 0.3f;
        }
    }
}