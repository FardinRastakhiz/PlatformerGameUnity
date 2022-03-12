using System;
using ThePotentialJump.Utilities;

namespace ThePotentialJump.ProgressSystem
{
    public class StageManager : MonoSingleton<StageManager>
    {
        public event EventHandler StageCompleted;
        public event EventHandler StageClosed;

        protected override void Awake()
        {
            destroyOnLoad = true;
            base.Awake();
        }
    }
}