using System;
using UnityEngine;
using UnityEngine.Pool;
using TowerDefense.Core;
using TowerDefense.Data;
using TowerDefense.Path;

namespace TowerDefense.Enemies
{
    /// <summary>
    /// Main enemy runtime entity.
    /// Uses static configuration from EnemyData (ScriptableObject) and manages mutable runtime state.
    /// Supports Unity's native Object Pooling for high-performance memory reuse.
    /// </summary>
    [RequireComponent(typeof(EnemyMovement))]
    [DisallowMultipleComponent]
    public class Enemy : MonoBehaviour
    {
        [Header("Base Data (ScriptableObject)")]
        [SerializeField] private EnemyData data;

        [Header("Components")]
        [SerializeField] private EnemyMovement movement;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;

        // Runtime state
        private float currentHealth;
        private bool isDead = false;
        private IObjectPool<Enemy> parentPool;

        // High-performance C# native events (Zero GC allocation compared to UnityEvent)
        public event Action<float, float> OnHealthChanged; // (currentHealth, maxHealth)
        public event Action<Enemy> OnEnemyDeath;
        public event Action<Enemy> OnEnemyReachedGoal;

        public EnemyData Data => data;
        public float CurrentHealth => currentHealth;
        public float MaxHealth => data != null ? data.MaxHealth : 10f;
        public int DamageToBase => data != null ? data.DamageToBase : 1;
        public int RewardCoins => data != null ? data.RewardCoins : 0;
        public EnemyMovement Movement => movement;
        public bool IsDead => isDead;

        private void Awake()
        {
            if (movement == null)
                movement = GetComponent<EnemyMovement>();

            if (spriteRenderer == null)
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            if (animator == null)
                animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            if (movement != null)
            {
                movement.OnReachedDestination += HandleGoalReached;
            }
        }

        private void OnDisable()
        {
            if (movement != null)
            {
                movement.OnReachedDestination -= HandleGoalReached;
            }
        }

        /// <summary>
        /// Assigns the object pool that manages this enemy instance.
        /// </summary>
        public void SetPool(IObjectPool<Enemy> pool)
        {
            parentPool = pool;
        }

        /// <summary>
        /// Applies the ScriptableObject data and resets runtime attributes.
        /// </summary>
        public void ApplyData(EnemyData enemyData)
        {
            data = enemyData;
            if (data == null) return;

            currentHealth = data.MaxHealth;
            isDead = false;

            if (movement != null)
            {
                movement.SetBaseSpeed(data.MoveSpeed);
            }

            if (spriteRenderer != null && data.Sprite != null)
            {
                spriteRenderer.sprite = data.Sprite;
            }

            if (animator != null && data.AnimatorController != null)
            {
                animator.runtimeAnimatorController = data.AnimatorController;
            }

            OnHealthChanged?.Invoke(currentHealth, MaxHealth);
        }

        /// <summary>
        /// Initializes the enemy with data, route, and sets active.
        /// </summary>
        public void Initialize(EnemyData enemyData, PathRoute path)
        {
            ApplyData(enemyData);

            if (movement != null)
            {
                movement.SetPath(path);
            }
        }

        /// <summary>
        /// Initializes the enemy with route using serialized EnemyData.
        /// </summary>
        public void Initialize(PathRoute path)
        {
            if (data != null)
            {
                ApplyData(data);
            }

            if (movement != null)
            {
                movement.SetPath(path);
            }
        }

        /// <summary>
        /// Applies damage from towers or projectiles.
        /// </summary>
        public void TakeDamage(float damage)
        {
            if (isDead) return;

            currentHealth = Mathf.Max(0f, currentHealth - damage);
            OnHealthChanged?.Invoke(currentHealth, MaxHealth);

            if (currentHealth <= 0f)
            {
                Die();
            }
        }

        /// <summary>
        /// Called when the enemy is defeated. Releases to pool or destroys.
        /// </summary>
        protected virtual void Die()
        {
            if (isDead) return;
            isDead = true;

            OnEnemyDeath?.Invoke(this);
            ReleaseOrDestroy();
        }

        /// <summary>
        /// Called automatically when the enemy reaches the final waypoint.
        /// </summary>
        protected virtual void HandleGoalReached()
        {
            if (isDead) return;
            isDead = true;

            if (PlayerBase.Instance != null)
            {
                PlayerBase.Instance.TakeDamage(DamageToBase);
            }
            else
            {
                Debug.LogWarning("[Enemy] Enemy reached goal, but no PlayerBase instance was found in scene.");
            }

            OnEnemyReachedGoal?.Invoke(this);
            ReleaseOrDestroy();
        }

        /// <summary>
        /// Releases the GameObject back to the ObjectPool for recycling, or destroys it if not pooled.
        /// </summary>
        private void ReleaseOrDestroy()
        {
            if (parentPool != null)
            {
                parentPool.Release(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
