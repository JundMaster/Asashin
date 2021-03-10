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
        if (context.canceled) Movement = new Vector2Int(0, 0);
    }

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
    /// Handles movement.
    /// </summary>
    /// <param name="context"></param>
    public void HandleTargetSet(InputAction.CallbackContext context)
    {
        if (context.started) OnTargetSet();
    }

    protected virtual void OnTargetSet() => TargetSet?.Invoke();

    /// <summary>
    /// Registered on Player Movement.
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
    /// Registered on Player Movement.
    /// </summary>
    public event Action<LeftOrRight> TargetChange;
}
