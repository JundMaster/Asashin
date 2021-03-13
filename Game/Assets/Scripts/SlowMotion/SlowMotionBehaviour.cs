using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Class responsible for handling slow motion.
/// </summary>
public class SlowMotionBehaviour : MonoBehaviour
{
    // Slowmotion Variables
    private float defaultTimeScale;
    private float defaultFixedDeltaTime;
    [SerializeField] private float slowMotionSpeed;
    [SerializeField] private float slowMotionDuration;
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

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerValues = FindObjectOfType<Player>().Values;
        roll = FindObjectOfType<PlayerRoll>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        defaultTimeScale = Time.timeScale;
        defaultFixedDeltaTime = Time.fixedDeltaTime;
        SlowMotionOn = false;
        SlowmotionCoroutine = null;
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

        currentTimePassed = 0;
        while(currentTimePassed < slowMotionDuration)
        {
            if (currentTimePassed < slowMotionDuration * 0.5f)
            {
                Time.timeScale = Mathf.Lerp(
                    Time.timeScale, 
                    slowMotionSpeed, 
                    slowMotionSmoothSpeed);
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
    /// Returns time to normal.
    /// </summary>
    public void StopSlowMotion()
    {
        Time.timeScale = defaultTimeScale;
        Time.fixedDeltaTime = defaultFixedDeltaTime;

        playerMovement.TurnSmooth = playerValues.TurnSmooth;

        mainCamera.GetComponent<CinemachineBrain>().m_UpdateMethod =
            CinemachineBrain.UpdateMethod.FixedUpdate;

        SlowMotionOn = false;
    }

    
}
