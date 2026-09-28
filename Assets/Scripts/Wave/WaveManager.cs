using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefense.Data;
using TowerDefense.Enemies;
using TowerDefense.Spawner;

namespace TowerDefense.Wave
{
    /// <summary>
    /// Orchestrates the wave flow for a level.
    /// Reads a LevelData ScriptableObject, drives the EnemySpawner, and fires events
    /// at key moments so the UI and game state can react accordingly.
    /// </summary>
    public class WaveManager : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("The level definition containing all waves for this scene.")]
        [SerializeField] private LevelData levelData;

        [Tooltip("The spawner that will be used to create enemy instances.")]
        [SerializeField] private EnemySpawner enemySpawner;

        [Header("Settings")]
        [Tooltip("Start the first wave automatically when the scene loads.")]
        [SerializeField] private bool autoStart = true;

        // ─── Events ─────────────────────────────────────────────────────────────────
        /// <summary>Fired when a wave begins. Passes the 1-based wave number.</summary>
        public event Action<int> OnWaveStarted;

        /// <summary>Fired when all enemies in a wave are defeated or reached the base. Passes the 1-based wave number.</summary>
        public event Action<int> OnWaveCompleted;

        /// <summary>Fired when the countdown to the next wave is ticking. Passes remaining seconds.</summary>
        public event Action<float> OnCountdownTick;

        /// <summary>Fired once all waves in the level have been completed.</summary>
        public event Action OnAllWavesCompleted;

        // ─── State ───────────────────────────────────────────────────────────────────
        private int currentWaveIndex = 0;
        private int activeEnemyCount = 0;
        private bool isRunning = false;
        private Coroutine waveCoroutine;

        // All enemies spawned in the current wave, so we can unsubscribe their events cleanly.
        private readonly List<Enemy> trackedEnemies = new List<Enemy>();

        /// <summary>Current 1-based wave number (0 means not started yet).</summary>
        public int CurrentWave => currentWaveIndex;

        /// <summary>Total number of waves in the loaded level.</summary>
        public int TotalWaves => levelData != null ? levelData.WaveCount : 0;

        /// <summary>Number of enemies still alive in the active wave.</summary>
        public int ActiveEnemyCount => activeEnemyCount;

        /// <summary>True while the WaveManager is actively running waves.</summary>
        public bool IsRunning => isRunning;

        // ─── Unity Messages ──────────────────────────────────────────────────────────

        private void Awake()
        {
            // Disable auto-start on the spawner during Awake so its Start() never
            // kicks off the standalone routine. Awake() always runs before Start(),
            // so this is guaranteed to execute first regardless of script order.
            if (enemySpawner == null)
                enemySpawner = FindFirstObjectByType<EnemySpawner>();

            if (enemySpawner != null)
                enemySpawner.AutoStartSpawning = false;
        }

        private void Start()
        {
            if (autoStart)
                StartWaves();
        }
        // ─── Public API ──────────────────────────────────────────────────────────────
        /// <summary>
        /// Starts the wave sequence from the beginning.
        /// Call this from a UI button or GameManager if <c>autoStart</c> is disabled.
        /// </summary>
        [ContextMenu("Start Waves")]
        public void StartWaves()
        {
            if (isRunning)
            {
                Debug.LogWarning("[WaveManager] Already running — ignoring StartWaves call.");
                return;
            }

            if (levelData == null)
            {
                Debug.LogError("[WaveManager] No LevelData assigned!");
                return;
            }

            if (levelData.WaveCount == 0)
            {
                Debug.LogWarning("[WaveManager] LevelData has no waves.");
                return;
            }

            currentWaveIndex = 0;
            isRunning = true;
            waveCoroutine = StartCoroutine(RunAllWaves());
        }

        /// <summary>
        /// Stops the wave sequence immediately. Active enemies remain alive.
        /// </summary>
        [ContextMenu("Stop Waves")]
        public void StopWaves()
        {
            if (waveCoroutine != null)
            {
                StopCoroutine(waveCoroutine);
                waveCoroutine = null;
            }

            UntrackAllEnemies();
            isRunning = false;
        }

        // ─── Core Coroutines ─────────────────────────────────────────────────────────
        private IEnumerator RunAllWaves()
        {
            while (currentWaveIndex < levelData.WaveCount)
            {
                WaveData wave = levelData.Waves[currentWaveIndex];

                // Use the level's global delay between waves.
                float delay = levelData.TimeBetweenWaves;

                if (delay > 0f)
                    yield return StartCoroutine(CountdownRoutine(delay));

                // Spawn the wave and wait until all enemies are cleared.
                yield return StartCoroutine(RunSingleWave(wave));

                currentWaveIndex++;
            }

            isRunning = false;
            waveCoroutine = null;
            OnAllWavesCompleted?.Invoke();
            Debug.Log("[WaveManager] All waves completed!");
        }

        private IEnumerator RunSingleWave(WaveData wave)
        {
            int waveNumber = currentWaveIndex + 1; // 1-based for readability
            activeEnemyCount = 0;
            trackedEnemies.Clear();

            Debug.Log($"[WaveManager] Starting Wave {waveNumber}/{levelData.WaveCount}");
            OnWaveStarted?.Invoke(waveNumber);

            // Spawn all enemy groups sequentially.
            foreach (WaveEnemyEntry entry in wave.EnemyEntries)
            {
                if (entry.enemyData == null)
                {
                    Debug.LogWarning("[WaveManager] WaveEnemyEntry has no EnemyData assigned — skipping entry.");
                    continue;
                }

                for (int i = 0; i < entry.count; i++)
                {
                    SpawnAndTrack(entry.enemyData);

                    // Wait between individual spawns within this group (except after the last one).
                    if (i < entry.count - 1 && entry.spawnInterval > 0f)
                        yield return new WaitForSeconds(entry.spawnInterval);
                }

                // Brief gap between different enemy groups in the same wave.
                yield return null;
            }

            // Wait until every tracked enemy has died or reached the base.
            yield return new WaitUntil(() => activeEnemyCount <= 0);

            Debug.Log($"[WaveManager] Wave {waveNumber} completed!");
            OnWaveCompleted?.Invoke(waveNumber);
        }

        private IEnumerator CountdownRoutine(float duration)
        {
            float remaining = duration;
            while (remaining > 0f)
            {
                OnCountdownTick?.Invoke(remaining);
                yield return new WaitForSeconds(1f);
                remaining -= 1f;
            }

            OnCountdownTick?.Invoke(0f);
        }

        // ─── Enemy Tracking ──────────────────────────────────────────────────────────
        private void SpawnAndTrack(EnemyData enemyData)
        {
            Enemy enemy = enemySpawner.SpawnSingleEnemy(enemyData);
            if (enemy == null) return;

            activeEnemyCount++;
            trackedEnemies.Add(enemy);

            enemy.OnEnemyDeath      += HandleEnemyRemoved;
            enemy.OnEnemyReachedGoal += HandleEnemyRemoved;
        }

        private void HandleEnemyRemoved(Enemy enemy)
        {
            enemy.OnEnemyDeath      -= HandleEnemyRemoved;
            enemy.OnEnemyReachedGoal -= HandleEnemyRemoved;

            activeEnemyCount = Mathf.Max(0, activeEnemyCount - 1);
        }

        private void UntrackAllEnemies()
        {
            foreach (Enemy enemy in trackedEnemies)
            {
                if (enemy == null) continue;
                enemy.OnEnemyDeath      -= HandleEnemyRemoved;
                enemy.OnEnemyReachedGoal -= HandleEnemyRemoved;
            }

            trackedEnemies.Clear();
            activeEnemyCount = 0;
        }

        private void OnDestroy()
        {
            UntrackAllEnemies();
        }
    }
}
