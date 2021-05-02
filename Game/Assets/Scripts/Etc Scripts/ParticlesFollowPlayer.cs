using UnityEngine;

/// <summary>
/// Class responsible for handling rain position.
/// </summary>
public class ParticlesFollowPlayer : MonoBehaviour, IFindPlayer
{
    private Player player;

    public void FindPlayer()
    {
        player = FindObjectOfType<Player>();
    }

    public void PlayerLost()
    {
        // Left blank on purpose
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            Vector3 playerPos = 
                new Vector3(
                    player.transform.position.x, 
                    transform.position.y, 
                    player.transform.position.z);

            transform.position = playerPos;
        }
    }
}
