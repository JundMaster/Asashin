using UnityEngine;
using System.Collections;

/// <summary>
/// Scriptable object responsible for handling shinobi's aggressive state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Shinobi Aggressive State")]
public sealed class EnemyShinobiAggressiveState : EnemyBossAbstractState
{
    [Range(1, 3)] [SerializeField] private float closeToPlayerRange;
    [Tooltip("Distance to stay from player while attacking")]
    [Range(1, 1.5f)] [SerializeField] private float distanceFromPlayer;
    [SerializeField] private LayerMask playerLayer;

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

    private SphereCollider weapon;
    private PlayerRoll playerRoll;
    private EnemyAnimationEvents animationEvents;

    [Tooltip("Must wait this time to alert enemies again")]
    [Range(0f, 5f)][SerializeField] private float delayTimerToInvokeAlert;
    private float currentTimerToInvokeAlert;

    [Header("Timer to change to reinforcements state")]
    [SerializeField] private float timeToChangeState;
    private float timeEnteringThisState;

    /// <summary>
    /// Runs once on start. Gets enemy variables.
    /// </summary>
    public override void Start()
    {
        base.Start();

        if (enemy.Player != null)
            playerRoll = enemy.Player.GetComponent<PlayerRoll>();

        animationEvents = enemy.GetComponentInChildren<EnemyAnimationEvents>();
        weapon = enemy.WeaponCollider;
    }

    /// <summary>
    /// Runs every time the enemy enters this state. Registers to events and
    /// sets variables.
    /// </summary>
    public override void OnEnter()
    {
        attacking = false;
        attackingAnimation = false;
        currentTimerToInvokeAlert = 0;

        timeEnteringThisState = Time.time;

        if (enemy.Player != null)
            playerRoll = enemy.Player.GetComponent<PlayerRoll>();

        animationEvents.Hit += WeaponHit;
    }

    /// <summary>
    /// Runs every fixed update. Checks the distance between the enemy and the
    /// player and attacks or gets close to the player depending on that
    /// distance.
    /// </summary>
    /// <returns>IState.</returns>
    public override IState FixedUpdate()
    {
        base.FixedUpdate();

        if (die)
            return enemy.DeathState;

        if (Time.time - timeEnteringThisState > timeToChangeState)
            return enemy.ReinforcementsState;

        float currentDistanceFromPlayer =
            Vector3.Distance(playerTarget.position, myTarget.position);

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

        return enemy.AggressiveState;
    }

    /// <summary>
    /// Happens once when the enemy leaves this state.
    /// </summary>
    public override void OnExit()
    {
        if (attackingCoroutine != null)
            enemy.StopCoroutine(attackingCoroutine);
        attackingAnimation = false;
        attacking = false;
        agent.isStopped = false;

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
            yield return wfd;

            // Only checks this once in a while, so it won't do it every frame
            if (Time.time - currentTimerToInvokeAlert > delayTimerToInvokeAlert)
            {
                enemy.AlertSurroundings();
                currentTimerToInvokeAlert = Time.time;
            }

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
        // If player just started rolling , while the enemy is attacking, it 
        // means the player was able to dodge, so it will trigger slow motion.
        if (playerRoll != null)
        {
            if (playerRoll.PerformingTime > 0 &&
                playerRoll.PerformingTime < 0.5f)
            {
                playerRoll.OnDodge();
            }

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
    /// Moves towards the player.
    /// </summary>
    /// <param name="distance">Distance from player.</param>
    /// <returns>Returns true if it's near the player.
    /// Returns false if it's still moving towards the player.</returns>
    protected override bool IsCloseToPlayer(float distance)
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