using UnityEngine;

/// <summary>
/// Class responsible for playing smoke grenade audio.
/// </summary>
public class SmokeGrenadeAudio : AbstractSoundBase
{
    [SerializeField] private AbstractSoundScriptableObject smokeGrenade;

    public override void PlaySound(Sound sound)
    {
        // Left blank on purpose
    }

    private void Start()
    {
        smokeGrenade.PlaySound(audioSource);
    }
}
