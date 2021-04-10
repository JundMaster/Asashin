﻿using UnityEngine;

/// <summary>
/// Abstract Scriptable object responsible for controlling enemy states with
/// vision.
/// </summary>
public abstract class EnemyStateWithVision : EnemyState
{
    // Vision
    [Header("Vision Cone Attributes")]
    [Range(1, 30)][SerializeField] protected byte coneRange;
    [Range(0, 90)] [SerializeField] protected byte desiredConeAngle;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] protected LayerMask collisionLayers;
    private const byte PLAYERLAYERNUMBER = 11;
    protected float lastTimeChecked;

    private PlayerWallHug playerWallHug;

    public override IState FixedUpdate()
    {
        if (playerWallHug == null) 
            playerWallHug = enemy.Player.GetComponent<PlayerWallHug>();

        return base.FixedUpdate();
    }

    /// <summary>
    /// Search for player every searchCheckDelay seconds inside a cone vision.
    /// </summary>
    /// <returns>True if player is inside a vision cone.</returns>
    protected bool PlayerInRange()
    {
        bool playerFound = false;

        Collider[] playerCollider =
            Physics.OverlapSphere(myTarget.position, coneRange, playerLayer);

        // If player is in this collider
        if (playerCollider.Length > 0)
        {
            if (playerTarget != null)
            {
                Vector3 direction = playerTarget.position - myTarget.position;
                Ray rayToPlayer = new Ray(myTarget.position, direction);

                // If player is in the cone range
                if (Vector3.Angle(
                    direction, myTarget.forward) < desiredConeAngle)
                {
                    if (Physics.Raycast(
                        rayToPlayer,
                        out RaycastHit hit,
                        coneRange,
                        collisionLayers))
                    {
                        // If it's player layer
                        if (hit.collider.gameObject.layer == PLAYERLAYERNUMBER)
                        {
                            // If player is performing wall hug
                            if (playerWallHug != null &&
                                playerWallHug.Performing)
                            {
                                // If the player is facing the enemy's forward
                                // (enemy only sees the player if they are
                                // basically facing or perpendicular to
                                // each other)
                                if (Vector3.Dot(
                                    enemy.transform.forward, 
                                    playerTarget.forward) < 
                                    0.5f)
                                {
                                    playerFound = true;
                                }
                            }
                            else
                            {
                                playerFound = true;
                            }
                            
                        }
                        else
                        {
                            playerFound = false;
                        }
                    }
                }
                else
                {
                    playerFound = false;
                }
            }
        }

        lastTimeChecked = Time.time;

        return playerFound;
    }
}
