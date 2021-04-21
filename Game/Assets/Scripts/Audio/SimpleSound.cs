﻿using UnityEngine;

/// <summary>
/// Scriptable object responsible for playing a random sound from a list.
/// </summary>
[CreateAssetMenu(fileName = "Simple sound")]
public class SimpleSound : AbstractSoundScriptableObject
{
    [Range(0f, 1f)][SerializeField] private float volume;
    [Range(0f, 1f)][SerializeField] private float pitch;

    /// <summary>
    /// Plays a sound on an audiosource.
    /// </summary>
    /// <param name="audioSource">Audio source to play the sound on.</param>
    public override void PlaySound(AudioSource audioSource)
    {
        int randomNum;
        randomNum = Random.Range(0, audioClips.Count);
        audioSource.pitch = pitch + Random.Range(-0.1f, 0.1f);
        audioSource.PlayOneShot(audioClips[randomNum], volume);
    }
}
