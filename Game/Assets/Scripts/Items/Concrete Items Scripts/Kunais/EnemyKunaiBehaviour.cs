using UnityEngine;

/// <summary>
/// Class that handles enemy kunai behaviours.
/// </summary>
public class EnemyKunaiBehaviour : KunaiBehaviour
{
    [SerializeField] protected LayerMask hittableLayersWithPlayer;

    public override Transform KunaiCurrentTarget { get; set; }
    private Transform playerTarget;
    private PlayerBlock playerBlock;
    private PlayerAnimations playerAnim;

    // Variables that checks if the kunai was reflected, so it doesn't
    // damage the enemy before being reflected
    private bool isReflected;

    private void Update()
    {
        // Can hit enemies if it's reflected, else it doesn't hit enemies.
        if (isReflected)
        {
            ParentKunai.HittableLayers = ParentKunai.HittableLayersWithEnemy;
            gameObject.layer = 23;
        }
        else
        {
            ParentKunai.HittableLayers = hittableLayersWithPlayer;
            gameObject.layer = 22;
        }
    }

    /// <summary>
    /// Happens on start.
    /// </summary>
    /// <param name="player">Player transform.</param>
    public override void OnStart(Transform player)
    {
        playerBlock = player.GetComponent<PlayerBlock>();
        playerAnim = player.GetComponent<PlayerAnimations>();
        playerTarget = GameObject.FindGameObjectWithTag("playerTarget").transform;
        PlayerMovement movement = player.GetComponent<PlayerMovement>();

        // If the player is moving, the enemy will throw the kunai to the
        // front of the player, else, it will throw it to the player's position
        if (movement.MovementSpeed > 0)
        {
            if (Vector3.Distance(transform.position, playerTarget.transform.position) > 15)
                transform.LookAt(playerTarget.transform.position + playerTarget.forward * 3f);
            else if (Vector3.Distance(transform.position, playerTarget.transform.position) > 10f)
                transform.LookAt(playerTarget.transform.position + playerTarget.forward * 2f);
            else if (Vector3.Distance(transform.position, playerTarget.transform.position) > 5f)
                transform.LookAt(playerTarget.transform.position + playerTarget.forward * 1.3f);
            else
                transform.LookAt(playerTarget.transform.position + playerTarget.forward * 0.5f);
        }
        else
            transform.LookAt(playerTarget);

        KunaiCurrentTarget = null;
        isReflected = false;
    }

    /// <summary>
    /// Happens after kunai hits something.
    /// </summary>
    /// <param name="damageableBody">Damageable body.</param>
    /// <param name="collider">Collider of the collision.</param>
    /// <param name="player">Player transform.</param>
    public override void Hit(IDamageable damageableBody, Collider collider, Transform player)
    {
        if (collider != null)
        {
            // If it collides with player layer
            if (collider.gameObject.layer == 11)
            {
                // and player is blocking
                if (playerBlock.Performing)
                {
                    // If the player is facing the enemy direction
                    // it reflects the kunai
                    if (Vector3.Angle(
                        ParentEnemy.transform.position - player.position,
                        player.forward) < 50f)
                    {
                        KunaiCurrentTarget = ParentEnemy.MyTarget;
                        playerAnim.TriggerBlockReflect();
                        isReflected = true;
                    }
                    // If the player is blocking but not facing the enemy
                    else
                    {
                        damageableBody?.TakeDamage(
                            ParentEnemy.GetComponent<Stats>().RangedDamage,
                            TypeOfDamage.EnemyRanged);
                        Destroy(gameObject);
                    }
                }
                // Else if the player isn't blocking
                else
                {
                    damageableBody?.TakeDamage(
                        ParentEnemy.GetComponent<Stats>().RangedDamage,
                        TypeOfDamage.EnemyRanged);

                    Destroy(gameObject);
                }
            }
            // Else if it's not the player (meaning the kunai was reflected)
            else
            {
                damageableBody?.TakeDamage(
                    ParentEnemy.GetComponent<Stats>().RangedDamage,
                    TypeOfDamage.PlayerRanged);

                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
