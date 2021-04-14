using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using System.Collections;

public class CinemachineTarget : MonoBehaviour, IFindPlayer, IUpdateOptions
{
    [SerializeField] private OptionsScriptableObj configScriptableObj;

    // Components
    private Player player;
    private PlayerWallHug playerWallHug;
    private PlayerInputCustom input;
    private PauseSystem pauseSystem;
    private SlowMotionBehaviour slowMotion;
    private Options optionsScript;

    // Camera Variables
    [SerializeField] private CinemachineFreeLook thirdPersonCamera;
    [SerializeField] private CinemachineVirtualCamera targetCamera;
    [SerializeField] private CinemachineVirtualCamera pauseMenuCamera;
    [SerializeField] private CinemachineVirtualCamera wallHugCamera;
    [SerializeField] private CinemachineBrain mainCameraBrain;
    private CinemachineFramingTransposer wallHugCameraTransposer;
    private float framingTranspX;
    private float framingTranspXDefault;
    private float framingDampingX;
    private float framingDampingXDefault;

    // Current target from player
    [SerializeField] private Transform currentTarget;
    public Transform CurrentTarget => currentTarget;

    // Player targets
    private Transform playerFrontTarget;
    private Transform playerBackTarget;

    // Target variables
    [SerializeField] private float findTargetSize;
    public bool Targeting { get; private set; }
    private Vector3 targetYOffset;

    // Enemies
    private Collider[] enemies;
    private List<Enemy> allEnemies;

    // Layers
    [SerializeField] private LayerMask enemyLayer;

    // Coroutine
    private Coroutine blendingCoroutine;
    private Coroutine isLerpingTargetCoroutine;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        input = FindObjectOfType<PlayerInputCustom>();
        pauseSystem = FindObjectOfType<PauseSystem>();
        optionsScript = FindObjectOfType<Options>();
        slowMotion = FindObjectOfType<SlowMotionBehaviour>();
        wallHugCameraTransposer = 
            wallHugCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void Start()
    {
        Targeting = false;
        targetYOffset = new Vector3(0, 1, 0);
        blendingCoroutine = null;
        isLerpingTargetCoroutine = null;
        framingTranspXDefault = 0.5f;
        framingTranspX = framingTranspXDefault;
        framingDampingXDefault = 25f;
        framingDampingX = framingDampingXDefault;

        allEnemies = new List<Enemy>();

        // Disables current player's target
        currentTarget.gameObject.SetActive(false);

        // Sets all cameras follows and lookAts.
        SetAllCamerasTargets();

        StartCoroutine(KeepsFindingClosestTarget());
    }

    /// <summary>
    /// Every x seconds, trieds to find an enemy, so the current target
    /// position is always updated.
    /// </summary>
    /// <returns></returns>
    private IEnumerator KeepsFindingClosestTarget()
    {
        YieldInstruction wfs = new WaitForSeconds(0.25f);

        while(true)
        {
            FindAllEnemiesAroundPlayer();

            // If there are enemies around and the camera is not blending
            if (allEnemies.Count > 0 && 
                currentTarget.gameObject.activeSelf == false &&
                mainCameraBrain.IsBlending == false)
            {
                Enemy[] organizedEnemiesByDistance =
                            allEnemies.OrderBy(i =>
                            (i.transform.position - player.transform.position).magnitude).
                            Where(i => i.gameObject.GetComponentInChildren<Renderer>().isVisible)
                            .ToArray();

                // Sets current target to closest enemy
                currentTarget.transform.position =
                    organizedEnemiesByDistance[0].transform.position + targetYOffset;
            }
            
            yield return wfs;
        }
    }

    private void OnEnable()
    {
        input.TargetSet += HandleTarget;
        input.TargetChange += SwitchTarget;
        pauseSystem.GamePaused += SwitchBeetweenPauseCamera;
        slowMotion.SlowMotionEvent += SlowMotionCamera;
        optionsScript.UpdatedValues += UpdateValues;
    }

    private void OnDisable()
    {

        input.TargetSet -= HandleTarget;
        input.TargetChange -= SwitchTarget;
        pauseSystem.GamePaused -= SwitchBeetweenPauseCamera;
        slowMotion.SlowMotionEvent -= SlowMotionCamera;
        optionsScript.UpdatedValues -= UpdateValues;
        if (playerWallHug != null)
        {
            playerWallHug.Border -= AdjustWallHugCamera;
            playerWallHug.WallHug -= SetWallHugCameraPriority;
        }
    }

    /// <summary>
    /// Sets current target on current targeted enemy.
    /// </summary>
    private void FixedUpdate()
    {
        if (FindCurrentTargetedEnemy() != null && isLerpingTargetCoroutine == null) 
            currentTarget.position = 
                FindCurrentTargetedEnemy().transform.position + targetYOffset;
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

        // While the camera is blending, the player can't move the camera
        if (mainCameraBrain.IsBlending)
        {
            if (blendingCoroutine == null)
            {
                blendingCoroutine = StartCoroutine(ChangeValuesOnBlending());
            }
        }
    }

