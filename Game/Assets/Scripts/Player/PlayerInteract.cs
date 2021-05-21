using UnityEngine;

/// <summary>
/// Class responsible for handling player interaction.
/// </summary>
public class PlayerInteract : MonoBehaviour
{
    private PlayerInputCustom input;

    [Header("Icon when interaction is possible")]
    [SerializeField] private GameObject interactionSprite;

    /// <summary>
    /// If the player is near an IInterectable, this variable becomes that object,
    /// else, this variable is null
    /// </summary>
    public IInterectable InterectableObject { get; set; }

    private void Awake() =>
        input = FindObjectOfType<PlayerInputCustom>();

    private void Start() =>
        InterectableObject = null;

    private void OnEnable() =>
        input.MeleeLightAttack += InteractWithObject;

    private void OnDisable() =>
        input.MeleeLightAttack -= InteractWithObject;

    private void Update()
    {
        if (InterectableObject != null)
        {
            if (interactionSprite.activeSelf == false)
                interactionSprite.SetActive(true);
        }
        else
        {
            if (interactionSprite.activeSelf == true)
                interactionSprite.SetActive(false);
        }
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
