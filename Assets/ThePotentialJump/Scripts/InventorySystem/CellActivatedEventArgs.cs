using System;

namespace ThePotentialJump.Inventory
{
    public class CellActivatedEventArgs : EventArgs
    {
        public InventoryCell ActivatedCell { get; set; }
    }
}