using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Class responsible for handling player movement.
/// </summary>
public class PlayerMovement : MonoBehaviour, IAction
{
    // Components
    public CharacterController Controller { get; private set; }
    private PlayerInputCustom input;
    private Transform mainCamera;
    private SlowMotionBehaviour slowMotion;
    private Player player;
    private PlayerValuesScriptableObj values;
    private PlayerMeleeAttack attack;
    private PlayerUseItem useItem;
    private PlayerRoll roll;
    private PlayerBlock block;
    private PlayerWallHug wallHug;
    private CinemachineTarget cineTarget;
    private PlayerStats stats;

    public bool Sprinting { get; private set; }
    public bool Performing { get; private set; }

    // Hidden variables
    public bool Walking { get; set; }
    private bool hidden;
    public bool Hidden 
    {
        get => hidden;
        private set
        {
            hidden = value;
            OnHide(Hidden);
        }
    }
    private IEnumerator startHiddenCoroutine;
    private IEnumerator stopHiddenCoroutine;
    private Volume postProcessing;

    private Vector3 verticalVelocity;

    // Ground variables
    [SerializeField] private LayerMask groundLayer;

    // Movement Variables
    public Vector3 Direction { get; private set; }
    private Vector3 moveDirection;
    public float MovementSpeed { get; set; }
    private bool stopMovementAfterWallHug;

    // Layers
    private const byte PLAYERLAYER = 11;
    private const byte PLAYERHIDDENLAYER = 15;

    // Rotation Variables
    private float turnSmooth;
    private float smoothTimeRotation;

    private void Awake()
    {
        Controller = GetComponent<CharacterController>();
        input = FindObjectOfType<PlayerInputCustom>();
        mainCamera = Camera.main.transform;
        slowMotion = FindObjectOfType<SlowMotionBehaviour>();
        values = GetComponent<Player>().Values;
        player = GetComponent<Player>();
        attack = GetComponent<PlayerMeleeAttack>();
        roll = GetComponent<PlayerRoll>();
        useItem = GetComponent<PlayerUseItem>();
        block = GetComponent<PlayerBlock>();
        wallHug = GetComponent<PlayerWallHug>();
        cineTarget = FindObjectOfType<CinemachineTarget>();
        stats = GetComponent<PlayerStats>();
        postProcessing =
            GameObject.FindGameObjectWithTag("postProcessing").GetComponent<Volume>();
    }

    private void Start()
    {
        turnSmooth = values.TurnSmooth;
        Walking = false;
        Sprinting = false;
        Hidden = false;
        MovementSpeed = 0f;
        stopMovementAfterWallHug = false;
    }

    private void OnEnable()
    {
        input.StopMoving += HandleStopMovement;
        slowMotion.SlowMotionEvent += ChangeTurnSmoothValue;
        input.Walk += HandleWalk;
        input.Sprint += HandleSprint;
        roll.Roll += () => Walking = false;
        useItem.UsedItemDelay += () => Walking = false;
        wallHug.WallHug += StopMovementAfterWallHug;
        stats.TookDamage += () => Walking = false;
        Hide += HandleHidden;
    }

    private void OnDisable()
    {
        input.StopMoving -= HandleStopMovement;
        slowMotion.SlowMotionEvent -= ChangeTurnSmoothValue;
        input.Walk -= HandleWalk;
        input.Sprint -= HandleSprint;
        roll.Roll -= () => Walking = false;
        useItem.UsedItemDelay -= () => Walking = false;
        wallHug.WallHug -= StopMovementAfterWallHug;
        stats.TookDamage -= () => Walking = false;
        Hide -= HandleHidden;
    }

    public void ComponentFixedUpdate()
    {
        Movement();

        if (wallHug.Performing == false)
            Rotation();

        // Gravity
        if (IsGrounded())
        {
            verticalVelocity.y = -0.5f;
        }

        verticalVelocity.y += values.Gravity * Time.fixedUnscaledDeltaTime;
        Controller.Move(verticalVelocity * Time.fixedUnscaledDeltaTime);
    }

