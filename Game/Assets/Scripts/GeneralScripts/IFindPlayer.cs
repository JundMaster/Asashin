/// <summary>
/// Interface with a method to find player. Executes a method every time
/// the player is spawned or lost. Must be applied by every class
/// that has player dependencies.
/// </summary>
public interface IFindPlayer 
{
    /// <summary>
    /// Finds player and updates important variables.
    /// </summary>
    void FindPlayer();

    /// <summary>
    /// Unregisters everything from player.
    /// </summary>
    void PlayerLost();
}
