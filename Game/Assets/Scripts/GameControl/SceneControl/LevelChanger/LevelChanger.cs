using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for triggering level change when the player hits
/// the level changer collider.
/// </summary>
public class LevelChanger : MonoBehaviour
{
    [SerializeField] private SceneEnum changeToLevel;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private Transform playerGoToPosition;
    
    private PlayerInputCustom input;

    private bool changedScene;

    private float smoothTimeRotation;
    private readonly float TURNSPEED = 0.125f;

    private void Awake() =>
        input = FindObjectOfType<PlayerInputCustom>();

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
            FindObjectOfType<Player>().gameObject.layer = 31;
            input.SwitchActionMapToDisable();
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
        SpawnerController spawner = FindObjectOfType<SpawnerController>();

        YieldInstruction wffu = new WaitForFixedUpdate();
        while (playerMovement.transform.position.Similiar(playerGoToPosition.position, 0.1f) == false)
        {
            playerMovement.MovementSpeed = 4f;
            Vector3 dir = playerMovement.transform.Direction(playerGoToPosition);
            playerController.Move((playerMovement.MovementSpeed * 0.75f) * dir * Time.fixedDeltaTime);
            playerMovement.transform.RotateToSmoothly(playerGoToPosition.position, ref smoothTimeRotation, TURNSPEED);

            if (Vector3.Distance(playerMovement.transform.position, playerGoToPosition.position) < 1)
            {
                if (changedScene == false)
                {
                    // Saves all stats so it can respawn the player with these stats in the next scene
                    spawner.GameState.SavePlayerStats();
                    OnLevelChanged(changeToLevel);
                    changedScene = true;
                }
            }
            yield return wffu;
        }
    }

    protected virtual void OnLevelChanged(SceneEnum spawnType) => 
        LevelChanged?.Invoke(spawnType);

    /// <summary>
    /// Event registered on SpawnerController.
    /// </summary>
    public event Action<SceneEnum> LevelChanged;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.6f);
        Gizmos.DrawCube(transform.position + boxCollider.center, boxCollider.size);
    }
}
