using System;
using System.Collections.Generic;
using ThePotentialJump.Gameplay;
using ThePotentialJump.Sounds;
using UnityEngine;

namespace ThePotentialJump.Inventory
{
    public class WeightDropRegion : DropRegion
    {
        [SerializeField] private SFXModule slidingObjectDestroyedSFX;
        private HashSet<Droppable> droppables = new HashSet<Droppable>();

        public override event EventHandler<ReplaceObjectEventArgs> Replace;
        [SerializeField] private JumpRuler jumpRuler;
        private void Awake()
        {
            if (jumpRuler == null) Debug.LogError("Jump Ruler cannot be null!");
        }

        public override void DropItem(Vector3 position, Droppable dropObject)
        {
            if (dropObject is FreeDropWeight weightDroppable)
            {
                var droppable = Instantiate(weightDroppable, position, Quaternion.identity, this.transform);
                droppable.MinHeight = minHeight;
                this.droppable = droppable;
                
                jumpRuler.OnStartUpdateRuler(droppable.transform);
                droppable.Replace += Replace;
                droppable.Replace += OnReplaceDroppable;
                droppable.Replace += (o, e) => jumpRuler.OnStopUpdateRuler();
                droppable.DropHeight = position.y;
                droppable.tag = "Weight";
                droppables.Add(droppable);
            }
        }

        private void OnReplaceDroppable(object sender, ReplaceObjectEventArgs e)
        {
            slidingObjectDestroyedSFX.PlayImmediate();
        }
    }

}