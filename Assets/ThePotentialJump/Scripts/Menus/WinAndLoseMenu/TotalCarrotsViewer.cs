using System;
using ThePotentialJump.Gameplay;
using TMPro;
using UnityEngine;

namespace ThePotentialJump.Menus
{

    public class TotalCarrotsViewer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMeshPro;
        private void Start()
        {
            EconomySystem.Instance.CurrencyChanged += OnCurrencyChanged;
        }

        private void OnCurrencyChanged(object sender, TotalCurrencyChangedEventArgs e)
        {
            textMeshPro.text = $"Now you have {e.TotalCurrency} {EconomySystem.Instance.CurrencyName.ToLower()}{(e.TotalCurrency > 1 ? "s" : "")}";
        }
    }

}