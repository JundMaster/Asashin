using System.Collections;
using UnityEngine;
using Cinemachine;
using System;

/// <summary>
/// Class responsible for handling slow motion.
/// </summary>
public class SlowMotionBehaviour : MonoBehaviour
{
    // Slowmotion Variables
    private float defaultTimeScale;
    private float defaultFixedDeltaTime;
    [Tooltip("Speed of slow motion (Time.timeScale)")]
    [SerializeField] private float slowMotionSpeed;
    [SerializeField] private float slowMotionDuration;
    [Tooltip("Smoothing time beetween the transition from normal time to slow motion")]
    [Range(0.03f, 0.04f)] [SerializeField] private float slowMotionSmoothSpeed;
    private float currentTimePassed;
    private Coroutine SlowmotionCoroutine;

    // Components
    private PlayerMovement playerMovement;
    private PlayerValuesScriptableObj playerValues;
    private PlayerRoll playerRoll;
    private PauseSystem pauseSystem;

    // Slow motion variables
    public bool Performing { get; private set; }

    // Particles from player's mesh
    public bool OptionsSlowMotionParticles { get; set; }
    [SerializeField] private ParticleSystem slowMotionParticles;
    private ParticleSystem cloneParticles;

    // Slow Motion Shader Variables
    [SerializeField] private Material slowMotionMaterial;

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerValues = FindObjectOfType<Player>().Values;
        playerRoll = FindObjectOfType<PlayerRoll>();
        pauseSystem = FindObjectOfType<PauseSystem>();
    }

    private void Start()
    {
        defaultTimeScale = Time.timeScale;
        defaultFixedDeltaTime = Time.fixedDeltaTime;
        Performing = false;
        SlowmotionCoroutine = null;

        // Defines if slow motion particles are played
        OptionsSlowMotionParticles = true;

        slowMotionMaterial.SetFloat("Vector1_1D53D2E0", 0f); // WaveSize
        slowMotionMaterial.SetFloat("Vector1_34F127BD", 0f); // WaveStrength
        slowMotionMaterial.SetFloat("Vector1_58B5DC2F", 0f); // TimeMultiplication
        slowMotionMaterial.SetFloat("Vector1_24514F13", 0f); // WaveTime
    }

    private void OnEnable()
    {
        playerRoll.Roll += TriggerSlowMotion;
    }

    private void OnDisable()
    {
        playerRoll.Roll -= TriggerSlowMotion;
    }

    private void Update()
    {
        if (playerMovement == null) playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerValues == null) playerValues = FindObjectOfType<Player>().Values;
        if (playerRoll == null) playerRoll = FindObjectOfType<PlayerRoll>();
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
        // Changes turn value on player and changes camera update mode
        playerMovement.TurnSmooth = playerValues.TurnSmoothInSlowMotion;
 
        // Event for Cinemachine Target. Controls cinemachine brain.
        OnSlowMotionEvent(SlowMotionEnum.SlowMotion);

        Performing = true;

        if (OptionsSlowMotionParticles)
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
            waveTime = currentTimePassed / (slowMotionDuration * 0.5f);

            if (!pauseSystem.PausedGame && currentTimePassed < slowMotionDuration * 0.5f)
            {
                Time.timeScale = Mathf.Lerp(
                    Time.timeScale, 
                    slowMotionSpeed, 
                    slowMotionSmoothSpeed);

                // Wave time
                // This variable goes from 0 to 1, growing the wave until the
                // edge of the screen
                slowMotionMaterial.SetFloat("Vector1_24514F13", waveTime);
            }

            else if (!pauseSystem.PausedGame && currentTimePassed > slowMotionDuration * 0.5f)
            {
                slowMotionMaterial.SetFloat("Vector1_1D53D2E0", 0f); // WaveSize
                slowMotionMaterial.SetFloat("Vector1_34F127BD", 0f); // WaveStrength
                slowMotionMaterial.SetFloat("Vector1_58B5DC2F", 0f); // TimeMultiplication
                slowMotionMaterial.SetFloat("Vector1_24514F13", 0f); // WaveTime
            }

            else if (!pauseSystem.PausedGame && currentTimePassed > (slowMotionDuration - (slowMotionDuration/3)))
            {
                Time.timeScale = Mathf.Lerp(
                    Time.timeScale, 
                    defaultTimeScale, 
                    slowMotionSmoothSpeed); 
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

        playerMovement.TurnSmooth = playerValues.TurnSmooth;

        // Event for Cinemachine Target. Controls cinemachine brain.
        OnSlowMotionEvent(SlowMotionEnum.NormalTime);

        Performing = false;

        if (OptionsSlowMotionParticles)
        {
            cloneParticles.Stop();
        }
    }

    protected virtual void OnSlowMotionEvent(SlowMotionEnum condition) => SlowMotionEvent?.Invoke(condition);

    /// <summary>
    /// Event registered on CinemachineTarget.
    /// </summary>
    public event Action<SlowMotionEnum> SlowMotionEvent;
}
