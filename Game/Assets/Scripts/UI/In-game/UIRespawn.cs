using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    }

    public void QuitButton()
    {

    }
}
