using UnityEngine;
using Alphabet.Data;
using Alphabet.Managers;

using Random = UnityEngine.Random;

namespace Alphabet.Entities.Enemy
{
    public class EnemyBase : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Data")] 
        public EnemyData EnemyData;
        [SerializeField] private Vector2 _movementDirection;

        public Vector2 MovementDirection => _movementDirection;
        public bool CanMove { get; private set; }

        // Pattern Targeting
        protected PatternBase CurrentPattern { get; set; }
        public Transform CurrentTarget { get; set; }
        public int CurrentTargetIndex { get; set; }
        public int FirstPositionIndex { get; set; }
        
        // Reference
        protected EnemyHelper EnemyHelper { get; private set; }
        
        #endregion

        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            EnemyHelper = new EnemyHelper();
        }

        private void Start()
        {
            InitializeEnemy();
        }
        
        private void Update()
        {
            EnemyMove();
            EnemyPatternDirection();
        }
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        // !- Initialize
        protected virtual void InitializeEnemy() 
        {
            transform.parent.name = EnemyData.EnemyName;
            StopMovement();
        }

        protected void SetFirstPosition(Transform[] wayPoints)
        {
            FirstPositionIndex = Random.Range(0, wayPoints.Length - 1);
            transform.position = wayPoints[FirstPositionIndex].position;
        }
        
        // !- Core
        protected virtual void EnemyPatternDirection() { }

        private void EnemyMove()
        {
            var enemyPosition = transform.position;
            var targetPosition = CurrentTarget.position;
            var currentSpeed = EnemyData.EnemyMoveSpeed * Time.deltaTime;
            
            EnemyDirection(targetPosition);
            
            if (!CanMove || !GameManager.Instance.IsGameStart) return;
            transform.position = Vector2.MoveTowards(enemyPosition, targetPosition, currentSpeed);
        }

        private void EnemyDirection(Vector3 targetPosition)
        {
            var enemyPosition = transform.position;
            var direction = targetPosition - enemyPosition;
            var moveX = direction.x;
            var moveY = direction.y;
            
            if (Mathf.Abs(moveX) > Mathf.Abs(moveY))
            {
                moveY = 0f;
            }
            else
            {
                moveX = 0f;
            }
            
            _movementDirection = new Vector2(moveX, moveY);
            _movementDirection.Normalize();
        }
        
        // !- Helper
        public void StartMovement() => CanMove = true;
        public void StopMovement() => CanMove = false;
        
        #endregion

        #region Pattern Callbacks

        protected void SwitchPattern(PatternBase newPattern)
        {
            CurrentPattern = newPattern;
        }

        protected void InitializePattern() 
        {
            CurrentPattern.InitializePattern(isReInitialize: false);
        }
        
        protected void ReInitializePattern() 
        {
            CurrentPattern.InitializePattern(isReInitialize: true);
        }
        
        protected void UpdatePattern()
        {
            CurrentPattern.UpdatePattern();
        }
        
        #endregion
        
    }
}