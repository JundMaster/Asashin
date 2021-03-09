using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

public class CinemachineTarget : MonoBehaviour
{
    // Components
    private CinemachineFreeLook thirdPersonCamera;
    private Player player;
    private PlayerInputCustom input;

    // Target camera
    [SerializeField] private CinemachineVirtualCamera secondTargetcamera;

    // Target player
    [SerializeField] private Transform currentTarget;
    public Transform CurrentTarget => currentTarget;

    // Target variables
    [SerializeField] private float findTargetSize = 20f;
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
        input.TargetChangeLeft += SwitchTargetLeft;
        input.TargetChangeRight += SwitchTargetRight;
    }

    private void OnDisable()
    {
        input.TargetSet -= HandleTarget;
        input.TargetChangeLeft -= SwitchTargetLeft;
        input.TargetChangeRight -= SwitchTargetRight;
    }


    

    /// <summary>
    /// Finds nearest enemy.
    /// </summary>
    private void HandleTarget()
    {
        if (Targeting == false)
        {
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
            secondTargetcamera.Priority = thirdPersonCamera.Priority + 1;
            UpdateTargetCameraLookAt();
            currentTarget.gameObject.SetActive(true);
            FindCurrentTargetedEnemy();

            Targeting = !Targeting;
        }
        else
        {
            // Switches camera back to third person camera
            secondTargetcamera.Priority = thirdPersonCamera.Priority - 1;

            Targeting = !Targeting;

            currentTarget.gameObject.SetActive(false);
        }
    }

    private void SwitchTargetLeft()
    {
        float shortestDistanceLeft = Mathf.Infinity;

        FindAllEnemiesAroundPlayer();

        for (int i = 0; i < allEnemies.Count; i++)
        {
            Vector3 relativePosition = 
                secondTargetcamera.transform.InverseTransformDirection(
                    allEnemies[i].transform.position - secondTargetcamera.transform.position);

            float distanceFromLeftTarget = 
                currentTarget.transform.position.x - allEnemies[i].transform.position.x;

            if (relativePosition.x < 0 && distanceFromLeftTarget < shortestDistanceLeft)
            {
                if (allEnemies[i].gameObject != FindCurrentTargetedEnemy())
                {
                    if (allEnemies[i].gameObject.GetComponentInChildren<Renderer>().isVisible)
                    {
                        shortestDistanceLeft = distanceFromLeftTarget;

                        currentTarget.transform.position = new Vector3(
                                    allEnemies[i].transform.position.x,
                                    allEnemies[i].transform.position.y + targetYOffset,
                                    allEnemies[i].transform.position.z);
                    }
                }
            }  
        }
        UpdateTargetCameraLookAt();
        FindCurrentTargetedEnemy();
    }

    private void SwitchTargetRight()
    {
        float shortestDistanceRight = Mathf.Infinity;

        FindAllEnemiesAroundPlayer();

        for (int i = 0; i < allEnemies.Count; i++)
        {
            Vector3 relativePosition = 
                secondTargetcamera.transform.InverseTransformDirection(
                    allEnemies[i].transform.position - secondTargetcamera.transform.position);

            float distanceFromRightTarget = 
                currentTarget.transform.position.x + allEnemies[i].transform.position.x;

            if (relativePosition.x > 0 && distanceFromRightTarget < shortestDistanceRight)
            {
                if (allEnemies[i].gameObject != FindCurrentTargetedEnemy())
                {
                    if (allEnemies[i].gameObject.GetComponentInChildren<Renderer>().isVisible)
                    {
                        shortestDistanceRight = distanceFromRightTarget;

                        currentTarget.transform.position = new Vector3(
                                    allEnemies[i].transform.position.x,
                                    allEnemies[i].transform.position.y + targetYOffset,
                                    allEnemies[i].transform.position.z);
                    }
                }
            }
        }
        UpdateTargetCameraLookAt();
        FindCurrentTargetedEnemy();
    }

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
            Physics.OverlapSphere(currentTarget.transform.position, 1f, enemyLayer);

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
    /// Updates target camera.
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
                // Applies target transform to target camera
                secondTargetcamera.LookAt = en.MyTarget;
            }
        }        
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
            Gizmos.DrawWireSphere(player.transform.position, findTargetSize);
    }
}
