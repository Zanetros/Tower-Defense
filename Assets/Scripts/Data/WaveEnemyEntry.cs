using System;
using UnityEngine;

namespace TowerDefense.Data
{
    /// <summary>
    /// Represents a single group of enemies of one type within a wave.
    /// Multiple entries per wave allow mixing different enemy types.
    /// </summary>
    [Serializable]
    public class WaveEnemyEntry
    {
        [Tooltip("Which enemy type to spawn in this group.")]
        public EnemyData enemyData;

        [Tooltip("How many enemies of this type to spawn.")]
        [Min(1)]
        public int count = 5;

        [Tooltip("Time in seconds between each individual spawn within this group.")]
        [Min(0f)]
        public float spawnInterval = 1f;
    }
}
