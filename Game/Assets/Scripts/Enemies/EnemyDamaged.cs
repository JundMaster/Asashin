using UnityEngine;

public class EnemyDamaged : MonoBehaviour, IFindPlayer
{
    private EnemyStats stats;
    private Player player;
    private Rigidbody rb;

    private void Awake()
    {
        stats = GetComponent<EnemyStats>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        stats.TookDamage += TakeImpact;
    }

    private void OnDisable()
    {
        stats.TookDamage -= TakeImpact;
    }

    private void TakeImpact()
    {
        Vector3 dir = player.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

        rb.isKinematic = false;
        rb.AddForce(-dir * 555f, ForceMode.Impulse);
        rb.isKinematic = true;
    }

    /// <summary>
    /// Rotates enemy towards the player.
    /// </summary>
    private void RotateEnemy()
    {
        Vector3 dir = player.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
    }

    public void FindPlayer()
    {
        player = FindObjectOfType<Player>();
    }

    public void PlayerLost()
    {
        // Left blank o npurpose
    }
}
