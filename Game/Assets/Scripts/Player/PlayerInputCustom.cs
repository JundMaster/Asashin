using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// Class responsible for controlling player input.
/// </summary>
public class PlayerInputCustom : MonoBehaviour, IFindPlayer
{
    [SerializeField] private PlayerInput controls;

    // Components
    private PlayerDeathBehaviour deathBehaviour;

    public Vector2 Movement { get; private set; }

    private void Awake()
    {
        deathBehaviour = FindObjectOfType<PlayerDeathBehaviour>();
        SwitchActionMapToGamePaused();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        if (deathBehaviour != null)
        {
            deathBehaviour.PlayerDied += SwitchActionMapToGamePaused;
        }
    }

    private void OnDisable()
    {
        if (deathBehaviour != null)
        {
            deathBehaviour.PlayerDied -= SwitchActionMapToGamePaused;
        }
    }

    /// <summary>
    /// Switches action map to gameplay.
    /// </summary>
    public void SwitchActionMapToGameplay() =>
        controls.SwitchCurrentActionMap("Gameplay");

    /// <summary>
    /// Switches action map to GamePaused.
    /// </summary>
    public void SwitchActionMapToGamePaused() =>
        controls.SwitchCurrentActionMap("GamePaused");

    /// <summary>
    /// Switches action map to Disable.
    /// </summary>
    public void SwitchActionMapToDisable() =>
        controls.SwitchCurrentActionMap("DisableControls");

    public string GetActionMap() => controls.currentActionMap.name;

    /// <summary>
    /// Disables or enables input module
    /// </summary>
    /// <param name="condition">If true, disables the input module, else
    /// enables the input module.</param>
    public void DisableInputModule(bool condition)
    {
        BaseInputModule inputModule = FindObjectOfType<BaseInputModule>();
        if (inputModule != null)
        {
            if (condition == true) inputModule.enabled = false;
            else inputModule.enabled = true;
        }
    }

    /// <summary>
    /// Handles movement.
    /// </summary>
    /// <param name="context"></param>
    public void HandleMovement(InputAction.CallbackContext context)
    {
        if (context.performed) Movement = context.ReadValue<Vector2>();
        if (context.canceled)
        {
            Movement = new Vector2Int(0, 0);
            OnStopMoving();
        }
    }

    protected virtual void OnStopMoving() => StopMoving?.Invoke();

    /// <summary>
    /// Event registered on PlayerMovement.
    /// </summary>
    public event Action StopMoving;


    /// <summary>
    /// Handles Player Roll.
    /// </summary>
    /// <param name="context"></param>
    public void HandleRoll(InputAction.CallbackContext context)
    {
        if (context.started) OnRoll();
    }

    protected virtual void OnRoll() => Roll?.Invoke();

    /// <summary>
    /// Registered on Player Roll.
    /// </summary>
    public event Action Roll;


    /// <summary>
    /// Handles Player Attack.
    /// </summary>
    /// <param name="context"></param>
    public void HandleMeleeLightAttack(InputAction.CallbackContext context)
    {
        if (context.started) OnMeleeLightAttack();
    }

    protected virtual void OnMeleeLightAttack() => MeleeLightAttack?.Invoke();

    /// <summary>
    /// Registered on Player Attack.
    /// </summary>
    public event Action MeleeLightAttack;

    /// <summary>
    /// Handles targeting.
    /// </summary>
    /// <param name="context"></param>
    public void HandleTargetSet(InputAction.CallbackContext context)
    {
        if (context.started) OnTargetSet();
    }

    protected virtual void OnTargetSet() => TargetSet?.Invoke();

    /// <summary>
    /// Registered on CinemachineTarget.
    /// </summary>
    public event Action TargetSet;

    /// <summary>
    /// Handles target switch.
    /// </summary>
    /// <param name="context"></param>
    public void HandleTargetChangeRight(InputAction.CallbackContext context)
    {
        if (context.started) OnTargetChange(Direction.Right);
    }

    /// <summary>
    /// Handles target switch.
    /// </summary>
    /// <param name="context"></param>
    public void HandleTargetChangeLeft(InputAction.CallbackContext context)
    {
        if (context.started) OnTargetChange(Direction.Left);
    }

