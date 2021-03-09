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
    private float playerSpeedInSlowMotion;

    // Components
    private PlayerMovement playerMovement;
    private PlayerJump playerJump;
    private Camera mainCamera;

    // Slow motion state
    public bool SlowMotionOn { get; set; }

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerJump = FindObjectOfType<PlayerJump>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        defaultTimeScale = Time.timeScale;
        defaultFixedDeltaTime = Time.fixedDeltaTime;
        playerSpeedInSlowMotion = 1 / slowMotionSpeed;
        SlowMotionOn = false;

        TriggerSlowMotion();
    }

    public void TriggerSlowMotion()
    {
        Time.timeScale = slowMotionSpeed;
        Time.fixedDeltaTime = Time.timeScale * 0.01f;

        playerMovement.TurnSmooth = 0.005f;

        mainCamera.GetComponent<CinemachineBrain>().m_UpdateMethod = 
            CinemachineBrain.UpdateMethod.LateUpdate;

        SlowMotionOn = true;
    }

    public void StopSlowMotion()
    {
        Time.timeScale = defaultTimeScale;
        Time.fixedDeltaTime = defaultFixedDeltaTime;

        playerMovement.TurnSmooth = 0.1f;

        mainCamera.GetComponent<CinemachineBrain>().m_UpdateMethod =
            CinemachineBrain.UpdateMethod.FixedUpdate;

        SlowMotionOn = false;
    }
}
