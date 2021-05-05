using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for handling kunai behaviour.
/// </summary>
public class Kunai : ItemBehaviour, IFindPlayer
{
    [SerializeField] private KunaiBehaviour behaviour;
    public KunaiBehaviour Behaviour => behaviour;

    private IList<IDamageable> bodiesToDamage;

    // Movement variables
    private float rotation;
    [SerializeField] protected float speed;
    
    private Transform player;
    private PlayerRoll playerRoll;

    // Hit variables
    [SerializeField] protected float hitRange;
    [SerializeField] protected float timeUntilVanish;
    private float timePassed;

    // Particles Hit
    [SerializeField] private GameObject hitParticles;

    // Layers to collide with kunai
    [SerializeField] protected LayerMask hittableLayersWithEnemy;
    public LayerMask HittableLayersWithEnemy => hittableLayersWithEnemy;
    public LayerMask HittableLayers { get; set; }

    private void Start()
    {
        player = FindObjectOfType<Player>().transform;
        playerRoll = FindObjectOfType<PlayerRoll>();
        behaviour.KunaiCurrentTarget = null;
        behaviour.ParentKunai = this;

        bodiesToDamage = new List<IDamageable>();

        rotation = 0;
        timePassed = Time.time;

        HittableLayers = hittableLayersWithEnemy;

        // Decides direction of kunai
        // Changes kunai layers if it's an enemy / pressure plate kunai
        behaviour.OnStart(player);

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

    private void OnTriggerEnter(Collider other) =>
        Hit(other);

    /// <summary>
    /// What happens when the kunai hits something.
    /// </summary>
    /// <param name="other">Parameter with collision collider.</param>
    protected virtual void Hit(Collider other)
    {
        if (other != null)
        {
            Collider[] collisions =
                Physics.OverlapSphere(other.transform.position, hitRange, HittableLayers);

            // Checks if this object or parent has a damageable body
            GameObject body = null;
            if (collisions.Length > 0)
                body = GetDamageableBody(collisions[0].gameObject);

            // If this object can receive damage
            if (body != null)
            {
                if (body.TryGetComponent(out IDamageable damageableBody))
                {
                    // Will only hit this body once
                    if (bodiesToDamage.Contains(damageableBody) == false)
                    {
                        // Trigers behaviour hit()
                        behaviour.Hit(damageableBody, other, player);

                        bodiesToDamage.Add(damageableBody);
                    }

                    // Ignores player
                    if (playerRoll.Performing == false)
                        Instantiate(hitParticles, transform.position, Quaternion.identity);
                }
            }
            // Else if the object can not receive damage
            else
            {
                behaviour.Hit(null, other, player);
                Destroy(gameObject);

                Instantiate(hitParticles, transform.position, Quaternion.identity);
            }
            
            // Doesn't hit anything, so it does nothing
        }
        else
        {
            behaviour.Hit(null, null, null);
        }
    }

    /// <summary>
    /// Checks if this object has a damgeable body, if it doesn't it will check
    /// its parent until parent is null.
    /// </summary>
    /// <param name="col">Gameobject to check.</param>
    /// <returns>Returns a gameobject with IDamageable interface.</returns>
    protected GameObject GetDamageableBody(GameObject col)
    {
        col.TryGetComponent(out IDamageable damageableBody);
        if (damageableBody != null && player != null) return col.gameObject;

        else if (col.gameObject.transform.parent != null)
        {
            GetDamageableBody(col.transform.parent.gameObject);
        }
        return null;
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
