using System.Collections;
using UnityEngine;

namespace Alphabet.Entities.Player
{
    public class PlayerFlash
    {
        private readonly int _playerLayerNum;
        private readonly int _enemyLayerNum;
        private readonly int _flashNumber;
        private readonly float _flashDuration;
        private readonly Color _flashColor;
        private readonly SpriteRenderer _playerSpriteRenderer;

        #region Constructor

        public PlayerFlash(int playerLayer, int enemyLayer, Color flashColor, float duration, int flashNumber, SpriteRenderer sprite)
        {
            _playerLayerNum = playerLayer;
            _enemyLayerNum = enemyLayer;
            _flashColor = flashColor;
            _flashDuration = duration;
            _flashNumber = flashNumber;
            _playerSpriteRenderer = sprite;
        }

        public PlayerFlash(Color flashColor, float duration, SpriteRenderer sprite)
        {
            _flashColor = flashColor;
            _flashDuration = duration;
            _playerSpriteRenderer = sprite;
        }

        #endregion
        
        public IEnumerator FlashWithTimeRoutine()
        {
            var _flashNumElapsed = 0;
            Physics2D.IgnoreLayerCollision(_playerLayerNum, _enemyLayerNum, true);

            while (_flashNumElapsed < _flashNumber)
            {
                _playerSpriteRenderer.color = _flashColor;
                yield return new WaitForSeconds(_flashDuration);

                _playerSpriteRenderer.color = Color.white;
                yield return new WaitForSeconds(_flashDuration);
                _flashNumElapsed++;
            }

            Physics2D.IgnoreLayerCollision(_playerLayerNum, _enemyLayerNum, false);
        }

        public IEnumerator FlashWithConditionRoutine(bool condition)
        {
            while (condition)
            {
                _playerSpriteRenderer.color = _flashColor;
                yield return new WaitForSeconds(_flashDuration);
                
                _playerSpriteRenderer.color = Color.white;
                yield return new WaitForSeconds(_flashDuration);
            }
        }
    }
}
