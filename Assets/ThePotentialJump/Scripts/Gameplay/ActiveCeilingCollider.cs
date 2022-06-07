using System.Collections;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public class ActiveCeilingCollider : MonoBehaviour
    {
        [SerializeField] private Collider2D plaformCollider;
        [SerializeField] private Transform player;
        private void Start()
        {
            StartCoroutine(CheckforActivate());
        }

        Collider2D hbCollider;
        private IEnumerator CheckforActivate()
        {
            while (true)
            {
                if (hbCollider == null) hbCollider = player.GetComponent<Collider2D>();
                if (player.position.y >= transform.position.y + plaformCollider.bounds.size.y / 2.0f)
                {
                    plaformCollider.isTrigger = true;
                }
                else if (player.position.y + hbCollider.bounds.size.y < transform.position.y - plaformCollider.bounds.size.y)
                {
                    plaformCollider.isTrigger = false;
                }
                yield return null;
            }
        }

    }

}
