
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    private CinemachineCamera cinemachineVirtualCamera;

    private void Start()
    {
        SetPlayerCameraFollow();
    }

    public void SetPlayerCameraFollow()
    {
        cinemachineVirtualCamera = Object.FindFirstObjectByType<CinemachineCamera>();
        cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;
    }
}
