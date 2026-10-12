using System;

namespace TowerDefense.ObstacleSystem
{
    /// <summary>
    /// Modular interface decoupling the obstacle system from any specific economy manager.
    /// If your project already has an existing coin/currency system, implement this interface.
    /// </summary>
    public interface ICurrencyHandler
    {
        /// <summary>
        /// Current currency amount available to the player.
        /// </summary>
        int CurrentCurrency { get; }

        /// <summary>
        /// Checks whether the player has at least the required amount of currency.
        /// </summary>
        bool HasCurrency(int amount);

        /// <summary>
        /// Attempts to deduct currency. Returns true if successful.
        /// </summary>
        bool TrySpendCurrency(int amount);

        /// <summary>
        /// Awards currency to the player.
        /// </summary>
        void AddCurrency(int amount);

        /// <summary>
        /// Event triggered when the player's currency balance changes.
        /// </summary>
        event Action<int> OnCurrencyChanged;
    }
}
