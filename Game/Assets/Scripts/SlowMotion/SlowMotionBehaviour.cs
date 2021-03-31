using System.Collections;
using UnityEngine;
using Cinemachine;
using System;

/// <summary>
/// Class responsible for handling slow motion.
/// </summary>
public class SlowMotionBehaviour : MonoBehaviour, IFindPlayer
{
    // Slowmotion Variables
    private float defaultTimeScale;
    private float defaultFixedDeltaTime;
    private float slowMotionSpeed; // Speed of slow motion (Time.timeScale)
    private float slowMotionDuration;
    private float slowMotionSmoothSpeed; //Smoothing time beetween the transition from normal time to slow motion
    private float currentTimePassed;
    private Coroutine SlowmotionCoroutine;

    // Components
    private PlayerRoll playerRoll;
    private PauseSystem pauseSystem;

    // Slow motion variables
    public bool Performing { get; private set; }

    // Particles from player's mesh
    [SerializeField] private OptionsScriptableObj options;
    [SerializeField] private ParticleSystem slowMotionParticles;
    private ParticleSystem cloneParticles;

    // Slow Motion Shader Variables
    [SerializeField] private Material slowMotionMaterial;

    private void Awake()
    {
        playerRoll = FindObjectOfType<PlayerRoll>();
        pauseSystem = FindObjectOfType<PauseSystem>();
    }

    private void Start()
    {
        defaultTimeScale = 1f;
        defaultFixedDeltaTime = 0.01f;
        Performing = false;
        SlowmotionCoroutine = null;

        slowMotionMaterial.SetFloat("Vector1_1D53D2E0", 0f); // WaveSize
        slowMotionMaterial.SetFloat("Vector1_34F127BD", 0f); // WaveStrength
        slowMotionMaterial.SetFloat("Vector1_58B5DC2F", 0f); // TimeMultiplication
        slowMotionMaterial.SetFloat("Vector1_24514F13", 0f); // WaveTime

        slowMotionSpeed = 0.1f;
        slowMotionDuration = 5f;
        slowMotionSmoothSpeed = 0.005f;

        StopSlowMotion();
    }

    private void OnEnable()
    {
        if (playerRoll != null) playerRoll.Roll += TriggerSlowMotion;
    }

    private void OnDisable()
    {
        playerRoll.Roll -= TriggerSlowMotion;
    }

    private void TriggerSlowMotion()
    {
        if (SlowmotionCoroutine == null)
            SlowmotionCoroutine = StartCoroutine(SlowMotion());
    }

    /// <summary>
    /// Coroutine responsible for slowing time.
    /// </summary>
    /// <returns>Null.</returns>
    private IEnumerator SlowMotion()
    {
        // Changes camera update mode.
        // Event for Cinemachine Target. Controls cinemachine brain.
        OnSlowMotionEvent(SlowMotionEnum.SlowMotion);

        Performing = true;

        // Only plays if after images are enabled
        if (options.AfterImages)
        {
            SkinnedMeshRenderer playerMesh =
               GameObject.FindGameObjectWithTag("playerMesh").
               GetComponent<SkinnedMeshRenderer>();
            cloneParticles = Instantiate(slowMotionParticles);
            var slowMotionParticlesMesh = cloneParticles.shape;
            slowMotionParticlesMesh.skinnedMeshRenderer = playerMesh;
        }

        currentTimePassed = 0;

        slowMotionMaterial.SetFloat("Vector1_1D53D2E0", 0.03f); // WaveSize
        slowMotionMaterial.SetFloat("Vector1_34F127BD", -0.2f); // WaveStrength
        slowMotionMaterial.SetFloat("Vector1_58B5DC2F", 1f);    // TimeMultiplication
        float waveTime = 0f;

        while (currentTimePassed < slowMotionDuration)
        {
            // This variable goes from 0 to 1, growing the wave until the
            // edge of the screen
            // Wave time
            if (waveTime < 0.99f) waveTime = currentTimePassed / (slowMotionDuration * 0.5f);
            slowMotionMaterial.SetFloat("Vector1_24514F13", waveTime);

            if (!pauseSystem.PausedGame && currentTimePassed < slowMotionDuration * 0.25f)
            {
                Time.timeScale = Mathf.Lerp(
                    Time.timeScale, 
                    slowMotionSpeed, 
                    slowMotionSmoothSpeed * 2);
            }

            else if (!pauseSystem.PausedGame && currentTimePassed > slowMotionDuration - (slowMotionDuration * 0.25f))
            {
                Time.timeScale = Mathf.Lerp(
                    Time.timeScale, 
                    defaultTimeScale, 
                    slowMotionSmoothSpeed * 2);

                slowMotionMaterial.SetFloat("Vector1_1D53D2E0", 0f); // WaveSize
                slowMotionMaterial.SetFloat("Vector1_34F127BD", 0f); // WaveStrength
                slowMotionMaterial.SetFloat("Vector1_58B5DC2F", 0f); // TimeMultiplication
                slowMotionMaterial.SetFloat("Vector1_24514F13", 0f); // WaveTime
            }

            if (pauseSystem.PausedGame)
            {
                Time.fixedDeltaTime = 0f;
                currentTimePassed += 0f;
            }
            else if (!pauseSystem.PausedGame)
            {
                Time.fixedDeltaTime = Time.timeScale * 0.01f;
                currentTimePassed += Time.unscaledDeltaTime;
            }

            
            yield return null;
        }

        StopSlowMotion();
        SlowmotionCoroutine = null;
    }

    /// <summary>
    /// Returns time to normal.
    /// </summary>
    private void StopSlowMotion()
    {
        Time.timeScale = defaultTimeScale;
        Time.fixedDeltaTime = defaultFixedDeltaTime;

        // Event for Cinemachine Target. Controls cinemachine brain.
        OnSlowMotionEvent(SlowMotionEnum.NormalTime);

        Performing = false;

        if (options.AfterImages)
        {
            if (cloneParticles && cloneParticles.isPlaying) 
                cloneParticles.Stop();
        }
    }

    protected virtual void OnSlowMotionEvent(SlowMotionEnum condition) => SlowMotionEvent?.Invoke(condition);

    public void FindPlayer()
    {
        playerRoll = FindObjectOfType<PlayerRoll>();
        playerRoll.Roll += TriggerSlowMotion;
    }

    public void PlayerLost()
    {
        playerRoll.Roll -= TriggerSlowMotion;
    }

    /// <summary>
    /// Event registered on CinemachineTarget. Event registered on PlayerMovement
    /// </summary>
    public event Action<SlowMotionEnum> SlowMotionEvent;
}
