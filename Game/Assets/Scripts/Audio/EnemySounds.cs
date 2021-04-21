using UnityEngine;

/// <summary>
/// Class responsible for handling enemy sounds.
/// </summary>
public class EnemySounds : AbstractSoundBase
{
    [SerializeField] private AbstractSoundScriptableObject swordSlash;
    [SerializeField] private AbstractSoundScriptableObject step;
    [SerializeField] private AbstractSoundScriptableObject laugh;
    [SerializeField] private AbstractSoundScriptableObject scream;

    /// <summary>
    /// Called on animation events.
    /// </summary>
    /// <param name="sound">Sound to play.</param>
    public override void PlaySound(Sound sound)
    {
        switch (sound)
        {
            case Sound.SwordSlash:
                swordSlash.PlaySound(audioSource);
                break;
            case Sound.RunningStep:
                step.PlaySound(audioSource);
                break;
            case Sound.VoiceLaugh:
                laugh.PlaySound(audioSource);
                break;
            case Sound.VoiceScream:
                scream.PlaySound(audioSource);
                break;
        }
    }
}
