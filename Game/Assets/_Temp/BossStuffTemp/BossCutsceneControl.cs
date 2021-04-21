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
    private GameObject spawnedBoss;

    [Header("Player stuff")]
    [SerializeField] private Transform playerFinalPosition;
    private CharacterController player;

    [Header("Boss camera")]
    [SerializeField] private CinemachineVirtualCamera bossFloorCamera;

    // Components
    private CinemachineBrain cineBrain;
    private CinemachineTarget cinemachine;
    private PlayerInputCustom input;
    private PlayerMovement playerMovement;
    private AudioSource musicSource;

    private void Awake()
    {
        cineBrain = Camera.main.GetComponent<CinemachineBrain>();
        cinemachine = FindObjectOfType<CinemachineTarget>();
        input = FindObjectOfType<PlayerInputCustom>();
        playerMovement = FindObjectOfType<PlayerMovement>();
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
            playerMovement.MovementSpeed = 4f;
            Vector3 dir = player.transform.Direction(playerFinalPosition); 
            player.Move((playerMovement.MovementSpeed * 0.75f) * dir * Time.fixedDeltaTime);
            player.transform.RotateTo(bossPosition.position);
            yield return null;          
        }
        playerMovement.MovementSpeed = 0;

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
        spawnedBoss = 
            Instantiate(boss, bossPosition.position, Quaternion.Euler(0, 90, 0));
    }

    public void EndCutscene()
    {
        bossFloorCamera.Priority = -1000;
        cinemachine.UpdateThirdPersonCameraPosition(
            spawnedBoss.transform.position + new Vector3(0, 0.5f, 0), 1.5f);
    }
}
