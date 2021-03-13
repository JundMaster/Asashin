using UnityEngine;

/// <summary>
/// Class responsible for FirebombKunai's behaviour.
/// </summary>
public class FirebombKunai : ItemBehaviour
{
    // Components
    private CinemachineTarget target;
        
    // Movement variables
    private float rotation;
    [SerializeField] private float speed;
    [SerializeField] private float explosionRange;

    // Transform from a target if there's one
    private Transform kunaiCurrentTarget;
    private Transform player;

    // Explosion variables
    [SerializeField] private Transform explosionPosition;
    [SerializeField] private GameObject explosion;
    [SerializeField] private float timeUntilExplosion;
    private float timePassed;

    [SerializeField] private ListOfItems itemType;
    public override ListOfItems ItemType { get => itemType; }

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
        if (Time.time - timePassed > timeUntilExplosion) Hit(null);
    }


    private void OnTriggerEnter(Collider other)
    {
        Hit(other);
    }

    private void Hit(Collider other)
    {
        Instantiate(explosion, explosionPosition.transform.position, Quaternion.identity);

        if (other != null)
        {
            Collider[] collisions = 
                Physics.OverlapSphere(other.ClosestPoint(transform.position), explosionRange);

            foreach (Collider col in collisions)
            {
                if (col.gameObject.TryGetComponent(out IDamageable enemy))
                {
                    enemy.TakeDamage(playerStats.FirebombKunaiDamage);
                }
            }
     
            Destroy(gameObject);
        }
    }

    public override void Execute()
    {
        playerStats.FirebombKunais--;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
