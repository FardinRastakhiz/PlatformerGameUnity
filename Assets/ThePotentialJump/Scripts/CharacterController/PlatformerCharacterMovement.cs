using System;
using System.Collections;
using ThePotentialJump.Inputs;
using ThePotentialJump.Utilities;
using UnityEngine;


namespace ThePotentialJump.CharacterController
{

    [Serializable]
    public class PlatformerCharacterMovement: Utilities.Singleton<PlatformerCharacterMovement>
    {
        protected PlatformerCharacterMovement(): base() { }

        private PlatformerCharacterController controller;
        private Transform transform;
        private Rigidbody2D rigidBody;
        private Coroutine updateCoroutine;
        public void SetupParameters(PlatformerCharacterController controller, Animator animator)
        {
            this.controller = controller;
            transform = controller.transform;
            rigidBody = controller.GetComponent<Rigidbody2D>();
            rigidBody.drag = 0;
            waitFixedDeltaTime = new WaitForSeconds(Time.fixedDeltaTime);
            this.animator = animator;
            if (updateCoroutine != null) controller.StopCoroutine(updateCoroutine);
            updateCoroutine = controller.StartCoroutine(UpdateMovement());
    }
    IEnumerator UpdateMovement()
        {
            while (true)
            {
                animator.SetFloat("Speed", Mathf.Abs(rigidBody.velocity.x));
                yield return null;
            }
        }

        public void OnEnable()
        {
            if (InputController.Instance == null) return;
            // InputController.Instance.PressSpace += OnPressSpace;
            InputController.Instance.PressLeft += OnPressLeft;
            InputController.Instance.PressRight += OnPressRight;

            // InputController.Instance.ReleaseSpace += OnReleaseSpace;
            InputController.Instance.ReleaseLeft += OnReleaseLeft;
            InputController.Instance.ReleaseRight += OnReleaseRight;
        }

        public void OnDisable()
        {
            if (InputController.Instance == null) return;
            // InputController.Instance.PressSpace -= OnPressSpace;
            InputController.Instance.PressLeft -= OnPressLeft;
            InputController.Instance.PressRight -= OnPressRight;

            // InputController.Instance.ReleaseSpace -= OnReleaseSpace;
            InputController.Instance.ReleaseLeft -= OnReleaseLeft;
            InputController.Instance.ReleaseRight -= OnReleaseRight;
        }


        private Coroutine goLeftCoroutine;
        private Coroutine goRightCoroutine;
        private Coroutine jumpCoroutine;
        public void OnPressLeft(object o, EventArgs e)
        {
            if (goLeftCoroutine != null)
                controller.StopCoroutine(goLeftCoroutine);
            goLeftCoroutine = controller.StartCoroutine(GoLeft());
        }

        public void OnPressRight(object o, EventArgs e)
        {
            if (goRightCoroutine != null)
                controller.StopCoroutine(goRightCoroutine);
            goRightCoroutine = controller.StartCoroutine(GoRight());
        }
        public void OnReleaseLeft(object o, EventArgs e)
        {
            if (goLeftCoroutine != null)
                controller.StopCoroutine(goLeftCoroutine);
        }

        public void OnReleaseRight(object o, EventArgs e)
        {
            if (goRightCoroutine != null)
                controller.StopCoroutine(goRightCoroutine);
        }

        //private void OnPressSpace(object sender, EventArgs e)
        //{
        //    if (IsJumping) return;
        //    jumpCoroutine = controller.StartCoroutine(HoldJumpEnergy());
        //}

        //public void OnReleaseSpace(object o, EventArgs e)
        //{
        //    if (jumpCoroutine != null)
        //        controller.StopCoroutine(jumpCoroutine);
        //    jumpCoroutine = controller.StartCoroutine(Jump());
        //}

        [Space]
        [Header("Character movement controller")]
        [SerializeField] private float dampSpeed = 1f;
        [SerializeField] private float playerSpeed = 10.0f;

        private WaitForSeconds waitFixedDeltaTime;
        private Animator animator;

        IEnumerator GoLeft()
        {
            animator.SetBool("Walking", true);
            while (true)
            {
                if (lookRight) Flip();
                Move();
                yield return waitFixedDeltaTime;
            }
            animator.SetBool("Walking", false);
        }

        IEnumerator GoRight()
        {
            animator.SetBool("Walking", true);
            while (true)
            {
                if (!lookRight) Flip();
                Move();
                yield return waitFixedDeltaTime;
            }
            animator.SetBool("Walking", false);
        }


        private Vector3 velVelocity = Vector3.zero;
        private void Move()
        {
            if (IsJumping) return;
            float x = Input.GetAxis("Horizontal");
            Vector3 move = new Vector3(x * playerSpeed, rigidBody.velocity.y, 0f);
            rigidBody.velocity = move;
        }

        public bool IsJumping { get; set; } = false;
        public IEnumerator Jump(float holdedEnergy)
        {
            var veclocity = rigidBody.velocity;
            veclocity.y = Mathf.Sqrt((holdedEnergy * 2.0f) / rigidBody.mass);
            rigidBody.velocity = veclocity;
            IsJumping = true;
            animator.SetBool("Jumping", true);
            float startHeight = transform.position.y;
            float jumpHeight = 0.0f;
            do
            {
                if ((transform.position.y - startHeight)> jumpHeight)
                {
                    jumpHeight = transform.position.y - startHeight;
                }
                yield return null;
            } while (IsJumping);
            Debug.Log("jumpHeight: " + jumpHeight);
            animator.SetBool("Jumping", false);
            IsJumping = false;
        }

        bool lookRight = false;

        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            lookRight = !lookRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}