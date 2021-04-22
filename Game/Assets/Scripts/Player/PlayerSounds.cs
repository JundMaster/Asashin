using UnityEngine;

/// <summary>
/// Class responsible for handling player's sounds.
/// </summary>
public class PlayerSounds : AbstractSoundBase
{
    [SerializeField] private AbstractSoundScriptableObject runningStep;
    [SerializeField] private AbstractSoundScriptableObject walkingStep;
    [SerializeField] private AbstractSoundScriptableObject walkingStepGrass;
    [SerializeField] private AbstractSoundScriptableObject thinking;
    [SerializeField] private AbstractSoundScriptableObject swordSlash;
    [SerializeField] private AbstractSoundScriptableObject voiceOnAttack;
    [SerializeField] private AbstractSoundScriptableObject laugh;
    [SerializeField] private AbstractSoundScriptableObject blockReflect;

    private PlayerMovement movement;

    private void Start() => movement = GetComponent<PlayerMovement>();

    /// <summary>
    /// Called on animation events.
    /// </summary>
    /// <param name="sound">Sound to play.</param>
    public override void PlaySound(Sound sound)
    {
        switch(sound)
        {
            case Sound.RunningStep:
                runningStep.PlaySound(audioSource);
                break;
            case Sound.SneakingStep:
                if (movement.Hidden)
                {
                    thinking.PlaySound(audioSource);
                    walkingStepGrass.PlaySound(audioSource);
                }
                else
                {
                    walkingStep.PlaySound(audioSource);
                }
                break;
            case Sound.SwordSlash:
                swordSlash.PlaySound(audioSource);
                break;
            case Sound.VoiceAttack:
                voiceOnAttack.PlaySound(audioSource);
                break;
            case Sound.VoiceLaugh:
                laugh.PlaySound(audioSource);
                break;
            case Sound.BlockReflect:
                blockReflect.PlaySound(audioSource);
                break;
        }
    }
}
