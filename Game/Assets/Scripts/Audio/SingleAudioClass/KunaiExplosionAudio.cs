using UnityEngine;

/// <summary>
/// Class responsible for playing kunai explosion audio.
/// </summary>
public class KunaiExplosionAudio : AbstractSoundBase
{
    [SerializeField] private AbstractSoundScriptableObject explosion;

    public override void PlaySound(Sound sound)
    {
        // Left blank on purpose
    }

    private void Start()
    {
        explosion.PlaySound(audioSource);
    }
}
