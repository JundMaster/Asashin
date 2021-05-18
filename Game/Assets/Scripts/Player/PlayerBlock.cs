using UnityEngine;

/// <summary>
/// Class responsible for handling player's block.
/// </summary>
public class PlayerBlock : MonoBehaviour, IAction
{
    // Components
    private PlayerInputCustom input;
    private PlayerRoll roll;
    private PlayerUseItem useItem;
    private CinemachineTarget cineTarget;

    public bool Performing { get; private set; }

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        roll = GetComponent<PlayerRoll>();
        useItem = GetComponent<PlayerUseItem>();
        cineTarget = FindObjectOfType<CinemachineTarget>();
    }

    private void OnEnable() =>
        input.Block += Block;

    private void OnDisable() =>
        input.Block -= Block;

    /// <summary>
    /// Sets block state to true or false.
    /// </summary>
    /// <param name="condition">Parameter to check if the player
    /// pressed block or released block.</param>
    private void Block(bool condition)
    {
        if (condition == true)
        {
            if (roll.Performing == false && useItem.Performing == false)
            {
                Performing = true;
            }
        }
        else
        {
            Performing = false;
        }
    }

    public void ComponentFixedUpdate()
    {
        if (Performing)
        {
            RotationToCurrentTarget();
        }
    }

    public void ComponentUpdate()
    {
        //
    }

    /// <summary>
    /// Rotates the character towards something.
    /// </summary>
    private void RotationToCurrentTarget()
    {
        if (cineTarget.Targeting)
        {
            transform.RotateTo(cineTarget.CurrentTarget.position);
        }
    }
}
