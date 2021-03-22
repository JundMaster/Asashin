using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeFontColor : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI optionTitleText;
    [SerializeField]
    private TextMeshProUGUI optionText;

    public void WhiteColor()
    {
        if(optionTitleText != null) optionTitleText.color = Color.white;
        if (optionText != null) optionText.color = Color.white;
    }

    public void GreyColor()
    {
        if (optionTitleText != null) optionTitleText.color = Color.grey;
        if (optionText != null) optionText.color = Color.grey;
    }
}
