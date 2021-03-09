using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for handling slow motion.
/// </summary>
public class SlowMotionBehaviour : MonoBehaviour
{
    // Slowmotion Variables
    private float defaultTimeScale;
    private float defaultFixedDeltaTime;

    private void Start()
    {
        TriggerSlowMotion();

        defaultTimeScale = Time.timeScale;
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void TriggerSlowMotion()
    {
        Time.timeScale = 0.05f;
        Time.fixedDeltaTime = Time.timeScale * 0.01f;

        FindObjectOfType<Player>().GetComponent<Animator>().speed = 10f;
    }

    public void StopSlowMotion()
    {
        Time.timeScale = defaultTimeScale;
        Time.fixedDeltaTime = defaultFixedDeltaTime;
    }
}
