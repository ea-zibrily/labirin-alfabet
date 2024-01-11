using System;
using UnityEngine;
using CariHuruf.Data;

namespace CariHuruf.Entities.Player
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerController : MonoBehaviour
    {
        #region Variable

        [Header("Player")] 
        [SerializeField] private PlayerData playerData;
        [SerializeField] private Vector2 movementDirection;
        
        // Const
        private const string HORIZONTAL_KEY = "Horizontal";
        private const string VERTICAL_KEY = "Vertical";
        private const string IS_MOVE = "isMove";
        
        [Header("Reference")] 
        private Rigidbody2D _playerRb;
        private Animator _playerAnimator;
        private PlayerInputHandler _playerInputHandler;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _playerRb = GetComponent<Rigidbody2D>();
            _playerInputHandler = GetComponentInChildren<PlayerInputHandler>();
            _playerAnimator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            gameObject.name = playerData.PlayerName;
        }

        private void FixedUpdate()
        {
            PlayerMove();
        }

        private void Update()
        {
            PlayerAnimation();
        }
        
        #endregion
        
        #region Zimbril Callbacks

        private void PlayerMove()
        {
            //--- W Enhanced Touch Input
            var moveX = _playerInputHandler.Direction.x;
            var moveY = _playerInputHandler.Direction.y;
            
            if (Mathf.Abs(moveX) > Mathf.Abs(moveY))
            {
                moveY = 0f;
            }
            else
            {
                moveX = 0f;
            }
            
            movementDirection = new Vector2(moveX, moveY);
            movementDirection.Normalize();
            _playerRb.velocity = movementDirection * playerData.MoveSpeed;
        }
        
        private void PlayerAnimation()
        {
            if (movementDirection != Vector2.zero)
            {
                _playerAnimator.SetFloat(HORIZONTAL_KEY, movementDirection.x);
                _playerAnimator.SetFloat(VERTICAL_KEY, movementDirection.y);
                _playerAnimator.SetBool(IS_MOVE, true);
            }
            else
            {
                _playerAnimator.SetBool(IS_MOVE, false);
            }
        }

        private void StopPlayer() => _playerRb.velocity = Vector2.zero;

        #endregion
    }
}
