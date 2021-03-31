using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using System.Collections;

public class CinemachineTarget : MonoBehaviour, IFindPlayer, IUpdateOptions
{
    [SerializeField] private OptionsScriptableObj options;

    // Components
    private Player player;
    private PlayerInputCustom input;
    private PauseSystem pauseSystem;
    private SlowMotionBehaviour slowMotion;

    // Camera Variables
    [SerializeField] private CinemachineFreeLook thirdPersonCamera;
    [SerializeField] private CinemachineFreeLook slowMotionThirdPersonCamera;
    [SerializeField] private CinemachineVirtualCamera targetCamera;
    [SerializeField] private CinemachineVirtualCamera pauseMenuCamera;
    [SerializeField] private CinemachineCollider targetCameraCollider;
    private CinemachineBrain mainCameraBrain;

    // Current target from player
    public Transform CurrentTarget { get; private set; }

    // Target variables
    [SerializeField] private float findTargetSize;
    public bool Targeting { get; private set; }
    private float targetYOffset;

    // Enemies
    private Collider[] enemies;
    private List<Enemy> allEnemies;

    // Layers
    [SerializeField] private LayerMask enemyLayer;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        input = FindObjectOfType<PlayerInputCustom>();
        pauseSystem = FindObjectOfType<PauseSystem>();
        mainCameraBrain = Camera.main.GetComponent<CinemachineBrain>();
        slowMotion = FindObjectOfType<SlowMotionBehaviour>();
        CurrentTarget = GameObject.FindGameObjectWithTag("targetUIForCinemachine").transform;
    }

    private void Start()
    {
        Targeting = false;
        targetYOffset = 1;

        allEnemies = new List<Enemy>();

        // Disables current player's target
        CurrentTarget.gameObject.SetActive(false);

        // Sets all cameras follows and lookAts.
        SetAllCamerasTargets();
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
    }

    private void OnDisable()
    {
        input.TargetSet -= HandleTarget;
        input.TargetChange -= SwitchTarget;
        pauseSystem.GamePaused -= SwitchBeetweenPauseCamera;
        slowMotion.SlowMotionEvent -= SlowMotionCamera;
    }

    private void Update()
    {
        // If distance becames too wide while targetting, it cancels the current target
        if (Targeting &&
            Vector3.Distance(
            player.transform.position, CurrentTarget.transform.position) >
            findTargetSize)
        {
            CancelCurrentTarget();
        }
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

                    
                    CurrentTarget.gameObject.SetActive(true);

                    // Sets current target to closest enemy
                    CurrentTarget.transform.position = new Vector3(
                                        organizedEnemiesByDistance[0].transform.position.x,
                                        organizedEnemiesByDistance[0].transform.position.y + targetYOffset,
                                        organizedEnemiesByDistance[0].transform.position.z);

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
                Vector3.Distance(CurrentTarget.transform.position, allEnemies[i].transform.position);

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

                            CurrentTarget.transform.position = new Vector3(
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
            CurrentTarget.transform.position = new Vector3(
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
            Physics.OverlapSphere(CurrentTarget.transform.position, 0.5f, enemyLayer);

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
                    CurrentTarget.transform.position.x,
                    CurrentTarget.transform.position.y - targetYOffset,
                    CurrentTarget.transform.position.z), 
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
        // Switches camera back to third person camera
        targetCamera.Priority = thirdPersonCamera.Priority - 3;
        if (CurrentTarget) CurrentTarget.gameObject.SetActive(false);
        Targeting = !Targeting;
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
            // Switches camera back to third person camera
            targetCamera.Priority = thirdPersonCamera.Priority - 3;
            if (CurrentTarget) CurrentTarget.gameObject.SetActive(false);
            Targeting = !Targeting;
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
        if (Targeting && options.AutoLock)
        {
            CurrentTarget.gameObject.SetActive(true);

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
                CurrentTarget.transform.position = new Vector3(
                                    organizedEnemiesByDistance[0].transform.position.x,
                                    organizedEnemiesByDistance[0].transform.position.y + targetYOffset,
                                    organizedEnemiesByDistance[0].transform.position.z);

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
            slowMotionThirdPersonCamera.Follow = player.transform;
            slowMotionThirdPersonCamera.LookAt = playerSpineTransform;
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

            // Gives priority to slow motion camera
            slowMotionThirdPersonCamera.Priority =
                thirdPersonCamera.Priority + 1;
        }
        else
        {
            mainCameraBrain.m_UpdateMethod =
            CinemachineBrain.UpdateMethod.FixedUpdate;

            mainCameraBrain.m_BlendUpdateMethod =
                CinemachineBrain.BrainUpdateMethod.LateUpdate;

            mainCameraBrain.m_DefaultBlend.m_Time = 1f;

            // Removes priority to slow motion camera
            slowMotionThirdPersonCamera.Priority =
                    thirdPersonCamera.Priority - 1;
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

    public void FindPlayer()
    {
        player = FindObjectOfType<Player>();
        input = FindObjectOfType<PlayerInputCustom>();
        input.TargetSet += HandleTarget;
        input.TargetChange += SwitchTarget;
        SetAllCamerasTargets();
    }

    public void PlayerLost()
    {
        input.TargetSet -= HandleTarget;
        input.TargetChange -= SwitchTarget;
    }

    /// <summary>
    /// Updates values when options are updated.
    /// </summary>
    public void UpdateValues()
    {
        thirdPersonCamera.m_YAxis.m_MaxSpeed = options.VerticalSensibility;
        thirdPersonCamera.m_XAxis.m_MaxSpeed = options.HorizontalSensibility;
        slowMotionThirdPersonCamera.m_YAxis.m_MaxSpeed = options.VerticalSensibility;
        slowMotionThirdPersonCamera.m_XAxis.m_MaxSpeed = options.HorizontalSensibility;
    }
}
