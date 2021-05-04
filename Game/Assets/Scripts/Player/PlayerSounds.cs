﻿using UnityEngine;

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
    [SerializeField] private AbstractSoundScriptableObject hit;
    [SerializeField] private AbstractSoundScriptableObject voiceOnAttack;
    [SerializeField] private AbstractSoundScriptableObject laugh;
    [SerializeField] private AbstractSoundScriptableObject blockReflect;
    [SerializeField] private AbstractSoundScriptableObject slowMotion;
    [SerializeField] private AbstractSoundScriptableObject rolling;

    private PlayerMovement movement;

    private new void Awake()
    {
        base.Awake();
        movement = GetComponent<PlayerMovement>();
    }

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
            case Sound.Hit:
                hit.PlaySound(audioSource);
                break;
            case Sound.SlowMotion:
                slowMotion.PlaySound(audioSource);
                break;
            case Sound.Roll:
                rolling.PlaySound(audioSource);
                break;
        }
    }
}
