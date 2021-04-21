using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Scriptable object base class for all sounds.
/// </summary>
public abstract class AbstractSoundScriptableObject : ScriptableObject
{
    [SerializeField] protected List<AudioClip> audioClips;
    
    /// <summary>
    /// Method responsible for playing sounds.
    /// </summary>
    /// <param name="audioSource">Audio source to play sounds.</param>
    public abstract void PlaySound(AudioSource audioSource);
}
