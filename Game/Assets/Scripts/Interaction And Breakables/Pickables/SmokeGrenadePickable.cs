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
        int quantity = 0;
        switch (TypeOfDrop)
        {
            case TypeOfDropEnum.Treasure:
                quantity = rand.Next(1, 4); // between 1,3
                break;
            case TypeOfDropEnum.Lootbox:
                quantity = 1; // 1
                break;
            case TypeOfDropEnum.Enemy:
                quantity = 1; // 1
                break;
        }
        playerStats.SmokeGrenades += quantity;
    }
}
