using UnityEngine;

/// <summary>
/// Class responsible for enabling and disabling marks look at camera script.
/// </summary>
public class EnableMarksLookAtCamera : MonoBehaviour
{
    [SerializeField] private MarksLookAtCamera marksLookAtCamera;

    private void OnEnable() =>
        marksLookAtCamera.enabled = true;

    private void OnDisable() =>
        marksLookAtCamera.enabled = false;

    /// <summary>
    /// Called with animation event.
    /// </summary>
    public void DisableObject() =>
        gameObject.SetActive(false);
}
