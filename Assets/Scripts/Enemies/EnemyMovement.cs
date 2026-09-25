using System;
using UnityEngine;
using TowerDefense.Path;

namespace TowerDefense.Enemies
{
    /// <summary>
    /// Controls enemy movement along waypoints in a 2D grid path.
    /// Provides continuous, frame-rate independent movement without corner micro-pauses or delta-time jitter.
    /// </summary>
    [DisallowMultipleComponent]
    public class EnemyMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Tooltip("Minimum distance required to consider a waypoint reached.")]
        [SerializeField] private float waypointTolerance = 0.01f;

        [Tooltip("Use smoothed delta time to eliminate frame-pacing micro-stutters in the editor.")]
        [SerializeField] private bool useSmoothDeltaTime = true;

        [Header("Visual Orientation (2D)")]
        [Tooltip("Flip the sprite horizontally based on movement direction?")]
        [SerializeField] private bool flipSpriteOnDirection = true;
        [SerializeField] private SpriteRenderer spriteRenderer;

        // Runtime variables
        private float baseSpeed = 2.5f;
        private float speedMultiplier = 1f;
        private PathRoute currentPath;
        private int currentWaypointIndex = 0;
        private bool isMoving = false;

        public event Action OnReachedDestination;

        public float BaseSpeed
        {
            get => baseSpeed;
            set => baseSpeed = Mathf.Max(0f, value);
        }

        public float SpeedMultiplier
        {
            get => speedMultiplier;
            set => speedMultiplier = Mathf.Max(0f, value);
        }

        public float EffectiveSpeed => baseSpeed * speedMultiplier;

        private void Awake()
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }
        }

        /// <summary>
        /// Sets the base speed provided by the EnemyData ScriptableObject.
        /// </summary>
        public void SetBaseSpeed(float speed)
        {
            baseSpeed = Mathf.Max(0f, speed);
        }

        /// <summary>
        /// Initializes the path for the enemy and sets its position at the start.
        /// </summary>
        public void SetPath(PathRoute path)
        {
            currentPath = path;
            currentWaypointIndex = 0;

            if (currentPath != null && currentPath.PointCount > 0)
            {
                transform.position = currentPath.GetStartPoint();
                isMoving = true;
            }
            else
            {
                isMoving = false;
                Debug.LogWarning($"[EnemyMovement] Empty or null path assigned to enemy: {gameObject.name}");
            }
        }

        private void Update()
        {
            if (!isMoving || currentPath == null || currentPath.PointCount == 0) return;

            float dt = useSmoothDeltaTime ? Time.smoothDeltaTime : Time.deltaTime;
            float distanceToTravel = EffectiveSpeed * dt;

            // Continuous movement loop to consume full frame distance even across corners
            while (distanceToTravel > 0f && isMoving)
            {
                Vector3 targetPosition = currentPath.GetPoint(currentWaypointIndex);
                Vector3 currentPosition = transform.position;
                float distanceToTarget = Vector3.Distance(currentPosition, targetPosition);

                HandleDirection(targetPosition);

                if (distanceToTarget <= 0.0001f || distanceToTravel >= distanceToTarget)
                {
                    transform.position = targetPosition;
                    distanceToTravel -= distanceToTarget;
                    AdvanceToNextWaypoint();
                }
                else
                {
                    transform.position = Vector3.MoveTowards(currentPosition, targetPosition, distanceToTravel);
                    distanceToTravel = 0f;
                }
            }
        }

        private void AdvanceToNextWaypoint()
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= currentPath.PointCount)
            {
                isMoving = false;
                OnReachedDestination?.Invoke();
            }
        }

        private void HandleDirection(Vector3 targetPosition)
        {
            if (!flipSpriteOnDirection || spriteRenderer == null) return;

            float directionX = targetPosition.x - transform.position.x;

            if (directionX > 0.01f)
            {
                spriteRenderer.flipX = false; // Facing right
            }
            else if (directionX < -0.01f)
            {
                spriteRenderer.flipX = true; // Facing left
            }
        }

        /// <summary>
        /// Pauses or resumes movement.
        /// </summary>
        public void SetMovementActive(bool active)
        {
            isMoving = active;
        }
    }
}
