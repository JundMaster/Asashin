/// <summary>
/// Class responsible for what happens when the player picks a Heart.
/// </summary>
public class HeartPickable : Pickable
{
    public override void Execute(PlayerStats playerStats)
    {
        playerStats.HealHealth(100);
        Destroy(gameObject);
    }   
}
