using UnityEngine;
using System.Collections;

/// <summary>
/// Class responsible for handling evfery checkpoint.
/// </summary>
public class Checkpoint : MonoBehaviour
{
    [SerializeField] private SpawnerController checkpointController;
    [SerializeField] private byte checkpointNumber;
  
    private CurrentLevelDefinitions definitions;
    public AbstractSoundBase CheckpointAudio { get; private set; }

    [SerializeField] private Transform spawnPlayerHere;
    public Transform SpawnPlayerHere => spawnPlayerHere;

    private void Awake()
    {
        definitions = FindObjectOfType<CurrentLevelDefinitions>();
        CheckpointAudio = GetComponent<AbstractSoundBase>();
    }


    public byte CheckpointNumber => checkpointNumber;

    private void OnTriggerEnter(Collider other) =>
        StartCoroutine(SaveGame());

    /// <summary>
    /// Coroutine waits for fixed update so the player won't be null, instead
    /// it gives time to load everything.
    /// Saves a checkpoint with its current number + current level area.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SaveGame()
    {
        yield return new WaitForFixedUpdate();

        checkpointController.SaveCheckpoint(checkpointNumber, definitions.ThisArea.Name, this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.15f);
        Gizmos.DrawCube(transform.position, GetComponent<BoxCollider>().size);

        Gizmos.DrawSphere(SpawnPlayerHere.position, 0.25f);
        Gizmos.DrawLine(SpawnPlayerHere.position, SpawnPlayerHere.position + SpawnPlayerHere.forward);
    }
}
