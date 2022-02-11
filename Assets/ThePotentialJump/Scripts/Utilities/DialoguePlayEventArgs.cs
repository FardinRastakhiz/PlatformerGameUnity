using UnityEngine;
using System;

namespace ThePotentialJump.Utilities
{
    public class DialoguePlayEventArgs : GameSequenceEventArgs
    {
        public int Stage { get; set; }
        public Vector2Int SequenceRange { get; set; }
    }

}
