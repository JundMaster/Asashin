using UnityEngine;

/// <summary>
/// Class responsible for controlling instant kill icon when the player is near.
/// </summary>
public class EnemyInstantKillIcon : MonoBehaviour, IFindPlayer
{
    private PlayerMovement playerMovement;
    private SpriteRenderer instantKillSprite;
    private MarksLookAtCamera lookAtCamera;

    [SerializeField] private LayerMask playerLayers;

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        instantKillSprite = GetComponent<SpriteRenderer>();
        lookAtCamera = GetComponent<MarksLookAtCamera>();
    }

    private void Start() =>
        instantKillSprite.enabled = false;

    private void Update()
    {
        if (playerMovement != null)
        {
            // Turns stealth kill icon canvas on if stealth kill is possible 
            if (playerMovement.Walking == false)
            {
                if (instantKillSprite.enabled == true)
                    instantKillSprite.enabled = false;
            }

            // Only activates lookatcamera script if the player is close
            if (Vector3.Distance(transform.position, playerMovement.transform.position) < 5)
            {
                if (lookAtCamera.enabled == false)
                    lookAtCamera.enabled = true;
            } 
            else
            {
                if (lookAtCamera.enabled == true)
                    lookAtCamera.enabled = false;
            }
        }
    }

    public void FindPlayer() =>
        playerMovement = FindObjectOfType<PlayerMovement>();

    public void PlayerLost() =>
        instantKillSprite.enabled = false;
}