    public void ComponentUpdate()
    {
        if (attack.Performing == false && useItem.Performing == false &&
            wallHug.Performing == false && stopMovementAfterWallHug == false &&
            input.GetActionMap() == "Gameplay")
        {
            Direction = new Vector3(input.Movement.x, 0f, input.Movement.y);
        }
        else
        {
            Direction = Vector3.zero;
        }

        // Cancels sneak
        if (wallHug.Performing || block.Performing || !IsGrounded())
        {
            Walking = false;
            OnHide(false);
        }

        // Cancels sneaking if player is fighting
        if (player.PlayerCurrentlyFighting)
            Hidden = false;
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
                HandleStopMovement();
                stopMovementAfterWallHug = true;
                yield return null;
            }
        }
        stopMovementAfterWallHug = false;
    }

    /// <summary>
    /// Turns sneak on or off.
    /// </summary>
    private void HandleWalk(bool condition)
    {
        if (condition == true)
        {
            Walking = true;
            return;
        }
        else
        {
            Walking = false;
            OnHide(false);
        }
    }

    /// <summary>
    /// Changes turn smooth value after slow motion.
    /// </summary>
    private void ChangeTurnSmoothValue(SlowMotionEnum condition)
    {
        switch (condition)
        {
            case SlowMotionEnum.SlowMotion:
                turnSmooth = values.TurnSmoothInSlowMotion;
                break;
            case SlowMotionEnum.NormalTime:
                turnSmooth = values.TurnSmooth;
                break;
        }
    }


    /// <summary>
    /// Starts and stops hidden coroutines for post processing.
    /// </summary>
    /// <param name="hiddenCondition">Hidden condition.</param>
    private void HandleHidden(bool hiddenCondition)
    {
        if (hiddenCondition == true)
        {
            if (stopHiddenCoroutine != null)
            {
                StopCoroutine(stopHiddenCoroutine);
                stopHiddenCoroutine = null;
            }
            if (startHiddenCoroutine == null)
            {
                startHiddenCoroutine = StartHiddenCoroutine();
                StartCoroutine(startHiddenCoroutine);
            }
            return;
        }
        // else if not hidden
        if (startHiddenCoroutine != null)
        {
            StopCoroutine(startHiddenCoroutine);
            startHiddenCoroutine = null;
        }
        if (stopHiddenCoroutine == null)
        {
            stopHiddenCoroutine = StopHiddenCoroutine();
            StartCoroutine(stopHiddenCoroutine);
        }
    }

    /// <summary>
    /// Changes player's layer.
    /// Increments vignette value.
    /// </summary>
    /// <returns>Wait for fixed update.</returns>
    private IEnumerator StartHiddenCoroutine()
    {
        YieldInstruction wffu = new WaitForFixedUpdate();
        gameObject.layer = PLAYERHIDDENLAYER;

        // Post process variables
        if (postProcessing.profile.TryGet(out Vignette vignette))
        {
            vignette.active = true;
            while (vignette.intensity.value < 0.3f)
            {
                vignette.intensity.value += Time.fixedUnscaledDeltaTime;
                yield return wffu;
            }
        }
    }

    /// <summary>
    /// Changes player's layer.
    /// Decrements vignette value.
    /// </summary>
    /// <returns>Wait for fixed update.</returns>
    private IEnumerator StopHiddenCoroutine()
    {
        YieldInstruction wffu = new WaitForFixedUpdate();
        gameObject.layer = PLAYERLAYER;

        // Post process variables
        if (postProcessing.profile.TryGet(out Vignette vignette))
        {
            while (vignette.intensity.value > 0)
            {
                vignette.intensity.value -= Time.fixedUnscaledDeltaTime;
                yield return wffu;
            }
            vignette.active = false;
        }
    }

    /// <summary>
    /// Stops movement speed float.
    /// </summary>
    private void HandleStopMovement() =>
        MovementSpeed = 0f;

    /// <summary>
    /// Handles movement.
    /// </summary>
    private void Movement()
    {
        if (Direction.magnitude > 0.01f && block.Performing == false &&
            roll.Performing == false && wallHug.Performing == false &&
            stopMovementAfterWallHug == false && input.GetActionMap() == "Gameplay")
        {
            // Moves Controllers towards the moveDirection set on Rotation()
            // Transition to walking
            if (Walking && Sprinting == false)
            {
                Controller.Move(
                    moveDirection.normalized * values.WalkingSpeed * Time.fixedUnscaledDeltaTime);

                // Animator variable
                MovementSpeed = Mathf.Lerp(MovementSpeed, values.WalkingSpeed, Time.fixedUnscaledDeltaTime * 5);
            }
            // Transition to sprinting
            else if (Walking == false && Sprinting)
            {
                Controller.Move(
                    moveDirection.normalized * values.SprintSpeed * Time.fixedUnscaledDeltaTime);

                // Animator variable
                MovementSpeed = Mathf.Lerp(MovementSpeed, values.SprintSpeed, Time.fixedUnscaledDeltaTime * 5);
            }
            // If it's stopped and starts moving
            else
            {
                Controller.Move(
                    moveDirection.normalized * values.Speed * Time.fixedUnscaledDeltaTime);

                // Animator variable
                MovementSpeed = Mathf.Lerp(MovementSpeed, values.Speed, Time.fixedUnscaledDeltaTime * 8);
            }
        }
    }

    private void HandleSprint (bool condition)
    {
        if (condition == true)
        {
            Walking = false;
            Sprinting = true;
            OnHide(false);
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
                turnSmooth);

            // Rotates to that angle
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Sets moving Direction
            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) *
                Vector3.forward;
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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("highGrass"))
            if (Walking)
                Hidden = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("highGrass"))
            Hidden = false;
    }

    public virtual void OnHide(bool hiddenCondition) => 
        Hide?.Invoke(hiddenCondition);

    /// <summary>
    /// Event registered on PlayerMovement.
    /// </summary>
    public event Action<bool> Hide;
}
