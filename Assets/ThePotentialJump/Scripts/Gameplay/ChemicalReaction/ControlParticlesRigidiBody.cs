using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public class ControlParticlesRigidiBody : MonoBehaviour
    {
        private Rigidbody2D[] particlesRigidBody;

        [SerializeField] private bool simulateFromAwake;

        // Start is called before the first frame update
        void Awake()
        {
            particlesRigidBody = GetComponentsInChildren<Rigidbody2D>();
        }

        private void Start()
        {
            if (simulateFromAwake)
                StartSimulation();
            else
                StopSimulation();
        }


        public void StopSimulation()
        {
            for (int i = 0; i < particlesRigidBody.Length; i++)
                particlesRigidBody[i].simulated = false;
        }

        public void StartSimulation()
        {
            for (int i = 0; i < particlesRigidBody.Length; i++)
                particlesRigidBody[i].simulated = true;
        }
    }
}