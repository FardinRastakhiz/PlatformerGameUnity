using System;
using System.Collections;
using ThePotentialJump.CharacterController;
using UnityEngine;
using UnityEngine.Events;

namespace ThePotentialJump.Gameplay
{
    public class PlatformerActiveCollider : MonoBehaviour
    {
        [SerializeField] private Collider2D plaformCollider;
        [SerializeField] private Transform hitbodyTransform;
        public Transform HitbodyTransform { get => hitbodyTransform; set => hitbodyTransform = value; }


        private Coroutine updateCoroutine;
        private void Start()
        {
            if(GameManager.Instance.GameType == GameType.PLATFORMER)
                PlatformerCharacterController.Instance.HitCeiling += OnHitCeiling;
            updateCoroutine = StartCoroutine(CheckforActivate());
        }

        private bool hitCeiling;
        private void OnHitCeiling(object sender, EventArgs e)
        {
            if (!passed)
            {
                hitCeiling = true;
                plaformCollider.isTrigger = true;
                PlatformerCharacterController.Instance.JumpEnd += JumpEnd;
            }
        }

        private bool passed = false;

        private void OnEnable()
        {
            if (updateCoroutine != null) StopCoroutine(updateCoroutine);
            updateCoroutine = StartCoroutine(CheckforActivate());
        }

        private void OnDisable()
        {
            plaformCollider.isTrigger = false;
            hitCeiling = false;
            hitbodyTransform = null;
            if (updateCoroutine != null) StopCoroutine(updateCoroutine);
        }

        Collider2D hbCollider;
        private IEnumerator CheckforActivate()
        {
            while (true)
            {
                yield return null;
                if (hitCeiling || hitbodyTransform == null)
                    continue;
                if(hbCollider==null) hbCollider = hitbodyTransform.GetComponent<Collider2D>();
                if (hitbodyTransform.position.y > transform.position.y + plaformCollider.bounds.size.y / 2.0f)
                {
                    plaformCollider.isTrigger =false;
                }
                else if (hitbodyTransform.position.y + hbCollider.bounds.size.y/3.0f < transform.position.y - plaformCollider.bounds.size.y)
                {
                    plaformCollider.isTrigger = true;
                }
            }
        }

        public void JumpEnd(object o, EventArgs e)
        {
            hitCeiling = false;
            PlatformerCharacterController.Instance.JumpEnd -= JumpEnd;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!passed && collision.gameObject.tag == "Player")
            {
                passed = true;
                PlatformerCharacterController.Instance.HitCeiling -= OnHitCeiling;
            }
        }
    }

}
