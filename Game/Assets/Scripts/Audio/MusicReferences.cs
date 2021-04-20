using UnityEngine;

/// <summary>
/// Class with audiosource getters.
/// </summary>
public class MusicReferences : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource backgroundSound;

    public AudioSource Music => music;
    public AudioSource BackgroundSound => BackgroundSound;
}
