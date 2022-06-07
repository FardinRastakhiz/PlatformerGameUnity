using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ThePotentialJump.Gameplay
{
    public class Projectile2D : Droppable
    {
        [SerializeField] private Rigidbody2D projectileRigidbody;
        [SerializeField] private SpriteRenderer spriteRenderer;
        public float Mass => projectileRigidbody.mass;

        public bool HitThePeak => hitThePeak;

        private bool hitThePeak = false;
        private bool isOnPlatform = true;
        private JumpRuler ruler;

        public void SetupParameters(JumpRuler ruler)
        {
            this.ruler = ruler;
        }

        public void OnDestroy()
        {
            Replace?.Invoke(this, new ReplaceObjectEventArgs
            {
                ReplacePrefab = ReplaceObjectPrefab,
                Parent = transform.parent,
                Position = transform.position
            });
        }

        private WaitForSeconds waitForSeconds;

        public override event EventHandler<ReplaceObjectEventArgs> Replace;

        public void Project(Vector3 velocity)
        {
            if (!isOnPlatform) return;
            if (waitForSeconds == null) waitForSeconds = new WaitForSeconds(Time.fixedDeltaTime);
            isOnPlatform = false;
            hitThePeak = false;

            StartCoroutine(Moving(velocity));
        }

        IEnumerator Moving(Vector3 velocity)
        {
            projectileRigidbody.velocity = velocity;
            ruler.OnStartUpdateRuler(projectileRigidbody.transform);
            while (projectileRigidbody.velocity.y>0)
            {
                yield return waitForSeconds;
            }
            ruler.OnStopUpdateRuler();
            hitThePeak = true;
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (hitThePeak && collision.gameObject.tag == "Platform")
            {
                isOnPlatform = true;
                //ruler.OnHideRuler();
            }
            if (hitThePeak && collision.gameObject.tag == "Ground")
            {
                Destroy(this.gameObject);
            }
        }

        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }
        public void SetMass(float mass)
        {
            projectileRigidbody.mass = mass;
        }
    }
}