using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for handling player's health bar UI.
/// </summary>
public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Image healthBarShadow;

    // Colors
    private readonly Color green = new Color(0.324f, 0.849f, 0.436f, 1);
    private readonly Color shadowGreen = new Color(0.733f, 1f, 0.786f, 1);
    private readonly Color red = new Color(0.801f, 0.332f, 0.139f, 1);
    private readonly Color shadowRed = new Color(0.952f, 0.624f, 0.489f, 1);

    // Components
    private PlayerStats playerStats;

    private void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>();
    }

    private void OnEnable()
    {
        playerStats.TookDamage += TookDamage;
    }

    private void OnDisable()
    {
        playerStats.TookDamage -= TookDamage;
    }

    private void TookDamage() => StartCoroutine(UpdateHealthBar());

    /// <summary>
    /// Smoothly updates player's health bar.
    /// </summary>
    /// <returns>Null.</returns>
    private IEnumerator UpdateHealthBar()
    {
        if (playerStats.Health < 20)
        {
            healthBarFill.color = red;
            healthBarShadow.color = shadowRed;
        }
        else
        {
            healthBarFill.color = green;
            healthBarShadow.color = shadowGreen;
        }

        while (healthBarFill.fillAmount < (playerStats.Health / 100) - 0.005f ||
            healthBarFill.fillAmount > (playerStats.Health / 100) + 0.005f)
        {
            healthBarFill.fillAmount =
                Mathf.Lerp(
                    healthBarFill.fillAmount,
                    playerStats.Health / 100,
                    Time.unscaledDeltaTime * 2.5f);

            yield return null;
        }

        if (healthBarShadow != null)
            StartCoroutine(UpdateShadowHealthBar());
    }

    /// <summary>
    /// Updates shadow health bar after health bar finished its update.
    /// </summary>
    /// <returns>Null.</returns>
    private IEnumerator UpdateShadowHealthBar()
    {
        while (healthBarShadow.fillAmount < healthBarFill.fillAmount - 0.005f ||
            healthBarShadow.fillAmount > healthBarFill.fillAmount + 0.005f)
        {
            healthBarShadow.fillAmount =
                Mathf.Lerp(
                    healthBarShadow.fillAmount,
                    healthBarFill.fillAmount,
                    Time.unscaledDeltaTime * 15f);

            yield return null;
        }
    }
}
