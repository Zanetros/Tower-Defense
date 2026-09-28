using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Data
{
    /// <summary>
    /// ScriptableObject that defines a complete level: its ordered list of waves
    /// and the default wait time between each wave.
    /// </summary>
    [CreateAssetMenu(fileName = "NewLevelData", menuName = "Tower Defense/Level Data")]
    public class LevelData : ScriptableObject
    {
        [Tooltip("Ordered list of waves for this level.")]
        [SerializeField] private List<WaveData> waves = new List<WaveData>();

        [Tooltip("Default time in seconds the player has between waves to prepare.")]
        [Min(0f)]
        [SerializeField] private float timeBetweenWaves = 10f;

        /// <summary>Ordered collection of waves for this level.</summary>
        public IReadOnlyList<WaveData> Waves => waves;

        /// <summary>Number of waves in this level.</summary>
        public int WaveCount => waves.Count;

        /// <summary>Default wait time between waves (can be overridden per WaveData).</summary>
        public float TimeBetweenWaves => timeBetweenWaves;
    }
}
