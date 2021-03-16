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

    public bool Performing { get; private set; }

    // Movement Variables
    public Vector3 Direction { get; private set; }
    private float hVel;
    private float vVel;
    private Vector3 moveDirection;
    private float runSpeed;

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
    }

    private void Start()
    {
        TurnSmooth = values.TurnSmooth;
    }

    private void OnEnable()
    {
        slowMotion.SlowMotionEvent += ChangeTurnSmoothValue;
    }

    private void OnDisable()
    {
        slowMotion.SlowMotionEvent += ChangeTurnSmoothValue;
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
        // Updates movement direction variable
        if ( attack.Performing == false &&
            slowMotion.Performing == false && useItem.Performing == false )
        {
            /*
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
            */
            Direction = new Vector3(input.Movement.x, 0f, input.Movement.y);
        }
        else if (attack.Performing == false &&
            slowMotion.Performing && useItem.Performing == false )
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
        if (Direction.magnitude > 0.01f && block.Performing == false &&
            roll.Performing == false)
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
