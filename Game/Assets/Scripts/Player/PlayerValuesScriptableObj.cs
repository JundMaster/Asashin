using UnityEngine;

[CreateAssetMenu(fileName = "PlayerValues")]
public class PlayerValuesScriptableObj : ScriptableObject
{
    [Header("Script Values")]
    [Header("Movement")]
    [SerializeField] private float walkingSpeed;
    [SerializeField] private float speed;
    [Header("Rotation")]
    [SerializeField] private float turnSmooth;
    [SerializeField] private float turnSmoothInSlowMotion;
    [SerializeField] private float smoothTimeVelocity;
    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    [SerializeField] private float isGroundedCheckSize;

    public float WalkingSpeed => walkingSpeed;
    public float Speed => speed;
    public float TurnSmooth => turnSmooth;
    public float TurnSmoothInSlowMotion => turnSmoothInSlowMotion;
    public float SmoothTimeVelocity => smoothTimeVelocity;
    public float JumpForce => jumpForce;
    public float Gravity => gravity;
    public float IsGroundedCheckSize => isGroundedCheckSize;
}