    /// <summary>
    /// Stops camera movement while blending cameras.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChangeValuesOnBlending()
    {
        yield return new WaitForEndOfFrame(); // Don't remove <<
        while (mainCameraBrain.IsBlending)
        {
            thirdPersonCamera.m_YAxis.m_MaxSpeed = 0;
            thirdPersonCamera.m_XAxis.m_MaxSpeed = 0;
            yield return null;
        }
        blendingCoroutine = null;
        UpdateValues();
    }

    /// <summary>
    /// If the player isnt targetting any enemy, finds the closest enemy 
    /// when the player presses the assigned key. Else it cancels the current
    /// target 
    /// </summary>
    private void HandleTarget()
    {
        if (mainCameraBrain.IsBlending == false && 
            playerWallHug.Performing == false)
        {
            mainCameraBrain.m_DefaultBlend.m_Time = 0.5f;
            if (Targeting == false)
            {
                FindAllEnemiesAroundPlayer();
                if (allEnemies.Count > 0)
                {
                    // Orders array with all VISIBLE enemies by distance
                    Enemy[] organizedEnemiesByDistance =
                        allEnemies.OrderBy(i =>
                        (i.transform.position - player.transform.position).magnitude).
                        Where(i => i.gameObject.GetComponentInChildren<Renderer>().isVisible)
                        .ToArray();

                    currentTarget.gameObject.SetActive(true);

                    // Sets current target to closest enemy
                    Vector3 aimTowards =
                        organizedEnemiesByDistance[0].transform.position +
                        targetYOffset;

                    // Moves the current target towards the desired target
                    if (isLerpingTargetCoroutine == null)
                        isLerpingTargetCoroutine = StartCoroutine(LerpingTargetToClosestTarget(aimTowards));

                    // Switches camera
                    targetCamera.Priority = thirdPersonCamera.Priority + 3;
                    UpdateTargetCameraLookAt();
                    FindCurrentTargetedEnemy();

                    Targeting = true;
                }
            }
            else
            {
                CancelCurrentTarget();
            }
        }
    }

    /// <summary>
    /// Switches to target on the left or right.
    /// </summary>
    public void SwitchTarget(Direction leftOrRight)
    {
        Vector3 definitiveTarget = default;
        float shortestDistance = Mathf.Infinity;

        FindAllEnemiesAroundPlayer();
      
        for (int i = 0; i < allEnemies.Count; i++)
        {
            Vector3 direction = allEnemies[i].transform.position - targetCamera.transform.position;
            float directionAngle = MathCustom.AngleDir(targetCamera.transform.forward, direction, transform.up);

            float distanceFromTarget =
                Vector3.Distance(currentTarget.transform.position, allEnemies[i].transform.position);

            if (leftOrRight == Direction.Left)
            {
                if (directionAngle < 0 && distanceFromTarget < shortestDistance)
                {
                    if (allEnemies[i].gameObject != FindCurrentTargetedEnemy())
                    {
                        if (allEnemies[i].gameObject.GetComponentInChildren<Renderer>().isVisible)
                        {
                            shortestDistance = distanceFromTarget;

                            definitiveTarget = 
                                allEnemies[i].transform.position + targetYOffset;
                        }
                    }
                }
            }
            else if (leftOrRight == Direction.Right)
            {
                if (directionAngle > 0 && distanceFromTarget < shortestDistance)
                {
                    if (allEnemies[i].gameObject != FindCurrentTargetedEnemy())
                    {
                        if (allEnemies[i].gameObject.GetComponentInChildren<Renderer>().isVisible)
                        {
                            shortestDistance = distanceFromTarget;

                            definitiveTarget = 
                                allEnemies[i].transform.position + targetYOffset;
                        }
                    }
                }
            }
        }

        if (definitiveTarget != default)
        {
            // Moves the current target towards the desired target
            if (isLerpingTargetCoroutine == null)
                isLerpingTargetCoroutine = 
                    StartCoroutine(LerpingTargetToClosestTarget(definitiveTarget));
        }
    }

    /// <summary>
    /// Moves the current target towards the desired target.
    /// </summary>
    /// <param name="aimTowards"></param>
    /// <returns></returns>
    private IEnumerator LerpingTargetToClosestTarget(Vector3 aimTowards)
    {
        YieldInstruction wffup = new WaitForFixedUpdate();

        while(currentTarget.transform.position != aimTowards)
        {
            currentTarget.transform.position = 
                Vector3.MoveTowards(
                    currentTarget.transform.position, 
                    aimTowards, 
                    45f * Time.fixedUnscaledDeltaTime);

            yield return wffup;
        }
        FindCurrentTargetedEnemy();
        UpdateTargetCameraLookAt();
        isLerpingTargetCoroutine = null;
    }

