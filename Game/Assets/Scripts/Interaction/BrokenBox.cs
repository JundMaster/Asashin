using UnityEngine;

[RequireComponent(typeof(SpawnItemBehaviour))]
/// <summary>
/// Class responsible for handling BrokenBox behaviour.
/// </summary>
public class BrokenBox : MonoBehaviour
{
    private ISpawnItemBehaviour spawnItemsBehaviour;

    private void Awake()
    {
        spawnItemsBehaviour = GetComponent<SpawnItemBehaviour>();
    }

    private void Start()
    {
        spawnItemsBehaviour.ExecuteBehaviour();
    }
}
