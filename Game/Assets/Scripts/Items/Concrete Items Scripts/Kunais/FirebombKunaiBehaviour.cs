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
    /// 
    /// </summary>
    /// <param name="damageableBody"></param>
    /// <param name="bodyTohit"></param>
    /// <param name="player"></param>
    public override void Hit(IDamageable damageableBody, Transform bodyTohit, Transform player)
    {
        Instantiate(explosion, explosionPosition.transform.position, Quaternion.identity);

        if (bodyTohit != null)
        {
            Collider[] collisions =
                Physics.OverlapSphere(bodyTohit.position, explosionRange);

            foreach (Collider col in collisions)
            {
                // Only does damage if it hits an enemy
                if (col.gameObject.TryGetComponent(out IDamageable body) &&
                    col.gameObject.TryGetComponent(out Enemy en))
                {
                    body.TakeDamage(playerStats.FirebombKunaiDamage);
                }
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
