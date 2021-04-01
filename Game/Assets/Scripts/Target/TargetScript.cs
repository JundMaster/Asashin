using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for handing target position in canvas.
/// </summary>
public class TargetScript : MonoBehaviour
{
    // Components
    private Transform targetParent;

    [SerializeField] private Canvas parentCanvas;
    [SerializeField] private RawImage crosshair;

    private void Awake()
    {
        targetParent = 
            GameObject.FindGameObjectWithTag("targetUIForCinemachine").transform;
    }

    private void FixedUpdate()
    {
        if (targetParent.gameObject.activeSelf)
        {
            if (parentCanvas.enabled == false) parentCanvas.enabled = true;
        }
        else
        {
            if (parentCanvas.enabled) parentCanvas.enabled = false;
        }

        // Gets target position in world space
        Vector3 targetPosition = 
            Camera.main.WorldToScreenPoint(targetParent.transform.position);

        // Updates target in canvas to be the same as targetPosition
        crosshair.transform.position = targetPosition;
    }
}
