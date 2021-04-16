using UnityEngine;

/// <summary>
/// Class responsible for handling player's sounds.
/// </summary>
public class PlayerSounds : AbstractSoundBase
{
    private PlayerMovement movement;

    private void Start() => movement = GetComponent<PlayerMovement>();

    /// <summary>
    /// Called on animation events.
    /// </summary>
    /// <param name="sound"></param>
    public override void PlaySound(Sound sound)
    {
        float probabilty = Random.Range(0f, 100f);
        int randomNum;

        switch(sound)
        {
            case Sound.SwordSlash:
                randomNum = Random.Range(0, 5);
                audioSource.PlayOneShot(audioClips[randomNum], 0.55f);
                break;
            case Sound.VoiceAttack:
                randomNum = Random.Range(5, 8);
                if (probabilty < 25) audioSource.PlayOneShot(audioClips[randomNum], 0.5f);
                break;
            case Sound.RunningStep:
                randomNum = Random.Range(8, 11);
                audioSource.PlayOneShot(audioClips[randomNum], 0.9f);
                break;
            case Sound.SneakingStep:
                if (movement.Hidden)
                {
                    // Plays "thinking sound" (low chance)
                    if (probabilty < 2f && audioSource.isPlaying == false) 
                        audioSource.PlayOneShot(audioClips[22], 0.3f);
                    randomNum = Random.Range(14, 17);
                    audioSource.PlayOneShot(audioClips[randomNum]);
                }
                else
                {
                    randomNum = Random.Range(11, 13);
                    audioSource.PlayOneShot(audioClips[randomNum], 0.7f);
                }
                break;
            case Sound.VoiceLaugh:
                if (probabilty < 20)
                {
                    randomNum = Random.Range(20, 22);
                    audioSource.PlayOneShot(audioClips[randomNum], 0.6f);
                }
                break;
            case Sound.BlockReflect:
                randomNum = Random.Range(23, 25);
                audioSource.PlayOneShot(audioClips[randomNum], 1f);
                break;
        }
    }
}
