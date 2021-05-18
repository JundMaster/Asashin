using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpawnItemBehaviour))]
/// <summary>
/// Class responsible for handling treasure box behaviour.
/// </summary>
public class TreasureBox : MonoBehaviour, IFindPlayer, IInterectable
{
    [SerializeField] private bool inTutorial;

    // Components
    private PlayerInteract playerInteract;
    private SphereCollider sphereCollider;
    private Animator anim;
    private TreasureChestAudio chestAudio;

    private ISpawnItemBehaviour spawnItemsBehaviour;

    private void Awake()
    {
        playerInteract = FindObjectOfType<PlayerInteract>();
        sphereCollider = GetComponent<SphereCollider>();
        anim = GetComponent<Animator>();
        spawnItemsBehaviour = GetComponent<SpawnItemBehaviour>();
        chestAudio = GetComponent<TreasureChestAudio>();
    }

    /// <summary>
    /// Method that defines what happens when the player interacts with this treasure box.
    /// </summary>
    public void Execute()
    {
        anim.SetTrigger("OpenBox");

        chestAudio.PlaySound(Sound.BoxOpen);

        // Disables treasure collider
        sphereCollider.enabled = false;

        if (inTutorial)
            OnTutorialTreasure(TypeOfTutorial.LootTreasure);

        spawnItemsBehaviour.ExecuteBehaviour();

        StartCoroutine(AfterExecute());
    }

    private IEnumerator AfterExecute()
    {
        yield return new WaitForFixedUpdate();
        // Removes current interaction item from player
        playerInteract.InterectableObject = null;
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

    ///////////////////// Tutorial methods and events //////////////////////////
    protected virtual void OnTutorialTreasure(TypeOfTutorial typeOfTutorial) =>
        TutorialTreasure?.Invoke(typeOfTutorial);

    public event Action<TypeOfTutorial> TutorialTreasure;
    ////////////////////////////////////////////////////////////////////////////
}
