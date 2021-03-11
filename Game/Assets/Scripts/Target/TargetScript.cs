using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TargetScript : MonoBehaviour
{
    // Components
    [SerializeField] private CinemachineTarget targetParent;

    // Cameras
    [SerializeField] private CinemachineVirtualCamera targetCamera;
    [SerializeField] private CinemachineFreeLook thirdPersonCamera;
    private bool thirdPersonActive;

    private void Start()
    {
        thirdPersonActive = true;
    }

    private void OnEnable()
    {
        targetParent.CameraChange += SwitchCamera;
    }

    private void OnDisable()
    {
        targetParent.CameraChange -= SwitchCamera;
    }

    private void SwitchCamera() => thirdPersonActive = !thirdPersonActive;

    private void Update()
    {
        if (thirdPersonActive)
            transform.LookAt(thirdPersonCamera.transform);
        else
            transform.LookAt(targetCamera.transform);
    }
}
