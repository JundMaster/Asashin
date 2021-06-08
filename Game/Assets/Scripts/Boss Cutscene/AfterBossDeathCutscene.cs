using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// What happens after boss dies.
/// </summary>
public class AfterBossDeathCutscene : MonoBehaviour
{
    // Components
    private EnemyBoss boss;
    private PlayerInputCustom input;
    private MusicReferences musicSource;

    [SerializeField] private PlayableDirector finalCutscene;
    [SerializeField] private GameObject bossHealthbar;

    /// <summary>
    /// Initializes variables and registers do boss die event.
    /// </summary>
    public void FindObjectsAndRegisterEvents()
    {
        boss = FindObjectOfType<EnemyBoss>();
        input = FindObjectOfType<PlayerInputCustom>();
        musicSource = FindObjectOfType<MusicReferences>();

        boss.Die += BossDeath;
    }

    private void OnDisable()
    {
        if (boss != null)
            boss.Die -= BossDeath;

        if (musicSource != null)
            musicSource.Music.Play();
    }

    private void BossDeath()
    {
        input.SwitchActionMapToDisable();
        finalCutscene.Play();
        StartCoroutine(ChangeMusicCoroutine());
        bossHealthbar.SetActive(false);
    }

    /// <summary>
    /// Lowers volume of current boss music.
    /// Cutscene music will be played through timeline.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChangeMusicCoroutine()
    {
        YieldInstruction wffu = new WaitForFixedUpdate();

        while (true)
        {
            while (musicSource.Music.volume > 0)
            {
                musicSource.Music.volume -= Time.fixedDeltaTime * 0.25f;
                yield return wffu;
            }
            musicSource.Music.Stop();
            musicSource.CombatMusic.Stop();

            yield return wffu;
            break;
        }
    }

    /// <summary>
    /// Called with timelined signal.
    /// </summary>
    public void ChangeToMainMenu()
    {
        FindObjectOfType<SceneControl>().LoadScene(SceneEnum.MainMenu);
    }
}
