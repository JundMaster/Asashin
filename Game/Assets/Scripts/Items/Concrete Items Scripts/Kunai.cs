using UnityEngine;

/// <summary>
/// Class responsible for handling kunai behaviour.
/// </summary>
public class Kunai : ItemBehaviour
{
    // Components
    private CinemachineTarget target;

    // Movement variables
    private float rotation;
    [SerializeField] protected float speed;
    
    // Transform from a target if there's one
    private Transform kunaiCurrentTarget;
    private Transform player;

    // Hit variables
    [SerializeField] protected float hitRange;
    [SerializeField] protected float timeUntilVanish;
    private float timePassed;

    private void Start()
    {
        target = FindObjectOfType<CinemachineTarget>();
        player = FindObjectOfType<Player>().transform;

        kunaiCurrentTarget = null;

        // If there's an active target, the kunai will have that same target
        // If there's no active target, the kunai will just go towards where
        // the player is facing.
        if (target.CurrentTarget.gameObject.activeSelf)
        {
            kunaiCurrentTarget = target.CurrentTarget;
        }
        else
        {
            Vector3 position = player.transform.position;
            Vector3 direction = position + player.transform.forward * 10f;

            transform.LookAt(direction);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);

            kunaiCurrentTarget = null;
        }

        rotation = 0;
        timePassed = Time.time;

        Execute();
    }

    private void FixedUpdate()
    {
        if (kunaiCurrentTarget)
        {
            transform.LookAt(target.CurrentTarget);
            transform.position += transform.forward * Time.fixedDeltaTime * speed;
        }
        else if (kunaiCurrentTarget == null)
        {
            // Only goes forward until it hits something.
            transform.eulerAngles +=
                new Vector3(0, 0, rotation++ * Time.fixedDeltaTime);

            transform.position += transform.forward * Time.fixedDeltaTime * speed;
        }
    }

    private void Update()
    {
        // Explodes kunai after x seconds
        if (Time.time - timePassed > timeUntilVanish) Hit(null);
    }

    private void OnTriggerEnter(Collider other)
    {
        Hit(other);
    }

    protected virtual void Hit(Collider other)
    {
        if (other != null)
        {
            Collider[] collisions =
                Physics.OverlapSphere(other.ClosestPoint(transform.position), hitRange);

            foreach (Collider col in collisions)
            {
                if (col.gameObject.TryGetComponent(out IDamageable enemy))
                {
                    enemy.TakeDamage(playerStats.RangedDamage);
                }
            }
        }

        Destroy(gameObject);
    }

    public override void Execute()
    {
        playerStats.Kunais--;
        base.Execute();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, hitRange);
    }
}
