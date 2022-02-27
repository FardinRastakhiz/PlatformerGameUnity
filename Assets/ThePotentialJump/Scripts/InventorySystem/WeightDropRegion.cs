using System;
using System.Collections.Generic;
using ThePotentialJump.Gameplay;
using UnityEngine;

namespace ThePotentialJump.Inventory
{
    public class WeightDropRegion : DropRegion
    {
        private HashSet<Droppable> droppables = new HashSet<Droppable>();

        public override event EventHandler<PlaceObjectEventArgs> Replace;
        [SerializeField] private JumpRuler jumpRuler;
        private void Awake()
        {
            if (jumpRuler == null) Debug.LogError("Jump Ruler cannot be null!");
        }

        public override void DropItem(Vector3 position, Droppable dropObject)
        {
            if (dropObject is WeightDroppable weightDroppable)
            {
                var droppable = Instantiate(weightDroppable, position, Quaternion.identity, this.transform);
                jumpRuler.OnStartUpdateRuler(droppable.transform);
                droppable.Replace += Replace;
                droppable.Replace += (o, e) => jumpRuler.OnStopUpdateRuler();
                droppable.DropHeight = position.y;
                droppable.tag = "Weight";
                droppables.Add(droppable);
            }
        }
    }

}