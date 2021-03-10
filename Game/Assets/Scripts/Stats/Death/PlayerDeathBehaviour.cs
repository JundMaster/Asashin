using UnityEngine;

/// <summary>
/// Handles player character's death.
/// </summary>
public class PlayerDeathBehaviour : DeathBehaviour
{
    public override void Die()
    {
        Debug.Log("Player died");
    }
}
