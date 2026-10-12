using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.ObstacleSystem
{
    /// <summary>
    /// UI Confirmation modal ("Yes" and "No") displayed when clicking on an obstacle.
    /// Displays obstacle name, clear duration, and currency reward.
    /// </summary>
    public class ObstacleConfirmationUI : MonoBehaviour
    {
        public static ObstacleConfirmationUI Instance { get; private set; }

        [Header("Main Panel")]
        [Tooltip("The GameObject of the panel/popup that will be enabled or disabled.")]
        [SerializeField] private GameObject modalPanel;

        [Header("Informational Texts")]
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text durationText;
        [SerializeField] private TMP_Text rewardText;

        [Header("Action Buttons")]
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;

        private ClearableObstacle _targetObstacle;

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

            if (confirmButton != null)
                confirmButton.onClick.AddListener(OnConfirmClicked);

            if (cancelButton != null)
                cancelButton.onClick.AddListener(CloseModal);

            // Start closed
            CloseModal();
        }

        private void OnDestroy()
        {
            if (confirmButton != null)
                confirmButton.onClick.RemoveListener(OnConfirmClicked);

            if (cancelButton != null)
                cancelButton.onClick.RemoveListener(CloseModal);
        }

        /// <summary>
        /// Opens the confirmation modal for the specified obstacle.
        /// </summary>
        public void OpenConfirmation(ClearableObstacle obstacle)
        {
            if (obstacle == null || obstacle.Data == null) return;

            _targetObstacle = obstacle;
            ObstacleData data = obstacle.Data;

            if (titleText != null)
                titleText.text = data.ObstacleName;

            if (durationText != null)
                durationText.text = $"Time: {data.ClearDuration:0}s";

            if (rewardText != null)
                rewardText.text = data.RewardCoins > 0 ? $"Reward: +{data.RewardCoins} coins" : "";

            if (modalPanel != null)
                modalPanel.SetActive(true);
            else
                gameObject.SetActive(true);
        }

        /// <summary>
        /// Action triggered by the "Yes" confirmation button.
        /// Since clearing is free, it immediately queues the obstacle.
        /// </summary>
        private void OnConfirmClicked()
        {
            if (_targetObstacle == null || _targetObstacle.Data == null)
            {
                CloseModal();
                return;
            }

            // Enqueue obstacle directly
            if (ObstacleClearQueueManager.Instance != null)
            {
                ObstacleClearQueueManager.Instance.EnqueueObstacle(_targetObstacle);
            }
            else
            {
                Debug.LogError("[ObstacleConfirmationUI] ObstacleClearQueueManager was not found in the scene!");
            }

            CloseModal();
        }

        /// <summary>
        /// Closes the confirmation modal dialog.
        /// </summary>
        public void CloseModal()
        {
            _targetObstacle = null;

            if (modalPanel != null)
                modalPanel.SetActive(false);
            else
                gameObject.SetActive(false);
        }
    }
}
