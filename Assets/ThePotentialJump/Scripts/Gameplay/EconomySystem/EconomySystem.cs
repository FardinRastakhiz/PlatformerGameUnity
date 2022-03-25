using System;
using System.Collections;
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
        public int CollectedOnCurrentScene { get; private set; } = 0;
        public int MaximumCapacity { get; internal set; } = 100;

        private void Start()
        {
            StartCoroutine(SetupWithDelay());
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            StartCoroutine(SetupWithDelay());
        }

        IEnumerator SetupWithDelay()
        {
            yield return null;
            CollectedOnCurrentScene = 0;
            if (totalCurrencyChanged == null) totalCurrencyChanged = new TotalCurrencyChangedEventArgs();
            totalCurrencyChanged.TotalCurrency = TotalCurrency;
            totalCurrencyChanged.CollectedOnCurrentScene = CollectedOnCurrentScene;
            CurrencyChanged?.Invoke(this, totalCurrencyChanged);
        }

        public void Deposit(int amount)
        {
            Debug.Log($"Depo amount: {amount},     Maximum capacity: {MaximumCapacity}");
            CollectedOnCurrentScene += amount;
            Debug.Log($"CollectedOnCurrentScene: {CollectedOnCurrentScene}");
            TotalCurrency += amount;
            TotalCurrency = TotalCurrency > MaximumCapacity ? MaximumCapacity : TotalCurrency;
            totalCurrencyChanged.TotalCurrency = TotalCurrency;
            totalCurrencyChanged.CollectedOnCurrentScene = CollectedOnCurrentScene;
            CurrencyChanged?.Invoke(this, totalCurrencyChanged);
        }

        public void Withdraw(int amount)
        {
            Debug.Log($"Depo amount: {amount}");
            if (TotalCurrency - amount < 0) return;
            TotalCurrency -= amount;
            if (CollectedOnCurrentScene - amount >= 0) CollectedOnCurrentScene -= amount;
            totalCurrencyChanged.TotalCurrency = TotalCurrency;
            totalCurrencyChanged.CollectedOnCurrentScene = CollectedOnCurrentScene;
            CurrencyChanged?.Invoke(this, totalCurrencyChanged); 
        }
    }

}