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

    public bool Walking { get; private set; }
    public bool Sprinting { get; private set; }
    public bool Performing { get; private set; }

    // Movement Variables
    public Vector3 Direction { get; private set; }
    private float hVel;
    private float vVel;
    private Vector3 moveDirection;
    private float runSpeed;
    public float MovementSpeed { get; private set; }

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
        attack = GetComponent<PlayerMeleeAttack>();
        roll = GetComponent<PlayerRoll>();
        useItem = GetComponent<PlayerUseItem>();
        block = GetComponent<PlayerBlock>();
        jump = GetComponent<PlayerJump>();
    }

    private void Start()
    {
        TurnSmooth = values.TurnSmooth;
        Walking = false;
        Sprinting = false;
        MovementSpeed = 0f;
    }

    private void OnEnable()
    {
        slowMotion.SlowMotionEvent += ChangeTurnSmoothValue;
        input.Walk += () => Walking = !Walking;
        input.Sprint += HandleSprint;
        attack.LightMeleeAttack += () => Walking = false;
        attack.LightMeleeAttack += () => Sprinting = false;
        attack.StrongMeleeAttack += () => Walking = false;
        attack.StrongMeleeAttack += () => Sprinting = false;
        roll.Roll += () => Walking = false;
        roll.Roll += () => Sprinting = false;
        useItem.UsedItemDelay += () => Walking = false;
        useItem.UsedItemDelay += () => Sprinting = false;
    }

    private void OnDisable()
    {
        slowMotion.SlowMotionEvent += ChangeTurnSmoothValue;
        input.Walk -= () => Walking = !Walking;
        input.Sprint -= HandleSprint;
        attack.LightMeleeAttack -= () => Walking = false;
        attack.LightMeleeAttack -= () => Sprinting = false;
        attack.StrongMeleeAttack -= () => Walking = false;
        attack.StrongMeleeAttack -= () => Sprinting = false;
        roll.Roll -= () => Walking = false;
        roll.Roll -= () => Sprinting = false;
        useItem.UsedItemDelay -= () => Walking = false;
        useItem.UsedItemDelay -= () => Sprinting = false;
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
        if (attack.Performing == false && useItem.Performing == false )
        {
            Direction = new Vector3(input.Movement.x, 0f, input.Movement.y);
        }
        else
        {
            Direction = Vector3.zero;
        }

        if (block.Performing || !jump.IsGrounded())
        {
            Walking = false;
            Sprinting = false;
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
        if (Direction.magnitude > 0.01f && block.Performing == false &&
            roll.Performing == false)
        {
            // Moves controllers towards the moveDirection set on Rotation()
            if (Walking && Sprinting == false)
            {
                controller.Move(
                    moveDirection.normalized * values.WalkingSpeed * Time.fixedUnscaledDeltaTime);
                MovementSpeed = 
                    (moveDirection.normalized * values.WalkingSpeed * Time.fixedUnscaledDeltaTime).magnitude;
            }
            else if (Walking == false && Sprinting)
            {
                controller.Move(
                    moveDirection.normalized * values.SprintSpeed * Time.fixedUnscaledDeltaTime);
                MovementSpeed = 
                    (moveDirection.normalized * values.SprintSpeed * Time.fixedUnscaledDeltaTime).magnitude;
            }
            else
            {
                controller.Move(
                    moveDirection.normalized * values.Speed * Time.fixedUnscaledDeltaTime);
                MovementSpeed = 
                    (moveDirection.normalized * values.Speed * Time.fixedUnscaledDeltaTime).magnitude;
            }
        }
    }

    private void HandleSprint (YesOrNo condition)
    {
        if (condition == YesOrNo.Yes) Sprinting = true;
        else Sprinting = false;
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
