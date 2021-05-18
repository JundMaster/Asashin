using UnityEngine;

/// <summary>
/// Changes tutorial to next scene.
/// </summary>
public class TutorialNext : MonoBehaviour
{
    [SerializeField] private SceneEnum sceneToLoad;

    [SerializeField] private BoxCollider col;

    private readonly LayerMask PLAYERLAYER = 11;

    private SceneControl sceneControl;

    private void Awake() =>
        sceneControl = FindObjectOfType<SceneControl>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == PLAYERLAYER)
            sceneControl.LoadScene(sceneToLoad);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.25f, 0.25f, 0.25f);
        Gizmos.DrawCube(transform.position + col.center, col.size);
    }
}