    /// <summary>
    /// Finds all enemies around the player.
    /// </summary>
    private void FindAllEnemiesAroundPlayer()
    {
        allEnemies = new List<Enemy>();

        // Finds all enemies around
        if (player != null)
        {
            enemies =
                Physics.OverlapSphere(player.transform.position, findTargetSize, enemyLayer);

            // If enemy has an Enemy script
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].gameObject.TryGetComponent<Enemy>(out Enemy en) &&
                    enemies[i].gameObject.GetComponentInChildren<Renderer>().isVisible)
                {
                    allEnemies.Add(en);
                }
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
            Physics.OverlapSphere(currentTarget.transform.position, 0.2f, enemyLayer);

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
    private void UpdateTargetCameraLookAt() => 
        targetCamera.LookAt = currentTarget;

    /// <summary>
    /// Cancels current target.
    /// </summary>
    public void CancelCurrentTarget()
    {
        // Switches camera back to third person camera
        targetCamera.Priority = thirdPersonCamera.Priority - 3;
        if (currentTarget) currentTarget.gameObject.SetActive(false);
        Targeting = false;
    }

    /// <summary>
    /// Calls a coroutine to cancel the current target. Only happens after an
    /// enemy dies.
    /// </summary>
    public void CancelCurrentTargetAutomaticallyCall() =>
        StartCoroutine(CancelCurrentTargetAutomatically());

    /// <summary>
    /// Cancels current target automatically. This method only happens when
    /// an enemy dies.
    /// </summary>
    private IEnumerator CancelCurrentTargetAutomatically()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        FindAllEnemiesAroundPlayer();
        if (allEnemies.Count == 0)
        {
            // Switches camera back to third person camera
            mainCameraBrain.m_DefaultBlend.m_Time = 0.75f;
            targetCamera.Priority = thirdPersonCamera.Priority - 3;
            if (currentTarget) currentTarget.gameObject.SetActive(false);
            Targeting = false;;
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

        // If player is targetting and autolock option is on
        if (Targeting && configScriptableObj.AutoLock)
        {
            currentTarget.gameObject.SetActive(true);

            FindAllEnemiesAroundPlayer();

            // Orders array with all VISIBLE enemies by distance
            Enemy[] organizedEnemiesByDistance =
                allEnemies.OrderBy(i =>
                (i.transform.position - player.transform.position).magnitude).
                Where(i => i.gameObject.GetComponentInChildren<Renderer>().isVisible).
                ToArray();

            // If there's a target
            if (organizedEnemiesByDistance.Length > 0)
            {
                // Moves the current target towards the desired target
                if (isLerpingTargetCoroutine == null)
                    isLerpingTargetCoroutine = 
                        StartCoroutine(LerpingTargetToClosestTarget(
                            organizedEnemiesByDistance[0].transform.position));

                // Switches camera
                targetCamera.Priority = thirdPersonCamera.Priority + 3;
                UpdateTargetCameraLookAt();
                FindCurrentTargetedEnemy();
            }
            else
            {
                CancelCurrentTarget();
            }
        }
        // Means the player has autolock option off
        else
        {
            CancelCurrentTarget();
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

            mainCameraBrain.m_DefaultBlend.m_Time = 0.1f;

            pauseMenuCamera.Priority = 100;
        }
        else if (pauseSystem == PauseSystemEnum.Unpaused)
        {
            SlowMotionBehaviour slowMotionBehaviour = 
                FindObjectOfType<SlowMotionBehaviour>();

            if (slowMotionBehaviour.Performing)
            {
                mainCameraBrain.m_BlendUpdateMethod =
                    CinemachineBrain.BrainUpdateMethod.FixedUpdate;     
            }
            else
            {
                mainCameraBrain.m_BlendUpdateMethod =
                    CinemachineBrain.BrainUpdateMethod.FixedUpdate;
            }
            mainCameraBrain.m_UpdateMethod =
                    CinemachineBrain.UpdateMethod.FixedUpdate;

            pauseMenuCamera.Priority = 0;

            StartCoroutine(CameraBlendTimeToNormal());
        }
    }

    private IEnumerator CameraBlendTimeToNormal()
    {
        yield return new WaitForFixedUpdate();
        while(mainCameraBrain.IsBlending)
        {
            yield return null;
        }
        mainCameraBrain.m_DefaultBlend.m_Time = 0.75f;
    }

