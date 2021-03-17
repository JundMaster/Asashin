using UnityEngine;

/// <summary>
/// Class responsible for handling kunai behaviour.
/// </summary>
public class Kunai : ItemBehaviour
{
    // Enemy variables
    [SerializeField] private bool enemyKunai;
    public Enemy ParentEnemy { get; set; }
    private Transform playerTarget;
    private PlayerBlock playerBlock;
    private PlayerAnimations playerAnim;
    /// //////////////////////////////////////

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

    // Particles Hit
    [SerializeField] private GameObject hitParticles;

    // Layers to collide with kunai
    [SerializeField] private LayerMask hittableLayers;
    [SerializeField] private LayerMask enemyLayer;

    private void Start()
    {
        target = FindObjectOfType<CinemachineTarget>();
        player = FindObjectOfType<Player>().transform;

        if (enemyKunai)
        {
            playerBlock = player.GetComponent<PlayerBlock>();
            playerAnim = player.GetComponent<PlayerAnimations>();
        }

        kunaiCurrentTarget = null;

        // If there's an active target, the kunai will have that same target.
        // If there's NO active target, the kunai will just go towards where
        // the player is facing.
        if (target.CurrentTarget.gameObject.activeSelf && enemyKunai == false)
        {
            // Finds enemies around the current target
            Collider[] currentTargetPosition =
                Physics.OverlapSphere(target.CurrentTarget.transform.position, 0.5f, enemyLayer);

            // If enemy has an Enemy script
            for (int i = 0; i < currentTargetPosition.Length; i++)
            {
                if (currentTargetPosition[i].gameObject.TryGetComponent<Enemy>(out Enemy en))
                {
                    // Sets kunai target to enemy MyTarget
                    kunaiCurrentTarget = en.MyTarget;
                }
            }
        }
        else
        {
            Vector3 position = player.transform.position;
            Vector3 direction = position + player.transform.forward * 10f;

            transform.LookAt(direction);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);

            kunaiCurrentTarget = null;
        }

        // In case this is a kunai spawned by an enemy, it will go towards the player
        if (enemyKunai)
        {
            playerTarget = GameObject.FindGameObjectWithTag("playerTarget").transform;
            kunaiCurrentTarget = playerTarget;   
        }

        rotation = 0;
        timePassed = Time.time;

        // Only for friendly kunais
        if (enemyKunai == false)
            Execute();
    }

    private void FixedUpdate()
    {
        // If kunai has an active target
        if (kunaiCurrentTarget)
        {
            transform.LookAt(kunaiCurrentTarget);
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
                Physics.OverlapSphere(other.ClosestPoint(transform.position), hitRange, hittableLayers);

            // If this object can receive damage
            if (collisions[0].gameObject.TryGetComponent(out IDamageable body))
            {
                // If it's an enemy kunai,
                // if the player is blocking, it will be reflected to the enemy
                // that spawned the kunai,
                // else, the player will take damage
                if (enemyKunai)
                {
                    // If it collides with player layer
                    // and player is blocking
                    if (collisions[0].gameObject.layer == 11)
                    {
                        if (playerBlock.Performing)
                        {
                            // If the player is facing the enemy direction
                            if (Vector3.Angle(
                                ParentEnemy.transform.position - player.position,
                                player.forward) < 50f)
                            {
                                kunaiCurrentTarget = ParentEnemy.MyTarget;
                                playerAnim.TriggerBlockReflect();
                            }
                            // If the player is blocking but not facing the enemy
                            else
                            {
                                //body.TakeDamage(ParentEnemy.GetComponent<Stats>().RangedDamage);
                                body.TakeDamage(5f);
                                Destroy(gameObject);
                            }
                        }
                        // Else if the player isn't blocking
                        else
                        {
                            //body.TakeDamage(ParentEnemy.GetComponent<Stats>().RangedDamage);
                            body.TakeDamage(5f);
                            Destroy(gameObject);
                        }
                    }
                    // Else if it's not the player (meaning the kunai was reflected)
                    else
                    {
                        //body.TakeDamage(ParentEnemy.GetComponent<Stats>().RangedDamage);
                        body.TakeDamage(5f);
                        Destroy(gameObject);
                    }
                }
                // If it's not an enemy kunai, then it means the body it's an
                // enemy. The enemy will take damage from playerStats ranged damage.
                else
                {
                    body.TakeDamage(playerStats.RangedDamage);
                    Destroy(gameObject);
                }
            }
            // Else, if it's not an IDamageable interface, the object is destroyed
            else
            {
                Destroy(gameObject);
            }

            Instantiate(hitParticles, transform.position, Quaternion.identity);
        }
    }

    /// <summary>
    /// Only executes if it's a player's kunai.
    /// Reduces the number of kunais and updates UI.
    /// </summary>
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
