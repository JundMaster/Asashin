using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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
    private Options optionsScript;
    private SlowMotionBehaviour slowMotion;
    private SceneControl sceneControl;

    #region Singleton
    public static AudioController instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);

            // Sets master to min volume so it can start fading in
            masterVolume.SetFloat("masterVolume", options.MinMasterVolume);

            // Sets normal volumes
            masterVolume.SetFloat("musicVolume", options.MusicVolume);
            masterVolume.SetFloat("soundVolume", options.SoundVolume);
            masterVolume.SetFloat("soundPitch", 1);

            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void OnEnable() =>
        SceneManager.sceneLoaded += OnSceneLoaded;

    private void OnDisable()
    {
        if (optionsScript != null) optionsScript.UpdatedValues -= UpdateValues;
        if (optionsScript != null) optionsScript.UpdateAudioRealTime -= TemporaryUpdateValuesInRealTime;
        if (slowMotion != null) slowMotion.SlowMotionEvent -= UpdatePitch;
        if (sceneControl != null) sceneControl.StartedLoadingScene -= FadeOutMaster;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(RegisterToEvents());
        StartCoroutine(FadeInMasterCoroutine());
    }

    private IEnumerator RegisterToEvents()
    {
        yield return new WaitForFixedUpdate();
        slowMotion = FindObjectOfType<SlowMotionBehaviour>();
        sceneControl = FindObjectOfType<SceneControl>();
        optionsScript = FindObjectOfType<Options>();

        if (optionsScript != null) optionsScript.UpdatedValues += UpdateValues;
        if (optionsScript != null) optionsScript.UpdateAudioRealTime += TemporaryUpdateValuesInRealTime;
        if (slowMotion != null) slowMotion.SlowMotionEvent += UpdatePitch;
        if (sceneControl != null) sceneControl.StartedLoadingScene += FadeOutMaster;
    }

    private void FadeOutMaster() => StartCoroutine(FadeOutMasterCoroutine());

    /// <summary>
    /// Fades out master volume.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeOutMasterCoroutine()
    {
        float masterSound = options.MasterVolume;

        while (masterSound > options.MinMasterVolume)
        {
            masterSound -= Time.fixedUnscaledDeltaTime * 25;
            masterVolume.SetFloat("masterVolume", masterSound);
            yield return null;
        }
    }

    /// <summary>
    /// Fades master sound to normal volume.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeInMasterCoroutine()
    {
        float masterSound = options.MinMasterVolume;

        YieldInstruction wffu = new WaitForFixedUpdate();
        while (masterSound < options.MasterVolume)
        {
            masterSound += Time.fixedDeltaTime * 25;
            masterVolume.SetFloat("masterVolume", masterSound);
            yield return wffu;
        }
    }

    /// <summary>
    /// Updates pitch on slow motion.
    /// </summary>
    /// <param name="condition">Condition to know if slow motion started.</param>
    private void UpdatePitch(SlowMotionEnum condition)
    {
        if (condition == SlowMotionEnum.SlowMotion)
        {
            masterVolume.SetFloat("soundPitch", 0.5f);
        }
        else
            masterVolume.SetFloat("soundPitch", 1);
    }

    /// <summary>
    /// Updates values.
    /// </summary>
    public void UpdateValues()
    {
        masterVolume.SetFloat("masterVolume", options.MasterVolume);
        masterVolume.SetFloat("musicVolume", options.MusicVolume);
        masterVolume.SetFloat("soundVolume", options.SoundVolume);
    }

    /// <summary>
    /// Updates music value on the beggining of the scene.
    /// </summary>
    private void UpdateMusicVolume() =>
        masterVolume.SetFloat("musicVolume", options.MusicVolume);

    /// <summary>
    /// Updates sound value on the beggining of the scene.
    /// </summary>
    private void UpdateSoundVolume() =>
        masterVolume.SetFloat("soundVolume", options.SoundVolume);

    /// <summary>
    /// Updates audio values in realtime.
    /// </summary>
    /// <param name="type">Type of audio to update.</param>
    /// <param name="value">Value to update to.</param>
    private void TemporaryUpdateValuesInRealTime(TypeOfAudio type, float value)
    {
        switch(type)
        {
            case TypeOfAudio.Master:
                masterVolume.SetFloat("masterVolume", value);
                break;
            case TypeOfAudio.Music:
                masterVolume.SetFloat("musicVolume", value);
                break;
            case TypeOfAudio.Sound:
                masterVolume.SetFloat("soundVolume", value);
                break;
        }
    }
}