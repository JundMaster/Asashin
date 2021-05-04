﻿using UnityEngine;
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
    public Animator Anim { get; private set; }
    private PlayerRoll roll;
    private PlayerUseItem useItem;
    private CinemachineTarget target;
    private PlayerStats stats;
    private PlayerBlock block;
    private PlayerInteract interact;
    private PlayerWallHug wallHug;
    private PlayerMovement movement;
    private SlowMotionBehaviour slowMotion;
    private PlayerValuesScriptableObj values;
    private PlayerSounds sounds;

    // Weapon
    [SerializeField] private SphereCollider sword;

    // Trail
    [SerializeField] private ParticleSystem[] particles;

    // Layers
    [SerializeField] private LayerMask hittableLayers;

    // Swordhit
    [SerializeField] private GameObject swordHitPrefab;

    public bool Performing { get; set; }
    public bool InInstantKill { get; set; }

    // Rotation
    private float smoothTimeRotation;
    private float turnSmooth;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        Anim = GetComponent<Animator>();
        useItem = GetComponent<PlayerUseItem>();
        roll = GetComponent<PlayerRoll>();
        target = FindObjectOfType<CinemachineTarget>();
        stats = GetComponent<PlayerStats>();
        block = GetComponent<PlayerBlock>();
        interact = GetComponent<PlayerInteract>();
        wallHug = GetComponent<PlayerWallHug>();
        movement = GetComponent<PlayerMovement>();
        slowMotion = FindObjectOfType<SlowMotionBehaviour>();
        values = GetComponent<Player>().Values;
        sounds = GetComponent<PlayerSounds>();
    }

    private void Start()
    {
        turnSmooth = values.TurnSmooth;

        foreach (ParticleSystem particle in particles)
            particle.Stop();
    }

    private void OnEnable()
    {
        input.MeleeLightAttack += MeleeLightAttack;
        slowMotion.SlowMotionEvent += ChangeTurnSmoothValue;
    }

    private void OnDisable()
    {
        input.MeleeLightAttack -= MeleeLightAttack;
        slowMotion.SlowMotionEvent -= ChangeTurnSmoothValue;
    }

    public void ComponentUpdate()
    {

    }

    public void ComponentFixedUpdate()
    {
        if (Performing && target.Targeting == false && 
            input.Movement.magnitude > 0 && InInstantKill == false)
        {
            // Rotates towards player's pressing direction
            Vector3 movement = new Vector3(input.Movement.x, 0, input.Movement.y);
            float targetAngle = Mathf.Atan2(movement.x, movement.z) *
                Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetAngle,
                ref smoothTimeRotation,
                turnSmooth);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    /// <summary>
    /// Changes turn smooth value after slow motion.
    /// </summary>
    private void ChangeTurnSmoothValue(SlowMotionEnum condition)
    {
        // Changes turn value on player and changes camera update mode
        if (condition == SlowMotionEnum.SlowMotion)
            turnSmooth = values.TurnSmoothInSlowMotion;
        else
            turnSmooth = values.TurnSmooth;
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
                transform.RotateTo(target.CurrentTarget.position);
            }

            OnMeleeAttack();
        }
    }

    /// <summary>
    /// Turns trail to false. Runs on Animation event.
    /// </summary>
    private void TurnTrailToTrue()
    {
        foreach (ParticleSystem particle in particles)
            particle.Play();
    }

    /// <summary>
    /// Turns trail to false. Runs on Animation event.
    /// </summary>
    public void TurnTrailToFalse()
    {
        foreach (ParticleSystem particle in particles)
            particle.Stop();
    }

    /// <summary>
    /// Checks attack collision through Animation event.
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
                sounds.PlaySound(Sound.Hit);
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
    /// Triggers light melee attack. Normal or instant kill Animation
    /// depending on the condition.
    /// </summary>
    protected virtual void OnMeleeAttack()
    {
        if (movement.Walking == false)
        {
            // Normal attack Anim
            OnLightMeleeAttack(true);
            return;
        }
        // ELSE
        // happens if the player is walking or an enemy is in blindness state

        // Creates a list with all enemies around the player
        List<Transform> allEnemies = new List<Transform>();
        Collider[] enemies =
                Physics.OverlapSphere(transform.position, 2f, enemyLayers);

        // If there are enemies
        if (enemies.Length > 0)
        {
            // If enemy has an Enemy script
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].gameObject.TryGetComponent(out EnemyBase allens))
                {
                    allEnemies.Add(enemies[i].transform);
                }
            }

            // Orders array with all VISIBLE enemies by distance
            Transform[] organizedEnemiesByDistance =
                allEnemies.OrderBy(i => (i.position - transform.position).magnitude).ToArray();

            Vector3 dir = transform.Direction(organizedEnemiesByDistance[0]);

            // If the player is facing the enemy's forward, meaning it's
            // looking towards him while he has is back turned
            if (Vector3.Dot(organizedEnemiesByDistance[0].forward, transform.forward) > 0f)
            {
                // Only happens if the player is BEHIND the enemy, prevents
                // from doing instant kill while the enemy is behind the player
                if (Vector3.Angle(dir, transform.forward) < 25)
                {
                    // Instant kill Anim.
                    OnLightMeleeAttack(false);
                    return;
                }
                // Else
                // Normal attack Anim
                OnLightMeleeAttack(true);
                return;
            }
            // Else
            // Normal attack Anim
            OnLightMeleeAttack(true);
            return;
        }
        // Else
        // Normal attack Anim
        OnLightMeleeAttack(true);
    }

    protected virtual void OnLightMeleeAttack(bool condition) =>
        LightMeleeAttack?.Invoke(condition);

    /// <summary>
    /// Event registered on PlayerAnimations.
    /// Event registered on PlayerMovement.
    /// /// <summary>
    /// Triggers light melee attack. Normal or instant kill Animation
    /// depending on the condition.
    /// </summary>
    /// <param name="condition">If true, it triggers normal attacks, else
    /// it triggers instant kill Animation.</param>
    /// </summary>
    public event Action<bool> LightMeleeAttack;

    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(sword.transform.position + sword.center,
                sword.radius);
    }
    #endregion
}
