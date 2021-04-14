using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Abstract base class for sound classes.
/// </summary>
public abstract class AbstractSoundBase : MonoBehaviour
{
    [SerializeField] protected List<AudioClip> audioClips;

    protected AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Plays sounds.
    /// </summary>
    /// <param name="sound">Sound to play.</param>
    public virtual void PlaySound(Sound sound)
    {
        audioSource.pitch = 1f;
    }
}
