using UnityEngine;

/// <summary>
/// Class responsible for playing kunai pressure plate audio.
/// </summary>
public class KunaiPressurePlateAudio : AbstractSoundBase
{
    [SerializeField] private AbstractSoundScriptableObject throwKunai;

    public override void PlaySound(Sound sound)
    {
        // Left blank on purpose
    }

    private void Start()
    {
        throwKunai.PlaySound(audioSource);
    }
}
