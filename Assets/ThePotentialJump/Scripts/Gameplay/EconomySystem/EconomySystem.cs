using System;
using ThePotentialJump.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThePotentialJump.Gameplay
{
    public class EconomySystem : MonoSingleton<EconomySystem>
    {
        [SerializeField] private int totalCurrency = 0;
        [SerializeField] private string currencyName = "Carrot";
        [SerializeField] private Sprite currencyIcon;
        public int TotalCurrency { get => totalCurrency; private set => totalCurrency = value; }
        public string CurrencyName { get => currencyName; }
        public Sprite CurrencyIcon { get => currencyIcon; }

        public event EventHandler<TotalCurrencyChangedEventArgs> CurrencyChanged;
        private TotalCurrencyChangedEventArgs totalCurrencyChanged = new TotalCurrencyChangedEventArgs();
        public int CollectedOnCurrentScene { get; set; }
        public int MaximumCapacity { get; internal set; } = 100;

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            CollectedOnCurrentScene = 0;
            totalCurrencyChanged.TotalCurrency = TotalCurrency;
            totalCurrencyChanged.CollectedOnCurrentScene = CollectedOnCurrentScene;
            CurrencyChanged?.Invoke(this, totalCurrencyChanged);
        }

        public void Deposit(int amount)
        {
            CollectedOnCurrentScene += amount;
            TotalCurrency += amount;
            TotalCurrency = TotalCurrency > MaximumCapacity ? MaximumCapacity : TotalCurrency;
            totalCurrencyChanged.TotalCurrency = TotalCurrency;
            totalCurrencyChanged.CollectedOnCurrentScene = CollectedOnCurrentScene;
            CurrencyChanged?.Invoke(this, totalCurrencyChanged);
        }

        public bool Withdraw(int amount)
        {
            if (TotalCurrency - amount < 0) return false;
            TotalCurrency -= amount;
            if (CollectedOnCurrentScene - amount >= 0) CollectedOnCurrentScene -= amount;
            totalCurrencyChanged.TotalCurrency = TotalCurrency;
            totalCurrencyChanged.CollectedOnCurrentScene = CollectedOnCurrentScene;
            CurrencyChanged?.Invoke(this, totalCurrencyChanged);
            return true;
        }
    }

}