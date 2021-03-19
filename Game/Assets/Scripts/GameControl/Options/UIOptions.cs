using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for controlling UIOptions.
/// </summary>
public class UIOptions : MonoBehaviour
{
    [Header("Difficulty")]
    [SerializeField] private GameObject[] difficultyLevels;
    [SerializeField] private GameObject[] arrows;

    private Options options;
    private OptionsTemporaryValues currentValues;

    private void Awake()
    {
        options = FindObjectOfType<Options>();
    }

    /// <summary>
    /// Every time this script is enabled, the values are copied from Options
    /// OptionsTemporaryValues struct, which is a struct with the current
    /// option values.
    /// </summary>
    private void OnEnable()
    {
        currentValues = options.CurrentValues;

        UpdateUI();
    }

    public void LeftButton(string whichButton)
    {
        switch (whichButton)
        {
            
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < difficultyLevels.Length; i++)
        {
            if (i == currentValues.Difficulty)
                difficultyLevels[i].SetActive(true);
            else
                difficultyLevels[i].SetActive(false);
        }
    }

    /// <summary>
    /// Passes a struct with current values to Options class, updates current
    /// values and saves those values in a file.
    /// </summary>
    public void AcceptValues() => options.UpdateValues(currentValues);
}
