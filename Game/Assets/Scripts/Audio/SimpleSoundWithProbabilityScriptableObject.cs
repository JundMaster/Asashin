using UnityEngine;

/// <summary>
/// Scriptable object responsible for playing a random sound from a list with
/// a determined probability.
/// </summary>
[CreateAssetMenu(fileName = "Simple sound with probability")]
public class SimpleSoundWithProbabilityScriptableObject : 
    AbstractSoundScriptableObject
{
    [Range(0f, 1f)] [SerializeField] private float volume;
    [Range(0f, 1f)] [SerializeField] private float pitch;
    [Range(0f, 100f)] [SerializeField] private float chanceOfPlaying;

    /// <summary>
    /// Plays a sound on an audiosource.
    /// </summary>
    /// <param name="audioSource">Audio source to play the sound on.</param>
    public override void PlaySound(AudioSource audioSource)
    {
        float probabilty = Random.Range(0f, 100f);
        int randomNum;
        randomNum = Random.Range(0, audioClips.Count);

        audioSource.pitch = pitch;
        if (probabilty < chanceOfPlaying && audioSource.isPlaying == false)
            audioSource.PlayOneShot(audioClips[randomNum], volume);
    }
}