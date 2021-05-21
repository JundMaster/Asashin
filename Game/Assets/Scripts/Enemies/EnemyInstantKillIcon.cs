using UnityEngine;

/// <summary>
/// Class responsible for controlling instant kill icon when the player is near.
/// </summary>
public class EnemyInstantKillIcon : MonoBehaviour, IFindPlayer
{
    [SerializeField] private GameObject instantKillSprite;
    private PlayerMovement playerMovement;
    private MarksLookAtCamera lookAtCamera;

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        lookAtCamera = GetComponent<MarksLookAtCamera>();
    }

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
        instantKillSprite.SetActive(false);
}
