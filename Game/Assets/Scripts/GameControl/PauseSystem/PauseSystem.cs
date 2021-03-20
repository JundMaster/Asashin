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

    private void Start()
    {
        PausedGame = false;
    }

    private void OnEnable()
    {
        if (input != null) input.GamePaused += HandlePause;
        if (playerAnimations != null) 
            playerAnimations.PlayerDiedEndOfAnimationPauseSystem += HandlePauseCharacterDead;
    }

    private void OnDisable()
    {
        input.GamePaused -= HandlePause;
        playerAnimations.PlayerDiedEndOfAnimationPauseSystem -= HandlePauseCharacterDead;
    }

    /// <summary>
    /// Only happens when the player presses pause key.
    /// </summary>
    /// <param name="pauseSystem"></param>
    private void HandlePause(PauseSystemEnum pauseSystem)
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
        ChangePlayerAnimatorMode();
    }

    /// <summary>
    /// Only happens when player death animation reaches the end.
    /// </summary>
    /// <param name="pauseSystem"></param>
    private void HandlePauseCharacterDead(PauseSystemEnum pauseSystem)
    {
        if (pauseSystem == PauseSystemEnum.Unpaused)
        {
            Time.timeScale = 1f;
            PausedGame = false;
        }
        else
        {
            Time.timeScale = 0f;
            PausedGame = true;
        }
        ChangePlayerAnimatorMode();
    }

    /// <summary>
    /// Sets animator update mode to scaled or unscaled when the game is paused.
    /// </summary>
    private void ChangePlayerAnimatorMode()
    {
        Animator anim = playerAnimations.GetComponent<Animator>();
        if (anim.updateMode == AnimatorUpdateMode.UnscaledTime)
            anim.updateMode = AnimatorUpdateMode.Normal;
        else
            anim.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    public void FindPlayer()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        playerAnimations = FindObjectOfType<PlayerAnimations>();
        input.GamePaused += HandlePause;
        playerAnimations.PlayerDiedEndOfAnimationPauseSystem += HandlePauseCharacterDead;
    }

    public void PlayerLost()
    {
        input.GamePaused -= HandlePause;
        playerAnimations.PlayerDiedEndOfAnimationPauseSystem -= HandlePauseCharacterDead;
    }

    protected virtual void OnGamePaused(PauseSystemEnum pauseEnum) =>
        GamePaused?.Invoke(pauseEnum);

    /// <summary>
    /// Event called on ShowOptionsMenu.
    /// </summary>
    public event Action<PauseSystemEnum> GamePaused;
}
