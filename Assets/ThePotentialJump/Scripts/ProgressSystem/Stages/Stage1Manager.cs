using System;
using ThePotentialJump.Utilities;
using UnityEngine;

namespace ThePotentialJump.ProgressSystem
{
    public class Stage1Manager : MonoSingleton<Stage1Manager>
    {
        void Start()
        {
            OnPlayBeginningAnimations(this, null);
        }

        public event EventHandler<AnimationPlayEventArgs> BeginningAnimations;
        [SerializeField] private AnimationClip beginningAnimationsClip;
        public void OnPlayBeginningAnimations(object o, EventArgs e)
        {
            BeginningAnimations?.Invoke(this, new AnimationPlayEventArgs
            {
                Clip = beginningAnimationsClip,
                OnFinishEventHandler = OnPlayDialogue1
            });
        }


        public event EventHandler Dialogue1Played;
        public void OnPlayDialogue1(object o, EventArgs e)
        {
            Dialogue1Played?.Invoke(this, new DialoguePlayEventArgs
            {
                Stage = 1,
                SequenceRange = new Vector2Int(1, 5),
                OnFinishEventHandler = OnPlayTheoryAnimations
            });
        }

        public event EventHandler<AnimationPlayEventArgs> TheoryAnimationPlayed;
        [SerializeField] private AnimationClip theoryAnimationsClip;
        public void OnPlayTheoryAnimations(object o, EventArgs e)
        {
            TheoryAnimationPlayed?.Invoke(this, new AnimationPlayEventArgs
            {
                Clip = theoryAnimationsClip,
                OnFinishEventHandler = OnPlayGame
            });
        }

        public event EventHandler<PlayGameEventArgs> GameplayBegan;
        public void OnPlayGame(object o, EventArgs e)
        {
            GameplayBegan?.Invoke(this, new PlayGameEventArgs
            {
                OnFinishEventHandler = OnPlayDialogue2
            });
        }

        public event EventHandler Dialogue2Played;
        public void OnPlayDialogue2(object o, EventArgs e)
        {
            Dialogue2Played?.Invoke(this, new DialoguePlayEventArgs
            {
                Stage = 1,
                SequenceRange = new Vector2Int(11, 13),
                OnFinishEventHandler = OnFinalizeStage1
            });
        }

        public event EventHandler<StageFinishedEventArgs> Stage1Finished;
        public void OnFinalizeStage1(object o, EventArgs e)
        {
            Stage1Finished?.Invoke(this, new StageFinishedEventArgs { WasSuccesful = true });
        }
    }
}