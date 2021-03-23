/// <summary>
/// Interface implemented by piackable items.
/// </summary>
public interface IPickable
{
    /// <summary>
    /// Method that defines what happens when this item is picked.
    /// </summary>
    /// <param name="playerStats">PlayerStats variable.</param>
    void Execute(PlayerStats playerStats);

    /// <summary>
    /// What dropped this item.
    /// </summary>
    TypeOfDropEnum TypeOfDrop { get; }
}
