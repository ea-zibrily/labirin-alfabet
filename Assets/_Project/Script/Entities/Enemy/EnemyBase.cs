using LabirinKata.Data;
using LabirinKata.Enum;
using LabirinKata.Managers;
using LabirinKata.Stage;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LabirinKata.Entities.Enemy
{
    public class EnemyBase : MonoBehaviour
    {
        #region Const Variable

        private const string HORIZONTAL_KEY = "Horizontal";
        private const string VERTICAL_KEY = "Vertical";

        #endregion

        #region Variable

        [Header("Data")] 
        public EnemyData EnemyData;
        public EnemyPattern EnemyType;
        public Transform[] MovePointTransform;
        
        protected Transform CurrentTarget;
        protected int CurrentTargetIndex;
        protected int EarlyPositionIndex { get; private set; }
        
        private Vector2 _enemyDirection;
        private bool _isDataAsync;
        
        [Header("Reference")] 
        private Animator _enemyAnimator;
        
        #endregion

        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            _enemyAnimator = GetComponentInChildren<Animator>();
        }
        
        private void Start()
        {
            _isDataAsync = CheckAsyncEnemyData();
            if (_isDataAsync) return;

            RandomizeTargetPoint();
        }
        
        private void Update()
        {
            if (_isDataAsync || !GameManager.Instance.IsGameStart) return;
            
            EnemyMove();
            // EnemyAnimation();
        }
        
        #endregion
        
        #region CariHuruf Callbacks
        
        protected virtual void EnemyMove()
        {
            var enemyPosition = transform.position;
            var targetPosition = CurrentTarget.position;
            var currentSpeed = EnemyData.EnemyMove * Time.deltaTime;
            
            _enemyDirection = targetPosition - enemyPosition;
            _enemyDirection.Normalize();
            
            transform.position = Vector2.MoveTowards(enemyPosition, targetPosition, currentSpeed);
        }
        
        protected virtual void RandomizeTargetPoint()
        {
            EarlyPositionIndex = Random.Range(0, MovePointTransform.Length - 1);
            transform.position = MovePointTransform[EarlyPositionIndex].position;
        }

        private void EnemyAnimation()
        {
            _enemyAnimator.SetFloat(HORIZONTAL_KEY, _enemyDirection.x);
            _enemyAnimator.SetFloat(VERTICAL_KEY, _enemyDirection.y);
        }

        private bool CheckAsyncEnemyData()
        {
            if (EnemyType != EnemyData.EnemyPattern || MovePointTransform.Length > EnemyData.MaxEnemyPoint)
            {
                Debug.LogError("Enemy data not valid! \n Check your selected enum in enemy data \n " +
                               "Max point Line (2), Elbow (3), Box (4)");
                return true;
            }
            
            return false;
        }
        
        #endregion
    }
}