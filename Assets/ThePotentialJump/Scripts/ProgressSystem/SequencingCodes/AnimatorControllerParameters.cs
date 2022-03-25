using System;
using ThePotentialJump.EditorUtilities;
using UnityEngine;
using UnityEngine.Events;

namespace ThePotentialJump.ProgressSystem
{
    public class AnimatorControllerParameters : MonoBehaviour
    {
        [SerializeField] private AnimatorControllerParameter[] parameters;
        public AnimatorControllerParameter[] Parameters { get => parameters; set => parameters = value; }


        internal void SetParameterFor(Animator animator)
        {
            if (Parameters == null) return;
            for (int i = 0; i < Parameters.Length; i++)
            {
                Parameters[i].SetParameterFor(animator);
            }
        }

        public UnityEvent Beginned;
        public UnityEvent Paused;
        public UnityEvent Finished;
        AnimationClipCompleted animationClipCompleted;
        public void Play(Animator animator)
        {
            animationClipCompleted = animator.GetBehaviour<AnimationClipCompleted>();
            if(animationClipCompleted!=null)
            {
                animationClipCompleted.Completed -= Finish;
                animationClipCompleted.Completed += Finish;
            }
            // animator.GetCurrentAnimatorClipInfo
            Beginned?.Invoke();
        }

        public void Pause()
        {
            Paused?.Invoke();
        }

        public void Finish(object o, EventArgs e)
        {
            Debug.Log(this.name);
            Debug.Log("AnimationExitted");
            Finished?.Invoke();
        }
    }

}