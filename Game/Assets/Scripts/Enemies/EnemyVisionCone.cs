using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for handling an enemie's vision cone.
/// </summary>
public class EnemyVisionCone : MonoBehaviour, IFindPlayer, IAction
{
    [SerializeField] private float coneRange;
    private Collider[] playerCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask collisionLayers;
    private Transform myTarget;
    private Transform playerTarget;

    public bool Performing { get; private set; }

    private void Awake()
    {
        myTarget = GetComponent<Enemy>().MyTarget;
    }

    private void Start()
    {
        Performing = false;
        StartCoroutine(SearchForPlayer());
    }

    private IEnumerator SearchForPlayer()
    {
        while (true)
        {
            playerCollider = Physics.OverlapSphere(transform.position, coneRange, playerLayer);

            // If player is in this collider
            if (playerCollider.Length > 0)
            {
                if (playerTarget != null)
                {
                    Vector3 direction = playerTarget.position - myTarget.position;
                    Ray rayToPlayer = new Ray(myTarget.position, direction);

                    // If player is in the cone range
                    if (Vector3.Angle(direction, transform.forward) < 45)
                    {
                        if (Physics.Raycast(rayToPlayer, out RaycastHit hit, coneRange, collisionLayers))
                        {
                            if (hit.collider.gameObject.layer == 11)
                            {
                                Performing = true;
                            }
                            else
                            {
                                Performing = false;
                            }
                        }
                    }
                    else
                    {
                        Performing = false;
                    }
                }
            }

            yield return new WaitForSecondsRealtime(1f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, coneRange);
    }

    public void FindPlayer()
    {
        playerTarget = GameObject.FindGameObjectWithTag("playerTarget").transform;
    }

    public void PlayerLost()
    {
        playerTarget = null;
    }

    public void ComponentUpdate()
    {
        // Left brank on purpose
    }

    public void ComponentFixedUpdate()
    {
        // Left brank on purpose
    }
}
