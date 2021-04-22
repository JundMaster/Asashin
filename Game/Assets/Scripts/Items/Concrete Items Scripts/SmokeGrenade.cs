using UnityEngine;

public class SmokeGrenade : ItemBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject smokePrefab;
    [SerializeField] private float smokeRange;

    public override void Execute()
    {
        Vector3 spawnPosition = new Vector3
            (playerStats.transform.position.x,
            playerStats.transform.position.y + 1,
            playerStats.transform.position.z);

        Instantiate(smokePrefab, spawnPosition, Quaternion.identity);

        // Gets enemies around the grenade
        Collider[] collisions =
                Physics.OverlapSphere(transform.position, smokeRange, enemyLayer);

        foreach (Collider col in collisions)
        {
            // Only applies if it's an an enemy
            if (col.gameObject.TryGetComponent(out EnemySimple en))
            {
                // Blinds enemies
                en.BlindEnemy();
            }
        }

        playerStats.SmokeGrenades--;
        base.Execute();
    }
}
