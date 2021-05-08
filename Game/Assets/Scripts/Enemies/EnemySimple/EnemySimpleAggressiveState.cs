﻿using System.Collections;
using UnityEngine;

/// <summary>
/// Scriptable object for controlling enemy aggressive state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Common Aggressive State")]
public class EnemySimpleAggressiveState : EnemySimpleAbstractState
{
    [Header("Player check ranges")]
    [Range(1,30)][SerializeField] private float checkForPlayerRange;
    private readonly float CLOSETOPLAYERRANGE = 2;
    private readonly float DISTANCEFROMPLAYER = 1;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask collisionLayers;

    [Header("Max time to change states after losing the player")]
    [Range(0.01f, 5)] [SerializeField] private float lostPlayerMaxTime;
    private float lostPlayerTime;

    [Header("Attacking Times")]
    [Range(1, 4)] [SerializeField] private float attackingDelay;
    [Range(1, 4)] [SerializeField] private float afterAttackDelay;
    private bool attacking;
    private bool attackingAnimation;
    private IEnumerator attackingCoroutine;

    [Header("Rotation speed when close to the player (less means faster)")]
    [Range(0.1f, 1f)] [SerializeField] private float turnSpeed;
    private float smoothTimeRotation;

    [Header("Prefab to spawn on melee attack hit")]
    [SerializeField] private GameObject meleeHitParticles;

    [Tooltip("Must wait this time to alert enemies again")]
    [Range(0f, 5f)] [SerializeField] private float delayTimerToInvokeAlert;
    private float currentTimerToInvokeAlert;

    // Components
    private SphereCollider weapon;
    private PlayerRoll playerRoll;
    private GameObject visionCone;
    private EnemyAnimationEvents animationEvents;

    /// <summary>
    /// Happens once on start.
    /// </summary>
    public override void Start()
    {
        base.Start();

        visionCone = enemy.VisionConeGameObject;
        if (enemy.Player != null)
            playerRoll = enemy.Player.GetComponent<PlayerRoll>();

        animationEvents = enemy.GetComponentInChildren<EnemyAnimationEvents>();
        weapon = enemy.Weapon;
    }

    /// <summary>
    /// Happens once while entering this state. Sets player's currently fighting
    /// to true.
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        enemy.PlayerCurrentlyFighting++;
        enemy.InCombat = true;
        attacking = false;
        attackingAnimation = false;
        agent.isStopped = false;
        currentTimerToInvokeAlert = 0;

        if (playerTarget != null ) 
            agent.SetDestination(playerTarget.position);

        if (playerRoll == null) 
            playerRoll = enemy.Player.GetComponent<PlayerRoll>();

        if (visionCone.activeSelf)
            visionCone.SetActive(false);

