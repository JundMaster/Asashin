using UnityEngine;

/// <summary>
/// Class responsible for handling kunai behaviour.
/// </summary>
public class Kunai : ItemBehaviour, IFindPlayer
{
    [SerializeField] private KunaiBehaviour behaviour;
    public KunaiBehaviour Behaviour => behaviour;
    /// //////////////////////////////////////

    // Movement variables
    private float rotation;
    [SerializeField] protected float speed;
    
    private Transform player;

    // Hit variables
    [SerializeField] protected float hitRange;
    [SerializeField] protected float timeUntilVanish;
    private float timePassed;

    // Particles Hit
    [SerializeField] private GameObject hitParticles;

    // Layers to collide with kunai
    [SerializeField] private LayerMask hittableLayers;
    

    private void Start()
    {
        player = FindObjectOfType<Player>().transform;
        behaviour.KunaiCurrentTarget = null;

        // Decides direction of kunai
        behaviour.OnStart(player);

        rotation = 0;
        timePassed = Time.time;

        behaviour.Execute(this);
    }

    private void FixedUpdate()
    {
        // If kunai has an active target
        if (behaviour.KunaiCurrentTarget)
        {
            transform.LookAt(behaviour.KunaiCurrentTarget);
            transform.position += transform.forward * Time.fixedDeltaTime * speed;
        }
        else if (behaviour.KunaiCurrentTarget == null)
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

    /// <summary>
    /// What happens when the kunai hits something.
    /// </summary>
    /// <param name="other">Parameter with collision collider.</param>
    protected virtual void Hit(Collider other)
    {
        if (other != null)
        {
            Collider[] collisions =
                Physics.OverlapSphere(other.ClosestPoint(transform.position), hitRange, hittableLayers);

            // Checks if it's the parent or not
            Transform bodyToHit = null;
            if (collisions[0].transform.parent != null)
                bodyToHit = collisions[0].transform.parent;
            else
                bodyToHit = collisions[0].transform;

            // If this object can receive damage
            if (bodyToHit.TryGetComponent(out IDamageable damageableBody))
            {
                // Trigers behaviour hit()
                behaviour.Hit(damageableBody, bodyToHit, player);
            }
            // Else, if it's not an IDamageable interface, the object is destroyed
            else
            {
                Destroy(gameObject);
            }
            Instantiate(hitParticles, transform.position, Quaternion.identity);
        }
        else
        {
            behaviour.Hit(null, null, null);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, hitRange);
    }

    new public void FindPlayer()
    {
        player = FindObjectOfType<Player>().transform;
    }
}
