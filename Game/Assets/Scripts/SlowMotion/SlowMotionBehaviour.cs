using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;

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
    private Camera mainCamera;
    private PlayerRoll roll;

    // Slow motion variabl
    public bool SlowMotionOn { get; set; }

    // Particles from player's mesh
    public bool OptionsSlowMotionParticles { get; set; }
    [SerializeField] private ParticleSystem slowMotionParticles;
    private ParticleSystem cloneParticles;

    // Slow Motion Shader Variables
    [SerializeField] private Material slowMotionMaterial;
    [Tooltip("URP forward renderer data")][SerializeField] private ForwardRendererData data;
    [Tooltip("Asset with desired shader")] [SerializeField] private ForwardRendererData slowMotionRender;

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerValues = FindObjectOfType<Player>().Values;
        roll = FindObjectOfType<PlayerRoll>();
        mainCamera = Camera.main;

        slowMotionMaterial.SetFloat("Vector1_1D53D2E0", 0f); // WaveSize
        slowMotionMaterial.SetFloat("Vector1_34F127BD", 0f); // WaveStrength
        slowMotionMaterial.SetFloat("Vector1_58B5DC2F", 0f); // TimeMultiplication
        slowMotionMaterial.SetFloat("Vector1_24514F13", 0f); // WaveTime
    }

    private void Start()
    {
        defaultTimeScale = Time.timeScale;
        defaultFixedDeltaTime = Time.fixedDeltaTime;
        SlowMotionOn = false;
        SlowmotionCoroutine = null;

        OptionsSlowMotionParticles = true;
    }

    private void OnEnable()
    {
        roll.Roll += TriggerSlowMotion;
    }

    private void OnDisable()
    {
        roll.Roll -= TriggerSlowMotion;
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
        mainCamera.GetComponent<CinemachineBrain>().m_UpdateMethod =
            CinemachineBrain.UpdateMethod.LateUpdate;

        SlowMotionOn = true;

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

        //StartCoroutine(ControlFullScreenShaderEffect());

        slowMotionMaterial.SetFloat("Vector1_1D53D2E0", 0.03f); // WaveSize
        slowMotionMaterial.SetFloat("Vector1_34F127BD", -0.2f); // WaveStrength
        slowMotionMaterial.SetFloat("Vector1_58B5DC2F", 1f);    // TimeMultiplication
        float waveTime = 0f;

        while (currentTimePassed < slowMotionDuration)
        {
            waveTime = currentTimePassed / (slowMotionDuration * 0.5f);

            if (currentTimePassed < slowMotionDuration * 0.5f)
            {
                Time.timeScale = Mathf.Lerp(
                    Time.timeScale, 
                    slowMotionSpeed, 
                    slowMotionSmoothSpeed);

                

                // Wave time
                slowMotionMaterial.SetFloat("Vector1_24514F13", waveTime);
            }

            else if (currentTimePassed > slowMotionDuration * 0.5f)
            {
                slowMotionMaterial.SetFloat("Vector1_1D53D2E0", 0f); // WaveSize
                slowMotionMaterial.SetFloat("Vector1_34F127BD", 0f); // WaveStrength
                slowMotionMaterial.SetFloat("Vector1_58B5DC2F", 0f); // TimeMultiplication
                slowMotionMaterial.SetFloat("Vector1_24514F13", 0f); // WaveTime
            }

            else if (currentTimePassed > (slowMotionDuration - (slowMotionDuration/3)))
            {
                Time.timeScale = Mathf.Lerp(
                    Time.timeScale, 
                    defaultTimeScale, 
                    slowMotionSmoothSpeed);
            
            }

            Time.fixedDeltaTime = Time.timeScale * 0.01f;

            currentTimePassed += Time.unscaledDeltaTime;
            yield return null;
        }

        StopSlowMotion();
        SlowmotionCoroutine = null;
    }

    /// <summary>
    /// Controls shader effect while in slow motion.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ControlFullScreenShaderEffect()
    {
        /*
        data.rendererFeatures.Add(temp2);
        data.SetDirty();

        while (currentTimePassed < slowMotionDuration)
        {
            yield return null;
        }

        data.rendererFeatures.Clear();
        data.SetDirty();
        */
        yield return null;
    }

    /// <summary>
    /// Returns time to normal.
    /// </summary>
    private void StopSlowMotion()
    {
        Time.timeScale = defaultTimeScale;
        Time.fixedDeltaTime = defaultFixedDeltaTime;

        playerMovement.TurnSmooth = playerValues.TurnSmooth;

        mainCamera.GetComponent<CinemachineBrain>().m_UpdateMethod =
            CinemachineBrain.UpdateMethod.FixedUpdate;

        SlowMotionOn = false;

        if (OptionsSlowMotionParticles)
        {
            cloneParticles.Stop();
        }
    } 
}