    /// <summary>
    /// Sets cameras follows and lookats.
    /// </summary>
    private void SetAllCamerasTargets()
    {
        if (player != null)
        {
            thirdPersonCamera.Follow = player.transform;
            thirdPersonCamera.LookAt = player.transform;
            targetCamera.Follow = player.transform;
            pauseMenuCamera.Follow = player.transform;
            pauseMenuCamera.LookAt = player.transform;
            wallHugCamera.Follow = playerFrontTarget.transform;
            wallHugCamera.LookAt = playerBackTarget.transform;
        }
    }

    /// <summary>
    /// Happens every time slow motion is triggered.
    /// </summary>
    /// <param name="condition">Parameter to check if it's slow motion or normal time</param>
    private void SlowMotionCamera(SlowMotionEnum condition)
    {
        if (condition == SlowMotionEnum.SlowMotion)
        {
            mainCameraBrain.m_DefaultBlend.m_Time = 0.1f;

            mainCameraBrain.m_UpdateMethod =
                CinemachineBrain.UpdateMethod.FixedUpdate;
            mainCameraBrain.m_BlendUpdateMethod =
                CinemachineBrain.BrainUpdateMethod.FixedUpdate;
        }
        else
        {
            mainCameraBrain.m_UpdateMethod =
            CinemachineBrain.UpdateMethod.FixedUpdate;

            mainCameraBrain.m_BlendUpdateMethod =
                CinemachineBrain.BrainUpdateMethod.LateUpdate;

            mainCameraBrain.m_DefaultBlend.m_Time = 0.1f;
        }
    }

    /// <summary>
    /// Sets wall hug camera priority.
    /// </summary>
    /// <param name="condition">Condition to check if player is wall hugging.</param>
    private void SetWallHugCameraPriority(bool condition)
    {
        if (condition)
        {
            mainCameraBrain.m_DefaultBlend.m_Time = 0.5f;
            wallHugCamera.Priority = 50;
        }
        else
        {
            mainCameraBrain.m_DefaultBlend.m_Time = 0.5f;
            wallHugCamera.Priority = 0;
        }
    }


    /// <summary>
    /// Checks if camera is blending.
    /// </summary>
    /// <returns>Returns true if camera is blending.</returns>
    public bool IsBlending()
    {
        if (mainCameraBrain.IsBlending) return true;
        return false;
    }

    /// <summary>
    /// Adjusts values when the player is on a wall border and huging a wall.
    /// </summary>
    /// <param name="dir">Direction to push the camera to.</param>
    private void AdjustWallHugCamera(Direction dir)
    {
        if (dir == Direction.Left)
        {
            framingDampingX = Mathf.Lerp(framingDampingX, 0, Time.fixedDeltaTime * 10f);
            framingTranspX = Mathf.Lerp(framingTranspX, framingTranspXDefault + 0.3f, Time.fixedDeltaTime * 4);
        }
        else if (dir == Direction.Right)
        {
            framingDampingX = Mathf.Lerp(framingDampingX, 0, Time.fixedDeltaTime * 10f);
            framingTranspX = Mathf.Lerp(framingTranspX, framingTranspXDefault - 0.3f, Time.fixedDeltaTime * 4);
        }
        else
        {
            framingDampingX = Mathf.Lerp(framingDampingX, framingDampingXDefault, Time.fixedDeltaTime * 5f);
            framingTranspX =
                Mathf.Lerp(framingTranspX, framingTranspXDefault, Time.fixedDeltaTime * 4);
        }

        wallHugCameraTransposer.m_XDamping = framingDampingX;
        wallHugCameraTransposer.m_ScreenX = framingTranspX;
    }

    /// <summary>
    /// If the player spawns on the scene.
    /// </summary>
    public void FindPlayer()
    {
        player = FindObjectOfType<Player>();
        playerWallHug = FindObjectOfType<PlayerWallHug>();
        playerBackTarget = 
            GameObject.FindGameObjectWithTag("playerBackTarget").transform;
        playerFrontTarget =
            GameObject.FindGameObjectWithTag("playerFrontTarget").transform;
        SetAllCamerasTargets();
        mainCameraBrain.enabled = true;

        if (playerWallHug != null)
        {
            playerWallHug.Border += AdjustWallHugCamera;
            playerWallHug.WallHug += SetWallHugCameraPriority;
        }
    }

    /// <summary>
    /// If the player is destroyed from the scene.
    /// </summary>
    public void PlayerLost()
    {
        thirdPersonCamera.Follow = null;
        thirdPersonCamera.LookAt = null;
        targetCamera.Follow = null;
        pauseMenuCamera.Follow = null;
        pauseMenuCamera.LookAt = null;
        mainCameraBrain.enabled = false;
    }

    /// <summary>
    /// Updates values when configScriptableObj are updated.
    /// </summary>
    public void UpdateValues()
    {
        thirdPersonCamera.m_YAxis.m_MaxSpeed = configScriptableObj.VerticalSensibility;
        thirdPersonCamera.m_XAxis.m_MaxSpeed = configScriptableObj.HorizontalSensibility;
    }
}
