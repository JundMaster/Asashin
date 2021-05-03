using UnityEngine;

/// <summary>
/// Class responsible for handling breakable box sounds.
/// </summary>
public class BreakableBoxSounds : AbstractSoundBase
{
    [SerializeField] private AbstractSoundScriptableObject boxBreakSound;

    public override void PlaySound(Sound sound)
    {
        switch (sound)
        {
            case Sound.BoxBreak:
                boxBreakSound.PlaySound(audioSource);
                break;
        }
    }
}
