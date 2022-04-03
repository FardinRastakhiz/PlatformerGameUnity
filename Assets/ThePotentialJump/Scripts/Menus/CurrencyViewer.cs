using System;
using ThePotentialJump.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ThePotentialJump.Menus
{
    public class CurrencyViewer : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI sliderText;
        [SerializeField] private int MaxCurrency = 100;
        [SerializeField] private int CollectedCurrency = 0;
        private void Awake()
        {
            if (slider == null)
                slider = GetComponentInChildren<Slider>();
            if (sliderText == null)
                sliderText = slider.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        }
        
        private void Start()
        {
            UpdateSlider();
            SetupParameters();
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            SetupParameters();
        }

        private void SetupParameters()
        {
            if (EconomySystem.Instance == null)
            {
                Debug.LogError("EconomySystem cannot be null!");
                return;
            }
            EconomySystem.Instance.CurrencyChanged += OnTotalCurrencyChanged;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }


        public void OnTotalCurrencyChanged(object o, TotalCurrencyChangedEventArgs e)
        {
            CollectedCurrency = e.TotalCurrency;
            UpdateSlider();
        }

        public void SetCollectedCarrots(int collectedCarrots)
        {
            CollectedCurrency = collectedCarrots;
            UpdateSlider();
        }

        private void UpdateSlider()
        {
            if (slider == null)
            {
                //Debug.LogError("slider cannot be null!");
                return;
            }
            if (sliderText == null)
            {
                Debug.LogError("sliderText cannot be null!");
                return;
            }
            sliderText.text = CollectedCurrency + " / " + MaxCurrency;
            slider.value = CollectedCurrency / (MaxCurrency * 1.0f);
        }
    }
}