using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIRespawn : MonoBehaviour
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
        respawnUI.SetActive(true);
    }

    public void RespawnButton()
    {
        SceneControl sceneControl = FindObjectOfType<SceneControl>();
        sceneControl.LoadScene(sceneControl.SceneToLoad());
        OnRespawnButtonPressed();
    }

    protected virtual void OnRespawnButtonPressed() =>
        RespawnButtonPressed?.Invoke();

    /// <summary>
    /// Event registered on PlayerInputCustom.
    /// </summary>
    public event Action RespawnButtonPressed;

    public void QuitButton()
    {

    }
}
