using System;
using UnityEngine;

namespace TowerDefense.ObstacleSystem
{
    /// <summary>
    /// Default currency manager implementation in case the project does not already have one.
    /// Operates as a singleton for easy global access while adhering to ICurrencyHandler.
    /// </summary>
    public class SimpleCurrencyManager : MonoBehaviour, ICurrencyHandler
    {
        public static SimpleCurrencyManager Instance { get; private set; }

        [Header("Initial Settings")]
        [SerializeField] private int startingCurrency = 100;

        private int _currentCurrency;

        public int CurrentCurrency => _currentCurrency;

        public event Action<int> OnCurrencyChanged;

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

            _currentCurrency = startingCurrency;
        }

        public bool HasCurrency(int amount)
        {
            return _currentCurrency >= amount;
        }

        public bool TrySpendCurrency(int amount)
        {
            if (!HasCurrency(amount))
                return false;

            _currentCurrency -= amount;
            OnCurrencyChanged?.Invoke(_currentCurrency);
            return true;
        }

        public void AddCurrency(int amount)
        {
            if (amount <= 0) return;

            _currentCurrency += amount;
            OnCurrencyChanged?.Invoke(_currentCurrency);
        }
    }
}
