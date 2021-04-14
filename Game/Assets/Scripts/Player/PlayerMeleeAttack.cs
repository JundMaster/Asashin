using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Class responsible for handling player attack.
/// </summary>
public class PlayerMeleeAttack : MonoBehaviour, IAction
{
    [SerializeField] private LayerMask enemyLayers;

    // Components
    private PlayerInputCustom input;
    private Animator anim;
    private PlayerRoll roll;
    private PlayerUseItem useItem;
    private CinemachineTarget target;
    private PlayerStats stats;
    private PlayerBlock block;
    private PlayerInteract interact;
    private PlayerWallHug wallHug;
    private PlayerMovement movement;

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
        input = FindObjectOfType<PlayerInputCustom>();
        anim = GetComponent<Animator>();
        useItem = GetComponent<PlayerUseItem>();
        roll = GetComponent<PlayerRoll>();
        target = FindObjectOfType<CinemachineTarget>();
        stats = GetComponent<PlayerStats>();
        block = GetComponent<PlayerBlock>();
        interact = GetComponent<PlayerInteract>();
        wallHug = GetComponent<PlayerWallHug>();
        movement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        foreach(ParticleSystem particle in particles)
            particle.Stop();
    }

    private void OnEnable()
    {
        input.MeleeLightAttack += MeleeLightAttack;
    }

    private void OnDisable()
    {
        input.MeleeLightAttack -= MeleeLightAttack;
    }

    public void ComponentUpdate()
    {

    }

    public void ComponentFixedUpdate()
    {
        
    }

    /// <summary>
    /// Handles light attack.
    /// Attacks if the player is not rolling, not using an item, not blocking
    /// and not near an interectable object.
    /// </summary>
    private void MeleeLightAttack()
    {
        if (roll.Performing == false && useItem.Performing == false &&
            block.Performing == false && interact.InterectableObject == null &&
            wallHug.Performing == false)
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

            Debug.Log("attackou");
            OnMeleeAttack();
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
            anim.GetNextAnimatorStateInfo(0).IsName("BotLightMelee3") == false)
        {
            anim.ResetTrigger("MeleeLightAttack");

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
                sword.transform.position + sword.center, 
                sword.radius, 
                hittableLayers);

        // Checks if this object or parent has a damageable body
        GameObject body = null;
        if (swordCol.Length > 0)
            body = GetDamageableBody(swordCol[0].gameObject);

        // If this object can receive damage
        if (body != null)
        {
            if (body.TryGetComponent(out IDamageable damageableBody))
            {
                damageableBody?.TakeDamage(stats.LightDamage, TypeOfDamage.PlayerMelee);
            }
            else if (body.TryGetComponent(out IBreakable breakable))
            {
                breakable?.Execute();
            }

            Instantiate(
                swordHitPrefab, 
                sword.transform.position, 
                Quaternion.identity);
        }
    }

    /// <summary>
    /// Checks if this object has a damgeable body, if it doesn't it will check
    /// its parent until parent is null.
    /// </summary>
    /// <param name="col">Gameobject to check.</param>
    /// <returns>Returns a gameobject with IDamageable interface.</returns>
    private GameObject GetDamageableBody(GameObject col)
    {
        col.TryGetComponent(out IDamageable damageableBody);
        col.TryGetComponent(out IBreakable breakable);
        if (damageableBody != null || breakable != null) return col.gameObject;

        else if (col.gameObject.transform.parent != null)
        {
            GetDamageableBody(col.transform.parent.gameObject);
        }
        return null;
    }

    /// <summary>
    /// Triggers light melee attack. Normal or instant kill animation
    /// depending on the condition.
    /// </summary>
    protected virtual void OnMeleeAttack()
    {
        if (movement.Walking == false)
        {
            // Normal attack anim
            OnLightMeleeAttack(true);
            return;
        }
        // ELSE
        // happens if the player is walking or an enemy is in blindness state

        // Creates a list with all enemies around the player
        List<Transform> allEnemies = new List<Transform>();
        Collider[] enemies =
                Physics.OverlapSphere(transform.position, 2.5f, enemyLayers);

        // If there are enemies
        if (enemies.Length > 0)
        {
            // If enemy has an Enemy script
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].gameObject.TryGetComponent<Enemy>(out Enemy allens))
                {
                    allEnemies.Add(enemies[i].transform);
                }
            }

            // Orders array with all VISIBLE enemies by distance
            Transform[] organizedEnemiesByDistance =
                allEnemies.OrderBy(i => (i.position - transform.position).magnitude).ToArray();

            // If the player is facing the enemy's forward
            // (player on blocks if he's basically facing
            // the enemy)
            // This means the player successfully blocked
            if (Vector3.Dot(organizedEnemiesByDistance[0].forward, transform.forward) > 0.5f)
            {
                // Instant kill anim.

                OnLightMeleeAttack(false);
                return;
            }
            else
            {
                // Normal attack anim
                OnLightMeleeAttack(true);
                return;
            }
        }
        else
        {
            // Normal attack anim
            OnLightMeleeAttack(true);
        }
    }

    protected virtual void OnLightMeleeAttack(bool condition) =>
        LightMeleeAttack?.Invoke(condition);

    /// <summary>
    /// Event registered on PlayerAnimations.
    /// Event registered on PlayerMovement.
    /// /// <summary>
    /// Triggers light melee attack. Normal or instant kill animation
    /// depending on the condition.
    /// </summary>
    /// <param name="condition">If true, it triggers normal attacks, else
    /// it triggers instant kill animation.</param>
    /// </summary>
    public event Action<bool> LightMeleeAttack;
}
