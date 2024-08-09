using UnityEngine;

namespace Alphabet.Entities.Player
{
    public class ButtonInputHandler : MonoBehaviour
    {
        #region Fields & Properties

        // Direction Value
        [SerializeField] private Vector2 direction;
        public Vector2 Direction => direction;

        // Reference
        private PlayerActionMap _playerActionMap;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake() => _playerActionMap = new PlayerActionMap();
        private void OnEnable() => _playerActionMap.Enable();
        private void OnDisable() => _playerActionMap.Disable();

        private void Update()
        {
            OnMovementPerformed();
        }

        #endregion

        #region Methods

        // !- Core
        private void OnMovementPerformed()
        {
            direction = _playerActionMap.Player.Move.ReadValue<Vector2>();
            direction.Normalize();
        }

        // !- Helper
        public void EnableInput() => _playerActionMap.Enable();
        public void DisableInput() => _playerActionMap.Disable();

        #endregion
    }
}
