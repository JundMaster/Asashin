using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
