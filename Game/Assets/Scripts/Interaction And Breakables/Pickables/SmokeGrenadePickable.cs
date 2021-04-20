/// <summary>
/// Class responsible for what happens when the player picks a SmokeGrenade.
/// </summary>
public class SmokeGrenadePickable : Pickable
{
    /// <summary>
    /// What happens when the player collides with this item.
    /// </summary>
    /// <param name="playerStats">Player stats variable.</param>
    public override void Execute(PlayerStats playerStats)
    {
        base.Execute(playerStats);
        playerStats.SmokeGrenades += quantity;
    }
}
