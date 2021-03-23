using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for handling treasure box behaviour.
/// </summary>
public class TreasureBox : MonoBehaviour, IFindPlayer, IInterectable
{
    private PlayerInteract playerInteract;
    private SphereCollider sphereCollider;
    private Animator anim;

    [SerializeField] private GameObject[] objectToSpawn;
    private int randomSpawnNumber;

    [Tooltip("The item needs a chance higher than this in order to be spawned")]
    [SerializeField] private float spawningChance;
    private Vector3 spawnPosition;

    private void Awake()
    {
        playerInteract = FindObjectOfType<PlayerInteract>();
        sphereCollider = GetComponent<SphereCollider>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        spawnPosition =
            new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
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

        // Spawns at least one item
        StartCoroutine(SpawnItem(100f));
    }

    /// <summary>
    /// Spawns a random item in a random direction.
    /// </summary>
    /// <param name="probability">Probability of spawning</param>
    /// <returns>Wait for seconds in realtime.</returns>
    private IEnumerator SpawnItem(float probability)
    {
        yield return new WaitForSecondsRealtime(1f);
        if (probability > spawningChance)
        {
            randomSpawnNumber = Random.Range(0, objectToSpawn.Length);

            // Instantiates the item
            GameObject spawnedObject = Instantiate(
                objectToSpawn[randomSpawnNumber], spawnPosition, Quaternion.identity);

            spawnedObject.GetComponent<Pickable>().TypeOfDrop = TypeOfDropEnum.Treasure;
            Rigidbody spawnedObjectRB = spawnedObject.GetComponent<Rigidbody>();

            // Gives it a random force
            spawnedObjectRB.AddForce(
                Random.Range(-75f, 75f), 70f, Random.Range(-75f, 75f), ForceMode.Impulse);

            // Starts the coroutine again with a random chance of spawning an item
            StartCoroutine(SpawnItem(Random.Range(0f, 100f)));
        }
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
