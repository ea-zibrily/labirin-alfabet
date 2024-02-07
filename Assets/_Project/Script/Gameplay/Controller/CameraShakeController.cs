using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using LabirinKata.Gameplay.EventHandler;

namespace LabirinKata.Gameplay.Controller
{
    public class CameraShakeController : MonoBehaviour
    {
        #region Variable

        [Header("Shake")] 
        [SerializeField] private float hitAmplitudeGain;
        [SerializeField] private float hitFrequencyGain;
        [SerializeField] private float shakeDuration;
        [SerializeField] private bool isShaking;

        private float _shakeTimeElapse;
        
        [Header("Reference")]
        private CinemachineVirtualCamera _myVirtualCamera;
        private CinemachineBasicMultiChannelPerlin _myVirtualCameraNoise;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _myVirtualCamera = GetComponent<CinemachineVirtualCamera>();
            _myVirtualCameraNoise = _myVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void OnEnable()
        {
            CameraEventHandler.OnCameraShake += ShakeCamera;
        }

        private void OnDisable()
        {
            CameraEventHandler.OnCameraShake -= ShakeCamera;
        }

        private void Start()
        {
            ResetShakeCamera();
        }

        private void Update()
        {
            if (!isShaking) return;
            
            _shakeTimeElapse += Time.deltaTime;
            if (_shakeTimeElapse >= shakeDuration)
            {
                ResetShakeCamera();
            }
        }

        #endregion

        #region Labirin Kata Callbacks

        // !-- Core Functionality
        private void ShakeCamera()
        {
            _myVirtualCameraNoise.m_AmplitudeGain = hitAmplitudeGain;
            _myVirtualCameraNoise.m_FrequencyGain = hitFrequencyGain;

            isShaking = true;
        }

        private void ResetShakeCamera()
        {
            _myVirtualCameraNoise.m_AmplitudeGain = 0f;
            _myVirtualCameraNoise.m_FrequencyGain = 0f;

            _shakeTimeElapse = 0f;
            isShaking = false;
        }

        #endregion
    }
}