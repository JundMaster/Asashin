using UnityEngine;

/// <summary>
/// Class responsible for handling enemy sounds.
/// </summary>
public class EnemySounds : AbstractSoundBase
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

        switch (sound)
        {
            case Sound.SwordSlash:
                randomNum = Random.Range(0, 5);
                audioSource.PlayOneShot(audioClips[randomNum]);
                break;
            case Sound.RunningStep:
                randomNum = Random.Range(5, 8);
                audioSource.PlayOneShot(audioClips[randomNum]);
                break;
            case Sound.VoiceLaugh:
                randomNum = Random.Range(8, 10);
                audioSource.PlayOneShot(audioClips[randomNum]);
                break;
            case Sound.VoiceScream:
                randomNum = Random.Range(10, 12);
                audioSource.PlayOneShot(audioClips[randomNum], 0.4f);
                break;
        }
    }
}
