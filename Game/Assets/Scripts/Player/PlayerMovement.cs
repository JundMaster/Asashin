using System.Collections;
using System.Collections.Concurrent;
using UnityEngine;

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
    private PlayerValuesScriptableObj values;
    private PlayerMeleeAttack attack;
    private PlayerUseItem useItem;
    private PlayerRoll roll;
    private PlayerBlock block;
    private PlayerJump jump;
    private PlayerWallHug wallHug;
    private Animator anim;
    private CinemachineTarget cineTarget;

    public bool Walking { get; private set; }
    public bool Sprinting { get; private set; }
    public bool Performing { get; private set; }

    // Movement Variables
    public Vector3 Direction { get; private set; }
    private Vector3 moveDirection;
    public float MovementSpeed { get; private set; }
    private bool stopMovementAfterWallHug;

    // Rotation Variables
    public float TurnSmooth { get; set; }
    private float smoothTimeRotation;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = FindObjectOfType<PlayerInputCustom>();
        mainCamera = Camera.main.transform;
        slowMotion = FindObjectOfType<SlowMotionBehaviour>();
        values = GetComponent<Player>().Values;
        attack = GetComponent<PlayerMeleeAttack>();
        roll = GetComponent<PlayerRoll>();
        useItem = GetComponent<PlayerUseItem>();
        block = GetComponent<PlayerBlock>();
        jump = GetComponent<PlayerJump>();
        wallHug = GetComponent<PlayerWallHug>();
        anim = GetComponent<Animator>();
        cineTarget = FindObjectOfType<CinemachineTarget>();
    }

    private void Start()
    {
        TurnSmooth = values.TurnSmooth;
        Walking = false;
        Sprinting = false;
        MovementSpeed = 0f;
        stopMovementAfterWallHug = false;
    }

    private void OnEnable()
    {
        input.StopMoving += HandleStopMovement;
        slowMotion.SlowMotionEvent += ChangeTurnSmoothValue;
        input.Walk += () => Walking = !Walking;
        input.Sprint += HandleSprint;
        attack.LightMeleeAttack += StopWalkingOnAttack;
        roll.Roll += () => Walking = false;
        useItem.UsedItemDelay += () => Walking = false;
        wallHug.WallHug += StopMovementAfterWallHug;
    }

    private void OnDisable()
    {
        input.StopMoving -= HandleStopMovement;
        slowMotion.SlowMotionEvent += ChangeTurnSmoothValue;
        input.Walk -= () => Walking = !Walking;
        input.Sprint -= HandleSprint;
        attack.LightMeleeAttack -= StopWalkingOnAttack;
        roll.Roll -= () => Walking = false;
        useItem.UsedItemDelay -= () => Walking = false;
        wallHug.WallHug -= StopMovementAfterWallHug;
    }

    /// <summary>
    /// Stops movement after wall hugging while the camera is blending.
    /// </summary>
    /// <param name="condition">False if cancelled wall hug.</param>
    private void StopMovementAfterWallHug(bool condition)
    {
        if (condition == false) StartCoroutine(StopMovementAfterWallHugCoroutine());
    }

    /// <summary>
    /// Stops movement after wall hugging.
    /// </summary>
    private IEnumerator StopMovementAfterWallHugCoroutine()
    {
        float currentTime = Time.time;
        // Has this timer to confirm it canceled movement for at least these seconds
        while (Time.time - currentTime < 0.5f)
        {
            Direction = Vector3.zero;
            MovementSpeed = 0;
            stopMovementAfterWallHug = true;
            yield return null;

            // Has to contain this while too to confirm the camera stopped blending
            while (cineTarget.IsBlending())
            {
                Direction = Vector3.zero;
                MovementSpeed = 0;
                stopMovementAfterWallHug = true;
                yield return null;
            }
        }
        stopMovementAfterWallHug = false;
    }

    /// <summary>
    /// Starts stop walking coroutine.
    /// </summary>
    /// <param name="condition"></param>
    private void StopWalkingOnAttack(bool condition) =>
        StartCoroutine(StopWalkingOnAttackCoroutine());

    /// <summary>
    /// Cancels walking after fixed update, so it can check if it hit an enemy,
    /// etc. before canceling the walk.
    /// </summary>
    /// <returns>Wait for fixed update.</returns>
    private IEnumerator StopWalkingOnAttackCoroutine()
    {
        yield return new WaitForFixedUpdate();
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
            // Waits for animation to end
        }
        Walking = false;
    }

    public void ComponentFixedUpdate()
    {
        Movement();

        if (wallHug.Performing == false)
            Rotation();
    }

    /// <summary>
    /// Changes turn smooth value after slow motion.
    /// </summary>
    private void ChangeTurnSmoothValue(SlowMotionEnum condition)
    {
        // Changes turn value on player and changes camera update mode
        if (condition == SlowMotionEnum.SlowMotion)
            TurnSmooth = values.TurnSmoothInSlowMotion;
        else 
            TurnSmooth = values.TurnSmooth;
    }

    public void ComponentUpdate()
    {
        if (attack.Performing == false && useItem.Performing == false &&
            wallHug.Performing == false && stopMovementAfterWallHug == false)
        {
            Direction = new Vector3(input.Movement.x, 0f, input.Movement.y);
        }
        else
        {
            Direction = Vector3.zero;
        }

        // Cancels sneak
        if (block.Performing || !jump.IsGrounded())
        {
            Walking = false;
        }
    }

    /// <summary>
    /// Stops movement speed float.
    /// </summary>
    private void HandleStopMovement()
    {
        MovementSpeed = 0f;
    }

    /// <summary>
    /// Handles movement.
    /// </summary>
    private void Movement()
    {
        if (Direction.magnitude > 0.01f && block.Performing == false &&
            roll.Performing == false && wallHug.Performing == false &&
            stopMovementAfterWallHug == false)
        {
            // Moves controllers towards the moveDirection set on Rotation()
            if (Walking && Sprinting == false)
            {
                controller.Move(
                    moveDirection.normalized * values.WalkingSpeed * Time.fixedUnscaledDeltaTime);

                MovementSpeed = values.WalkingSpeed;
            }
            else if (Walking == false && Sprinting)
            {
                controller.Move(
                    moveDirection.normalized * values.SprintSpeed * Time.fixedUnscaledDeltaTime);

                MovementSpeed = values.SprintSpeed;
            }
            else
            {
                controller.Move(
                    moveDirection.normalized * values.Speed * Time.fixedUnscaledDeltaTime);

                MovementSpeed = values.Speed;
            }
        }
    }

    private void HandleSprint (bool condition)
    {
        if (condition == true)
        {
            Walking = false;
            Sprinting = true;
        }
        else
        {
            Sprinting = false;
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
