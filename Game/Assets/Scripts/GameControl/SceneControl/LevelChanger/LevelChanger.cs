using UnityEngine;
using System.Collections;

/// <summary>
/// Class responsible for triggering level change when the player hits
/// the level changer collider.
/// </summary>
public class LevelChanger : MonoBehaviour
{
    [SerializeField] private SceneEnum changeToLevel;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private Transform playerGoToPosition;
    [SerializeField] private PlayerSavedStatsScriptableObj playerSavedStats;

    private SceneControl sceneController;
    private GameState gameState;

    private bool changedScene;

    private void Awake()
    {
        sceneController = FindObjectOfType<SceneControl>();
        gameState = new GameState(playerSavedStats);
    }

    private void Start()
    {
        boxCollider.isTrigger = true;
        changedScene = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            boxCollider.enabled = false;
            StartCoroutine(ChangeSceneCoroutine());
        }
    }

    /// <summary>
    /// Starts moving player towards ending position. When the character is
    /// reaching the final position, it will triger scene change.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChangeSceneCoroutine()
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        CharacterController playerController = playerMovement.GetComponent<CharacterController>();

        while (playerMovement.transform.position.Similiar(playerGoToPosition.position, 0.1f) == false)
        {
            playerMovement.MovementSpeed = 4f;
            Vector3 dir = playerMovement.transform.Direction(playerGoToPosition);
            playerController.Move((playerMovement.MovementSpeed * 0.75f) * dir * Time.fixedDeltaTime);
            playerMovement.transform.RotateTo(playerGoToPosition.position);

            if (Vector3.Distance(playerMovement.transform.position, playerGoToPosition.position) < 1)
            {
                if (changedScene == false)
                {
                    gameState.SavePlayerStats();
                    sceneController.LoadScene(changeToLevel);
                    changedScene = true;
                }
            }
            yield return null;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.6f);
        Gizmos.DrawCube(transform.position + boxCollider.center, boxCollider.size);
    }
}
