using UnityEngine;
using System;

/// <summary>
/// Class responsible for UIRespawn menu.
/// </summary>
public class UIRespawn : MonoBehaviour, IFindPlayer
{
    // Components
    private PlayerAnimations playerAnims;

    [SerializeField] private GameObject respawnUI;

    private void Awake()
    {
        playerAnims = FindObjectOfType<PlayerAnimations>();
    }

    private void OnEnable()
    {
        if (playerAnims != null)
            playerAnims.PlayerDiedEndOfAnimationUIRespawn += EnableRespawnUI;
    }

    private void OnDisable()
    {
        playerAnims.PlayerDiedEndOfAnimationUIRespawn -= EnableRespawnUI;
    }

    /// <summary>
    /// After the player's death animation, this method happens.
    /// </summary>
    private void EnableRespawnUI()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        respawnUI.SetActive(true);
    }

    /// <summary>
    /// Defines what happens when respawn button is pressed.
    /// </summary>
    public void RespawnButton()
    {
        OnRespawnButtonPressed(SpawnTypeEnum.Respawn);
    }

    protected virtual void OnRespawnButtonPressed(SpawnTypeEnum typeOfSpawn) =>
        RespawnButtonPressed?.Invoke(typeOfSpawn);

    /// <summary>
    /// Event registered on SpawnerController.
    /// </summary>
    public event Action<SpawnTypeEnum> RespawnButtonPressed;

    public void QuitButton()
    {
        ////////////////////////////////////////////////////////////
        Debug.Log("Quit to main menu");
    }

    public void FindPlayer()
    {
        playerAnims = FindObjectOfType<PlayerAnimations>();
        playerAnims.PlayerDiedEndOfAnimationUIRespawn += EnableRespawnUI;
    }

    public void PlayerLost()
    {
        playerAnims.PlayerDiedEndOfAnimationUIRespawn -= EnableRespawnUI;
    }
}
