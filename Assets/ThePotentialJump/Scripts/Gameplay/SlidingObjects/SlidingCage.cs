using ThePotentialJump.Inventory;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public class SlidingCage : SlidingObject
    {
        [SerializeField] private float impactEnergyTolerance = 100;
        public override event System.EventHandler<PlaceObjectEventArgs> Replace;
        private void OnDestroy()
        {
            Replace?.Invoke(this, new PlaceObjectEventArgs
            {
                Position = transform.position,
                Parent = transform.parent,
                ReplacePrefab = ReplaceObjectPrefab
            });
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Weight")
            {
                var droppableWeight = collision.gameObject.GetComponent<WeightDroppable>();
                var energy = collision.rigidbody.mass * Mathf.Pow(droppableWeight.MaxSpeed, 2) * 0.5f;
                if (energy > impactEnergyTolerance)
                {
                    droppableWeight.ReleaseEnergy(impactEnergyTolerance);
                    Destroy(droppableWeight.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
    }

}