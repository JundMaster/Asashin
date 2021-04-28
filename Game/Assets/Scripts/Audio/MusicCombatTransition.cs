using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class responsible for handling music transitions when the player enters or
/// leaves combat. Has singleton from audio controller.
/// </summary>
public class MusicCombatTransition : MonoBehaviour
{
    [SerializeField] private AudioSource baseBackground;
    [SerializeField] private AudioSource combatBackground;
    [Range(0.1f, 1f)][SerializeField] private float transitionTimeBetweenSongs;

    // Music management
    private float baseDefaultVolume;
    private float combatDefaultVolume;
    private enum MusicTrack { Basetrack, Combattrack, };
    private MusicTrack currentTrack;
    private IEnumerator switchTracks;

    // Components
    private Player player;
    private BossCutsceneControl bossCutscene;
    private EnemySimple[] simpleEnemies;
    private CurrentLevelDefinitions levelDefinitions;

    private void Awake()
    {
        baseDefaultVolume = baseBackground.volume;
        combatDefaultVolume = combatBackground.volume;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Triggered every time a scene is loaded. Finds all components needed for
    /// this class.
    /// </summary>
    /// <param name="scene">Null.</param>
    /// <param name="mode">Null.</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) =>
        StartCoroutine(GetVariables());

    private IEnumerator GetVariables()
    {
        YieldInstruction wffu = new WaitForFixedUpdate();
        
        bossCutscene = FindObjectOfType<BossCutsceneControl>();
        simpleEnemies = FindObjectsOfType<EnemySimple>();
        levelDefinitions = FindObjectOfType<CurrentLevelDefinitions>();
        
        yield return wffu;
        player = FindObjectOfType<Player>();

        SwitchBackgroundToThisLevel();
        SwitchToBackgroundMusic();
    }

    private void Update()
    {
        if (bossCutscene == null || 
            (bossCutscene != null && bossCutscene.OnBossFight == false))
        {
            if (player != null)
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
    /// When an enemy returns back to patrol state, it calls this method.
    /// Changes music back to background music.
    /// </summary>
    public void SwitchToBackgroundMusic()
    {
        if (bossCutscene == null ||
            (bossCutscene != null && bossCutscene.OnBossFight == false))
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
    /// Switches to base background music smoothly.
    /// </summary>
    /// <returns>Null.</returns>
    private IEnumerator SwitchToBackground()
    {
        YieldInstruction wffu = new WaitForFixedUpdate();

        while (true)
        {
            if (combatBackground.isPlaying)
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
            }

            yield return wffu;
            break;
        }
    }

    /// <summary>
    /// Checks if it should change the audiotrack.
    /// </summary>
    private void SwitchBackgroundToThisLevel()
    {
        if (levelDefinitions.ThisArea.Music != baseBackground.clip)
        {
            baseBackground.clip = levelDefinitions.ThisArea.Music;
            baseBackground.Play();
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
