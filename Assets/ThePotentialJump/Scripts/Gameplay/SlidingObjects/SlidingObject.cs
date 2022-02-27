using System;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public abstract class SlidingObject: MonoBehaviour
    {
        public event EventHandler Destroyed;

        private void OnDestroy()
        {
            Destroyed?.Invoke(this, null);
        }
    }

}