using UnityEngine;

/// <summary>
/// Class responsible for FirebombKunai's behaviour.
/// </summary>
public class FirebombKunai : Kunai
{
    // Explosion variables
    [SerializeField] private Transform explosionPosition;
    [SerializeField] private GameObject explosion;

    /// <summary>
    /// What happens when the kunai hits something.
    /// </summary>
    /// <param name="other">Parameter with collision collider.</param>
    protected override void Hit(Collider other)
    {
        Instantiate(explosion, explosionPosition.transform.position, Quaternion.identity);

        if (other != null)
        {
            Collider[] collisions =
                Physics.OverlapSphere(other.ClosestPoint(transform.position), hitRange);

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

    public override void Execute()
    {
        playerStats.FirebombKunais--;
        FindObjectOfType<ItemUIParent>().UpdateAllItemUI();
    }
}
