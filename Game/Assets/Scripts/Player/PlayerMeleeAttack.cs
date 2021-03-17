using UnityEngine;
using System;

/// <summary>
/// Class responsible for handling player attack.
/// </summary>
public class PlayerMeleeAttack : MonoBehaviour, IAction
{
    // Components
    private PlayerInputCustom input;
    private Animator anim;
    private PlayerRoll roll;
    private PlayerUseItem useItem;
    private CinemachineTarget target;
    private PlayerStats stats;
    private PlayerBlock block;
    private PlayerJump jump;

    // Weapon
    [SerializeField] private SphereCollider sword;

    // Trail
    [SerializeField] private ParticleSystem[] particles;

    // Layers
    [SerializeField] private LayerMask hittableLayers;

    // Swordhit
    [SerializeField] private GameObject swordHitPrefab;

    public bool Performing { get; private set; }

    private void Awake()
    {
        input = GetComponent<PlayerInputCustom>();
        anim = GetComponent<Animator>();
        useItem = GetComponent<PlayerUseItem>();
        roll = GetComponent<PlayerRoll>();
        target = FindObjectOfType<CinemachineTarget>();
        stats = GetComponent<PlayerStats>();
        block = GetComponent<PlayerBlock>();
        jump = GetComponent<PlayerJump>();
    }

    private void Start()
    {
        foreach(ParticleSystem particle in particles)
            particle.Stop();
    }

    private void OnEnable()
    {
        input.MeleeLightAttack += MeleeLightAttack;
        input.MeleeStrongAttack += MeleeStrongAttack;
        input.MeleeLightAttack += MeleeAirAttack;
        input.MeleeStrongAttack += MeleeAirAttack;
    }

    private void OnDisable()
    {
        input.MeleeLightAttack -= MeleeLightAttack;
        input.MeleeStrongAttack -= MeleeStrongAttack;
        input.MeleeLightAttack -= MeleeAirAttack;
        input.MeleeStrongAttack -= MeleeAirAttack;
    }

    public void ComponentUpdate()
    {

    }

    public void ComponentFixedUpdate()
    {
        
    }

    private void MeleeAirAttack()
    {
        if (jump.IsGrounded() == false)
        {
            OnAirAttack();
            Performing = true;
            anim.applyRootMotion = true;
        }
    }

    /// <summary>
    /// Handles light attack.
    /// </summary>
    private void MeleeLightAttack()
    {
        if (roll.Performing == false && useItem.Performing == false &&
            block.Performing == false)
        {
            if (target.Targeting)
            {
                // Rotates player to target
                transform.LookAt(target.CurrentTarget);
                transform.eulerAngles = new Vector3(
                    0f, transform.eulerAngles.y, transform.eulerAngles.z);
            }

            Performing = true;

            anim.applyRootMotion = true;

            OnLightMeleeAttack();
        }
    }

    /// <summary>
    /// Handles strong attack.
    /// </summary>
    private void MeleeStrongAttack()
    {
        if (roll.Performing == false && useItem.Performing == false &&
            block.Performing == false)
        {
            if (target.Targeting)
            {
                transform.LookAt(target.CurrentTarget);
                transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z);
            }

            Performing = true;

            anim.applyRootMotion = true;

            OnStrongMeleeAttack();
        }
    }

    /// <summary>
    /// Turns trail to false. Runs on animation event.
    /// </summary>
    private void TurnTrailToTrue()
    {
        foreach (ParticleSystem particle in particles)
            particle.Play();
    }

    /// <summary>
    /// Turns trail to false. Runs on animation event.
    /// </summary>
    private void TurnTrailToFalse()
    {
        foreach (ParticleSystem particle in particles)
            particle.Stop();
    }

    /// <summary>
    /// Turns of root motion. Runs on animation event.
    /// Only turns of root motion if the player isn't continuing the combo.
    /// </summary>
    private void TurnOffRootMotion()
    {
        // If next animation is not an attack it removes root motion
        if (anim.GetNextAnimatorStateInfo(0).IsName("BotLightMelee2") == false &&
            anim.GetNextAnimatorStateInfo(0).IsName("BotLightMelee3") == false &&
            anim.GetNextAnimatorStateInfo(0).IsName("BotStrongMelee3") == false)
        {
            anim.ResetTrigger("MeleeLightAttack");
            anim.ResetTrigger("MeleeStrongAttack");

            Performing = false;

            anim.applyRootMotion = false;
        }
    }

    /// <summary>
    /// Checks attack collision through animation event.
    /// </summary>
    public void CheckLightAttackCollision()
    {
        Collider[] swordCol = 
            Physics.OverlapSphere(
                sword.transform.position + sword.center, sword.radius, hittableLayers);

        if (swordCol.Length > 0)
        {
            for (int i = 0; i < swordCol.Length; i++)
            {
                if (swordCol[i].transform.parent.gameObject.TryGetComponent(out IDamageable enemy))
                {
                    enemy.TakeDamage(stats.LightDamage);
                    break;
                }
            }
            Instantiate(swordHitPrefab, swordCol[0].ClosestPoint(sword.transform.position), Quaternion.identity);
        }
    }

    /// <summary>
    /// Checks attack collision through animation event.
    /// </summary>
    public void CheckStrongAttackCollision()
    {
        Collider[] swordCol =
            Physics.OverlapSphere(
                sword.transform.position + sword.center, sword.radius * 1.5f, hittableLayers);

        if (swordCol.Length > 0)
        {
            for (int i = 0; i < swordCol.Length; i++)
            {
                if (swordCol[i].gameObject.TryGetComponent(out IDamageable enemy))
                {
                    enemy.TakeDamage(stats.StrongDamage);
                    break;
                }
            }
            Instantiate(swordHitPrefab, sword.transform.position + sword.center, Quaternion.identity);
        }
    }

    /// <summary>
    /// Checks attack collision through animation event.
    /// </summary>
    public void CheckAirAttackCollision()
    {
        Collider[] swordCol =
            Physics.OverlapSphere(
                sword.transform.position + sword.center, sword.radius * 2.5f, hittableLayers);

        if (swordCol.Length > 0)
        {
            for (int i = 0; i < swordCol.Length; i++)
            {
                if (swordCol[i].gameObject.TryGetComponent(out IDamageable enemy))
                {
                    enemy.TakeDamage(stats.StrongDamage);
                    break;
                }

                Instantiate(swordHitPrefab, sword.transform.position + sword.center, Quaternion.identity);
            }
        }
    }

    protected virtual void OnLightMeleeAttack() => LightMeleeAttack?.Invoke();

    protected virtual void OnStrongMeleeAttack() => StrongMeleeAttack?.Invoke();

    protected virtual void OnAirAttack() => AirAttack?.Invoke();

    /// <summary>
    /// Event registered on PlayerAnimations.
    /// Event registered on PlayerMovement.
    /// </summary>
    public event Action LightMeleeAttack;

    /// <summary>
    /// Event registered on PlayerAnimations.
    /// Event registered on PlayerMovement.
    /// </summary>
    public event Action StrongMeleeAttack;

    /// <summary>
    /// Event registered on PlayerAnimations.
    /// </summary>
    public event Action AirAttack;
}
