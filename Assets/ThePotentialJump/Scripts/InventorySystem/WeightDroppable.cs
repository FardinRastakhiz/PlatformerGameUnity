using System.Collections;
using ThePotentialJump.Gameplay;
using UnityEngine;

namespace ThePotentialJump.Inventory
{
    public class WeightDroppable : Droppable
    {
        public float DropHeight { get; set; }
        private new Rigidbody2D rigidbody;
        private float maxSpeedSquare = 0.0f;

        public override event System.EventHandler<ReplaceObjectEventArgs> Replace;

        public float MaxSpeed => Mathf.Sqrt(maxSpeedSquare);
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            StartCoroutine(UpdateParameters());
        }

        public void OnDestroy()
        {
            Replace?.Invoke(this, new ReplaceObjectEventArgs { 
                ReplacePrefab = ReplaceObjectPrefab,
                Parent = transform.parent,
                Position = transform.position });
        }

        IEnumerator UpdateParameters()
        {
            var speedSquare = rigidbody.velocity.sqrMagnitude;
            while (maxSpeedSquare <= speedSquare)
            {
                maxSpeedSquare = Mathf.Max(speedSquare, maxSpeedSquare);
                yield return null;
                speedSquare = rigidbody.velocity.sqrMagnitude;
                if(transform.position.y < MinHeight)
                {
                    Destroy(this.gameObject);
                }
            }
            Destroy(this.gameObject);
        }

        public void ReleaseEnergy(float energy)
        {
            var reduceSpeed = (2.0f * energy) / rigidbody.mass;
            maxSpeedSquare = maxSpeedSquare - reduceSpeed > 0.0f ? maxSpeedSquare - reduceSpeed : 0.0f;
        }
    }
}