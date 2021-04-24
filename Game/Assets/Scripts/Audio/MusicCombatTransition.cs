using System.Collections;
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
    private float timeAfterLeavingCombat;
    private float delayAfterLeavingCombat;
    private enum MusicTrack { Basetrack, Combattrack, };
    private MusicTrack currentTrack;

    // Components
    private Player player;
    private BossCutsceneControl bossCutscene;

    private IEnumerator switchTracks;
    
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        bossCutscene = FindObjectOfType<BossCutsceneControl>();

        baseDefaultVolume = baseBackground.volume;
        combatDefaultVolume = combatBackground.volume;
    }

    private void OnEnable()
    {
        if (player != null) player.EnteredCombat += SwitchMusics;
    }

    private void OnDisable()
    {
        if (player != null) player.EnteredCombat -= SwitchMusics;
    }

    private void Start()
    {
        switchTracks = null;
        delayAfterLeavingCombat = 2f;
    }

    private void Update()
    {
        // Keeps time so it won't change to base music immediatly after
        // leaving the fight
        if (bossCutscene == null)
        {
            if (player.PlayerCurrentlyFighting)
            {
                timeAfterLeavingCombat = Time.time;
            }
            else
            {
                if (currentTrack == MusicTrack.Combattrack)
                    SwitchMusics(false);
            }
            return;
        }

        // Else if the boss cutscene is not null
        if (bossCutscene.OnBossFight == false)
        {
            if (player.PlayerCurrentlyFighting)
            {
                timeAfterLeavingCombat = Time.time;
            }
            else
            {
                if (currentTrack == MusicTrack.Combattrack)
                    SwitchMusics(false);
            }
        }
    }

    /// <summary>
    /// Switches between 2 musics through an event.
    /// </summary>
    /// <param name="condition">True to change to one music, false to change
    /// to another one.</param>
    private void SwitchMusics(bool condition)
    {
        if (bossCutscene == null)
        {
            if (Time.time - timeAfterLeavingCombat > delayAfterLeavingCombat)
            {
                if (condition == true)
                {
                    currentTrack = MusicTrack.Combattrack;
                    switchTracks = SwitchToCombat();
                    StartCoroutine(switchTracks);
                    return;
                }
                else if (condition == false)
                {
                    currentTrack = MusicTrack.Basetrack;
                    switchTracks = SwitchToBackground();
                    StartCoroutine(switchTracks);
                }
            }
            return;
        }

        // Else if the boss cutscene is not null
        if (bossCutscene.OnBossFight == false)
        {
            if (Time.time - timeAfterLeavingCombat > delayAfterLeavingCombat)
            {
                if (condition == true)
                {
                    currentTrack = MusicTrack.Combattrack;
                    switchTracks = SwitchToCombat();
                    StartCoroutine(switchTracks);
                    return;
                }
                else if (condition == false)
                {
                    currentTrack = MusicTrack.Basetrack;
                    switchTracks = SwitchToBackground();
                    StartCoroutine(switchTracks);
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

        yield return wffu;

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

            if (player.PlayerCurrentlyFighting == false)
                StartCoroutine(SwitchToBackground());

            break;
        }
        switchTracks = null;
    }

    /// <summary>
    /// Switches to base background music smoothly.
    /// </summary>
    /// <returns>Null.</returns>
    private IEnumerator SwitchToBackground()
    {
        YieldInstruction wffu = new WaitForFixedUpdate();

        yield return wffu;

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
        switchTracks = null;
    }

    public void FindPlayer()
    {
        player = FindObjectOfType<Player>();
        player.EnteredCombat += SwitchMusics;
    }

    public void PlayerLost()
    {
        // Left blank on purpose
    }
}
