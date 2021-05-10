using UnityEngine;

/// <summary>
/// Class responsible for playing enemy death audio.
/// </summary>
public class EnemyDeathAudio : AbstractSoundBase
{
    [SerializeField] private AbstractSoundScriptableObject enemyDeathAudio;

    public override void PlaySound(Sound sound)
    {
        // Left blank on purpose
    }

    private void Start()
    {
        enemyDeathAudio.PlaySound(audioSource);
    }
}
