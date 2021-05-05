using System.Collections;
using UnityEngine;

/// <summary>
/// Base class responsible for every pickable item.
/// SpawnItemBehaviour.cs determines the quantity of this pickable object.
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public abstract class Pickable : MonoBehaviour, IPickable
{
    [SerializeField] private float destroyAfterSeconds;
    private SphereCollider sphereCollider;
    private CapsuleCollider capsuleCollider;
    private readonly LayerMask PLAYERLAYER = 11;
    private Rigidbody rb;
    protected System.Random rand;

    public CustomVector2 Quantity { get; set; }
    protected int quantity;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        sphereCollider.enabled = false;
        capsuleCollider.enabled = false;
        rand = new System.Random();
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Waits some time before being possible to collide with this item.
    /// </summary>
    /// <returns></returns>
    private void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
        StartCoroutine(ActivateCollider());
    }

    /// <summary>
    /// Activates collider if the item is falling.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ActivateCollider()
    {
        YieldInstruction wffu = new WaitForFixedUpdate();
        yield return wffu;
        while(true)
        {
            if (rb.velocity.y < 0)
            {
                if (capsuleCollider.enabled == false)
                {
                    capsuleCollider.enabled = true;
                    break;
                }
            }
            yield return wffu;
        }
        yield return new WaitForSeconds(1);
        sphereCollider.enabled = true;
    }

    /// <summary>
    /// When player collides with this item.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == PLAYERLAYER)
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            ItemUIParent itemsUI = FindObjectOfType<ItemUIParent>();
            Execute(playerStats);
            itemsUI.UpdateAllItemUI();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// What happens when the player picks this item.
    /// </summary>
    /// <param name="playerStats">Player stats variable.</param>
    public virtual void Execute(PlayerStats playerStats)
    {
        if (Quantity.x <= Quantity.y) quantity = Quantity.x;
        else quantity = rand.Next(Quantity.x, Quantity.y + 1);
    }
}
