using UnityEngine;
using Cinemachine;
using System.Collections;

public class BossCutsceneControl : MonoBehaviour
{
    [Header("Boss stuff")]
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject smokeParticles;
    [SerializeField] private Transform bossPosition;
    [SerializeField] private AudioClip bossMusic;

    [Header("Player stuff")]
    [SerializeField] private Transform playerFinalPosition;
    private CharacterController player;

    // Components
    private CinemachineBrain cineBrain;
    private PlayerInputCustom input;
    private AudioSource musicSource;

    private void Awake()
    {
        cineBrain = Camera.main.GetComponent<CinemachineBrain>();
        input = FindObjectOfType<PlayerInputCustom>();
        musicSource = FindObjectOfType<MusicReferences>().Music;
    }

    public void StartCutscene() => StartCoroutine(StartCutsceneCoroutine());
    private IEnumerator StartCutsceneCoroutine()
    {
        SetCameraBlend(0);
        SetPlayerControls(false);

        player = FindObjectOfType<PlayerMovement>().Controller;

        while(player.transform.position.Similiar(playerFinalPosition.position, 0.1f) == false)
        {
            Vector3 dir = player.transform.Direction(playerFinalPosition); 
            player.Move(dir * Time.fixedDeltaTime);
            player.transform.RotateTo(bossPosition.position);
            yield return null;          
        }

        float currentVolume = musicSource.volume * 1.5f;
        YieldInstruction wffu = new WaitForFixedUpdate();
        while (true)
        {
            while (musicSource.volume > 0)
            {
                musicSource.volume -= Time.fixedDeltaTime * 0.1f;
                yield return wffu;
            }
            musicSource.Stop();
            musicSource.clip = bossMusic;
            musicSource.Play();
            while (musicSource.volume < currentVolume)
            {
                musicSource.volume += Time.fixedDeltaTime * 0.1f;
                yield return wffu;
            }
            yield return wffu;
            break;
        }
    }

    public void SetCameraBlend(float cameraBlend) =>
        cineBrain.m_DefaultBlend.m_Time = cameraBlend;

    public void SetPlayerControls(bool controls)
    {
        if (controls == true) input.SwitchActionMapToGameplay();
        else input.SwitchActionMapToDisable();
    }

    public void SpawnBoss()
    {
        Instantiate(smokeParticles, bossPosition.position, Quaternion.identity);
        Instantiate(boss, bossPosition.position, Quaternion.Euler(0, 90, 0));
    }
}
