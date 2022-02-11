using System;
using UnityEngine;

namespace ThePotentialJump.ProgressSystem
{
    public class GameSequence : Utilities.MonoSingleton<GameSequence>
    {
        [SerializeField] private Stage[] stages;
        public Stage ActiveStage
        {
            get
            {
                if (activeStage < 0 || activeStage >= stages.Length) return null;
                return stages[activeStage];
            }
        }
        protected override void Awake()
        {
            destroyOnLoad = true;
            base.Awake();
            for (int i = 0; i < stages.Length; i++)
                stages[i].SetUpStage();
        }


        public event EventHandler<StageEventArgs> NextStageActivated;
        private int activeStage = -1;

        public void ActivateStage(int stage)
        {

            StopCurrentStage();
            activeStage = stage;
            stages[activeStage].IsActive = true;
            NextStageActivated?.Invoke(this, new StageEventArgs { Stage = stages[activeStage] });
        }
        public void GoNextStage()
        {
            StopCurrentStage();
            activeStage++;
            stages[activeStage].IsActive = true;
            NextStageActivated?.Invoke(this, new StageEventArgs { Stage = stages[activeStage] });
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
