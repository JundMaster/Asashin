using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for handing target position in canvas.
/// </summary>
public class TargetScript : MonoBehaviour
{
    // Components
    [SerializeField] private Transform targetParent;

    [SerializeField] private RawImage crosshair;

    private void Update()
    {
        // Gets target position in world space
        Vector3 targetPosition = 
            Camera.main.WorldToScreenPoint(targetParent.transform.position);

        // Updates target in canvas to be the same as targetPosition
        crosshair.transform.position = targetPosition;
    }
}
