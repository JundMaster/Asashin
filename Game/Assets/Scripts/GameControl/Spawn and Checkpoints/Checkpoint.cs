using UnityEngine;

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
        checkpointController.SaveCheckpoint(checkpointNumber, checkpointScene);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.15f);
        Gizmos.DrawCube(transform.position, GetComponent<BoxCollider>().size);
    }
}
