using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Scriptable object responsible for keeping each scene definitions.
/// </summary>
[CreateAssetMenu(fileName = "Level Definitions")]
public class LevelDefinitionsScriptableObject : ScriptableObject
{
    [Header("Current area")]
    [SerializeField] private new SceneEnum name;

    /// <summary>
    /// Name of this area.
    /// </summary>
    public SceneEnum Name => name;

    [Header("Post process profile")]
    [SerializeField] private VolumeProfile postProcess;

    /// <summary>
    /// Post process volume for this area.
    /// </summary>
    public VolumeProfile PostProcess => postProcess;
}
