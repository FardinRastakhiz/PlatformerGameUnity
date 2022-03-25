using SimpleJSON;
using System;
using ThePotentialJump.Gameplay;
using TMPro;
using UnityEngine;

namespace ThePotentialJump.Menus
{

    public class TotalCarrotsViewer : TextPresenter
    {
        [SerializeField] private string id2;

        private string text1 = "Now you have";
        private string text2 = "s";
        private void Start()
        {
            if (EconomySystem.Instance == null)
            {
                Debug.LogError("EconomySystem.Instance cannot be null!");
                return;
            }
            EconomySystem.Instance.CurrencyChanged += OnCurrencyChanged;
        }

        private void OnCurrencyChanged(object sender, TotalCurrencyChangedEventArgs e)
        {
            textComponent.text = $"{text1} {e.TotalCurrency} {EconomySystem.Instance.CurrencyName.ToLower()}{(e.TotalCurrency > 1 ? text2 : "")}";
        }

        public override void AlterLanguage(JSONNode langNode)
        {
            text1 = langNode?[id];
            text2 = langNode?[id2];
        }
    }

}