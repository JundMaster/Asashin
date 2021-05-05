using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for FirebombKunai's behaviour.
/// </summary>
public class FirebombKunaiBehaviour : FriendlyKunaiBehaviour
{
    // Explosion variables
    [SerializeField] private GameObject explosion;
    [SerializeField] private float explosionRange;

    private byte numberOfExplosions;

    private void Start()
    {
        numberOfExplosions = 0;
    }

    /// <summary>
    /// Happens after kunai hits something.
    /// </summary>
    /// <param name="damageableBody">Damageable body.</param>
    /// <param name="collider">Collider of the collision.</param>
    /// <param name="player">Player transform.</param>
    public override void Hit(IDamageable damageableBody, Collider collider, Transform player)
    {
        if (numberOfExplosions == 0)
        {
            numberOfExplosions++;

            if (collider != null)
            {
                Vector3 expPos =
                    new Vector3(
                        transform.position.x, 
                        transform.position.y + 1f, 
                        transform.position.z) + 
                        transform.Direction(player) * 1.5f;

                Instantiate(explosion, expPos, Quaternion.identity);
            }
        }

        HashSet<IDamageable> bodiesToDamage = new HashSet<IDamageable>();

        // Gets all enemies around explosion range
        Collider[] collisions =
            Physics.OverlapSphere(transform.position, explosionRange, enemyLayer);

        // Gets every damageable body and adds it to a hashset
        foreach (Collider col in collisions)
        {
            // If it's an enemy, the kundai does damage = to firebombKunaiDamage
            // Only does damage if the collider has an enemy
            if (col.gameObject.TryGetComponent(out IDamageable body) &&
                col.gameObject.TryGetComponent(out EnemyBase en))
            {
                bodiesToDamage.Add(body);
            }
        }

        // Damages all bodies
        foreach(IDamageable body in bodiesToDamage)
            body.TakeDamage(playerStats.FirebombKunaiDamage, TypeOfDamage.PlayerRanged);

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
