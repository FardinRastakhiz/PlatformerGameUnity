using System;
using System.Collections;
using ThePotentialJump.Inputs;
using ThePotentialJump.Utilities;
using UnityEngine;


namespace ThePotentialJump.CharacterController
{

    public class PlatformerCharacterController : Utilities.Singleton<PlatformerCharacterController>
    {
        [SerializeField] private Rigidbody2D rigidBody;

        protected override void Awake()
        {
            destroyOnLoad = true;
            base.Awake();
            if (rigidBody == null) rigidBody = GetComponent<Rigidbody2D>();
            waitFixedDeltaTime = new WaitForSeconds(Time.fixedDeltaTime);
        }

        private void Start()
        {
            OnEnable();
        }

        private void OnEnable()
        {
            if (InputController.Instance == null) return;
            InputController.Instance.PressSpace += OnPressSpace;
            InputController.Instance.PressLeft += OnPressLeft;
            InputController.Instance.PressRight += OnPressRight;

            InputController.Instance.ReleaseSpace += OnReleaseSpace;
            InputController.Instance.ReleaseLeft += OnReleaseLeft;
            InputController.Instance.ReleaseRight += OnReleaseRight;
        }

        private void OnDisable()
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
                StopCoroutine(goLeftCoroutine);
            goLeftCoroutine = StartCoroutine(GoLeft());
            print("goLeftCoroutine1 " + (goLeftCoroutine == null));
        }

        public void OnPressRight(object o, EventArgs e)
        {
            if (goRightCoroutine != null)
                StopCoroutine(goRightCoroutine);
            goRightCoroutine = StartCoroutine(GoRight());
        }
        public void OnReleaseLeft(object o, EventArgs e)
        {
            print("OnReleaseLeft, ");
            print("goLeftCoroutine2 " + (goLeftCoroutine == null));
            if (goLeftCoroutine != null)
                StopCoroutine(goLeftCoroutine);
        }

        public void OnReleaseRight(object o, EventArgs e)
        {
            if (goRightCoroutine != null)
                StopCoroutine(goRightCoroutine);
        }

        private void OnPressSpace(object sender, EventArgs e)
        {
            if (isInTheAir) return;
            jumpCoroutine = StartCoroutine(HoldJumpEnergy());
        }

        public void OnReleaseSpace(object o, EventArgs e)
        {
            if (jumpCoroutine != null)
                StopCoroutine(jumpCoroutine);
            jumpCoroutine = StartCoroutine(Jump());
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
            print("OnCollisionEnter: " + isInTheAir);
            if (isInTheAir && collision.gameObject.tag == "Ground")
                isInTheAir = false;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            print("OnCollisionExit2D: " + isInTheAir);
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
                holdEnergyEventArgs.HoldedEnergy = holdedEnergy;
                HoldedEnergyChanged?.Invoke(this, holdEnergyEventArgs);
                yield return waitFixedDeltaTime;
            }
        }


        IEnumerator Jump()
        {
            rigidBody.AddForce(new Vector2(0f, holdedEnergy*10));
            holdedEnergy = 0;
            do
            {
                yield return null;
            } while (isInTheAir);
            print(jumpCoroutine);
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