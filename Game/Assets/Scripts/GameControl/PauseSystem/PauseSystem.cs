using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseSystem : MonoBehaviour, IFindPlayer
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

    private IEnumerator Start()
    {
        PausedGame = false;

        yield return new WaitForSeconds(1f);
    }

    private void OnEnable()
    {
        if (input != null) input.GamePaused += HandlePauseGame;
        if (playerAnimations != null) 
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

    public void FindPlayer()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        playerAnimations = FindObjectOfType<PlayerAnimations>();
        input.GamePaused += HandlePauseGame;
        playerAnimations.PlayerDiedEndOfAnimationPauseSystem += HandlePauseGame;
    }

    public void PlayerLost()
    {
        input.GamePaused -= HandlePauseGame;
        playerAnimations.PlayerDiedEndOfAnimationPauseSystem -= HandlePauseGame;
    }

    /// <summary>
    /// Event called on PlayerAnimations.
    /// </summary>
    public event Action<PauseSystemEnum> GamePaused;
}
