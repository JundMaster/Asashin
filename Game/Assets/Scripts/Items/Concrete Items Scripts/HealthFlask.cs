using UnityEngine;

/// <summary>
/// Class responsible for handling health flask behaviour.
/// </summary>
public class HealthFlask : ItemBehaviour
{
    /// <summary>
    /// Heals player for 50 health points.
    /// </summary>
    public override void Execute()
    {
        playerStats.HealHealth(50f);
        playerStats.HealthFlasks--;
        base.Execute();
    }
}
