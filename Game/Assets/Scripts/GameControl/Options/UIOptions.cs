using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Class responsible for controlling UIOptions. Updates values on ui
/// and values on Options.cs.
/// </summary>
public class UIOptions : MonoBehaviour
{
    [SerializeField] private GameObject initialSelectedButton;
    [SerializeField] private OptionsScriptableObj defaultOptions;

    [Header("Difficulty")]
    [SerializeField] private TextMeshProUGUI difficultyText;

    [Header("Graphics Quality")]
    [SerializeField] private TextMeshProUGUI graphicsQualityText;
    [SerializeField] private TextMeshProUGUI shadowQualityText;

    // Components
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
        //EventSystem.current.SetSelectedGameObject(initialSelectedButton);
        UpdateAllUI();
    }

    public void LeftButton(string option)
    {
        switch (option)
        {
            case "Difficulty":
                if (currentValues.Difficulty - 1 >= 0) currentValues.Difficulty--;
                else currentValues.Difficulty = defaultOptions.MaxDifficulty;
                UpdateUI(GameOptionsEnum.Difficulty, currentValues.Difficulty);
                break;

            case "Graphics Quality":
                if (currentValues.GraphicsQuality - 1 >= 0) currentValues.GraphicsQuality--;
                else currentValues.GraphicsQuality = defaultOptions.MaxGraphicsQuality;
                UpdateUI(GameOptionsEnum.GraphicsQuality, currentValues.GraphicsQuality);
                break;

            case "Shadow Quality":
                if (currentValues.ShadowQuality - 1 >= 0) currentValues.ShadowQuality--;
                else currentValues.ShadowQuality = defaultOptions.MaxShadowQuality;
                UpdateUI(GameOptionsEnum.ShadowQuality, currentValues.ShadowQuality);
                break;
        }
    }

    public void RightButton(string option)
    {
        switch (option)
        {
            case "Difficulty":
                if (currentValues.Difficulty + 1 <= defaultOptions.MaxDifficulty) currentValues.Difficulty++;
                else currentValues.Difficulty = 0;
                UpdateUI(GameOptionsEnum.Difficulty, currentValues.Difficulty);
                break;

            case "Graphics Quality":
                if (currentValues.GraphicsQuality + 1 <= defaultOptions.MaxGraphicsQuality) currentValues.GraphicsQuality++;
                else currentValues.GraphicsQuality = 0;
                UpdateUI(GameOptionsEnum.GraphicsQuality, currentValues.GraphicsQuality);
                break;

            case "Shadow Quality":
                if (currentValues.ShadowQuality + 1 <= defaultOptions.MaxShadowQuality) currentValues.ShadowQuality++;
                else currentValues.ShadowQuality = 0;
                UpdateUI(GameOptionsEnum.ShadowQuality, currentValues.ShadowQuality);
                break;
        }
    }

    private void UpdateUI(GameOptionsEnum option, short number)
    {
        switch(option)
        {
            case GameOptionsEnum.Difficulty:
                switch(number)
                {
                    case 0:
                        difficultyText.text = "Easy";
                        break;
                    case 1:
                        difficultyText.text = "Medium";
                        break;
                    case 2:
                        difficultyText.text = "Hard";
                        break;
                    case 3:
                        difficultyText.text = "Impossible";
                        break;
                }
                break;

            case GameOptionsEnum.GraphicsQuality:
                switch (number)
                {
                    case 0:
                        graphicsQualityText.text = "Low";
                        break;
                    case 1:
                        graphicsQualityText.text = "Medium";
                        break;
                    case 2:
                        graphicsQualityText.text = "High";
                        break;
                }
                break;

            case GameOptionsEnum.ShadowQuality:
                switch (number)
                {
                    case 0:
                        shadowQualityText.text = "Low";
                        break;
                    case 1:
                        shadowQualityText.text = "Medium";
                        break;
                    case 2:
                        shadowQualityText.text = "High";
                        break;
                }
                break;
        }
    }

    private void UpdateAllUI()
    {
        switch (currentValues.Difficulty)
        {
            case 0:
                difficultyText.text = "Easy";
                break;
            case 1:
                difficultyText.text = "Medium";
                break;
            case 2:
                difficultyText.text = "Hardcore";
                break;
            case 3:
                difficultyText.text = "Impossible";
                break;
        }

        switch (currentValues.GraphicsQuality)
        {
            case 0:
                difficultyText.text = "Low";
                break;
            case 1:
                difficultyText.text = "Medium";
                break;
            case 2:
                difficultyText.text = "High";
                break;
        }
        switch (currentValues.ShadowQuality)
        {
            case 0:
                difficultyText.text = "Low";
                break;
            case 1:
                difficultyText.text = "Medium";
                break;
            case 2:
                difficultyText.text = "High";
                break;
        }
    }

    /// <summary>
    /// Passes a struct with current values to Options class, updates current
    /// values and saves those values in a file.
    /// </summary>
    public void AcceptValues() => options.UpdateValues(currentValues);
}
