using UnityEngine;

/// <summary>
/// Class responsible for playing checkpoint audio.
/// </summary>
public class CheckpointAudio : AbstractSoundBase
{
    [SerializeField] private AbstractSoundScriptableObject checkpointAudio;

    public override void PlaySound(Sound sound)
    {
        if (sound == Sound.Checkpoint)
            checkpointAudio.PlaySound(audioSource);
    }
}
