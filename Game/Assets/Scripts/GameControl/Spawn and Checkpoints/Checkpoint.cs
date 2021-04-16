using UnityEngine;
using System.Collections;

/// <summary>
/// Class responsible for handling evfery checkpoint.
/// </summary>
public class Checkpoint : MonoBehaviour
{
    [SerializeField] private SpawnerController checkpointController;
    [SerializeField] private byte checkpointNumber;
    [SerializeField] private SceneEnum checkpointScene;

    public byte CheckpointNumber => checkpointNumber;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(SaveGame());
    }

    /// <summary>
    /// Coroutine waits for fixed update so the player won't be null, instead
    /// it gives time to load everything.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SaveGame()
    {
        yield return new WaitForSeconds(1);
        checkpointController.SaveCheckpoint(checkpointNumber, checkpointScene);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.15f);
        Gizmos.DrawCube(transform.position, GetComponent<BoxCollider>().size);
    }
}
