using UnityEngine;

/// <summary>
/// Class responsible for pressure plate's kunai behaviour.
/// </summary>
public class PressurePlateKunaiBehaviour : KunaiBehaviour
{
    [SerializeField] protected LayerMask hittableLayersWithPlayer;
    private const int HITTABLELAYERWITHPLAYER = 22;

    public override Transform KunaiCurrentTarget { get; set; }
    private Transform playerTarget;
    private PlayerRoll playerRoll;

    /// <summary>
    /// Happens on start.
    /// Changes layers and sets direction of kunai.
    /// </summary>
    /// <param name="player">Player transform.</param>
    public override void OnStart(Transform player)
    {
        playerTarget = GameObject.FindGameObjectWithTag("playerTarget").transform;
        playerRoll = player.GetComponent<PlayerRoll>();

        ParentKunai.HittableLayers = hittableLayersWithPlayer;
        gameObject.layer = HITTABLELAYERWITHPLAYER;

        transform.LookAt(playerTarget);
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
                // Else if the player isn't blocking
                if (playerRoll.Performing)
                {
                    // Changes to a layer that ignores player
                    gameObject.layer = 30;
                }
                else
                {
                    damageableBody?.TakeDamage(
                        5,
                        TypeOfDamage.EnemyRanged);

                    Destroy(gameObject);
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
