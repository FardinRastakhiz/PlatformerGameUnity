using UnityEngine;
using UnityEngine.Events;

namespace ThePotentialJump.Gameplay
{
    public class EndLevelTrigger : MonoBehaviour
    {
        public UnityEvent GameplayEnd;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                GameplayEnd?.Invoke();
            }
        }
    }
}