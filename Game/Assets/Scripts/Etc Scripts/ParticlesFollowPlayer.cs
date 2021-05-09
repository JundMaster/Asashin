using UnityEngine;

/// <summary>
/// Class responsible for handling particles positions (ex: rain, dust particles).
/// </summary>
public class ParticlesFollowPlayer : MonoBehaviour, IFindPlayer
{
    private Player player;

    private void Awake() =>
        player = FindObjectOfType<Player>();

    public void FindPlayer() =>
        player = FindObjectOfType<Player>();

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
