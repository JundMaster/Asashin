using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Base class responsible for every pickable item.
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public abstract class Pickable : MonoBehaviour, IPickable
{
    [SerializeField] private float destroyAfterSeconds;
    private SphereCollider triggerCollider;
    private LayerMask playerLayer;
    protected System.Random rand;

    public TypeOfDropEnum TypeOfDrop { get; set; }

    private void Awake()
    {
        triggerCollider = GetComponent<SphereCollider>();
        triggerCollider.enabled = false;
        rand = new System.Random();
        playerLayer = 11;
    }

    /// <summary>
    /// Waits some time before being possible to collide with this item.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(1f);
        triggerCollider.enabled = true;

        Destroy(gameObject, destroyAfterSeconds);
    }

    /// <summary>
    /// When player collides with this item.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
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
    public abstract void Execute(PlayerStats playerStats);
}
