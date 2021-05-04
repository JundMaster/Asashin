using UnityEngine;

/// <summary>
/// Abstract base class for sound classes.
/// </summary>
public abstract class AbstractSoundBase : MonoBehaviour
{
    protected AudioSource audioSource;
    protected void Awake() => audioSource = GetComponent<AudioSource>();

    /// <summary>
    /// Plays sounds.
    /// </summary>
    /// <param name="sound">Sound to play.</param>
    public abstract void PlaySound(Sound sound);
}
