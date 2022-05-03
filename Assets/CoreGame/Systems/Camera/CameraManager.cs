using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;
    private CinemachineVirtualCamera currentVirtualCamera;

    public enum CameraShakeType
    {
        NONE,
        MILD,
        NORMAL,
        STRONG
    }

    [Header("Camera shake")]
    [SerializeField] private float mildShakeFrequency;
    [SerializeField] private float mildShakeAmplitude;

    [SerializeField] private float normalShakeFrequency;
    [SerializeField] private float normalShakeAmplitude;

    [SerializeField] private float strongShakeFrequency;
    [SerializeField] private float strongShakeAmplitude;

    private CameraShakeType currentShakeType = CameraShakeType.NONE;
    private float currentShakeTimer = 0f;
    private float currentShakeDuration;


    private Vector3 currentProjectedFront;
    private Vector3 currentProjectedRight;


    private void Update()
    {
        if (GameManager.instance.GetCurrentGameState() == GameManager.GameState.COMBAT)
        {
            UpdateCameraVectors();

            if (currentShakeType != CameraShakeType.NONE)
            {
                currentShakeTimer += Time.deltaTime;
                if (currentShakeTimer >= currentShakeDuration)
                {
                    ShakeCamera(CameraShakeType.NONE, 0f);
                }
            }
        }
    }

    private void UpdateCameraVectors()
    {
        currentProjectedFront = Vector3.ProjectOnPlane(mainCamera.transform.forward, Vector3.up);
        currentProjectedRight = Vector3.ProjectOnPlane(mainCamera.transform.right, Vector3.up);
    }

    public Vector3 GetCurrentProjectedFront()
    {
        return currentProjectedFront;
    }

    public Vector3 GetCurrentProjectedRight()
    {
        return currentProjectedRight;
    }

    public void ShakeCamera(CameraShakeType cameraShakeType, float duration)
    {
        currentShakeType = cameraShakeType;
        currentShakeDuration = duration;

        CinemachineBasicMultiChannelPerlin cinemachinePerlin = currentVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        switch (currentShakeType)
        {
            case CameraShakeType.NONE:
                cinemachinePerlin.m_FrequencyGain = 0f;
                cinemachinePerlin.m_AmplitudeGain = 0f;
                break;

            case CameraShakeType.MILD:
                cinemachinePerlin.m_FrequencyGain = mildShakeFrequency;
                cinemachinePerlin.m_AmplitudeGain = mildShakeAmplitude;
                break;

            case CameraShakeType.NORMAL:
                cinemachinePerlin.m_FrequencyGain = normalShakeFrequency;
                cinemachinePerlin.m_AmplitudeGain = normalShakeAmplitude;
                break;

            case CameraShakeType.STRONG:
                cinemachinePerlin.m_FrequencyGain = strongShakeFrequency;
                cinemachinePerlin.m_AmplitudeGain = strongShakeAmplitude;
                break;
        }

        currentShakeTimer = 0f;
    }

    public void LoadCamera(CinemachineVirtualCamera camera)
    {
        if (currentVirtualCamera == camera)
        {
            return;
        }

        UnloadCurrentCamera();
        currentVirtualCamera = camera;
        currentVirtualCamera.enabled = true;
    }

    private void UnloadCurrentCamera()
    {
        if (currentVirtualCamera != null)
        {
            currentVirtualCamera.enabled = false;
        }
    }
}
