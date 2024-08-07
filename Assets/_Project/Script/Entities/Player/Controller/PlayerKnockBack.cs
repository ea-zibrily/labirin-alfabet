using System;
using System.Collections;
using UnityEngine;

namespace Alphabet.Entities.Player
{
    public class PlayerKnockBack : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Knock")] 
        [SerializeField] private float knockBackTime;
        [SerializeField] private float hitDirectionForce;
        [SerializeField] private float constForce;
        [SerializeField] private float inputForce;

        private Coroutine _knockBackCoroutine;

        [Header("Reference")] 
        private PlayerController _playerController;
        private Rigidbody2D _playerRb;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _playerRb = GetComponent<Rigidbody2D>();
            _playerController = GetComponent<PlayerController>();
        }

        #endregion
        
        #region Methods
        
        public void CallKnockBack(Vector2 hitDir, Vector2 constForceDir, Vector2 inputDir)
        {
            _knockBackCoroutine = StartCoroutine(KnockBackRoutine(hitDir, constForceDir, inputDir));
        }
        
        private IEnumerator KnockBackRoutine(Vector2 hitDir, Vector2 constForceDir, Vector2 inputDir)
        {
            var hitForce = hitDir * hitDirectionForce;
            var tempConstForce = constForceDir * constForce;
            
            var elapsedTime = 0f;
            _playerController.StopMovement();
            while (elapsedTime < knockBackTime)
            {
                elapsedTime += Time.fixedDeltaTime;

                var knockBackForce = hitForce + tempConstForce;
                var combinedForce = inputDir != Vector2.zero
                    ? knockBackForce + new Vector2(inputDir.x, inputDir.y)
                    : knockBackForce;
                
                _playerRb.velocity = combinedForce;
                yield return new WaitForFixedUpdate();
            }
            _playerController.StartMovement();
        }
        
        #endregion
        
    }
}