using System.Collections;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public class Projectile2D : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D rigidbody;

        public float Mass => rigidbody.mass;

        public bool HitThePeak => hitThePeak;

        private bool hitThePeak = false;
        private bool isOnPlatform = true;


        private WaitForSeconds waitForSeconds;
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
            rigidbody.velocity = velocity;
            Debug.Log(velocity);
            while (rigidbody.velocity.y>0)
            {
                yield return waitForSeconds;
            }
            hitThePeak = true;
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (hitThePeak && collision.gameObject.tag == "Platform")
                isOnPlatform = true;
        }
    }
}