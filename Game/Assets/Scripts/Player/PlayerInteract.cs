using UnityEngine;

/// <summary>
/// Class responsible for handling player interaction.
/// </summary>
public class PlayerInteract : MonoBehaviour
{
    private PlayerInputCustom input;

    /// <summary>
    /// If the player is near an IInterectable, this variable becomes that object,
    /// else, this variable is null
    /// </summary>
    public IInterectable InterectableObject { get; set; }

    private void Awake()
    {
        input = GetComponent<PlayerInputCustom>();
    }

    private void Start()
    {
        InterectableObject = null;
    }

    private void OnEnable()
    {
        input.MeleeLightAttack += InteractWithObject;
    }

    private void OnDisable()
    {
        input.MeleeLightAttack -= InteractWithObject;
    }

    /// <summary>
    /// When the player presses attack, if near an IInterectable, it runs that
    /// object's execute instead.
    /// </summary>
    private void InteractWithObject()
    {
        InterectableObject?.Execute();
    }
}
