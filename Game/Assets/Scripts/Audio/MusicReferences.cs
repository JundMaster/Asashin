using UnityEngine;

/// <summary>
/// Class with audiosource getters.
/// </summary>
public class MusicReferences : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource backgroundSound;
    [SerializeField] private AudioSource combatMusic;

    public AudioSource Music => music;
    public AudioSource BackgroundSound => BackgroundSound;
    public AudioSource CombatMusic => combatMusic;
}
