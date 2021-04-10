using UnityEngine;

/// <summary>
/// Class responsible for handling wall hug behaviour.
/// </summary>
public class PlayerWallHug : MonoBehaviour, IAction
{
    [SerializeField] private LayerMask walls;

    // Components
    private PlayerMovement movement;
    private PlayerInputCustom input;
    private PlayerJump jump;
    private PlayerMeleeAttack attack;
    private PlayerUseItem useItem;
    private Animator anim;
    private PlayerBlock block;
    private CharacterController controller;

    private Collider[] wallsColliders;
    private Vector3 contactDirection;

    public bool Performing { get; private set; }

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        input = FindObjectOfType<PlayerInputCustom>();
        jump = GetComponent<PlayerJump>();
        attack = GetComponent<PlayerMeleeAttack>();
        anim = GetComponent<Animator>();
        useItem = GetComponent<PlayerUseItem>();
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
            if (attack.Performing == false &&
                jump.IsGrounded() &&
                useItem.Performing == false &&
                block.Performing == false)
            {
                if (wallsColliders.Length > 0)
                {
                    Performing = true;

                    if (Physics.Raycast(new Ray(transform.position, transform.forward), out RaycastHit hit, walls))
                    {
                        contactDirection = hit.normal;
                    }
                }
            }
        }
        else
            Performing = false;
    }

    public void ComponentUpdate()
    {
        wallsColliders =
                Physics.OverlapSphere(transform.position, 0.75f, walls);

        if (Performing)
        {
            // Rotation
            float targetAngle = 
                Mathf.Atan2(contactDirection.x, contactDirection.z) * 
                Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, -90, 0f) * transform.forward;

            controller.Move(
                    input.Movement.x * moveDirection * Time.fixedUnscaledDeltaTime);

            ////////////////////////////////////////////////////////////////////

            if (wallsColliders.Length == 0)
            {
                anim.applyRootMotion = false;
                Performing = false;
            }
        }

        anim.SetFloat("WallHugSpeed", input.Movement.x);
        anim.SetBool("BotWallHug", Performing);
    }

    public void ComponentFixedUpdate()
    {

    }
}