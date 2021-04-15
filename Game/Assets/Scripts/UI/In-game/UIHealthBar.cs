using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for handling player's health bar UI.
/// </summary>
public class UIHealthBar : MonoBehaviour, IFindPlayer
{
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Image healthBarShadow;

    // Colors
    private readonly Color green = new Color(0.324f, 0.849f, 0.436f, 1);
    private readonly Color red = new Color(0.801f, 0.332f, 0.139f, 1);

    // Components
    private PlayerStats playerStats;

    private void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>();
    }

    private void OnEnable()
    {
        if (playerStats != null) playerStats.TookDamage += UpdateHealthBar;
        if (playerStats != null) playerStats.HealedDamage += UpdateHealthBar;
    }

    private void OnDisable()
    {
        if (playerStats != null) playerStats.TookDamage -= UpdateHealthBar;
        if (playerStats != null) playerStats.HealedDamage -= UpdateHealthBar;
    }

    private void UpdateHealthBar() => StartCoroutine(UpdateHealthBarCoroutine());

    /// <summary>
    /// Smoothly updates player's health bar.
    /// </summary>
    /// <returns>Null.</returns>
    private IEnumerator UpdateHealthBarCoroutine()
    {
        if (playerStats.Health < 20)
        {
            healthBarFill.color = red;
        }
        else
        {
            healthBarFill.color = green;
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
    }

    public void FindPlayer()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        playerStats.TookDamage += UpdateHealthBar;
        playerStats.HealedDamage += UpdateHealthBar;
    }

    public void PlayerLost()
    {
        playerStats.TookDamage -= UpdateHealthBar;
        playerStats.HealedDamage -= UpdateHealthBar;
    }
}
