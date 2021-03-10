using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles Kenshusei death behaviour.
/// </summary>
public class KenshuseiDeathBehaviour : DeathBehaviour
{
    private CinemachineTarget cinemachineTarget;

    private void Awake()
    {
        cinemachineTarget = FindObjectOfType<CinemachineTarget>();
    }

    public override void Die()
    {
        Debug.Log("Character died");

        cinemachineTarget.CancelCurrentTarget();
        Destroy(gameObject);
    }
}
