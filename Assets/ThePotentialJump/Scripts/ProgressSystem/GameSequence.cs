using System;
using UnityEngine;

namespace ThePotentialJump.ProgressSystem
{
    public class GameSequence : Utilities.Singleton<GameSequence>
    {
        [SerializeField] private Stage[] stages;
        protected override void Awake()
        {
            destroyOnLoad = true;
            base.Awake();
            for (int i = 0; i < stages.Length; i++)
                stages[i].SetUpStage();
        }


        public event EventHandler<StageEventArgs> NextStageActivated;
        private int activeStage = -1;
        public void GoNextStage()
        {
            StopCurrentStage();
            activeStage++;
            stages[activeStage].IsActive = true;
            NextStageActivated?.Invoke(this, new StageEventArgs { Stage = stages[activeStage]});
        }


        public event EventHandler<StageEventArgs> StageStopped;
        public void StopCurrentStage()
        {
            if (!stages[activeStage].IsActive) return;
            stages[activeStage].IsActive = false;
            stages[activeStage].StopAllSequences();
            StageStopped?.Invoke(this, new StageEventArgs { Stage = stages[activeStage] });
        }

    }
}
