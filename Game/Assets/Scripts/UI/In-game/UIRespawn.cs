using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Class responsible for UIRespawn menu.
/// </summary>
public class UIRespawn : MonoBehaviour, IFindPlayer
{
    // Components
    private PlayerAnimations playerAnims;
    private EventSystem eventSys;
    private PlayerInputCustom input;

    [SerializeField] private GameObject respawnUI;
    [SerializeField] private GameObject confirmButton;

    private void Awake()
    {
        playerAnims = FindObjectOfType<PlayerAnimations>();
        eventSys = FindObjectOfType<EventSystem>();
        input = FindObjectOfType<PlayerInputCustom>();
    }

    private void OnEnable()
    {
        if (playerAnims != null)
            playerAnims.PlayerDiedEndOfAnimationUIRespawn += EnableRespawnUI;
    }

    private void OnDisable()
    {
        if (playerAnims != null)
            playerAnims.PlayerDiedEndOfAnimationUIRespawn -= EnableRespawnUI;
    }


    private void Update()
    {
        eventSys.SetSelectedGameObject(confirmButton);
    }



    /// <summary>
    /// After the player's death animation, this method happens.
    /// </summary>
    private void EnableRespawnUI()
    {
        FindObjectOfType<PlayerInputCustom>().SwitchActionMapToGamePaused();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        respawnUI.SetActive(true);
        eventSys.SetSelectedGameObject(confirmButton);
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
