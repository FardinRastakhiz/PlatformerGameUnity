using System;
using UnityEngine;

namespace ThePotentialJump.ProgressSystem
{
    [Serializable]
    public class SeqPart
    {
        [SerializeField]
        private string name;
        public string Name { get => name; set => name = value; }
        public event EventHandler OnSequencePlayed;
        public event EventHandler OnSequenceStopped;
        private bool isRunning = false;

        public virtual void Play()
        {
            if (isRunning) return;
            isRunning = true;
            OnSequencePlayed?.Invoke(this, null);
        }
        public virtual void Stop()
        {
            if (!isRunning) return;
            isRunning = false;
            OnSequenceStopped?.Invoke(this, null);
        }
    }
}
