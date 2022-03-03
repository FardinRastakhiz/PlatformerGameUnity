using System;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{ 
    public class ReplaceObjectEventArgs : EventArgs
    {
        public Vector3 Position { get; set; }
        public Transform Parent { get; set; }
        public GameObject ReplacePrefab { get; set; }
    }

    public class ReplaceCageEventArgs : ReplaceObjectEventArgs
    {
        public bool isBroke { get; set; }
    }

}