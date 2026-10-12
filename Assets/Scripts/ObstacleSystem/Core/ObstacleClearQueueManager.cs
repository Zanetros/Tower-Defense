using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.ObstacleSystem
{
    /// <summary>
    /// Central manager for the obstacle clearing queue.
    /// Strictly processes obstacles sequentially (one by one in FIFO order).
    /// The next obstacle in the queue will ONLY begin clearing after the active obstacle
    /// has completely finished its countdown.
    /// </summary>
    public class ObstacleClearQueueManager : MonoBehaviour
    {
        public static ObstacleClearQueueManager Instance { get; private set; }

        // Queue of pending obstacles waiting to be cleared
        private readonly Queue<ClearableObstacle> _obstacleQueue = new Queue<ClearableObstacle>();

        // Currently active obstacle being cleared
        private ClearableObstacle _currentObstacle;

        // Coroutine reference handling queue execution
        private Coroutine _processQueueCoroutine;

        // Modular reference to currency handler
        private ICurrencyHandler _currencyHandler;

        // Global events
        public event Action<ClearableObstacle> OnObstacleEnqueued;
        public event Action<ClearableObstacle> OnClearingStarted;
        public event Action<ClearableObstacle> OnClearingCompleted;
        public event Action<int> OnQueueCountChanged;

        public int QueueCount => _obstacleQueue.Count;
        public ClearableObstacle CurrentObstacle => _currentObstacle;
        public bool IsProcessing => _currentObstacle != null;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            // Locate any ICurrencyHandler implementation in the scene
            _currencyHandler = SimpleCurrencyManager.Instance != null 
                ? SimpleCurrencyManager.Instance 
                : FindObjectOfType<SimpleCurrencyManager>();
        }

        /// <summary>
        /// Injects an external custom currency handler.
        /// </summary>
        public void SetCurrencyHandler(ICurrencyHandler handler)
        {
            _currencyHandler = handler;
        }

        /// <summary>
        /// Adds an obstacle to the queue and starts processing if idle.
        /// </summary>
        public void EnqueueObstacle(ClearableObstacle obstacle)
        {
            if (obstacle == null) return;

            // Prevent duplicate entries
            if (_obstacleQueue.Contains(obstacle) || _currentObstacle == obstacle)
            {
                return;
            }

            // Put obstacle in waiting state
            obstacle.EnterQueue();
            _obstacleQueue.Enqueue(obstacle);

            OnObstacleEnqueued?.Invoke(obstacle);
            OnQueueCountChanged?.Invoke(_obstacleQueue.Count);

            // If not currently processing any obstacle, start the sequential runner
            if (_processQueueCoroutine == null)
            {
                _processQueueCoroutine = StartCoroutine(ProcessQueueRoutine());
            }
        }

        /// <summary>
        /// Dedicated sequential runner. Processes obstacles strictly one at a time.
        /// The next obstacle in the queue will never start until the current one finishes.
        /// </summary>
        private IEnumerator ProcessQueueRoutine()
        {
            while (_obstacleQueue.Count > 0)
            {
                _currentObstacle = _obstacleQueue.Dequeue();
                OnQueueCountChanged?.Invoke(_obstacleQueue.Count);

                // If obstacle was destroyed while waiting in queue, skip to next
                if (_currentObstacle == null || _currentObstacle.CurrentState == ObstacleState.Cleared)
                {
                    _currentObstacle = null;
                    continue;
                }

                // Start clearing the current obstacle
                _currentObstacle.StartClearing();
                OnClearingStarted?.Invoke(_currentObstacle);

                float totalDuration = _currentObstacle.Data != null ? _currentObstacle.Data.ClearDuration : 0f;
                float remainingTime = totalDuration;

                // Countdown loop strictly for the active obstacle
                while (remainingTime > 0f)
                {
                    if (_currentObstacle == null)
                    {
                        break;
                    }

                    remainingTime -= Time.deltaTime;
                    _currentObstacle.UpdateProgress(Mathf.Max(0f, remainingTime));
                    yield return null;
                }

                // Finish active obstacle
                if (_currentObstacle != null)
                {
                    _currentObstacle.FinishClearing();

                    // Award currency upon completion
                    if (_currentObstacle.Data != null && _currentObstacle.Data.RewardCoins > 0)
                    {
                        if (_currencyHandler != null)
                        {
                            _currencyHandler.AddCurrency(_currentObstacle.Data.RewardCoins);
                        }
                        else if (SimpleCurrencyManager.Instance != null)
                        {
                            SimpleCurrencyManager.Instance.AddCurrency(_currentObstacle.Data.RewardCoins);
                        }
                    }

                    OnClearingCompleted?.Invoke(_currentObstacle);
                }

                // Reset current obstacle before picking up the next one
                _currentObstacle = null;
                yield return null;
            }

            _processQueueCoroutine = null;
        }
    }
}
