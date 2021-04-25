using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for handling music transitions when the player enters or
/// leaves combat.
/// </summary>
public class MusicCombatTransition : MonoBehaviour, IFindPlayer
{
    [SerializeField] private AudioSource baseBackground;
    [SerializeField] private AudioSource combatBackground;
    [Range(0.1f, 1f)][SerializeField] private float transitionTimeBetweenSongs;

    private float baseDefaultVolume;
    private float combatDefaultVolume;
    private enum MusicTrack { Basetrack, Combattrack, };
    private MusicTrack currentTrack;
    private IEnumerator switchTracks;

    // Components
    private Player player;
    private BossCutsceneControl bossCutscene;
    private EnemySimple[] simpleEnemies;
    
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        bossCutscene = FindObjectOfType<BossCutsceneControl>();

        baseDefaultVolume = baseBackground.volume;
        combatDefaultVolume = combatBackground.volume;

        simpleEnemies = FindObjectsOfType<EnemySimple>();
    }


    private void Update()
    {
        if (bossCutscene == null || bossCutscene?.OnBossFight == false)
        {
            if (player.PlayerCurrentlyFighting > 0)
            {
                if (currentTrack == MusicTrack.Basetrack)
                {
                    currentTrack = MusicTrack.Combattrack;
                    if (switchTracks != null) StopCoroutine(switchTracks);
                    switchTracks = SwitchToCombat();
                    StartCoroutine(switchTracks);
                }
            }
        }
    }

    /// <summary>
    /// When an enemy returns back to patrol state, it calls this method.
    /// Changes msuic back to background music.
    /// </summary>
    public void SwitchToBackgroundMusic()
    {
        if (bossCutscene == null || bossCutscene?.OnBossFight == false)
        {
            // Checks if all alive enemies are in patrol state
            byte enemiesNotInCombat = 0;

            for (int i = 0; i < simpleEnemies.Length; i++)
            {
                if (simpleEnemies[i] == null)
                    enemiesNotInCombat++;

                else if (simpleEnemies[i] != null &&
                    simpleEnemies[i].InCombat == false)
                    enemiesNotInCombat++;
            }

            // If all alive enemies are in patrol state it changes to normal
            // music
            if (enemiesNotInCombat == simpleEnemies.Length)
            {
                currentTrack = MusicTrack.Basetrack;
                if (switchTracks != null) StopCoroutine(switchTracks);
                switchTracks = SwitchToBackground();
                StartCoroutine(switchTracks);
            }
        }
    }

    /// <summary>
    /// Switches to base combat music smoothly.
    /// </summary>
    /// <returns>Null.</returns>
    private IEnumerator SwitchToCombat()
    {
        YieldInstruction wffu = new WaitForFixedUpdate();

        while (true)
        {
            // Resets combat music
            combatBackground.Stop();

            while (baseBackground.volume > 0)
            {
                baseBackground.volume -= Time.fixedUnscaledDeltaTime * (1 - transitionTimeBetweenSongs);
                yield return wffu;
            }
            baseBackground.Pause();

            combatBackground.Play();
            while (combatBackground.volume < combatDefaultVolume)
            {
                combatBackground.volume += Time.fixedUnscaledDeltaTime * (1 - transitionTimeBetweenSongs);
                yield return wffu;
            }

            yield return wffu;

            break;
        }
    }

    /// <summary>
    /// Switches to base background music smoothly.
    /// </summary>
    /// <returns>Null.</returns>
    private IEnumerator SwitchToBackground()
    {
        YieldInstruction wffu = new WaitForFixedUpdate();

        while (true)
        {
            while (combatBackground.volume > 0)
            {
                combatBackground.volume -= Time.fixedDeltaTime * (1 - transitionTimeBetweenSongs);
                yield return wffu;
            }
            combatBackground.Stop();

            baseBackground.Play();
            while (baseBackground.volume < baseDefaultVolume)
            {
                baseBackground.volume += Time.fixedDeltaTime * (1 - transitionTimeBetweenSongs);
                yield return wffu;
            }

            yield return wffu;
            break;
        }
    }

    public void FindPlayer()
    {
        player = FindObjectOfType<Player>();
    }

    public void PlayerLost()
    {
        // Left blank on purpose
    }
}
