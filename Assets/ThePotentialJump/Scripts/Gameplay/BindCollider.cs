using System.Collections;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public class BindCollider : MonoBehaviour
    {
        [SerializeField] private bool reverse = false;
        [SerializeField] private Collider2D otherCollider;
        [SerializeField] private Collider2D selfCollider;
        private void Start()
        {
            StartCoroutine(UpdateCollider());
        }

        IEnumerator UpdateCollider()
        {
            while (true)
            {
                if (reverse && otherCollider.isTrigger == selfCollider.isTrigger)
                    selfCollider.isTrigger = !otherCollider.isTrigger;
                else if(!reverse && otherCollider.isTrigger != selfCollider.isTrigger)
                    selfCollider.isTrigger = otherCollider.isTrigger;

                yield return null;
            }
        }
    }

}
