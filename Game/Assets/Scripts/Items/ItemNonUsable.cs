using UnityEngine;

/// <summary>
/// Class responsible for what happens when an item is used. This class
/// is common to every item in-game.
/// </summary>
public class ItemNonUsable : MonoBehaviour, IItem
{
    [SerializeField] protected ItemBehaviour behaviour;

    /// <summary>
    /// Method that defines what happens when the item is used.
    /// </summary>
    public virtual void Execute()
    {
        behaviour.Execute();
    }
}
