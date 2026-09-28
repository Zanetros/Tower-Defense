using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Data
{
    /// <summary>
    /// ScriptableObject that defines a single wave: an ordered list of enemy groups.
    /// The delay between waves is controlled globally by LevelData.
    /// </summary>
    [CreateAssetMenu(fileName = "NewWaveData", menuName = "Tower Defense/Wave Data")]
    public class WaveData : ScriptableObject
    {
        [Tooltip("All groups of enemies that compose this wave. Each entry can be a different enemy type.")]
        [SerializeField] private List<WaveEnemyEntry> enemyEntries = new List<WaveEnemyEntry>();

        /// <summary>All enemy groups that make up this wave.</summary>
        public IReadOnlyList<WaveEnemyEntry> EnemyEntries => enemyEntries;

        /// <summary>
        /// Total number of enemies across all entries in this wave.
        /// </summary>
        public int TotalEnemyCount
        {
            get
            {
                int total = 0;
                foreach (WaveEnemyEntry entry in enemyEntries)
                    total += entry.count;
                return total;
            }
        }
    }
}
