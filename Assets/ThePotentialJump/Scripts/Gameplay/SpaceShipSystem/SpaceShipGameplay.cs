using System.Collections;
using System.Collections.Generic;
using ThePotentialJump.Gameplay;
using ThePotentialJump.Sounds;
using UnityEngine;
using UnityEngine.Events;

namespace ThePotentialJump.Gameplay
{
    public class SpaceShipGameplay : MonoBehaviour
    {
        [SerializeField] Rigidbody2D spaceShipRigidBody;
        [SerializeField] private MusicModule musicPlayer;
        private Transform spaceshipTransform;

        [Space]
        [SerializeField] private Vector2 speedRange = new Vector2(250.0f, 260.0f);
        [SerializeField] private float maxHeight = 15000;
        [Space]
        [SerializeField] private float timeToWin = 3.0f;

        [Space]
        [SerializeField] private UnityEvent succeed;
        [SerializeField] private UnityEvent lost;

        private WaitForSeconds waitForFixedUpdate;

        private void Start()
        {
            waitForFixedUpdate = new WaitForSeconds(Time.fixedDeltaTime);
            spaceshipTransform = spaceShipRigidBody.transform;
            StartCoroutine(CheckSpaceShip());
        }

        public bool isInTargetRange;
        private bool isSucceed = false;
        private bool isLost = false;
        IEnumerator CheckSpaceShip()
        {
            float speed = 0.0f;
            while (true)
            {
                speed = spaceShipRigidBody.velocity.y;
                if (!isInTargetRange && speed >= speedRange.x && speed <= speedRange.y)
                {
                    if (winCounterCoroutine != null) StopCoroutine(winCounterCoroutine);
                    winCounterCoroutine = StartCoroutine(WinCounter());
                    isInTargetRange = true;
                }
                else if (isInTargetRange && (speed < speedRange.x || speed > speedRange.y))
                {
                    if (winCounterCoroutine != null) StopCoroutine(winCounterCoroutine);
                    isInTargetRange = false;
                }
                if (spaceshipTransform.position.y > maxHeight)
                {
                    lost?.Invoke();
                    isLost = true;
                }
                if (isSucceed || isLost)
                {
                    musicPlayer.ChangeMaxVolume(musicPlayer.MaxVolume / 10.0f);
                    yield break;
                }
                yield return null;
            }
        }

        private Coroutine winCounterCoroutine;
        IEnumerator WinCounter()
        {
            var localTimer = timeToWin;
            while (localTimer > 0.0f)
            {
                localTimer -= Time.fixedDeltaTime;
                yield return waitForFixedUpdate;
            }
            isSucceed = true;
            EconomySystem.Instance?.Deposit(20);
            succeed?.Invoke();
        }
    }
}