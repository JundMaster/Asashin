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

            optionsScript = FindObjectOfType<Options>();
            slowMotion = FindObjectOfType<SlowMotionBehaviour>();

            // Sets master to min volume so it can start fading in
            masterVolume.SetFloat("masterVolume", options.MinMasterVolume);

            // Sets normal volumes
            masterVolume.SetFloat("musicVolume", options.MusicVolume);
            masterVolume.SetFloat("soundVolume", options.SoundVolume);
            masterVolume.SetFloat("soundPitch", 1);

            StartCoroutine(FadeInMasterCoroutine());

            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void OnEnable()
    {
        if (optionsScript != null) optionsScript.UpdatedValues += UpdateValues;
        if (slowMotion != null) slowMotion.SlowMotionEvent += UpdatePitch;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        if (optionsScript != null) optionsScript.UpdatedValues -= UpdateValues;
        if (slowMotion != null) slowMotion.SlowMotionEvent -= UpdatePitch;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        slowMotion = FindObjectOfType<SlowMotionBehaviour>();
        sceneControl = FindObjectOfType<SceneControl>();
        slowMotion.SlowMotionEvent += UpdatePitch;
        sceneControl.StartedLoadingScene += FadeOutMaster;

        StartCoroutine(FadeInMasterCoroutine());
    }

    private void FadeOutMaster() => StartCoroutine(FadeOutMasterCoroutine());

    private IEnumerator FadeOutMasterCoroutine()
    {
        float masterSound = options.MasterVolume;
        YieldInstruction wffu = new WaitForFixedUpdate();
        while (masterSound > options.MinMasterVolume)
        {
            masterSound -= Time.fixedDeltaTime * 25;
            masterVolume.SetFloat("masterVolume", masterSound);
            yield return wffu;
        }
    }

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
}