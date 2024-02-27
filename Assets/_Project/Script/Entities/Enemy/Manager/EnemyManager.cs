using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LabirinKata.Gameplay.EventHandler;

namespace LabirinKata.Entities.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        #region Fields & Property

        private EnemyBase _enemyBase;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake() {
            _enemyBase = GetComponent<EnemyBase>();
        }

        private void OnEnable()
        {
            CameraEventHandler.OnCameraShiftIn += _enemyBase.StopMovement;
            CameraEventHandler.OnCameraShiftOut += _enemyBase.StartMovement;
        }
        
        private void OnDisable()
        {
            CameraEventHandler.OnCameraShiftIn -= _enemyBase.StopMovement;
            CameraEventHandler.OnCameraShiftOut -= _enemyBase.StartMovement;
        }

        #endregion
    }
}
