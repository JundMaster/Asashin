using System.Collections;
using UnityEngine;

/// <summary>
/// Scriptable object for controlling enemy aggressive state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Common Aggressive State")]
public class EnemyAggressiveState : EnemyState
{
    [Header("Player check ranges")]
    [Range(1,30)][SerializeField] private float checkForPlayerRange;
    [Range(1, 3)][SerializeField] private float closeToPlayerRange;
    [Tooltip("Distance to stay from player while attacking")]
    [Range(1, 1.5f)][SerializeField] private float distanceFromPlayer;
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

    [Header("Rotation speed")]
    [Range(0.01f, 2)] [SerializeField] private float turnSpeed;
    private float smoothTimeRotation;

    [Header("Prefab to spawn on melee attack hit")]
    [SerializeField] private GameObject meleeHitParticles;

    // Components
    private Animator anim;
    private SphereCollider weapon;
    private PlayerRoll playerRoll;
    private GameObject visionCone;

    /// <summary>
    /// Happens once on start.
    /// </summary>
    public override void Start()
    {
        base.Start();

        visionCone = enemy.VisionCone;
        playerRoll = enemy.Player.GetComponent<PlayerRoll>();
        anim = enemy.Anim;
        weapon = enemy.WeaponCollider;
    }

    /// <summary>
    /// Happens once while entering this state. Sets player's currently fighting
    /// to true.
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        enemy.PlayerCurrentlyFighting = true;
        attacking = false;
        attackingAnimation = false;
        agent.isStopped = false;

        if (playerTarget != null ) 
            agent.SetDestination(playerTarget.position);

        if (playerRoll == null) 
            playerRoll = enemy.Player.GetComponent<PlayerRoll>();

        if (visionCone.activeSelf)
            visionCone.SetActive(false);

        enemy.WeaponHit += WeaponHit;
    }

    /// <summary>
    /// Runs on fixed update. If the player is in enemy's range, the enemy will
    /// attack, else it will change its state to LostPlayerState.
    /// </summary>
    /// <returns></returns>
    public override IState FixedUpdate()
    {
        base.FixedUpdate();

        if (instantKill)
            return enemy.DeathState;

        float currentDistanceFromPlayer =
            Vector3.Distance(playerTarget.position, myTarget.position);

        // Checks if player is in max range
        if (IsPlayerInMyRange(currentDistanceFromPlayer))
        {
            if (myTarget.CanSee(playerTarget, collisionLayers))
            {
                // Checks and moves enemy close to player
                if (IsCloseToPlayer(currentDistanceFromPlayer))
                {
                // If there are no obstacles between the enemy and the player
                
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
        enemy.PlayerCurrentlyFighting = false;
        if (attackingCoroutine != null)
            enemy.StopCoroutine(attackingCoroutine);
        attackingAnimation = false;
        attacking = false;
        agent.isStopped = false;

        enemy.WeaponHit -= WeaponHit;
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
            yield return wfd;

            // Checks if player is still in range
            if (Vector3.Distance(playerTarget.position, myTarget.position) >
                closeToPlayerRange)
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
            // After delay from attack animation

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
        // If player is rolling, while the enemy is attacking, it means the
        // player was able to dodge, so it will trigger slow motion.
        if (playerRoll.Performing)
        {
            playerRoll.OnDodge();
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
        if (distance > closeToPlayerRange)
        {
            Vector3 dir = 
                myTarget.position.InvertedDirection(playerTarget.position);

            if (attackingAnimation == false)
            {
                agent.SetDestination(
                    playerTarget.position + dir * distanceFromPlayer);
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