    protected virtual void OnTargetChange(Direction dir) => TargetChange?.Invoke(dir);

    /// <summary>
    /// Registered on CinemachineTarget.
    /// </summary>
    public event Action<Direction> TargetChange;

    /// <summary>
    /// Handles item use.
    /// </summary>
    /// <param name="context"></param>
    public void HandleItemUse(InputAction.CallbackContext context)
    {
        if (context.started) OnItemUse();
    }

    protected virtual void OnItemUse() => ItemUse?.Invoke();

    /// <summary>
    /// Registered on Item Control.
    /// </summary>
    public event Action ItemUse;

    /// <summary>
    /// Handles item switch to the left.
    /// </summary>
    /// <param name="context"></param>
    public void HandleItemChangeLeft(InputAction.CallbackContext context)
    {
        if (context.started) OnItemChange(Direction.Left);
    }

    /// <summary>
    /// Handles item switch to the right.
    /// </summary>
    /// <param name="context"></param>
    public void HandleItemChangeRight(InputAction.CallbackContext context)
    {
        if (context.started) OnItemChange(Direction.Right);
    }

    protected virtual void OnItemChange(Direction dir) => ItemChange?.Invoke(dir);

    /// <summary>
    /// Registered on Item Control.
    /// </summary>
    public event Action<Direction> ItemChange;

    /// <summary>
    /// Handles pause game.
    /// </summary>
    /// <param name="context"></param>
    public void HandleGamePaused(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnGamePaused(PauseSystemEnum.Paused);
            controls.SwitchCurrentActionMap("GamePaused");

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    protected virtual void OnGamePaused(PauseSystemEnum pauseSystem) => GamePaused?.Invoke(pauseSystem);

    /// <summary>
    /// Event registered on PauseSystem.
    /// </summary>
    public event Action<PauseSystemEnum> GamePaused;


    /// <summary>
    /// Handles pause game.
    /// </summary>
    /// <param name="context"></param>
    public void HandleBlock(InputAction.CallbackContext context)
    {
        if (context.performed) OnBlock(true);
        if (context.canceled) OnBlock(false);
    }

    protected virtual void OnBlock(bool condition) => Block?.Invoke(condition);

    /// <summary>
    /// Event registered on PauseSystem.
    /// </summary>
    public event Action<bool> Block;


    /// <summary>
    /// Handles walking.
    /// </summary>
    /// <param name="context"></param>
    public void HandleWalk(InputAction.CallbackContext context)
    {
        if (context.started) OnWalk(true);
        if (context.canceled) OnWalk(false);
    }

    protected virtual void OnWalk(bool condition) => Walk?.Invoke(condition);

    /// <summary>
    /// Event registered on PlayerMovement.
    /// </summary>
    public event Action<bool> Walk;


    /// <summary>
    /// Handles sprint.
    /// </summary>
    /// <param name="context"></param>
    public void HandleSprint(InputAction.CallbackContext context)
    {
        if (context.started) OnSprint(true);
        if (context.canceled) OnSprint(false);
    }

    protected virtual void OnSprint(bool condition) => Sprint?.Invoke(condition);

    /// <summary>
    /// Event registered on PauseSystem.
    /// </summary>
    public event Action<bool> Sprint;


    /// <summary>
    /// Handles wall hug.
    /// </summary>
    /// <param name="context"></param>
    public void HandleWallHug(InputAction.CallbackContext context)
    {
        if (context.started) OnWallHug();
    }

    protected virtual void OnWallHug() => WallHug?.Invoke();

    /// <summary>
    /// Event registered on PauseSystem.
    /// </summary>
    public event Action WallHug;


    public void FindPlayer()
    {
        deathBehaviour = FindObjectOfType<PlayerDeathBehaviour>();
        deathBehaviour.PlayerDied += () =>
            controls.SwitchCurrentActionMap("GamePaused");

        StartCoroutine(SwitchToGameplayAfterSeconds());
    }

    /// <summary>
    /// Switches to gameplay controls after fixed update.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SwitchToGameplayAfterSeconds()
    {
        yield return new WaitForFixedUpdate();
        SwitchActionMapToGameplay();
    }

    public void PlayerLost()
    {
        deathBehaviour.PlayerDied -= () =>
            controls.SwitchCurrentActionMap("Death");
    }
}
