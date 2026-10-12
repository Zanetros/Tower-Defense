using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TowerDefense.ObstacleSystem
{
    public enum ObstacleState
    {
        Idle,
        InQueue,
        Clearing,
        Cleared
    }

    /// <summary>
    /// Component attached to each obstacle in the scene.
    /// Detects clicks using Unity's EventSystem (IPointerClickHandler)
    /// and manages state transitions, countdown timer, and visual updates.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class ClearableObstacle : MonoBehaviour, IPointerClickHandler
    {
        [Header("Obstacle Data")]
        [Tooltip("Configurable data asset for this obstacle.")]
        [SerializeField] private ObstacleData obstacleData;

        [Header("Visual References")]
        [Tooltip("SpriteRenderer of the obstacle. If null, automatically retrieved from the GameObject.")]
        [SerializeField] private SpriteRenderer spriteRenderer;

        [Tooltip("Text component used to display the countdown timer in seconds (TMP).")]
        [SerializeField] private TMP_Text countdownText;

        [Tooltip("Status message displayed while waiting in queue.")]
        [SerializeField] private string inQueueStatusMessage = "In Queue";

        [Header("Lifecycle Events")]
        public UnityEvent onEnqueued;
        public UnityEvent onClearingStarted;
        public UnityEvent onClearingCompleted;

        private ObstacleState _currentState = ObstacleState.Idle;

        public ObstacleData Data => obstacleData;
        public ObstacleState CurrentState => _currentState;

        private void Awake()
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            // Hide countdown text on start if present
            if (countdownText != null)
            {
                countdownText.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Called by Unity's EventSystem when the player clicks on this obstacle's Collider2D.
        /// Requires a Physics2DRaycaster on the Main Camera and an EventSystem in the scene.
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            // Ignore clicks if already cleared, currently clearing, or waiting in queue
            if (_currentState != ObstacleState.Idle)
            {
                return;
            }

            if (obstacleData == null)
            {
                Debug.LogWarning($"[ClearableObstacle] {gameObject.name} does not have an ObstacleData assigned!", this);
                return;
            }

            // Open the confirmation modal
            if (ObstacleConfirmationUI.Instance != null)
            {
                ObstacleConfirmationUI.Instance.OpenConfirmation(this);
            }
            else
            {
                Debug.LogError("[ClearableObstacle] ObstacleConfirmationUI.Instance was not found in the scene!");
            }
        }

        /// <summary>
        /// Called when the player confirms removal in the UI modal.
        /// Places the obstacle into the waiting queue.
        /// </summary>
        public void EnterQueue()
        {
            if (_currentState != ObstacleState.Idle) return;

            _currentState = ObstacleState.InQueue;

            if (countdownText != null && !string.IsNullOrEmpty(inQueueStatusMessage))
            {
                countdownText.text = inQueueStatusMessage;
                countdownText.gameObject.SetActive(true);
            }

            onEnqueued?.Invoke();
        }

        /// <summary>
        /// Called by the queue manager when clearing starts for this obstacle.
        /// </summary>
        public void StartClearing()
        {
            _currentState = ObstacleState.Clearing;

            if (countdownText != null)
            {
                countdownText.gameObject.SetActive(true);
            }

            onClearingStarted?.Invoke();
        }

        /// <summary>
        /// Updates the visual countdown timer in seconds.
        /// </summary>
        /// <param name="remainingSeconds">Remaining time in seconds.</param>
        public void UpdateProgress(float remainingSeconds)
        {
            if (countdownText != null)
            {
                countdownText.text = $"{Mathf.CeilToInt(remainingSeconds)}s";
            }
        }

        /// <summary>
        /// Completes the obstacle clearing process, swapping the sprite and hiding the timer.
        /// </summary>
        public void FinishClearing()
        {
            _currentState = ObstacleState.Cleared;

            // Swap sprite to cleared version
            if (obstacleData != null && obstacleData.ClearedSprite != null && spriteRenderer != null)
            {
                spriteRenderer.sprite = obstacleData.ClearedSprite;
            }

            // Hide countdown text
            if (countdownText != null)
            {
                countdownText.gameObject.SetActive(false);
            }

            onClearingCompleted?.Invoke();
        }
    }
}
