using System;
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
    private PlayerMeleeAttack attack;
    private PlayerUseItem useItem;
    private PlayerRoll roll;
    private Animator anim;
    private PlayerBlock block;
    private CharacterController controller;
    private CinemachineTarget cinemachineTarget;
    private PlayerStats stats;
    private PlayerMovement movement;

    private Collider[] wallsColliders;
    private Collider[] wallsCollidersRight;
    private Collider[] wallsCollidersLeft;

    private Camera cam;

    public bool Performing { get; private set; }
    public bool InvertedAnimation { get; private set; }

    // Rotation variables
    private float smoothTimeRotation;
    private float turnSpeed;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        attack = GetComponent<PlayerMeleeAttack>();
        anim = GetComponent<Animator>();
        useItem = GetComponent<PlayerUseItem>();
        roll = GetComponent<PlayerRoll>();
        block = GetComponent<PlayerBlock>();
        controller = GetComponent<CharacterController>();
        cinemachineTarget = FindObjectOfType<CinemachineTarget>();
        stats = GetComponent<PlayerStats>();
        movement = GetComponent<PlayerMovement>();
        cam = Camera.main;
    }

    private void Start()
    {
        Performing = false;

        // Value for rotation when wall hughing (lest means faster)
        turnSpeed = 0.15f;
    }

    private void OnEnable()
    {
        input.WallHug += HandleWallHug;
        stats.TookDamage += CancelWallHug;
    }

    private void OnDisable()
    {
        input.WallHug -= HandleWallHug;
        stats.TookDamage -= CancelWallHug;
    }

    /// <summary>
    /// Cancels wall hug.
    /// </summary>
    private void CancelWallHug()
    {
        anim.applyRootMotion = false;
        Performing = false;
        OnWallHug(false);
 
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
                movement.IsGrounded() &&
                useItem.Performing == false &&
                block.Performing == false &&
                roll.Performing == false)
            {
                if (wallsColliders.Length > 0)
                {
                    cinemachineTarget.CancelCurrentTarget();
                    Performing = true;
                    OnWallHug(true);

                    // Finds closest point between collisions
                    Vector3 closesestPoint
                        = wallsColliders[0].ClosestPoint(transform.position);

                    // Gets direction with that same closest point
                    Vector3 contactDirection =
                        closesestPoint.Direction(transform.position);

                    // Rotation angle with that direction
                    float targetAngle =
                        Mathf.Atan2(contactDirection.x, contactDirection.z) *
                        Mathf.Rad2Deg;

                    // Rotates player agaisnt target angle direction
                    transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
                }
            }
        }
        else
        {
            CancelWallHug();
        }
    }

    /// <summary>
    /// Creates colliders needed to wall hug.
    /// </summary>
    public void ComponentUpdate()
    {
        // Creates main collider
        wallsColliders =
            Physics.OverlapSphere(
                    mainCol.transform.position, mainCol.radius, walls);

        // Checks colliders on sides
        if (Performing)
        {     
            wallsCollidersLeft =
                    Physics.OverlapSphere(
                        leftCol.transform.position, leftCol.radius, walls);

            wallsCollidersRight =
                    Physics.OverlapSphere(
                        rightCol.transform.position, rightCol.radius, walls);
        }
    }

    /// <summary>
    /// If wall hugging, gets closest point to wall, rotates player against it,
    /// gives new movement to player (horizontal relative to wall)
    /// </summary>
    public void ComponentFixedUpdate()
    {
        if (Performing)
        {
            // If the player isn't performing any of these actions
            if (attack.Performing == false &&
                movement.IsGrounded() &&
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
                        closesestPoint.Direction(transform.position);

                    // Rotation angle with that direction
                    float targetAngle =
                        Mathf.Atan2(contactDirection.x, contactDirection.z) *
                        Mathf.Rad2Deg;

                    float angle = Mathf.SmoothDampAngle(
                        transform.eulerAngles.y,
                        targetAngle,
                        ref smoothTimeRotation,
                        turnSpeed);

                    // Rotates player agaisnt that direction
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);

                }
            }

            // Move direction perpendicular to player's back
            Vector3 moveDirection =
                Quaternion.Euler(0f, -90, 0f) * transform.forward;

            // Pressing a direction with walls on the sides
            if ((input.Movement.x > 0 && wallsCollidersLeft.Length > 0) ||
                (input.Movement.x < 0 && wallsCollidersRight.Length > 0))
            {
                if (Vector3.Dot(cam.transform.forward, transform.forward) <= 0)
                {
                    controller.Move(
                    input.Movement.x *
                    moveDirection *
                    Time.fixedUnscaledDeltaTime);

                    InvertedAnimation = false;
                }
                else
                {
                    controller.Move(
                    -input.Movement.x *
                    moveDirection *
                    Time.fixedUnscaledDeltaTime);

                    InvertedAnimation = true;
                }
            }

            // If the player leaves the wall, cancels wall hug
            if (wallsColliders.Length == 0)
            {
                CancelWallHug();
            }
        }
    }

    protected virtual void OnWallHug(bool condition) => WallHug?.Invoke(condition);

    /// <summary>
    /// Event registered on PlayerMovement.
    /// </summary>
    public event Action<bool> WallHug;
}