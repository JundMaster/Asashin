using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for handling health flask behaviour.
/// </summary>
public class HealthFlask : ItemBehaviour
{
    /// <summary>
    /// Heals player for 50 health points.
    /// Must run on a coroutine to check if the player didn't die while
    /// drinking the flask.
    /// </summary>
    public override void Execute()
    {
        StartCoroutine(Heal());
    }

    private IEnumerator Heal()
    {
        yield return new WaitForFixedUpdate();
        if (playerStats.Health > 0)
        {
            playerStats.HealHealth(50f);
            playerStats.HealthFlasks--;
            base.Execute();
        }
    }
}
