using UnityEngine;

/// <summary>
/// Class responsible for handling player's sounds.
/// </summary>
public class PlayerSounds : AbstractSoundBase
{
    /// <summary>
    /// Called on animation events.
    /// </summary>
    /// <param name="sound"></param>
    public override void PlaySound(Sound sound)
    {
        float probabilty = Random.Range(0f, 100f);
        int randomNum;
        base.PlaySound(Sound.Null);

        switch(sound)
        {
            case Sound.SwordSlash:
                randomNum = Random.Range(0, 5);
                audioSource.PlayOneShot(audioClips[randomNum]);
                break;
            case Sound.VoiceAttack:
                randomNum = Random.Range(5, 8);
                if (probabilty < 10) audioSource.PlayOneShot(audioClips[randomNum], 0.5f);
                break;

        }
    }
}
