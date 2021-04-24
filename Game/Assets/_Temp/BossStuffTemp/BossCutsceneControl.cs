using UnityEngine;
using Cinemachine;
using System.Collections;
using UnityEngine.AI;

public class BossCutsceneControl : MonoBehaviour
{
    [Header("Boss stuff")]
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private GameObject smokeParticles;
    [SerializeField] private GameObject bossPosition;
    [SerializeField] private Transform finalBossPosition;
    [SerializeField] private AudioClip bossMusic;
    private GameObject spawnedBoss;

    [Header("Player stuff")]
    [SerializeField] private Transform playerFinalPosition;

    [Header("Boss camera")]
    [SerializeField] private CinemachineVirtualCamera bossFloorCamera;

    // Components
    private CinemachineBrain cineBrain;
    private CinemachineTarget cinemachine;
    private PlayerInputCustom input;
    private MusicReferences musicSource;
    private EnemyBoss boss;

    public bool OnBossFight { get; private set; }

    private void Awake()
    {
        cineBrain = Camera.main.GetComponent<CinemachineBrain>();
        cinemachine = FindObjectOfType<CinemachineTarget>();
        input = FindObjectOfType<PlayerInputCustom>();
        musicSource = FindObjectOfType<MusicReferences>();
    }

    public void StartCutscene() => StartCoroutine(StartCutsceneCoroutine());
    private IEnumerator StartCutsceneCoroutine()
    {
        OnBossFight = true;
        SetCameraBlend(0);
        SetPlayerControls(false);

        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        CharacterController player = FindObjectOfType<PlayerMovement>().Controller;

        while(player.transform.position.Similiar(playerFinalPosition.position, 0.1f) == false)
        {
            playerMovement.MovementSpeed = 4f;
            Vector3 dir = player.transform.Direction(playerFinalPosition); 
            player.Move((playerMovement.MovementSpeed * 0.75f) * dir * Time.fixedDeltaTime);
            player.transform.RotateTo(bossPosition.transform.position);
            yield return null;          
        }
        playerMovement.MovementSpeed = 0;

        float currentVolume = musicSource.Music.volume;
        YieldInstruction wffu = new WaitForFixedUpdate();
        while (true)
        {
            while (musicSource.Music.volume > 0 && musicSource.CombatMusic.volume > 0)
            {
                musicSource.Music.volume -= Time.fixedDeltaTime * 0.25f;
                musicSource.CombatMusic.volume -= Time.fixedDeltaTime * 0.25f;
                yield return wffu;
            }
            musicSource.Music.Stop();
            musicSource.CombatMusic.Stop();

            musicSource.Music.clip = bossMusic;
            musicSource.Music.Play();

            while (musicSource.Music.volume < currentVolume)
            {
                musicSource.Music.volume += Time.fixedDeltaTime * 0.25f;
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
        Instantiate(smokeParticles, bossPosition.transform.position, Quaternion.identity);
        spawnedBoss = 
            Instantiate(bossPrefab, bossPosition.transform.position, Quaternion.Euler(0, 90, 0));
    }

    public void BossJump() => StartCoroutine(BossJumpCoroutine());
    private IEnumerator BossJumpCoroutine()
    {
        YieldInstruction wffu = new WaitForFixedUpdate();

        // Gets boss enemy
        boss = spawnedBoss.GetComponentInChildren<EnemyBoss>();
        Animator agentAnimator = spawnedBoss.GetComponentInChildren<Animator>();
        NavMeshAgent agent = spawnedBoss.GetComponentInChildren<NavMeshAgent>();
        agentAnimator.SetTrigger("Jump");
        agent.enabled = false;
        boss.enabled = false;

        // Gets boss position animator
        Animator bossPos = bossPosition.GetComponentInParent<Animator>();
        yield return new WaitForSeconds(0.1f);
        bossPos.SetTrigger("MoveBoss");

        // While boss position isn't similiar to final position updates boss's position
        while (!bossPosition.transform.position.Similiar(finalBossPosition.position))
        {
            agent.transform.position = bossPosition.transform.position;
            yield return wffu;
        }

        agent.enabled = true;
        agentAnimator.SetTrigger("Taunt");

        // Updates camera to boss position
        cinemachine.UpdateThirdPersonCameraPosition(
            finalBossPosition.position + new Vector3(0, 0.5f, 0), 3f);
    }

    public void EndCutscene()
    {
        boss.enabled = true;
        boss.InitializeStateMachine();
        bossFloorCamera.Priority = -1000;
    }
}
