using System;
using UnityEngine;

namespace TowerDefense.Core
{
    /// <summary>
    /// Represents the player's base that must be defended against enemies.
    /// Uses native C# events for fast, allocation-free notifications.
    /// </summary>
    public class PlayerBase : MonoBehaviour
    {
        public static PlayerBase Instance { get; private set; }

        [Header("Base Health")]
        [SerializeField] private int maxHealth = 100;
        private int currentHealth;

        // High-performance C# native events
        public event Action<int, int> OnHealthChanged; // (currentHealth, maxHealth)
        public event Action OnBaseDestroyed;

        public int CurrentHealth => currentHealth;
        public int MaxHealth => maxHealth;
        public bool IsDestroyed => currentHealth <= 0;

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

            currentHealth = maxHealth;
        }

        private void Start()
        {
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        /// <summary>
        /// Applies damage to the base when an enemy reaches the goal.
        /// </summary>
        public void TakeDamage(int damage)
        {
            if (IsDestroyed) return;

            currentHealth = Mathf.Max(0, currentHealth - damage);
            Debug.Log($"[PlayerBase] Base took {damage} damage! Remaining health: {currentHealth}/{maxHealth}");

            OnHealthChanged?.Invoke(currentHealth, maxHealth);

            if (currentHealth <= 0)
            {
                Debug.LogWarning("[PlayerBase] Base destroyed! Game Over.");
                OnBaseDestroyed?.Invoke();
            }
        }

        /// <summary>
        /// Heals the player base.
        /// </summary>
        public void Heal(int amount)
        {
            if (IsDestroyed) return;

            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }
    }
}
