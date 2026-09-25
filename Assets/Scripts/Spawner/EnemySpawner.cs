using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using TowerDefense.Data;
using TowerDefense.Enemies;
using TowerDefense.Path;

namespace TowerDefense.Spawner
{
    /// <summary>
    /// High-performance enemy spawner utilizing Unity's native ObjectPool to eliminate GC allocations.
    /// Recycles enemy GameObjects across waves instead of calling Instantiate/Destroy.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Route Settings")]
        [SerializeField] private PathRoute pathRoute;

        [Header("Base Enemy Prefab")]
        [Tooltip("Base enemy prefab that will be pooled and receive ScriptableObject data.")]
        [SerializeField] private Enemy enemyPrefab;

        [Header("Enemy Data (ScriptableObject)")]
        [Tooltip("Default enemy data asset to spawn.")]
        [SerializeField] private EnemyData enemyDataToSpawn;

        [Header("Spawn Settings")]
        [SerializeField] private bool autoStartSpawning = true;
        [SerializeField] private int totalEnemiesToSpawn = 10;
        [SerializeField] private float timeBetweenSpawns = 1.5f;
        [SerializeField] private float initialDelay = 1f;

        [Header("Object Pool Settings")]
        [SerializeField] private int defaultPoolCapacity = 20;
        [SerializeField] private int maxPoolSize = 100;
        [SerializeField] private bool collectionCheck = true;

        private IObjectPool<Enemy> enemyPool;
        private int enemiesSpawnedCount = 0;
        private Coroutine spawnCoroutine;

        public bool IsSpawning => spawnCoroutine != null;
        public int EnemiesSpawnedCount => enemiesSpawnedCount;
        public IObjectPool<Enemy> EnemyPool => enemyPool;

        private void Awake()
        {
            InitializePool();
        }

        private void Start()
        {
            if (pathRoute == null)
            {
                pathRoute = FindFirstObjectByType<PathRoute>();
            }

            if (autoStartSpawning)
            {
                StartSpawning();
            }
        }

        private void InitializePool()
        {
            enemyPool = new ObjectPool<Enemy>(
                createFunc: CreatePooledEnemy,
                actionOnGet: OnGetFromPool,
                actionOnRelease: OnReleaseToPool,
                actionOnDestroy: OnDestroyPooledEnemy,
                collectionCheck: collectionCheck,
                defaultCapacity: defaultPoolCapacity,
                maxSize: maxPoolSize
            );
        }

        private Enemy CreatePooledEnemy()
        {
            if (enemyPrefab == null)
            {
                Debug.LogError("[EnemySpawner] No EnemyPrefab assigned to spawner!");
                return null;
            }

            Enemy enemyInstance = Instantiate(enemyPrefab, transform);
            enemyInstance.SetPool(enemyPool);
            return enemyInstance;
        }

        private void OnGetFromPool(Enemy enemy)
        {
            enemy.gameObject.SetActive(true);
        }

        private void OnReleaseToPool(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
        }

        private void OnDestroyPooledEnemy(Enemy enemy)
        {
            if (enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }

        /// <summary>
        /// Starts the spawning coroutine.
        /// </summary>
        public void StartSpawning()
        {
            if (spawnCoroutine == null)
            {
                spawnCoroutine = StartCoroutine(SpawnRoutine());
            }
        }

        /// <summary>
        /// Stops the spawning coroutine.
        /// </summary>
        public void StopSpawning()
        {
            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
                spawnCoroutine = null;
            }
        }

        private IEnumerator SpawnRoutine()
        {
            if (initialDelay > 0f)
            {
                yield return new WaitForSeconds(initialDelay);
            }

            while (enemiesSpawnedCount < totalEnemiesToSpawn || totalEnemiesToSpawn <= 0)
            {
                SpawnSingleEnemy(enemyDataToSpawn);
                enemiesSpawnedCount++;

                yield return new WaitForSeconds(timeBetweenSpawns);
            }

            spawnCoroutine = null;
        }

        /// <summary>
        /// Retrieves an enemy from the pool, applies data, and starts its movement on the path.
        /// </summary>
        public Enemy SpawnSingleEnemy(EnemyData customData = null)
        {
            if (pathRoute == null)
            {
                Debug.LogError("[EnemySpawner] No PathRoute configured for Spawner!");
                return null;
            }

            if (enemyPool == null)
            {
                InitializePool();
            }

            Enemy enemy = enemyPool.Get();
            if (enemy == null) return null;

            Vector3 spawnPosition = pathRoute.GetStartPoint();
            enemy.transform.position = spawnPosition;

            EnemyData dataToApply = customData != null ? customData : enemyDataToSpawn;
            if (dataToApply != null)
            {
                enemy.Initialize(dataToApply, pathRoute);
            }
            else
            {
                enemy.Initialize(pathRoute);
            }

            return enemy;
        }
    }
}
