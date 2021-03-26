using UnityEngine;

[RequireComponent(typeof(SpawnItemBehaviour))]
/// <summary>
/// Class responsible for handling treasure box behaviour.
/// </summary>
public class TreasureBox : MonoBehaviour, IFindPlayer, IInterectable
{
    // Components
    private PlayerInteract playerInteract;
    private SphereCollider sphereCollider;
    private Animator anim;

    private ISpawnItemBehaviour spawnItemsBehaviour;

    private void Awake()
    {
        playerInteract = FindObjectOfType<PlayerInteract>();
        sphereCollider = GetComponent<SphereCollider>();
        anim = GetComponent<Animator>();
        spawnItemsBehaviour = GetComponent<SpawnItemBehaviour>();
    }

    /// <summary>
    /// Method that defines what happens when the player interacts with this treasure box.
    /// </summary>
    public void Execute()
    {
        anim.SetTrigger("OpenBox");
        // Removes current interaction item from player
        playerInteract.InterectableObject = null;
        // Disables treasure collider
        sphereCollider.enabled = false;

        spawnItemsBehaviour.ExecuteBehaviour();
    }

    /// <summary>
    /// If player enters the treasure's range.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        // Player layer
        if (other.gameObject.layer == 11)
        {
            playerInteract.InterectableObject = this;
        }
    }

    /// <summary>
    /// If player leaves the treasure's range.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        // Player layer
        if (other.gameObject.layer == 11)
        {
            playerInteract.InterectableObject = null;
        }
    }

    public void FindPlayer()
    {
        playerInteract = FindObjectOfType<PlayerInteract>();
    }

    public void PlayerLost()
    {
        //
    }
}
