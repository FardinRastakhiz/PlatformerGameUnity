using System;

namespace ThePotentialJump.Gameplay
{
    public class NewHeightEventArgs : EventArgs
    {
        public int JumpedHeight { get; set; }
        public int MaxHeightApproached{ get; set; }
    }

}