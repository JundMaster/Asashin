/// <summary>
/// Interface that extends IItem. Used for usable items.
/// </summary>
public interface IUsableItems : IItem
{
    /// <summary>
    /// Which type of item is this.
    /// </summary>
    ListOfItems ItemType { get; }
}
