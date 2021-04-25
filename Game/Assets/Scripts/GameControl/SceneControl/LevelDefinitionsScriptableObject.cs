using UnityEngine;

/// <summary>
/// Scriptable object responsible for keeping each scene definitions.
/// </summary>
[CreateAssetMenu(fileName = "Level Definitions")]
public class LevelDefinitionsScriptableObject : ScriptableObject
{
    [Header("Music to play")]
    [SerializeField] private AudioClip music;

    /// <summary>
    /// Music to play on this area.
    /// </summary>
    public AudioClip Music => music;

    [Header("Current area")]
    [SerializeField] private new SceneEnum name;

    /// <summary>
    /// Name of this area.
    /// </summary>
    public SceneEnum Name => name;
}
