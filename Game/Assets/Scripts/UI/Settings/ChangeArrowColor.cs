using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for changing arrow colors when selected.
/// </summary>
public class ChangeArrowColor : MonoBehaviour
{
    [SerializeField]
    private GameObject leftArrow;

    [SerializeField]
    private GameObject rightArrow;

    [SerializeField]
    private Sprite toggledArrow;

    [SerializeField]
    private Sprite untoggledArrow;

    public void ToggleSprite()
    {
        rightArrow.GetComponent<Image>().sprite = toggledArrow;
        leftArrow.GetComponent<Image>().sprite = toggledArrow;
    }

    public void UntoggledSprite()
    {
        rightArrow.GetComponent<Image>().sprite = untoggledArrow;
        leftArrow.GetComponent<Image>().sprite = untoggledArrow;
    }
}
