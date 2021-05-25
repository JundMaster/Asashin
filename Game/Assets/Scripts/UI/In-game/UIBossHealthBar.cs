using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for controlling boss ui health bar.
/// </summary>
public class UIBossHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;

    // Components
    private Stats enemyStats;
    public Stats EnemyStats { set => enemyStats = value; }

    private void Awake()
    {
        enemyStats = FindObjectOfType<EnemyBoss>().Stats;
        RegisterEvents();
    }

    public void RegisterEvents()
    {
        if (enemyStats != null)
        {
            enemyStats.AnyDamageOnEnemy += UpdateHealthBar;
            enemyStats.Die += UpdateHealthBarToZero;
        }
    }

    private void OnDisable()
    {
        if (enemyStats != null)
        {
            enemyStats.AnyDamageOnEnemy -= UpdateHealthBar;
            enemyStats.Die -= UpdateHealthBarToZero;
        }
    }
        

    private void UpdateHealthBar() =>
        StartCoroutine(UpdateHealthBarCoroutine());

    private void UpdateHealthBarToZero() =>
        StartCoroutine(UpdateHealthBarToZeroCoroutine());

    /// <summary>
    /// Smoothly updates boss' health bar.
    /// </summary>
    /// <returns>Null.</returns>
    private IEnumerator UpdateHealthBarCoroutine()
    {
        while (healthBarFill.fillAmount < (enemyStats.Health / enemyStats.MaxHealth) - 0.005f ||
            healthBarFill.fillAmount > (enemyStats.Health / enemyStats.MaxHealth) + 0.005f)
        {
            healthBarFill.fillAmount =
                Mathf.Lerp(
                    healthBarFill.fillAmount,
                    enemyStats.Health / enemyStats.MaxHealth,
                    Time.unscaledDeltaTime * 2.5f);

            yield return null;
        }
    }

    /// <summary>
    /// Smoothly updates boss' health bar to zero.
    /// </summary>
    /// <returns>Null.</returns>
    private IEnumerator UpdateHealthBarToZeroCoroutine()
    {
        while (healthBarFill.fillAmount > 0)
        {
            healthBarFill.fillAmount =
                Mathf.Lerp(
                    healthBarFill.fillAmount,
                    0,
                    Time.unscaledDeltaTime * 2.5f);

            yield return null;
        }
    }
}
