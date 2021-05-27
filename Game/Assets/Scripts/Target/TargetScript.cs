using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for handing target position in canvas.
/// </summary>
public class TargetScript : MonoBehaviour
{
    // Components
    private Transform targetParent;
    private PauseSystem pause;

    [SerializeField] private GameObject spriteGameObject;
    [SerializeField] private RawImage crosshair;

    private void Awake()
    {
        targetParent = 
            GameObject.FindGameObjectWithTag("targetUIForCinemachine").transform;

        pause = FindObjectOfType<PauseSystem>();
    }

    private void OnEnable() =>
        pause.GamePaused += SetSpriteActive;

    private void OnDisable() =>
        pause.GamePaused -= SetSpriteActive;

    private void FixedUpdate()
    {
        if (targetParent.gameObject.activeSelf)
        {
            if (spriteGameObject.activeSelf == false)
                spriteGameObject.SetActive(true);
        }
        else
        {
            if (spriteGameObject.activeSelf == true)
                spriteGameObject.SetActive(false);
        }

        // Gets target position in world space
        Vector3 targetPosition = 
            Camera.main.WorldToScreenPoint(targetParent.transform.position);

        // Updates target in canvas to be the same as targetPosition
        crosshair.transform.position = targetPosition;
    }

    /// <summary>
    /// Sets canvas on or off if the player enters pause menu.
    /// </summary>
    /// <param name="pauseEnum">Paused or unpaused.</param>
    private void SetSpriteActive(PauseSystemEnum pauseEnum)
    {
        if (pauseEnum == PauseSystemEnum.Paused)
            spriteGameObject.GetComponent<RawImage>().enabled = false;
        else
        {
            spriteGameObject.GetComponent<RawImage>().enabled = true;
        }
    }
}
