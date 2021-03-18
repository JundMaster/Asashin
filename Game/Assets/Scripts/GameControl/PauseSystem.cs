using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseSystem : MonoBehaviour
{
    // Components
    private PlayerInputCustom input;
    private PlayerAnimations playerAnimations;

    // Used to stop time in slow motion coroutine
    public bool PausedGame { get; private set; }

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        playerAnimations = FindObjectOfType<PlayerAnimations>();
    }

    private void Start()
    {
        PausedGame = false;
    }

    private void OnEnable()
    {
        input.GamePaused += HandlePauseGame;
        playerAnimations.PlayerDiedEndOfAnimationPauseSystem += HandlePauseGame;
    }

    private void OnDisable()
    {
        input.GamePaused -= HandlePauseGame;
        playerAnimations.PlayerDiedEndOfAnimationPauseSystem -= HandlePauseGame;
    }

    private void HandlePauseGame(PauseSystemEnum pauseSystem)
    {
        if (pauseSystem == PauseSystemEnum.Unpaused)
        {
            Time.timeScale = 1f;
            OnGamePaused(PauseSystemEnum.Unpaused);
            PausedGame = false;
        }
        else
        {
            Time.timeScale = 0f;
            OnGamePaused(PauseSystemEnum.Paused);
            PausedGame = true;
        }
    }

    protected virtual void OnGamePaused(PauseSystemEnum pauseEnum) => 
        GamePaused?.Invoke(pauseEnum);

    /// <summary>
    /// Event called on PlayerAnimations.
    /// </summary>
    public event Action<PauseSystemEnum> GamePaused;
}
