using UnityEngine;

/// <summary>
/// Class responsible for blocking level changer in case the player is currently fighting.
/// </summary>
public class BlockerOnLevelChanger : MonoBehaviour, IFindPlayer
{
    [SerializeField] private BoxCollider boxCollider;
    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();    
    }

    private void Start()
    {
        boxCollider.enabled = false;
        boxCollider.isTrigger = false;
    }

    public void FindPlayer()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (player != null)
        {
            if (player.PlayerCurrentlyFighting)
            {
                if (boxCollider.enabled == false) boxCollider.enabled = true;
                return;
            }
            if (boxCollider.enabled) boxCollider.enabled = false;
        }
    }

    public void PlayerLost()
    {
        // Left blank on purpose
    }
}
