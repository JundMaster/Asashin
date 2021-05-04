using UnityEngine;

/// <summary>
/// Destroyes this gameobject after x seconds after the object spawns.
/// </summary>
public class DestroyAfterSecsAwake : MonoBehaviour
{
    [SerializeField] private float destroyAfterThisTime;

    private void Awake() =>
        Destroy(gameObject, destroyAfterThisTime);
}
