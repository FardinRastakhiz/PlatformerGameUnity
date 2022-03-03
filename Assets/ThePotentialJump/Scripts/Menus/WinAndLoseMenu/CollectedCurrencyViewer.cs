using ThePotentialJump.Gameplay;
using TMPro;
using UnityEngine;

namespace ThePotentialJump.Menus
{
    public class CollectedCurrencyViewer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMeshPro;
        private void Start()
        {
            EconomySystem.Instance.CurrencyChanged += OnCurrencyChanged;
        }

        private void OnCurrencyChanged(object sender, TotalCurrencyChangedEventArgs e)
        {
            textMeshPro.text = $"+{e.CollectedOnCurrentScene} {EconomySystem.Instance.CurrencyName.ToLower()}{(e.CollectedOnCurrentScene > 1 ? "s":"")}";
        }
    }

}