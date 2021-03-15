using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using System.Collections;

public class CinemachineTarget : MonoBehaviour
{
    // Components
    private CinemachineFreeLook thirdPersonCamera;
    private Player player;
    private PlayerInputCustom input;
    private PauseSystem pauseSystem;

    // Target camera
    [SerializeField] private CinemachineVirtualCamera targetCamera;
    [SerializeField] private CinemachineCollider targetCameraCollider;
    [SerializeField] private CinemachineVirtualCamera pauseMenuCamera;
    private Camera mainCamera;
    private CinemachineBrain mainCameraBrain;

    // Cinemachine Brain
    private CinemachineBrain cinemachineBrain;

    // Target player
    [SerializeField] private Transform currentTarget;
    public Transform CurrentTarget => currentTarget;

    // Target variables
    [SerializeField] private float findTargetSize;
    public bool Targeting { get; private set; }
    private float targetYOffset;

    // Enemies
    private Collider[] enemies;
    private List<Enemy> allEnemies;

    // Layers
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask wallLayer;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        input = FindObjectOfType<PlayerInputCustom>();
        thirdPersonCamera = GetComponent<CinemachineFreeLook>();
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        pauseSystem = FindObjectOfType<PauseSystem>();
        mainCamera = Camera.main;
        mainCameraBrain = mainCamera.GetComponent<CinemachineBrain>();
    }

    private void Start()
    {
        Targeting = false;
        targetYOffset = 1;

        allEnemies = new List<Enemy>();

        currentTarget.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        input.TargetSet += HandleTarget;
        input.TargetChange += SwitchTarget;
        pauseSystem.GamePaused += SwitchBeetweenPauseCamera;
    }

    private void OnDisable()
    {
        input.TargetSet -= HandleTarget;
        input.TargetChange -= SwitchTarget;
        pauseSystem.GamePaused -= SwitchBeetweenPauseCamera;
    }

    private void Update()
    {
        // If distance becames too wide while targetting, it cancels the current target
        if (Targeting &&
            Vector3.Distance(
            player.transform.position, currentTarget.transform.position) >
            findTargetSize)
        {
            CancelCurrentTarget();
        }

        if (player == null || input == null)
        {
            player = FindObjectOfType<Player>();
            input = FindObjectOfType<PlayerInputCustom>();
            thirdPersonCamera.Follow = player.transform;
            Transform playerSpineTransform = 
                GameObject.FindGameObjectWithTag("playerSpine").transform;
            thirdPersonCamera.LookAt = playerSpineTransform;
            pauseMenuCamera.Follow = playerSpineTransform;
            pauseMenuCamera.LookAt = playerSpineTransform;
        }
    }

    /// <summary>
    /// If the player isnt targetting any enemy, finds the closest enemy 
    /// when the player presses the assigned key. Else it cancels the current
    /// target 
    /// </summary>
    private void HandleTarget()
    {
        if (cinemachineBrain.IsBlending == false)
        {
            if (Targeting == false)
            {
                FindAllEnemiesAroundPlayer();

                if (allEnemies.Count > 0)
                {
                    // Fixes rought transitions beetween cameras
                    if (Time.timeScale < 1) cinemachineBrain.m_DefaultBlend.m_Time = 0.1f;
                    else cinemachineBrain.m_DefaultBlend.m_Time = 1f;

                    currentTarget.gameObject.SetActive(true);

                    // Orders array with all enemies
                    Enemy[] organizedEnemiesByDistance =
                        allEnemies.OrderBy(i =>
                        (i.transform.position - player.transform.position).magnitude).ToArray();

                    // Sets current target to closest enemy
                    currentTarget.transform.position = new Vector3(
                                        organizedEnemiesByDistance[0].transform.position.x,
                                        organizedEnemiesByDistance[0].transform.position.y + targetYOffset,
                                        organizedEnemiesByDistance[0].transform.position.z);

                    // Switches camera
                    targetCamera.Priority = thirdPersonCamera.Priority + 1;
                    UpdateTargetCameraLookAt();
                    FindCurrentTargetedEnemy();

                    Targeting = !Targeting;
                }
            }
            else
            {
                CancelCurrentTarget();
            }
        }
    }

    /// <summary>
    /// Method to return if a position is left or right of a target.
    /// </summary>
    /// <param name="fwd">Forward vector.</param>
    /// <param name="targetDir">Direction.</param>
    /// <param name="up">Up vector.</param>
    /// <returns>-1, 0, 1 depending on the target's position.</returns>
    float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            return 1f;
        }
        else if (dir < 0f)
        {
            return -1f;
        }
        else
        {
            return 0f;
        }
    }

    /// <summary>
    /// Switches to target on the left or right.
    /// </summary>
    public void SwitchTarget(LeftOrRight leftOrRight)
    {
        Vector3 definitiveTarget = default;
        float shortestDistance = Mathf.Infinity;

        FindAllEnemiesAroundPlayer();
      
        for (int i = 0; i < allEnemies.Count; i++)
        {
            Vector3 direction = allEnemies[i].transform.position - targetCamera.transform.position;
            float directionAngle = AngleDir(targetCamera.transform.forward, direction, transform.up);

            float distanceFromTarget =
                Vector3.Distance(currentTarget.transform.position, allEnemies[i].transform.position);

            if (leftOrRight == LeftOrRight.Left)
            {
                if (directionAngle < 0 && distanceFromTarget < shortestDistance)
                {
                    if (allEnemies[i].gameObject != FindCurrentTargetedEnemy())
                    {
                        if (allEnemies[i].gameObject.GetComponentInChildren<Renderer>().isVisible)
                        {
                            shortestDistance = distanceFromTarget;

                            definitiveTarget = allEnemies[i].transform.position;
                        }
                    }
                }
            }
            else if (leftOrRight == LeftOrRight.Right)
            {
                if (directionAngle > 0 && distanceFromTarget < shortestDistance)
                {
                    if (allEnemies[i].gameObject != FindCurrentTargetedEnemy())
                    {
                        if (allEnemies[i].gameObject.GetComponentInChildren<Renderer>().isVisible)
                        {
                            shortestDistance = distanceFromTarget;

                            currentTarget.transform.position = new Vector3(
                                        allEnemies[i].transform.position.x,
                                        allEnemies[i].transform.position.y + targetYOffset,
                                        allEnemies[i].transform.position.z);
                        }
                    }
                }
            }
        }

        if (definitiveTarget != default)
        {
            currentTarget.transform.position = new Vector3(
                                        definitiveTarget.x,
                                        definitiveTarget.y + targetYOffset,
                                        definitiveTarget.z);
        }

        FindCurrentTargetedEnemy();
        UpdateTargetCameraLookAt(); 
    }

    /// <summary>
    /// Finds all enemies around the player.
    /// </summary>
    private void FindAllEnemiesAroundPlayer()
    {
        allEnemies = new List<Enemy>();

        // Finds all enemies around
        enemies =
                Physics.OverlapSphere(player.transform.position, findTargetSize, enemyLayer);

        // If enemy has an Enemy script
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].gameObject.TryGetComponent<Enemy>(out Enemy en))
            {
                allEnemies.Add(en);
            }
        }
    }

    /// <summary>
    /// Returns currently targeted enemy.
    /// </summary>
    /// <returns>Returns a target.</returns>
    private GameObject FindCurrentTargetedEnemy()
    {
        // Finds enemies around the current target
        Collider[] currentTargetPosition =
            Physics.OverlapSphere(currentTarget.transform.position, 0.5f, enemyLayer);

        // If enemy has an Enemy script
        for (int i = 0; i < currentTargetPosition.Length; i++)
        {
            if (currentTargetPosition[i].gameObject.TryGetComponent<Enemy>(out Enemy en))
            {
                return currentTargetPosition[i].gameObject;
            }
        }
        return null;
    }

    /// <summary>
    /// Updates target camera with current target.
    /// </summary>
    private void UpdateTargetCameraLookAt()
    {
        // Finds closest enemy
        Collider[] closestEnemy =
            Physics.OverlapSphere(
                new Vector3(
                    currentTarget.transform.position.x,
                    currentTarget.transform.position.y - targetYOffset,
                    currentTarget.transform.position.z), 
                0.1f, 
                enemyLayer);

        // Finds if the enemy has Enemy script
        foreach (Collider enemy in closestEnemy)
        {
            if (enemy.gameObject.TryGetComponent(out Enemy en))
            {
                targetCamera.LookAt = en.MyTarget;
            }
        }
    }

    /// <summary>
    /// Cancels current target.
    /// </summary>
    private void CancelCurrentTarget()
    {
        // Fixes rough trainsition with cameras on slow motion
        if (Time.timeScale < 1f) cinemachineBrain.m_DefaultBlend.m_Time = 0.5f;
        else cinemachineBrain.m_DefaultBlend.m_Time = 1f;

        // Switches camera back to third person camera
        targetCamera.Priority = thirdPersonCamera.Priority - 1;
        if (currentTarget) currentTarget.gameObject.SetActive(false);
        Targeting = !Targeting;

        cinemachineBrain.m_DefaultBlend.m_Time = 1f;
    }

    /// <summary>
    /// Cancels current target automatically. This method only happens when
    /// an enemy dies.
    /// </summary>
    public void CancelCurrentTargetAutomatically()
    {
        FindAllEnemiesAroundPlayer();
        if (allEnemies.Count == 0)
        {
            // Fixes rough trainsition with cameras on slow motion
            if (Time.timeScale < 1f) cinemachineBrain.m_DefaultBlend.m_Time = 0.5f;
            else cinemachineBrain.m_DefaultBlend.m_Time = 1f;

            // Switches camera back to third person camera
            targetCamera.Priority = thirdPersonCamera.Priority - 1;
            if (currentTarget) currentTarget.gameObject.SetActive(false);
            Targeting = !Targeting;

            cinemachineBrain.m_DefaultBlend.m_Time = 1f;
        }
    }

    /// <summary>
    /// Invokes coroutine to find the second closest target automatically. 
    /// This method is used when an enemy dies.
    /// </summary>
    public void AutomaticallyFindTargetCall()
        => StartCoroutine(AutomaticallyFindTarget());

    /// <summary>
    /// Finds second closest target automatically after ending the current frame. 
    /// </summary>
    /// <returns>Wait for end of frame.</returns>
    private IEnumerator AutomaticallyFindTarget()
    {
        yield return new WaitForEndOfFrame();

        if (Targeting)
        {
            // Fixes rough transitions beetween cameras
            if (Time.timeScale < 1) cinemachineBrain.m_DefaultBlend.m_Time = 0.1f;
            else cinemachineBrain.m_DefaultBlend.m_Time = 1f;

            currentTarget.gameObject.SetActive(true);

            FindAllEnemiesAroundPlayer();

            // Orders array with all enemies
            Enemy[] organizedEnemiesByDistance =
                allEnemies.OrderBy(i =>
                (i.transform.position - player.transform.position).magnitude).
                ToArray();

            if (organizedEnemiesByDistance.Length > 0)
            {
                // Sets current target to closest enemy
                currentTarget.transform.position = new Vector3(
                                    organizedEnemiesByDistance[0].transform.position.x,
                                    organizedEnemiesByDistance[0].transform.position.y + targetYOffset,
                                    organizedEnemiesByDistance[0].transform.position.z);

                // Switches camera
                targetCamera.Priority = thirdPersonCamera.Priority + 1;
                UpdateTargetCameraLookAt();
                FindCurrentTargetedEnemy();
            }
            else
            {
                CancelCurrentTarget();
            }
        }
    }

    /// <summary>
    /// Switches cameras when the player pauses or unpauses the game.
    /// </summary>
    /// <param name="pauseSystem">Parameter that checks if the player
    /// paused or unpaused the game.</param>
    private void SwitchBeetweenPauseCamera(PauseSystemEnum pauseSystem)
    {
        if (pauseSystem == PauseSystemEnum.Paused)
        {
            mainCameraBrain.m_UpdateMethod =
            CinemachineBrain.UpdateMethod.LateUpdate;
            mainCameraBrain.m_BlendUpdateMethod =
            CinemachineBrain.BrainUpdateMethod.LateUpdate;

            pauseMenuCamera.Priority = 100;
        }
        else if (pauseSystem == PauseSystemEnum.Unpaused)
        {
            SlowMotionBehaviour slowMotionBehaviour = 
                FindObjectOfType<SlowMotionBehaviour>();

            if (slowMotionBehaviour.Performing)
            {
                mainCameraBrain.m_UpdateMethod =
                    CinemachineBrain.UpdateMethod.LateUpdate;

                mainCameraBrain.m_BlendUpdateMethod =
                    CinemachineBrain.BrainUpdateMethod.LateUpdate;
            }
            else
            {
                mainCameraBrain.m_UpdateMethod =
                    CinemachineBrain.UpdateMethod.FixedUpdate;                
            }
            pauseMenuCamera.Priority = 0;

            StartCoroutine(ChangeBlendToFixedUpdate());
        }
    }

    /// <summary>
    /// After the end of frame, after camera stops blending, turns camera brain 
    /// blend to fixed update.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChangeBlendToFixedUpdate()
    {
        yield return new WaitForEndOfFrame();
        while (mainCameraBrain.IsBlending)
        {
            yield return null;
        }

        mainCameraBrain.m_BlendUpdateMethod =
                CinemachineBrain.BrainUpdateMethod.FixedUpdate;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            if (player)
            {
                Gizmos.DrawWireSphere(player.transform.position, findTargetSize);
            }
        }
    }
}
