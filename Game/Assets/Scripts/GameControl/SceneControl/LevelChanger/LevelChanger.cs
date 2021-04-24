using UnityEngine;

/// <summary>
/// Class responsible for triggering level change when the player hits
/// the level changer collider.
/// </summary>
public class LevelChanger : MonoBehaviour
{
    [SerializeField] private SceneEnum changeToLevel;
    [SerializeField] private BoxCollider boxCollider;

    private SceneControl sceneController;

    private void Awake()
    {
        sceneController = FindObjectOfType<SceneControl>();
    }

    private void Start()
    {
        boxCollider.isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            boxCollider.enabled = false;
            sceneController.LoadScene(changeToLevel);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.6f);
        Gizmos.DrawCube(transform.position + boxCollider.center, boxCollider.size);
    }
}
