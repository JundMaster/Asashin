using UnityEngine;
using System;

public class PlayerRoll : MonoBehaviour, IAction
{
    // Components
    public Animator Anim { get; private set; }
    private PlayerInputCustom input;
    private PlayerUseItem useItem;
    private PlayerBlock block;
    private PlayerWallHug wallHug;
    private PlayerMovement movement;

    public bool Performing { get; set; }
    public float PerformingTime { get; set; }

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        Anim = GetComponent<Animator>();
        useItem = GetComponent<PlayerUseItem>();
        block = GetComponent<PlayerBlock>();
        wallHug = GetComponent<PlayerWallHug>();
        movement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        Performing = false;
    }

    private void OnEnable()
    {
        input.Roll += HandleRoll;
    }

    private void OnDisable()
    {
        input.Roll -= HandleRoll;
    }

    public void ComponentUpdate()
    {

    }

    public void ComponentFixedUpdate()
    {
        
    }

    /// <summary>
    /// Handles rolling.
    /// </summary>
    private void HandleRoll()
    {
        if (Performing == false &&
            movement.IsGrounded() && useItem.Performing == false 
            && wallHug.Performing == false)
        {
            OnRoll();
            OnTutorialRoll(TypeOfTutorial.Roll);
        }
    }

    /// <summary>
    /// Invokes Roll event.
    /// </summary>
    protected virtual void OnRoll() => Roll?.Invoke();

    /// <summary>
    /// Event registered on PlayerAnimations.
    /// Event registered on PlayerMovement.
    /// </summary>
    public event Action Roll;

    /// <summary>
    /// Invokes Dodge event.
    /// </summary>
    public virtual void OnDodge() => Dodge?.Invoke();

    /// <summary>
    /// Event registered on SlowMotionBehaviour.
    /// </summary>
    public event Action Dodge;

    ///////////////////// Tutorial methods and events //////////////////////////
    protected virtual void OnTutorialRoll(TypeOfTutorial typeOfTutorial) => 
        TutorialRoll?.Invoke(typeOfTutorial);

    public event Action<TypeOfTutorial> TutorialRoll;
    ////////////////////////////////////////////////////////////////////////////
}
