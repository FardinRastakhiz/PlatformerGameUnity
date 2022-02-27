using System;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{ 
    public class PlaceObjectEventArgs : EventArgs
    {
        public Vector3 Position { get; set; }
        public Transform Parent { get; set; }
        public GameObject ReplacePrefab { get; set; }
    }

}