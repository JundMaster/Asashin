using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Class responsible for UIRespawn menu.
/// </summary>
public class UIRespawn : MonoBehaviour, IFindPlayer
{
    // Components
    private PlayerAnimations playerAnims;
    private EventSystem eventSys;
    private GameObject lastSelectedGameObject;

    [SerializeField] private GameObject respawnUI;
    [SerializeField] private GameObject confirmButton;

    private void Awake()
    {
        playerAnims = FindObjectOfType<PlayerAnimations>();
        eventSys = FindObjectOfType<EventSystem>();
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

    /// <summary>
    /// After the player's death animation, this method happens.
    /// </summary>
    private void EnableRespawnUI()
    {
        FindObjectOfType<PlayerInputCustom>().SwitchActionMapToGamePaused();

        respawnUI.SetActive(true);
        StartCoroutine(WaitForLoading());
    }

    private IEnumerator WaitForLoading()
    {
        yield return new WaitForSecondsRealtime(3f);
        OnRespawnButtonPressed(SpawnTypeEnum.Respawn);
    }


    protected virtual void OnRespawnButtonPressed(SpawnTypeEnum typeOfSpawn) =>
        RespawnButtonPressed?.Invoke(typeOfSpawn);

    /// <summary>
    /// Event registered on SpawnerController.
    /// </summary>
    public event Action<SpawnTypeEnum> RespawnButtonPressed;

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
