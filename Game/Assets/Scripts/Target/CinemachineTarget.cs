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

    // Target camera
    [SerializeField] private CinemachineVirtualCamera targetCamera;

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

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        input = FindObjectOfType<PlayerInputCustom>();
        thirdPersonCamera = GetComponent<CinemachineFreeLook>();
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
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
    }

    private void OnDisable()
    {
        input.TargetSet -= HandleTarget;
        input.TargetChange -= SwitchTarget;
    }

    /// <summary>
    /// Finds nearest enemy.
    /// </summary>
    private void HandleTarget()
    {
        if (Targeting == false)
        {
            // Fixes rought transitions beetween cameras
            if (Time.timeScale < 1) cinemachineBrain.m_DefaultBlend.m_Time = 0.1f;
            else cinemachineBrain.m_DefaultBlend.m_Time = 1f;

            FindAllEnemiesAroundPlayer();

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
            currentTarget.gameObject.SetActive(true);
            FindCurrentTargetedEnemy();

            Targeting = !Targeting;
        }
        else
        {
            CancelCurrentTarget();
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
    private void SwitchTarget(LeftOrRight leftOrRight)
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
    public void CancelCurrentTarget()
    {
        // Fixes rough trainsition with cameras on slow motion
        if (Time.timeScale < 1f) cinemachineBrain.m_DefaultBlend.m_Time = 0.5f;
        else cinemachineBrain.m_DefaultBlend.m_Time = 1f;

        // Switches camera back to third person camera
        targetCamera.Priority = thirdPersonCamera.Priority - 1;
        currentTarget.gameObject.SetActive(false);
        Targeting = !Targeting;

        cinemachineBrain.m_DefaultBlend.m_Time = 1f;
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

        
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
            Gizmos.DrawWireSphere(player.transform.position, findTargetSize);
    }
}
