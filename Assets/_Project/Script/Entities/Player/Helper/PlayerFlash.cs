using System.Collections;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Alphabet.Entities.Player
{
    public class PlayerFlash
    {
        #region Fields

        private readonly int _playerLayerNum;
        private readonly int _enemyLayerNum;
        private readonly int _flashNumber;
        private readonly float _flashDuration;

        private readonly Color _flashColor;
        private readonly Skeleton _playerSkeleton;

        #endregion

        #region Methods

        // !- Initialize
        public PlayerFlash(int playerLayer, int enemyLayer, Color flashColor, float duration, int flashNumber, Skeleton skeleton)
        {
            _playerLayerNum = playerLayer;
            _enemyLayerNum = enemyLayer;
            _flashColor = flashColor;
            _flashDuration = duration;
            _flashNumber = flashNumber;
            _playerSkeleton = skeleton;
        }

        public PlayerFlash(Color flashColor, float duration, Skeleton skeleton)
        {
            _flashColor = flashColor;
            _flashDuration = duration;
            _playerSkeleton = skeleton;
        }

        // !- Core
        public IEnumerator FlashWithTimeRoutine()
        {
            var _flashNumElapsed = 0;
            Physics2D.IgnoreLayerCollision(_playerLayerNum, _enemyLayerNum, true);

            while (_flashNumElapsed < _flashNumber)
            {
                _playerSkeleton.SetColor(_flashColor);
                yield return new WaitForSeconds(_flashDuration);

                _playerSkeleton.SetColor(Color.white);
                yield return new WaitForSeconds(_flashDuration);
                _flashNumElapsed++;
            }
            Physics2D.IgnoreLayerCollision(_playerLayerNum, _enemyLayerNum, false);
        }

        public IEnumerator FlashWithConditionRoutine(bool condition)
        {
            while (condition)
            {
                 _playerSkeleton.SetColor(_flashColor);
                yield return new WaitForSeconds(_flashDuration);

                _playerSkeleton.SetColor(Color.white);
                yield return new WaitForSeconds(_flashDuration);
            }
        }

        #endregion
    }
}
