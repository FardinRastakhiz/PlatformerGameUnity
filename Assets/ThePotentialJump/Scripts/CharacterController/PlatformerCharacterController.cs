using System;
using System.Collections;
using ThePotentialJump.Gameplay;
using ThePotentialJump.Inputs;
using ThePotentialJump.Utilities;
using UnityEngine;


namespace ThePotentialJump.CharacterController
{


    public class PlatformerCharacterController : MonoSingleton<PlatformerCharacterController>
    {
        [SerializeField] private Rigidbody2D bunnyRigidBody;

        [SerializeField] private PlatformerCharacterMovement movement;

        [SerializeField] private Animator animator;

        [SerializeField] private EnergyUIController energyUIController;
        [SerializeField] private float maxJumpHeight = 5.0f;
        public float MaxEnergy => GetEnergy(maxJumpHeight);
        AnimatorOverrideController animatorOverrideController;
        private WaitForSeconds waitFixedDeltaTime;
        protected override void Awake()
        {
            energyUIController.SetupSlider(transform.position, MaxEnergy);
            energyUIController.ValueFinalized += OnEnergyValueFinalized;
            energyUIController.ValueChanged += OnEnergyValueChanged;

            HoldedEnergyChanged += OnEnergyValueChanged;

            waitFixedDeltaTime = new WaitForSeconds(Time.fixedDeltaTime);

            InputController.Instance.PressSpace += OnPressSpace;
            InputController.Instance.ReleaseSpace += OnReleaseSpace;
            movement.SetupParameters(this);
            destroyOnLoad = true;
            base.Awake();
            if (bunnyRigidBody == null) bunnyRigidBody = GetComponent<Rigidbody2D>();
            animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = animatorOverrideController;
        }

        private void Start()
        {
            OnEnable();
            StartCoroutine(UpdateParameters());
        }
        IEnumerator UpdateParameters()
        {
            while (true)
            {
                energyUIController.SetPosition(transform.position);
                yield return null;
            }
        }

        private void OnEnable()
        {
            movement.OnEnable();
        }

        private void OnDisable()
        {
            movement.OnDisable();
        }

        private Coroutine jumpCoroutine;
        private Coroutine holdEnergyCoroutine;
        private void OnPressSpace(object o, EventArgs e)
        {
            if (movement.IsJumping) return;
            holdedEnergy = 0.0f;
            holdEnergyCoroutine = StartCoroutine(HoldJumpEnergy());
        }
        private void OnReleaseSpace(object o, EventArgs e)
        {
            if (movement.IsJumping) return;

            if (jumpCoroutine != null)
                StopCoroutine(jumpCoroutine);
            if (holdEnergyCoroutine != null)
                StopCoroutine(holdEnergyCoroutine);
            jumpCoroutine = StartCoroutine(movement.Jump(holdedEnergy));
        }

        private void OnEnergyValueChanged(object sender, HoldEnergyEventArgs e)
        {
            if (movement.IsJumping) return;
            holdedEnergy = e.Value;
            // ?? 
        }

        private void OnEnergyValueFinalized(object sender, HoldEnergyEventArgs e)
        {
            if (movement.IsJumping) return;
            holdedEnergy = e.Value;
            jumpCoroutine = StartCoroutine(movement.Jump(holdedEnergy));
        }

        public float GetEnergy(float maxHeight)
        {
            return bunnyRigidBody.mass * -Physics2D.gravity.y * maxHeight;
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



        [Space]
        [Header("Hold energy parameters")]
        [SerializeField] private float saveEnergyRate = 10.0f;
        public event EventHandler<HoldEnergyEventArgs> HoldedEnergyChanged;
        private HoldEnergyEventArgs holdEnergyEventArgs = new HoldEnergyEventArgs();
        private float holdedEnergy = 0;
        IEnumerator HoldJumpEnergy()
        {
            float maxEnergy = MaxEnergy;
            float addDirection = 1.0f;
            while (!movement.IsJumping)
            {
                if (holdedEnergy > maxEnergy && addDirection > 0)
                {
                    addDirection = -1;
                    holdedEnergy = maxEnergy;
                }
                else if (holdedEnergy < 0 && addDirection < 0)
                {
                    addDirection = 1;
                    holdedEnergy = 0;
                }
                Debug.Log($"{holdedEnergy} : {addDirection}");
                holdedEnergy += Time.fixedDeltaTime * saveEnergyRate * addDirection;
                holdEnergyEventArgs.Value = holdedEnergy;
                energyUIController.SetUIValues(holdedEnergy);
                HoldedEnergyChanged?.Invoke(this, holdEnergyEventArgs);
                yield return waitFixedDeltaTime;
            }
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("OnCollisionEnter: " + movement.IsJumping);
            if (movement.IsJumping && collision.gameObject.tag == "Ground")
                movement.IsJumping = false;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            Debug.Log("OnCollisionExit2D: " + movement.IsJumping);
            if (!movement.IsJumping && collision.gameObject.tag == "Ground")
                movement.IsJumping = true;
        }
    }


}