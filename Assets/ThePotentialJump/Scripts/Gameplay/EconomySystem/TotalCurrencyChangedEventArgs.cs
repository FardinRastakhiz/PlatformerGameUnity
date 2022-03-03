using System;

namespace ThePotentialJump.Gameplay
{
    public class TotalCurrencyChangedEventArgs : EventArgs
    {
        public int TotalCurrency { get; set; }
        public int CollectedOnCurrentScene { get; internal set; }
    }

}