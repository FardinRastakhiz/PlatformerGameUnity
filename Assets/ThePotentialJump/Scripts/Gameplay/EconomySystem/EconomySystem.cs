using System;
using System.Collections;
using ThePotentialJump.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThePotentialJump.Gameplay
{
    public class EconomySystem : MonoSingleton<EconomySystem>
    {
        private int totalCurrency = 0;
        private int temporaryTotalCurrency = 0;
        [SerializeField] private string currencyName = "Carrot";
        [SerializeField] private Sprite currencyIcon;
        public int TotalCurrency { get => temporaryTotalCurrency; private set => temporaryTotalCurrency = value; }
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
            SetupPassOrFailHandlers();
            SetupPauseActions();

            CollectedOnCurrentScene = 0;
            if (totalCurrencyChanged == null) totalCurrencyChanged = new TotalCurrencyChangedEventArgs();
            temporaryTotalCurrency = totalCurrency;
            totalCurrencyChanged.TotalCurrency = temporaryTotalCurrency;
            totalCurrencyChanged.CollectedOnCurrentScene = CollectedOnCurrentScene;
            CurrencyChanged?.Invoke(this, totalCurrencyChanged);
        }

        public void Deposit(int amount)
        {
            Debug.Log($"Depo amount: {amount},     Maximum capacity: {MaximumCapacity}");
            CollectedOnCurrentScene += amount;
            Debug.Log($"CollectedOnCurrentScene: {CollectedOnCurrentScene}");
            temporaryTotalCurrency += amount;
            temporaryTotalCurrency = temporaryTotalCurrency > MaximumCapacity ? MaximumCapacity : temporaryTotalCurrency;
            totalCurrencyChanged.TotalCurrency = temporaryTotalCurrency;
            totalCurrencyChanged.CollectedOnCurrentScene = CollectedOnCurrentScene;
            CurrencyChanged?.Invoke(this, totalCurrencyChanged);
        }

        public void Withdraw(int amount)
        {
            Debug.Log($"Depo amount: {amount}");
            if (temporaryTotalCurrency - amount < 0) return;
            temporaryTotalCurrency -= amount;
            if (CollectedOnCurrentScene - amount >= 0) CollectedOnCurrentScene -= amount;
            totalCurrencyChanged.TotalCurrency = temporaryTotalCurrency;
            totalCurrencyChanged.CollectedOnCurrentScene = CollectedOnCurrentScene;
            CurrencyChanged?.Invoke(this, totalCurrencyChanged); 
        }

        public void LoadSavedCurrency(int amount)
        {
            Deposit(amount);
            totalCurrency = temporaryTotalCurrency;
        }

        private void SetupPassOrFailHandlers()
        {
            var passOrFail = FindObjectOfType<PassOrFailUIContrller>();
            if (passOrFail == null) return;
            passOrFail.ViewedPassUI.AddListener(OnStagePassed);
            passOrFail.ViewedFailUI.AddListener(OnStageLost);
        }

        public void SetupPauseActions()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.Restarted.AddListener(OnStageLost);
            GameManager.Instance.GoneToMainMenu.AddListener(OnStageLost);
        }

        public void OnStagePassed()
        {
            totalCurrency = temporaryTotalCurrency;
        }

        public void OnStageLost()
        {

        }
    }

}