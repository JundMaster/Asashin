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

    [Header("Attacking Times")]
    [Range(1, 4)] [SerializeField] private float attackingDelay;
    [Range(0, 2)] [SerializeField] private float afterAttackCollisionCheckTime;
    private bool attacking;
    private bool attackingAnimation;

    private Animator anim;

    /// <summary>
    /// Happens once on start.
    /// </summary>
    public override void Start()
    {
        base.Start();

        attacking = false;
        anim = enemy.Anim;
    }

    /// <summary>
    /// Happens once while entering this state. Sets player's currently fighting
    /// to true.
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        attacking = false;
        agent.isStopped = false;
        if (playerTarget != null ) 
            agent.SetDestination(playerTarget.position);

        enemy.PlayerCurrentlyFighting = true;

        stats.AnyDamageOnEnemy += TakeImpact;
    }

    /// <summary>
    /// Runs on fixed update. If the player is in enemy's range, the enemy will
    /// attack, else it will change its state to LostPlayerState.
    /// </summary>
    /// <returns></returns>
    public override IState FixedUpdate()
    {
        base.FixedUpdate();

        float currentDistanceFromPlayer =
            Vector3.Distance(playerTarget.position, myTarget.position);

        // Checks if player is in max range
        if (IsPlayerInMyRange(currentDistanceFromPlayer))
        {
            // Checks and moves enemy close to player
            if(IsCloseToPlayer(currentDistanceFromPlayer))
            {
                RotateEnemy();

                if (attacking == false)
                {
                    attacking = true;
                    enemy.StartCoroutine(AttackPlayerCoroutine());
                }
            }
            else
            {
                attacking = false;
            }
            return enemy.AggressiveState;
        }
        return enemy.LostPlayerState;    
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
        stats.AnyDamageOnEnemy -= TakeImpact;
    }

    /// <summary>
    /// Coroutine that controls enemy's attack. Waits for a delay, triggers
    /// attack animations and checks if there is a gameobject to do damage to.
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackPlayerCoroutine()
    {
        YieldInstruction wfd = new WaitForSeconds(attackingDelay);
        YieldInstruction wfac = 
            new WaitForSeconds(afterAttackCollisionCheckTime);

        // While in range with the player
        while (attacking)
        {
            yield return wfd;

            if (Vector3.Distance(playerTarget.position, myTarget.position) >
                closeToPlayerRange)
                break;
                
            attackingAnimation = true;
            anim.SetTrigger("MeleeAttack");

            yield return wfac;

            Collider[] swordCollider = Physics.OverlapSphere(myTarget.position, 1f, playerLayer);

            // Checks if this object or parent has a damageable body
            GameObject body = null;
            if (swordCollider.Length > 0)
                body = GetDamageableBody(swordCollider[0].gameObject);

            // If this object can receive damage
            if (body != null)
            {
                if (body.TryGetComponent(out IDamageable damageableBody) &&
                    body.TryGetComponent(out PlayerBlock block))
                {
                    damageableBody?.TakeDamage(
                        stats.LightDamage, TypeOfDamage.EnemyMelee);

                    Debug.Log(body.gameObject.name);
                }

                /*
                Instantiate(
                    swordHitPrefab,
                    sword.transform.position + sword.center,
                    Quaternion.identity);
                */
            }

            // Waits until throw kunai animation ends
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                Debug.Log("inside animation");
                yield return null;
            }
            attackingAnimation = false;
            attacking = false;
        }

        // Also happens if the coroutine is cancelled
        attackingAnimation = false;
        attacking = false;
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
            Vector3 dir = (myTarget.position - playerTarget.position).normalized;

            if (attacking == false)
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
    /// Rotates enemy towards the player.
    /// </summary>
    private void RotateEnemy()
    {
        Vector3 dir = playerTarget.transform.position - myTarget.position;
        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        enemy.transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
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
