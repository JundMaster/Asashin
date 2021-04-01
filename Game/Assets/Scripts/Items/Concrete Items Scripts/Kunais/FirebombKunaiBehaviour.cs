using UnityEngine;

/// <summary>
/// Class responsible for FirebombKunai's behaviour.
/// </summary>
public class FirebombKunaiBehaviour : FriendlyKunaiBehaviour
{
    // Explosion variables
    [SerializeField] private Transform explosionPosition;
    [SerializeField] private GameObject explosion;
    [SerializeField] private float explosionRange;

    /// <summary>
    /// Happens after kunai hits something.
    /// </summary>
    /// <param name="damageableBody">Damageable body.</param>
    /// <param name="collider">Collider of the collision.</param>
    /// <param name="player">Player transform.</param>
    public override void Hit(IDamageable damageableBody, Collider collider, Transform player)
    {
        Instantiate(explosion, explosionPosition.transform.position, Quaternion.identity);

        // Gets all enemies around explosion range
        Collider[] collisions =
            Physics.OverlapSphere(transform.position, explosionRange, enemyLayer);

        foreach (Collider col in collisions)
        {
            // If it's an enemy, the kundai does damage = to firebombKunaiDamage
            // Only does damage if it hits an enemy
            if (col.gameObject.TryGetComponent(out IDamageable body) &&
                col.gameObject.TryGetComponent(out Enemy en))
            {
                body.TakeDamage(playerStats.FirebombKunaiDamage);
            }
        }  

        Destroy(gameObject);
    }

    /// <summary>
    /// Happens when IUsableItem execute is called.
    /// </summary>
    /// <param name="baseClass">Kunai base class.</param>
    public override void Execute(ItemBehaviour baseClass)
    {
        playerStats.FirebombKunais--;
        baseClass.Execute();
    }
}
