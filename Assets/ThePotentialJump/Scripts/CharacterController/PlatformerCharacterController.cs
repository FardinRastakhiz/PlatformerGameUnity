using System;
using UnityEngine;


namespace ThePotentialJump.CharacterController
{


    public class PlatformerCharacterController : Utilities.MonoSingleton<PlatformerCharacterController>
    {
        [SerializeField] private Rigidbody2D rigidBody;

        [SerializeField] private PlatformerCharacterMovement movement;

        [SerializeField] private Animator animator;

        AnimatorOverrideController animatorOverrideController;
        protected override void Awake()
        {
            movement.SetupParameters(this);
            destroyOnLoad = true;
            base.Awake();
            if (rigidBody == null) rigidBody = GetComponent<Rigidbody2D>();
            animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = animatorOverrideController;
        }

        private void Start()
        {
            OnEnable();
        }

        private void OnEnable()
        {
            movement.OnEnable();
        }

        private void OnDisable()
        {
            movement.OnDisable();
        }


        public void PlayCutsceneAnimation(AnimationClip animationClip, EventHandler animationExitHandler = null)
        {
            animatorOverrideController["Watching"] = animationClip;
            animator.SetBool("Watching", true);
            var cutsceneAnimator = animator.GetBehaviour<AnimationClipExit>();
            if (animationExitHandler != null)
                cutsceneAnimator.OnAnimationExit += animationExitHandler;
        }

        public void StopCutsceneAnimation()
        {
            animator.SetBool("Watching", false);
        }
    }


}