using System;
using ThePotentialJump.Inventory;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public class SlidingCage : SlidingObject
    {
        [SerializeField] private GameObject brokenObjectPrefab;
        [SerializeField] private float minEnergyTolerance = 100;
        [SerializeField] private float maxEnergyTolerance = 150;
        [SerializeField] private float energyThreshold = 0.5f;
        [SerializeField] private int openingReward = 5;
        [SerializeField] private int breakingCost = 1;
        public override event System.EventHandler<ReplaceObjectEventArgs> Replace;
        private ReplaceCageEventArgs placeObjectEventArgs;

        public GameObject BrokenObjectPrefab { get => brokenObjectPrefab; set => brokenObjectPrefab = value; }
        public float MinEnergyTolerance => minEnergyTolerance;
        public float MaxEnergyTolerance => maxEnergyTolerance;

        public event EventHandler CageBroke;
        public event EventHandler CageOpened;

        private void Awake()
        {
            placeObjectEventArgs = new ReplaceCageEventArgs { Parent = transform.parent };
        }
        private void OnDestroy()
        {
            placeObjectEventArgs.Position = transform.position;
            Replace?.Invoke(this, placeObjectEventArgs);
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Weight")
            {
                var droppableWeight = collision.gameObject.GetComponent<FreeDropWeight>();
                var energy = collision.rigidbody.mass * 9.81f * droppableWeight.DropHeight;
                if (energy > minEnergyTolerance - energyThreshold)
                {
                    droppableWeight.ReleaseEnergy(energy);
                    if (energy < maxEnergyTolerance + energyThreshold)
                    {
                        EconomySystem.Instance?.Deposit(openingReward);
                        placeObjectEventArgs.ReplacePrefab = ReplaceObjectPrefab;
                        placeObjectEventArgs.isBroke = false;
                        CageOpened?.Invoke(this, null);
                    }
                    else
                    {
                        EconomySystem.Instance?.Withdraw(breakingCost);
                        placeObjectEventArgs.ReplacePrefab = BrokenObjectPrefab;
                        placeObjectEventArgs.isBroke = true;
                        CageBroke?.Invoke(this, null);
                    }

                    Destroy(droppableWeight.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
    }

}