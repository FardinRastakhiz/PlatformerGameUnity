using UnityEngine;
using System;

namespace ThePotentialJump.Utilities
{
    public class GameSequenceEventArgs : EventArgs
    {
        public EventHandler OnFinishEventHandler { get; set; }
    }

    public class AnimationPlayEventArgs : GameSequenceEventArgs
    {
        public AnimationClip Clip { get; set; }
    }

    public class PlayGameEventArgs : GameSequenceEventArgs
    {
        public EventHandler OnFinishEventHandler { get; set; }
    }

}
