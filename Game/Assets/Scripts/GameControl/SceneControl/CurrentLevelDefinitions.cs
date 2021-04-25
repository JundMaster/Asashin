using UnityEngine;

/// <summary>
/// Class responsible for handing current level definitions.
/// </summary>
public class CurrentLevelDefinitions : MonoBehaviour
{
    [SerializeField] private LevelDefinitionsScriptableObject thisArea;

    public LevelDefinitionsScriptableObject ThisArea => thisArea;
}
