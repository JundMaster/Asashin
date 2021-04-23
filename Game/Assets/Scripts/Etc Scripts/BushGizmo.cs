using UnityEngine;

public class BushGizmo : MonoBehaviour
{
    [SerializeField] private BoxCollider col;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 1, 0.3f);
        Vector3 final = new Vector3(
            col.size.x * transform.localScale.x,
            col.size.y * transform.localScale.y,
            col.size.z * transform.localScale.z);
        Gizmos.DrawCube(transform.position, final);
    }
}
