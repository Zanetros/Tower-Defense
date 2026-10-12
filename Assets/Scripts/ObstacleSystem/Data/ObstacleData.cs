using UnityEngine;

namespace TowerDefense.ObstacleSystem
{
    /// <summary>
    /// ScriptableObject defining properties and configuration for an obstacle.
    /// Allows designers to create multiple obstacle types (Rock, Tree, Ruins, etc.)
    /// directly in the Unity Editor without modifying code.
    /// </summary>
    [CreateAssetMenu(fileName = "NewObstacleData", menuName = "Tower Defense/Obstacle Data")]
    public class ObstacleData : ScriptableObject
    {
        [Header("Identification")]
        [Tooltip("Name displayed on the confirmation UI modal.")]
        [SerializeField] private string obstacleName = "Obstacle";

        [Header("Timing and Visuals")]
        [Tooltip("Time in seconds required to clear this obstacle.")]
        [SerializeField] private float clearDuration = 5f;

        [Tooltip("Sprite displayed after the obstacle is cleared (e.g. clean ground, rubble).")]
        [SerializeField] private Sprite clearedSprite;

        [Header("Economy and Rewards")]
        [Tooltip("Amount of currency awarded to the player upon completing removal.")]
        [SerializeField] private int rewardCoins = 10;

        // Public properties for clean, read-only access
        public string ObstacleName => obstacleName;
        public float ClearDuration => Mathf.Max(0.1f, clearDuration);
        public Sprite ClearedSprite => clearedSprite;
        public int RewardCoins => Mathf.Max(0, rewardCoins);
    }
}
