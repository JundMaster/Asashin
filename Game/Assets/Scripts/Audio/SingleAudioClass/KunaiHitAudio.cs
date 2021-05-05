using UnityEngine;

/// <summary>
/// Class responsible for playing kunai hit audio.
/// </summary>
public class KunaiHitAudio : AbstractSoundBase
{
    [SerializeField] private AbstractSoundScriptableObject kunaiHit;

    public override void PlaySound(Sound sound)
    {
        // Left blank on purpose
    }

    private void Start()
    {
        kunaiHit.PlaySound(audioSource);
    }
}
