using System.Collections;
using UnityEngine;

/// <summary>
/// Scriptable object for controlling enemy aggressive state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Aggressive State")]
public class EnemyAggressiveState : EnemyState
{
    [Header("Player check ranges")]
    [Range(1,30)][SerializeField] private float checkForPlayerRange;
    [Range(1, 3)][SerializeField] private float closeToPlayerRange;
    [Tooltip("Distance to stay from player while attacking")]
    [Range(1, 1.5f)][SerializeField] private float distanceFromPlayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask collisionLayers;

    [Header("Attacking Times")]
    [Range(1, 4)] [SerializeField] private float attackingDelay;
    [Range(1, 4)] [SerializeField] private float afterAttackDelay;
    private bool attacking;
    private bool attackingAnimation;

    [Header("Prefab to spawn on melee attack hit")]
    [SerializeField] private GameObject meleeHitParticles;

    // Components
    private Animator anim;
    private SphereCollider weapon;

    /// <summary>
    /// Happens once on start.
    /// </summary>
    public override void Start()
    {
        base.Start();

        attacking = false;
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
        agent.isStopped = false;
        if (playerTarget != null ) 
            agent.SetDestination(playerTarget.position);

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
            // Checks and moves enemy close to player
            if (IsCloseToPlayer(currentDistanceFromPlayer))
            {
                if (myTarget.CanSee(playerTarget, collisionLayers))
                {
                    if (attacking == false)
                    {
                        attacking = true;
                        enemy.transform.RotateTo(playerTarget.position);
                        enemy.StartCoroutine(AttackPlayerCoroutine());
                    }
                }
                else
                    return enemy.LostPlayerState ?? enemy.DefenseState ?? 
                        enemy.PatrolState;
                
            }
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
        attacking = false;
        enemy.PlayerCurrentlyFighting = false;
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
                break;
                
            // Starts atacking animation
            attackingAnimation = true;
            anim.SetTrigger("MeleeAttack");

            // WeaponHit() happens here with animation event

            yield return wfdaa;

            break;
        }

        // Also happens if the coroutine is cancelled
        attackingAnimation = false;
        attacking = false;
    }

    /// <summary>
    /// Tries to hit something while atacking. Checks collisions, checks if
    /// the collision has a damgeablebody, damages it if the player isn't
    /// blocking.
    /// </summary>
    private void WeaponHit()
    {
        // Collisions of the melee weapon
        Collider[] swordCollider = Physics.OverlapSphere(
            weapon.transform.position + weapon.center,
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
                        enemy.transform.forward, playerTarget.forward) >
                        -0.5f)
                    {
                        damageableBody?.TakeDamage(
                            stats.LightDamage, TypeOfDamage.EnemyMelee);
                    }
                    // If the player was NOT able to block
                    else
                    {
                        damageableBody?.TakeDamage(
                            0f, TypeOfDamage.PlayerBlockDamage);

                        // Pushes enemy back
                        TakeImpact();
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
