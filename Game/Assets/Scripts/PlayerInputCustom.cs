using UnityEngine;
using UnityEngine.InputSystem;
using System;

/// <summary>
/// Class responsible for controlling player input.
/// </summary>
public class PlayerInputCustom : MonoBehaviour, IComponent
{
    [SerializeField] private PlayerInput controls;

    public Vector2 Movement { get; private set; }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ComponentFixedUpdate()
    {
        
    }

    public void ComponentUpdate()
    {
        
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
    /// Handles movement.
    /// </summary>
    /// <param name="context"></param>
    public void HandleJump(InputAction.CallbackContext context)
    {
        if (context.started) OnJump();
    }

    protected virtual void OnJump() => Jump?.Invoke();

    /// <summary>
    /// Registered on Player Movement.
    /// </summary>
    public event Action Jump;


    /// <summary>
    /// Handles movement.
    /// </summary>
    /// <param name="context"></param>
    public void HandleRoll(InputAction.CallbackContext context)
    {
        if (context.started) OnRoll();
    }

    protected virtual void OnRoll() => Roll?.Invoke();

    /// <summary>
    /// Registered on Player Movement.
    /// </summary>
    public event Action Roll;
}
