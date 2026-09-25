using UnityEngine;

namespace TowerDefense.Data
{
    /// <summary>
    /// ScriptableObject storing static configuration data for an enemy archetype (Flyweight Pattern).
    /// </summary>
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "Tower Defense/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string enemyName = "Basic Enemy";
        [SerializeField] private Sprite sprite;
        [SerializeField] private RuntimeAnimatorController animatorController;

        [Header("Base Attributes")]
        [Tooltip("Starting maximum health for this enemy type.")]
        [SerializeField] private float maxHealth = 10f;

        [Tooltip("Movement speed across the grid.")]
        [SerializeField] private float moveSpeed = 2.5f;

        [Tooltip("Damage dealt to player base upon reaching the goal.")]
        [SerializeField] private int damageToBase = 1;

        [Tooltip("Coins/currency rewarded upon defeat.")]
        [SerializeField] private int rewardCoins = 5;

        public string EnemyName => enemyName;
        public Sprite Sprite => sprite;
        public RuntimeAnimatorController AnimatorController => animatorController;
        public float MaxHealth => maxHealth;
        public float MoveSpeed => moveSpeed;
        public int DamageToBase => damageToBase;
        public int RewardCoins => rewardCoins;
    }
}
