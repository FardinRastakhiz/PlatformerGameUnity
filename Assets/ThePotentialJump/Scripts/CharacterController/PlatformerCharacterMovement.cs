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
        public void SetupParameters(PlatformerCharacterController controller)
        {
            this.controller = controller;
            transform = controller.transform;
            rigidBody = controller.GetComponent<Rigidbody2D>();
            waitFixedDeltaTime = new WaitForSeconds(Time.fixedDeltaTime);
        }

        public void OnEnable()
        {
            if (InputController.Instance == null) return;
            InputController.Instance.PressSpace += OnPressSpace;
            InputController.Instance.PressLeft += OnPressLeft;
            InputController.Instance.PressRight += OnPressRight;

            InputController.Instance.ReleaseSpace += OnReleaseSpace;
            InputController.Instance.ReleaseLeft += OnReleaseLeft;
            InputController.Instance.ReleaseRight += OnReleaseRight;
        }

        public void OnDisable()
        {
            if (InputController.Instance == null) return;
            InputController.Instance.PressSpace -= OnPressSpace;
            InputController.Instance.PressLeft -= OnPressLeft;
            InputController.Instance.PressRight -= OnPressRight;

            InputController.Instance.ReleaseSpace -= OnReleaseSpace;
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
            Debug.Log("goLeftCoroutine1 " + (goLeftCoroutine == null));
        }

        public void OnPressRight(object o, EventArgs e)
        {
            if (goRightCoroutine != null)
                controller.StopCoroutine(goRightCoroutine);
            goRightCoroutine = controller.StartCoroutine(GoRight());
        }
        public void OnReleaseLeft(object o, EventArgs e)
        {
            Debug.Log("OnReleaseLeft, ");
            Debug.Log("goLeftCoroutine2 " + (goLeftCoroutine == null));
            if (goLeftCoroutine != null)
                controller.StopCoroutine(goLeftCoroutine);
        }

        public void OnReleaseRight(object o, EventArgs e)
        {
            if (goRightCoroutine != null)
                controller.StopCoroutine(goRightCoroutine);
        }

        private void OnPressSpace(object sender, EventArgs e)
        {
            if (isInTheAir) return;
            jumpCoroutine = controller.StartCoroutine(HoldJumpEnergy());
        }

        public void OnReleaseSpace(object o, EventArgs e)
        {
            if (jumpCoroutine != null)
                controller.StopCoroutine(jumpCoroutine);
            jumpCoroutine = controller.StartCoroutine(Jump());
        }

        [Space]
        [Header("Character movement controller")]
        [SerializeField] private float dampSpeed = 1f;
        [SerializeField] private float playerSpeed = 10.0f;

        private WaitForSeconds waitFixedDeltaTime;
        IEnumerator GoLeft()
        {
            while (true)
            {
                if (lookRight) Flip();
                Move(Time.fixedDeltaTime * -playerSpeed);
                yield return waitFixedDeltaTime;
            }
        }

        private Vector3 velVelocity = Vector3.zero;
        IEnumerator GoRight()
        {
            while (true)
            {
                if (!lookRight) Flip();
                Move(Time.fixedDeltaTime * playerSpeed);
                yield return waitFixedDeltaTime;
            }
        }


        private void Move(float deltaMove)
        {
            Vector3 targetVelocity = new Vector2(deltaMove, rigidBody.velocity.y);
            rigidBody.velocity = Vector3.SmoothDamp(rigidBody.velocity, targetVelocity, ref velVelocity, dampSpeed);
        }

        [Space]
        [Header("Jump energy")]
        [SerializeField]
        private float maxEnergy = 1000;
        private float holdedEnergy = 0;
        [SerializeField]
        private float energySavingRate = 350;
        private bool isInTheAir = false;
        public float MaxEnergy { get => maxEnergy; set => maxEnergy = value; }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("OnCollisionEnter: " + isInTheAir);
            if (isInTheAir && collision.gameObject.tag == "Ground")
                isInTheAir = false;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            Debug.Log("OnCollisionExit2D: " + isInTheAir);
            if (!isInTheAir && collision.gameObject.tag == "Ground")
                isInTheAir = true;
        }

        public event EventHandler<HoldEnergyEventArgs> HoldedEnergyChanged;
        private HoldEnergyEventArgs holdEnergyEventArgs = new HoldEnergyEventArgs();
        IEnumerator HoldJumpEnergy()
        {
            holdedEnergy = 0;
            while (!isInTheAir)
            {
                holdedEnergy = holdedEnergy >= maxEnergy ? maxEnergy : holdedEnergy + Time.fixedDeltaTime * energySavingRate;
                holdEnergyEventArgs.Value = holdedEnergy;
                HoldedEnergyChanged?.Invoke(this, holdEnergyEventArgs);
                yield return waitFixedDeltaTime;
            }
        }


        IEnumerator Jump()
        {
            rigidBody.AddForce(new Vector2(0f, holdedEnergy * 10));
            holdedEnergy = 0;
            do
            {
                yield return null;
            } while (isInTheAir);
            Debug.Log(jumpCoroutine);
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