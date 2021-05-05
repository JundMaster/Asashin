using UnityEngine;

/// <summary>
/// Class that handles friendly kunai behaviours.
/// </summary>
public class FriendlyKunaiBehaviour : KunaiBehaviour
{
    [SerializeField] private GameObject kunaiSoundGameobject;

    public override Transform KunaiCurrentTarget { get; set; }

    [SerializeField] protected LayerMask enemyLayer;

    // Components
    private CinemachineTarget target;
    protected PlayerStats playerStats;

    /// <summary>
    /// Happens on start.
    /// </summary>
    /// <param name="player">Player transform.</param>
    public override void OnStart(Transform player)
    {
        target = FindObjectOfType<CinemachineTarget>();
        playerStats = FindObjectOfType<PlayerStats>();

        // If there's an active target, the kunai will have that same target.
        // If there's NO active target, the kunai will just go towards where
        // the player is facing.
        if (target.CurrentTarget.gameObject.activeSelf)
        {
            // Finds enemies around the current target
            Collider[] currentTargetPosition =
                Physics.OverlapSphere(target.CurrentTarget.transform.position, 0.5f, enemyLayer);

            // If enemy has an Enemy script
            for (int i = 0; i < currentTargetPosition.Length; i++)
            {
                if (currentTargetPosition[i].gameObject.TryGetComponent(out EnemyBase en))
                {
                    // Sets kunai target to enemy MyTarget
                    KunaiCurrentTarget = en.MyTarget;
                }
            }
        }
        else
        {
            Vector3 position = player.transform.position;
            Vector3 direction = position + player.transform.forward * 10f;

            transform.LookAt(direction);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);

            KunaiCurrentTarget = null;
        }
    }

    /// <summary>
    /// Happens after kunai hits something.
    /// </summary>
    /// <param name="damageableBody">Damageable body.</param>
    /// <param name="collider">Collider of the collision.</param>
    /// <param name="player">Player transform.</param>
    public override void Hit(IDamageable damageableBody, Collider collider, Transform player)
    {
        damageableBody?.TakeDamage(playerStats.RangedDamage, TypeOfDamage.PlayerRanged);

        // If it hits a wall, plays a sound
        if (damageableBody == null)
            Instantiate(kunaiSoundGameobject, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    /// <summary>
    /// Only executes if it's a player's kunai.
    /// Reduces the number of kunais and updates UI.
    /// </summary>
    public override void Execute(ItemBehaviour baseClass)
    {
        playerStats.Kunais--;
        baseClass.Execute();
    }
}
