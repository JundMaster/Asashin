using UnityEngine;

/// <summary>
/// Class responsible for calling an animation event on uiRespawn.
/// </summary>
public class UIRespawnAnimationEvent : MonoBehaviour
{
    private UIRespawn uiRespawn;

    private void Awake()
    {
        uiRespawn = GetComponentInParent<UIRespawn>();
    }

    /// <summary>
    /// Animation event to select confirmation button on uiRespawn.
    /// </summary>
    public void SelectButton() =>
        uiRespawn.SelectButton();
}
