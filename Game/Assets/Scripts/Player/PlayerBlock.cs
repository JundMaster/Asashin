using UnityEngine;

/// <summary>
/// Class responsible for handling player's block.
/// </summary>
public class PlayerBlock : MonoBehaviour, IAction
{
    // Components
    private PlayerInputCustom input;
    private Animator anim;
    private PlayerMeleeAttack attack;
    private PlayerJump jump;
    private PlayerRoll roll;
    private PlayerUseItem useItem;
    private PlayerMovement movement;
    private CinemachineTarget cineTarget;

    public bool Performing { get; private set; }

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        anim = GetComponent<Animator>();
        attack = GetComponent<PlayerMeleeAttack>();
        jump = GetComponent<PlayerJump>();
        roll = GetComponent<PlayerRoll>();
        useItem = GetComponent<PlayerUseItem>();
        movement = GetComponent<PlayerMovement>();
        cineTarget = FindObjectOfType<CinemachineTarget>();
    }

    private void OnEnable()
    {
        input.Block += Block;
    }

    private void OnDisable()
    {
        input.Block -= Block;
    }

    /// <summary>
    /// Sets block state to true or false.
    /// </summary>
    /// <param name="condition">Parameter to check if the player
    /// pressed block or released block.</param>
    private void Block(bool condition)
    {
        if (condition == true)
        {
            if (attack.Performing == false && jump.Performing == false &&
                roll.Performing == false && useItem.Performing == false)
            {
                Performing = true;
                anim.applyRootMotion = true;
            }
        }
        else
        {
            Performing = false;
            anim.applyRootMotion = false;
        }
    }

    public void ComponentFixedUpdate()
    {
        if (Performing)
        {
            RotationToCurrentTarget();
        }
    }

    public void ComponentUpdate()
    {
        //
    }

    /// <summary>
    /// Rotates the character towards something.
    /// </summary>
    private void RotationToCurrentTarget()
    {
        if (cineTarget.Targeting)
        {
            transform.LookAt(cineTarget.CurrentTarget);
            transform.eulerAngles = 
                new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }
}
