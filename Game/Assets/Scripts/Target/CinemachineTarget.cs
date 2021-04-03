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
    private PlayerInputCustom input;
    private PauseSystem pauseSystem;
    private SlowMotionBehaviour slowMotion;
    private Options optionsScript;

    // Camera Variables
    [SerializeField] private CinemachineFreeLook thirdPersonCamera;
    [SerializeField] private CinemachineVirtualCamera targetCamera;
    [SerializeField] private CinemachineVirtualCamera pauseMenuCamera;
    [SerializeField] private CinemachineCollider targetCameraCollider;
    [SerializeField] private CinemachineBrain mainCameraBrain;

    // Current target from player
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
    }

    private void Start()
    {
        Targeting = false;
        targetYOffset = 1;
        blendingCoroutine = null;
        isLerpingTargetCoroutine = null;

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
        YieldInstruction wfs = new WaitForSeconds(2f);

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
                currentTarget.transform.position = new Vector3(
                                    organizedEnemiesByDistance[0].transform.position.x,
                                    organizedEnemiesByDistance[0].transform.position.y + targetYOffset,
                                    organizedEnemiesByDistance[0].transform.position.z);
            }
            
            yield return wfs;
        }
    }

    private void OnEnable()
    {
        if (input != null)
        {
            input.TargetSet += HandleTarget;
            input.TargetChange += SwitchTarget;
        }
        pauseSystem.GamePaused += SwitchBeetweenPauseCamera;
        slowMotion.SlowMotionEvent += SlowMotionCamera;
        optionsScript.UpdatedValues += UpdateValues;
    }

    private void OnDisable()
    {
        if (input != null)
        {
            input.TargetSet -= HandleTarget;
            input.TargetChange -= SwitchTarget;
        }
        pauseSystem.GamePaused -= SwitchBeetweenPauseCamera;
        slowMotion.SlowMotionEvent -= SlowMotionCamera;
        optionsScript.UpdatedValues -= UpdateValues;
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
        yield return new WaitForEndOfFrame();
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
        if (mainCameraBrain.IsBlending == false)
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
                    Vector3 aimTowards = new Vector3(
                                        organizedEnemiesByDistance[0].transform.position.x,
                                        organizedEnemiesByDistance[0].transform.position.y + targetYOffset,
                                        organizedEnemiesByDistance[0].transform.position.z);

                    // Moves the current target towards the desired target
                    if (isLerpingTargetCoroutine == null)
                        isLerpingTargetCoroutine = StartCoroutine(LerpingTargetToClosestTarget(aimTowards));

                    // Switches camera
                    targetCamera.Priority = thirdPersonCamera.Priority + 3;
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
            float directionAngle = MathCustom.AngleDir(targetCamera.transform.forward, direction, transform.up);

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

                            definitiveTarget = allEnemies[i].transform.position;
                        }
                    }
                }
            }
        }

        if (definitiveTarget != default)
        {
            // Target + target y offset
            Vector3 aimTowards = new Vector3(
                                        definitiveTarget.x,
                                        definitiveTarget.y + targetYOffset,
                                        definitiveTarget.z);

            // Moves the current target towards the desired target
            if (isLerpingTargetCoroutine == null)
                isLerpingTargetCoroutine = StartCoroutine(LerpingTargetToClosestTarget(aimTowards));
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
                    25f * Time.fixedUnscaledDeltaTime);

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
    private void UpdateTargetCameraLookAt() => 
        targetCamera.LookAt = currentTarget;

    /// <summary>
    /// Cancels current target.
    /// </summary>
    private void CancelCurrentTarget()
    {
        // Switches camera back to third person camera
        targetCamera.Priority = thirdPersonCamera.Priority - 3;
        if (currentTarget) currentTarget.gameObject.SetActive(false);
        Targeting = !Targeting;
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
                // Sets current target to closest enemy
                Vector3 aimTowards = new Vector3(
                                    organizedEnemiesByDistance[0].transform.position.x,
                                    organizedEnemiesByDistance[0].transform.position.y + targetYOffset,
                                    organizedEnemiesByDistance[0].transform.position.z);

                // Moves the current target towards the desired target
                if (isLerpingTargetCoroutine == null)
                    isLerpingTargetCoroutine = StartCoroutine(LerpingTargetToClosestTarget(aimTowards));

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
            Transform playerSpineTransform =
                GameObject.FindGameObjectWithTag("playerSpine").transform;
            thirdPersonCamera.LookAt = playerSpineTransform;
            targetCamera.Follow = playerSpineTransform;
            pauseMenuCamera.Follow = playerSpineTransform;
            pauseMenuCamera.LookAt = playerSpineTransform;
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

    /// <summary>
    /// If the player spawns on the scene.
    /// </summary>
    public void FindPlayer()
    {
        player = FindObjectOfType<Player>();
        SetAllCamerasTargets();
        mainCameraBrain.enabled = true;
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