        animationEvents.Hit += WeaponHit;
    }

    /// <summary>
    /// Runs on fixed update. If the player is in enemy's range, the enemy will
    /// attack, else it will change its state to LostPlayerState.
    /// </summary>
    /// <returns></returns>
    public override IState FixedUpdate()
    {
        base.FixedUpdate();

        if (die)
            return enemy.DeathState;

        float currentDistanceFromPlayer =
            Vector3.Distance(playerTarget.position, myTarget.position);

        // Checks if player is in max range
        if (IsPlayerInMyRange(currentDistanceFromPlayer))
        {
            // If there are no obstacles between the enemy and the player
            if (myTarget.CanSee(playerTarget, collisionLayers))
            {
                // Checks and moves enemy close to player
                if (IsCloseToPlayer(currentDistanceFromPlayer))
                { 
                    // If the enemy is not inside attacking anim, it rotates
                    if (attackingAnimation == false)
                        enemy.transform.RotateToSmoothly(
                            playerTarget.position, ref smoothTimeRotation, 
                            turnSpeed);

                    // If the enemy has not performing an attack yet
                    // It starts attacking coroutine
                    if (attacking == false)
                    {
                        attacking = true;
                        attackingCoroutine = AttackPlayerCoroutine();
                        enemy.StartCoroutine(attackingCoroutine);
                    }
                }
                // If player leaves range, it cancels the attacking coroutine
                else
                {
                    if (attackingCoroutine != null)
                    {
                        attacking = false;
                        attackingAnimation = false;
                        enemy.StopCoroutine(attackingCoroutine);
                    }
                }

                lostPlayerTime = Time.time;
                return enemy.AggressiveState;
            }

            // If the enemy is not able to see the player anymore, it will start
            // a timer to check if it should change states.
            if (Time.time - lostPlayerTime > lostPlayerMaxTime)
                return enemy.LostPlayerState ?? enemy.PatrolState;

            return enemy.AggressiveState;
        }

        return enemy.LostPlayerState ?? enemy.PatrolState;    
    }

    /// <summary>
    /// Happens once when the enemy leaves this state.
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();
        enemy.PlayerCurrentlyFighting--;
        if (attackingCoroutine != null)
            enemy.StopCoroutine(attackingCoroutine);
        attackingAnimation = false;
        attacking = false;
        agent.isStopped = false;
        agent.speed = runningSpeed;

        animationEvents.Hit -= WeaponHit;
    }

    /// <summary>
    /// Coroutine that controls enemy's attack. Waits for a delay, triggers
    /// attack animations and checks if there is a gameobject to do damage to.
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackPlayerCoroutine()
    {
        YieldInstruction wfd = new WaitForSeconds(attackingDelay);
        YieldInstruction wfdaa = new WaitForSeconds(afterAttackDelay);

        // While in range with the player
        while (attacking)
        {
            agent.speed = 0;
            yield return wfd;

            // Only checks this once in a while, so it won't do it every frame
            if (Time.time - currentTimerToInvokeAlert > delayTimerToInvokeAlert)
            {
                enemy.AlertSurroundings();
                currentTimerToInvokeAlert = Time.time;
            }

            // Checks if player is still in range
            if (Vector3.Distance(playerTarget.position, myTarget.position) >
                CLOSETOPLAYERRANGE)
            {
                attackingCoroutine = null;
                break;
            }

            // Starts atacking animation
            attackingAnimation = true;
            anim.SetTrigger("MeleeAttack");

            // WeaponHit() happens here with animation events from
            // EnemyAnimationEvents

            yield return wfdaa;
            // Delay after attack

            attackingAnimation = false;
        }
    }

    /// <summary>
    /// Tries to hit something while atacking. Checks collisions, checks if
    /// the collision has a damgeablebody, damages it if the player isn't
    /// blocking. If the player dodges this attack, slow motion is triggered
    /// if possible.
    /// </summary>
    private void WeaponHit()
    {
        // If player just started rolling , while the enemy is attacking, it 
        // means the player was able to dodge, so it will trigger slow motion.
        // Checks if it's greater than 1.5 in case it's still transitioning
        // between animations
        if (playerRoll != null)
        {
            if (playerRoll.PerformingTime > 0 && 
                playerRoll.PerformingTime < 0.5f ||
                playerRoll.PerformingTime > 1.5f)
            {
                playerRoll.OnDodge();
            }

            // If player is rolling ignores everything else
            if (playerRoll.Performing)
                return;
        }
        
        // Collisions of the melee weapon
        Collider[] swordCollider = Physics.OverlapSphere(
            weapon.transform.position,
            weapon.radius,
            playerLayer);

        // Checks if this object or parent has a damageable body
        GameObject body = null;
        if (swordCollider.Length > 0)
            body = GetDamageableBody(swordCollider[0].gameObject);
  
        // If this object can receive damage
        if (body != null)
        {
            if (body.TryGetComponent(out IDamageable damageableBody) &&
                body.TryGetComponent(out PlayerBlock playerBlock))
            {
                // If player is blocking
                if (playerBlock.Performing)
                {
                    // If the player is facing the enemy's forward
                    // (player on blocks if he's basically facing
                    // the enemy)
                    // This means the player successfully blocked
                    if (Vector3.Dot(
                        enemy.transform.forward, playerTarget.forward) < -0.5f)
                    {
                        damageableBody?.TakeDamage(
                            0f, TypeOfDamage.PlayerBlockDamage);

                        anim.SetTrigger("BlockReflected");

                        // Pushes enemy back
                        TakeImpact();
                    }
                    // If the player was NOT able to block
                    else
                    {
                        damageableBody?.TakeDamage(
                            stats.LightDamage, TypeOfDamage.EnemyMelee);
                    }
                }
                // Player isn't blocking
                else
                {
                    damageableBody?.TakeDamage(
                        stats.LightDamage, TypeOfDamage.EnemyMelee);
                }

                Instantiate(
                meleeHitParticles,
                playerTarget.position,
                Quaternion.identity);
            }
        }
    }

    /// <summary>
    /// Checks if player is in enemy's range.
    /// </summary>
    /// <param name="distance">Distance from player.</param>
    /// <returns>Returns true if player is still in enemy's range,
    /// else returns false.</returns>
    private bool IsPlayerInMyRange(float distance)
    {
        if (distance <= checkForPlayerRange)
            return true;
 
        return false;
    }
    
    /// <summary>
    /// Moves towards the player.
    /// </summary>
    /// <param name="distance">Distance from player.</param>
    /// <returns>Returns true if it's near the player.
    /// Returns false if it's still moving towards the player.</returns>
    private bool IsCloseToPlayer(float distance)
    {
        // If the enemy is not close to the player
        if (distance > CLOSETOPLAYERRANGE)
        {
            // Only happens if the enemy is not doing something else
            // for example, it doesn't happen while atacking
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
            {
                // If enemy is reaching player's radius
                if (distance < CLOSETOPLAYERRANGE + 0.5f)
                    agent.speed = Mathf.Lerp(
                        agent.speed, walkingSpeed, Time.fixedDeltaTime * 50);
                else
                    agent.speed = Mathf.Lerp(
                        agent.speed, runningSpeed, Time.fixedDeltaTime * 50);

                Vector3 dir =
                    myTarget.position.InvertedDirection(playerTarget.position);

                if (attackingAnimation == false)
                {
                    agent.SetDestination(
                        playerTarget.position + dir * DISTANCEFROMPLAYER);
                }
            }
            return false;
        }
        // Else if the enemy is close to the player
        return true;
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
        if (damageableBody != null) 
            return col.gameObject;

        else if (col.gameObject.transform.parent != null)
        {
            GetDamageableBody(col.transform.parent.gameObject);
        }

        return null;
    }
}
