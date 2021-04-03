using UnityEngine;
using UnityEngine.Animations.Rigging;

/// <summary>
/// Class responsible for constraining the player's head.
/// </summary>
public class PlayerRigHeadConstrainPosition : MonoBehaviour
{
    // Components
    private Camera mainCam;
    private CinemachineTarget cinemachineTarget;

    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Transform currentTarget;

    private void Awake()
    {
        mainCam = Camera.main;
        cinemachineTarget = FindObjectOfType<CinemachineTarget>();
    }

    private void FixedUpdate()
    {
        if (cinemachineTarget.Targeting)
        {
            currentTarget.position = cinemachineTarget.CurrentTarget.position;
        }
        else
        {
            currentTarget.position = cameraTarget.position;
            cameraTarget.position = mainCam.transform.forward * 50;
        }
    }
}
