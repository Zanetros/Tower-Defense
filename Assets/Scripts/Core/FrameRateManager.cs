using UnityEngine;

namespace TowerDefense.Core
{
    /// <summary>
    /// Manages target frame rate and VSync settings for optimized rendering and power consumption.
    /// Provides modular runtime methods for future UI/Settings menus.
    /// </summary>
    [DisallowMultipleComponent]
    public class FrameRateManager : MonoBehaviour
    {
        public static FrameRateManager Instance { get; private set; }

        [Header("Frame Rate Settings")]
        [Tooltip("Target framerate. -1 means unlimited.")]
        [SerializeField] private int targetFrameRate = 60;

        [Header("VSync Settings")]
        [Tooltip("If enabled, VSync takes priority over targetFrameRate.")]
        [SerializeField] private bool enableVSync = false;

        [Header("Persistence")]
        [Tooltip("Keep this manager across scene transitions?")]
        [SerializeField] private bool dontDestroyOnLoad = true;

        public int CurrentTargetFrameRate => targetFrameRate;
        public bool IsVSyncEnabled => enableVSync;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                if (dontDestroyOnLoad)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            ApplySettings();
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                ApplySettings();
            }
        }

        /// <summary>
        /// Applies current VSync and targetFrameRate settings.
        /// Note: QualitySettings.vSyncCount must be 0 for Application.targetFrameRate to take effect.
        /// </summary>
        public void ApplySettings()
        {
            if (enableVSync)
            {
                QualitySettings.vSyncCount = 1; // Sync with monitor refresh rate
            }
            else
            {
                QualitySettings.vSyncCount = 0; // Disable VSync to allow custom targetFrameRate
                Application.targetFrameRate = targetFrameRate;
            }

            Debug.Log($"[FrameRateManager] Target FPS: {targetFrameRate} | VSync: {enableVSync} (vSyncCount: {QualitySettings.vSyncCount})");
        }

        /// <summary>
        /// Modular method to change target framerate at runtime (e.g., from Options Menu).
        /// </summary>
        public void SetTargetFrameRate(int fps)
        {
            targetFrameRate = fps;
            if (!enableVSync)
            {
                Application.targetFrameRate = targetFrameRate;
            }
        }

        /// <summary>
        /// Modular method to enable or disable VSync at runtime.
        /// </summary>
        public void SetVSync(bool enabled)
        {
            enableVSync = enabled;
            ApplySettings();
        }
    }
}
