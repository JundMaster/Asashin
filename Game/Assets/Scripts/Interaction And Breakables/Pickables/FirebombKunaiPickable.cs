/// <summary>
/// Class responsible for what happens when the player picks a FirebombKunai.
/// </summary>
public class FirebombKunaiPickable : Pickable
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
                quantity = rand.Next(3, 7); // between 3,6
                break;
            case TypeOfDropEnum.Lootbox:
                quantity = rand.Next(1, 3); // between 1,2
                break;
            case TypeOfDropEnum.Enemy:
                quantity = 1; // between 1,5
                break;
        }
        playerStats.FirebombKunais += quantity;
    }
}
