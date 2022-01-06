using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera defaultVirtualCamera, endVirtualCamera;

    public CinemachineVirtualCamera DefaultVirtualCamera { get { return defaultVirtualCamera; } }
    public CinemachineVirtualCamera EndVirtualCamera { get { return endVirtualCamera; } }
    public void SetCameraLook(CinemachineVirtualCamera cameraLookingAtTarget,  Transform diceTransform = null)
    {
        cameraLookingAtTarget.LookAt = diceTransform;
    }
    public void SetEndCamera(GameObject workoutDice)
    {
        endVirtualCamera = workoutDice.GetComponentInChildren<CinemachineVirtualCamera>();
        endVirtualCamera.gameObject.SetActive(true);
       
        SetCameraLook(endVirtualCamera, workoutDice.transform);
    }
    public void SwitchPripritiesVirtualCameras(CinemachineVirtualCamera virtualCameraToActivate, CinemachineVirtualCamera virtualCameraToDeactivate)
    {
        virtualCameraToActivate.Priority = 1;
        virtualCameraToDeactivate.Priority = 0;
    }
}
