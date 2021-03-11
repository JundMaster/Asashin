using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for handling player attack.
/// </summary>
public class PlayerMeleeAttack : MonoBehaviour, IAction
{
    // Components
    private PlayerInputCustom input;
    private Animator anim;
    private PlayerMovement movement;
    private PlayerJump jump;
    private PlayerRoll roll;
    private CinemachineTarget target;
    private PlayerStats stats;

    // Weapon
    [SerializeField] private SphereCollider sword;

    // Layers
    [SerializeField] private LayerMask hittableLayers;

    // Swordhit
    [SerializeField] private GameObject swordHitPrefab;

    private void Awake()
    {
        input = GetComponent<PlayerInputCustom>();
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        jump = GetComponent<PlayerJump>();
        roll = GetComponent<PlayerRoll>();
        target = FindObjectOfType<CinemachineTarget>();
        stats = GetComponent<PlayerStats>();
    }

    private void OnEnable()
    {
        input.MeleeLightAttack += MeleeLightAttack;
        input.MeleeStrongAttack += MeleeStrongAttack;
    }

    private void OnDisable()
    {
        input.MeleeLightAttack -= MeleeLightAttack;
        input.MeleeStrongAttack -= MeleeStrongAttack;
    }

    public void ComponentUpdate()
    {
        
    }

    public void ComponentFixedUpdate()
    {
        
    }

    /// <summary>
    /// Handles light attack.
    /// </summary>
    private void MeleeLightAttack()
    {
        if (target.Targeting)
        {
            // Rotates player to target
            transform.LookAt(target.CurrentTarget);
            transform.eulerAngles = new Vector3(
                0f, transform.eulerAngles.y, transform.eulerAngles.z);
        }

        movement.CanMove = false;
        jump.CanJump = false;
        roll.CanRoll = false;

        anim.applyRootMotion = true;

        anim.SetTrigger("MeleeLightAttack");
        anim.ResetTrigger("MeleeStrongAttack");
    }

    /// <summary>
    /// Handles strong attack.
    /// </summary>
    private void MeleeStrongAttack()
    {
        if (target.Targeting)
        {
            transform.LookAt(target.CurrentTarget);
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z);
        }

        movement.CanMove = false;
        jump.CanJump = false;
        roll.CanRoll = false;

        anim.applyRootMotion = true;

        anim.SetTrigger("MeleeStrongAttack");
        anim.ResetTrigger("MeleeLightAttack");
    }

    /// <summary>
    /// Turns of root motion. Runs on animation event.
    /// </summary>
    private void TurnOffRootMotion()
    {
        anim.ResetTrigger("MeleeLightAttack");
        anim.ResetTrigger("MeleeStrongAttack");

        movement.CanMove = true;
        jump.CanJump = true;
        roll.CanRoll = true;

        anim.speed = 1f;
        anim.applyRootMotion = false;
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
                if (swordCol[i].gameObject.TryGetComponent(out IDamageable enemy))
                {
                    enemy.TakeDamage(stats.LightDamage);       
                }
                Instantiate(swordHitPrefab, sword.transform.position + sword.center, Quaternion.identity);
            }
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
                }
                Instantiate(swordHitPrefab, sword.transform.position + sword.center, Quaternion.identity);
            }
        }
    }
}
