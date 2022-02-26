using UnityEngine;
using UnityEngine.Events;

namespace ThePotentialJump.Gameplay
{
    public abstract class EndingCheck : MonoBehaviour
    {
        public UnityEvent Passed;
        public UnityEvent Failed;
    }
}
