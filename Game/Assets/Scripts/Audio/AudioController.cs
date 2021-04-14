using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Class responsible for handling audio configuration.
/// </summary>
public class AudioController : MonoBehaviour, IUpdateOptions
{
    [Tooltip("Time of transition to current sound values after loading a scene.")]
    [SerializeField] private float timeForTransition;

    // Components
    [SerializeField] private AudioMixer masterVolume;
    [SerializeField] private OptionsScriptableObj options;

    // Audio variables
    private float master;
    private float ambience;
    private float sfx;

    private Options optionsScript;

    private void Awake()
    {
        masterVolume.SetFloat("masterVolume", options.MinMasterVolume);
        masterVolume.SetFloat("musicVolume", options.MinMusicVolume);
        masterVolume.SetFloat("soundVolume", options.MinSoundVolume);

        optionsScript = FindObjectOfType<Options>();
    }

    private void OnEnable() => optionsScript.UpdatedValues += UpdateValues;
    private void OnDisable() => optionsScript.UpdatedValues -= UpdateValues;

    /// <summary>
    /// Only happens after configuration's awake (after loading all values)
    /// </summary>
    private IEnumerator Start()
    {
        YieldInstruction wfu = new WaitForFixedUpdate();

        // Current volumes
        master = options.MinMasterVolume;
        ambience = options.MinMusicVolume;
        sfx = options.MinSoundVolume;

        float timePassed = 0;
        // Scales volumes until they reach the current volume
        while (timePassed < timeForTransition)
        {
            // Lerps volume
            master = Mathf.Lerp(master, options.MasterVolume, timePassed / timeForTransition);
            ambience = Mathf.Lerp(ambience, options.MusicVolume, timePassed / timeForTransition);
            sfx = Mathf.Lerp(sfx, options.SoundVolume, timePassed / timeForTransition);

            // Updates current volumes for audio mixers
            masterVolume.SetFloat("masterVolume", master);
            masterVolume.SetFloat("musicVolume", ambience);
            masterVolume.SetFloat("soundVolume", sfx);

            // Increments timePassed with current time
            timePassed = Time.timeSinceLevelLoad;
            yield return wfu;
        }
    }

    /// <summary>
    /// Updates values.
    /// </summary>
    public void UpdateValues()
    {
        masterVolume.SetFloat("masterVolume", options.MasterVolume);
        masterVolume.SetFloat("ambienceVolume", options.MusicVolume);
        masterVolume.SetFloat("soundVolume", options.SoundVolume);
    }
}