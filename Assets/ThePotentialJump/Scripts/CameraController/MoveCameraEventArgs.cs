using System;
using UnityEngine;

namespace ThePotentialJump.Cameras
{
    public class MoveCameraEventArgs : EventArgs
    {
        public Vector3 TargetPosition { get; set; }
    }
}