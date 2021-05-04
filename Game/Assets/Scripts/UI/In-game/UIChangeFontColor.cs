using UnityEngine;
using TMPro;

/// <summary>
/// Class responsible for changing font color.
/// </summary>
public class UIChangeFontColor : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI optionTitleText;
    [SerializeField] private TextMeshProUGUI optionText;

    public void WhiteColor()
    {
        if (optionTitleText != null) optionTitleText.color = Color.white;
        if (optionText != null) optionText.color = Color.white;
    }

    public void GreyColor()
    {
        if (optionTitleText != null) optionTitleText.color = Color.grey;
        if (optionText != null) optionText.color = Color.grey;
    }
}
