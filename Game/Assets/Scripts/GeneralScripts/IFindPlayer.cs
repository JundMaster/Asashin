/// <summary>
/// Interface with a method to find player. Executes a method every time
/// the player is spawned or lost. Should be applied by every class
/// that has player dependencies.
/// </summary>
public interface IFindPlayer 
{
    /// <summary>
    /// Finds player.
    /// </summary>
    void FindPlayer();

    /// <summary>
    /// Unregisters everything from player.
    /// </summary>
    void PlayerLost();
}
