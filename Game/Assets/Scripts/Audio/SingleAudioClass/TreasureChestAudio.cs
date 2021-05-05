using UnityEngine;

/// <summary>
/// Class responsible for playing Treasure Chest Audio.
/// </summary>
public class TreasureChestAudio : AbstractSoundBase
{
    [SerializeField] private AbstractSoundScriptableObject boxOpen;

    public override void PlaySound(Sound sound)
    {
        if (sound == Sound.BoxOpen)
            boxOpen.PlaySound(audioSource);
    }
}
