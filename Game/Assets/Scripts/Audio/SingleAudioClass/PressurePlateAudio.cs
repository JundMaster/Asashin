using UnityEngine;

/// <summary>
/// Class responsible for playing Pressure Plate Audio.
/// </summary>
public class PressurePlateAudio : AbstractSoundBase
{
    [SerializeField] private AbstractSoundScriptableObject pressurePlateEnter;

    public override void PlaySound(Sound sound)
    {
        if (sound == Sound.PressurePlate)
            pressurePlateEnter.PlaySound(audioSource);
    }
}
