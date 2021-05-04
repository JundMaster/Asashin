using UnityEngine;

/// <summary>
/// Class responsible for destroying a gameobject.
/// </summary>
public class Destroy : MonoBehaviour
{
    [SerializeField] private float destroyAfterThisTime;
    public void DestroyMe() => Destroy(gameObject, destroyAfterThisTime);
}
