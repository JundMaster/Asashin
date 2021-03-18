using UnityEngine;
using UnityEngine.InputSystem;
using System;

/// <summary>
/// Class responsible for controlling player input.
/// </summary>
public class PlayerInputCustom : MonoBehaviour
{
    [SerializeField] private PlayerInput controls;

    public Vector2 Movement { get; private set; }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
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
    /// Handles Jump.
    /// </summary>
    /// <param name="context"></param>
    public void HandleJump(InputAction.CallbackContext context)
    {
        if (context.started) OnJump();
    }

    protected virtual void OnJump() => Jump?.Invoke();

    /// <summary>
    /// Registered on Player Jump.
    /// </summary>
    public event Action Jump;


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
    /// Handles MeleeStrongAttack.
    /// </summary>
    /// <param name="context"></param>
    public void HandleMeleeStrongAttack(InputAction.CallbackContext context)
    {
        if (context.started) OnMeleeStrongAttack();
    }

    protected virtual void OnMeleeStrongAttack() => MeleeStrongAttack?.Invoke();

    /// <summary>
    /// Registered on Player Attack.
    /// </summary>
    public event Action MeleeStrongAttack;

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
        if (context.started) OnTargetChange(LeftOrRight.Right);
    }

    /// <summary>
    /// Handles target switch.
    /// </summary>
    /// <param name="context"></param>
    public void HandleTargetChangeLeft(InputAction.CallbackContext context)
    {
        if (context.started) OnTargetChange(LeftOrRight.Left);
    }

    protected virtual void OnTargetChange(LeftOrRight dir) => TargetChange?.Invoke(dir);

    /// <summary>
    /// Registered on CinemachineTarget.
    /// </summary>
    public event Action<LeftOrRight> TargetChange;

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
        if (context.started) OnItemChange(LeftOrRight.Left);
    }

    /// <summary>
    /// Handles item switch to the right.
    /// </summary>
    /// <param name="context"></param>
    public void HandleItemChangeRight(InputAction.CallbackContext context)
    {
        if (context.started) OnItemChange(LeftOrRight.Right);
    }

    protected virtual void OnItemChange(LeftOrRight dir) => ItemChange?.Invoke(dir);

    /// <summary>
    /// Registered on Item Control.
    /// </summary>
    public event Action<LeftOrRight> ItemChange;

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
        }
    }

    /// <summary>
    /// Handles pause game.
    /// </summary>
    /// <param name="context"></param>
    public void HandleGameUnpause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnGamePaused(PauseSystemEnum.Unpaused);
            controls.SwitchCurrentActionMap("Gameplay");
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
        if (context.started) OnBlock(YesOrNo.Yes);
        if (context.canceled) OnBlock(YesOrNo.No);
    }

    protected virtual void OnBlock(YesOrNo condition) => Block?.Invoke(condition);

    /// <summary>
    /// Event registered on PauseSystem.
    /// </summary>
    public event Action<YesOrNo> Block;


    /// <summary>
    /// Handles walking.
    /// </summary>
    /// <param name="context"></param>
    public void HandleWalk(InputAction.CallbackContext context)
    {
        if (context.started) OnWalk();
    }

    protected virtual void OnWalk() => Walk?.Invoke();

    /// <summary>
    /// Event registered on PlayerMovement.
    /// </summary>
    public event Action Walk;


    /// <summary>
    /// Handles sprint.
    /// </summary>
    /// <param name="context"></param>
    public void HandleSprint(InputAction.CallbackContext context)
    {
        if (context.started) OnSprint(YesOrNo.Yes);
        if (context.canceled) OnSprint(YesOrNo.No);
    }

    protected virtual void OnSprint(YesOrNo condition) => Sprint?.Invoke(condition);

    /// <summary>
    /// Event registered on PauseSystem.
    /// </summary>
    public event Action<YesOrNo> Sprint;
}
