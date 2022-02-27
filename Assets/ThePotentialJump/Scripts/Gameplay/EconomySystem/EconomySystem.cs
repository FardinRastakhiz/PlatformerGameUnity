using ThePotentialJump.Utilities;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{

    public class EconomySystem : MonoSingleton<EconomySystem>
    {
        [SerializeField] private int totalCurrency;
        [SerializeField] private string currencyName = "Carrot";
        [SerializeField] private Sprite currencyIcon;
        public int TotalCurrency { get => totalCurrency; private set => totalCurrency = value; }
        public string CurrencyName { get => currencyName; }
        public Sprite CurrencyIcon { get => currencyIcon; }

        public void Deposit(int amount)
        {
            TotalCurrency += amount;
        }

        public bool Withdraw(int amount)
        {
            if (TotalCurrency - amount < 0) return false;
            TotalCurrency -= amount;
            return true;
        }
    }

}