using UnityEngine;

/// <summary>
/// Class responsible for controlling instant kill icon when the player is near.
/// </summary>
public class EnemyInstantKillIcon : MonoBehaviour, IFindPlayer
{
    [SerializeField] private GameObject instantKillSprite;
    private PlayerMovement playerMovement;

    private void Awake() =>
        playerMovement = FindObjectOfType<PlayerMovement>();

    private void Start() =>
        instantKillSprite.SetActive(false);

    private void Update()
    {
        if (playerMovement != null)
        {
            // Turns stealth kill icon canvas on if stealth kill is possible 
            if (playerMovement.Walking == false)
            {
                if (instantKillSprite.activeSelf == true)
                    instantKillSprite.SetActive(false);
            }
        }
    }

    public void FindPlayer() =>
        playerMovement = FindObjectOfType<PlayerMovement>();

    public void PlayerLost() =>
        instantKillSprite.SetActive(false);
}
