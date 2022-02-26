using System.Collections.Generic;
using UnityEngine;

namespace ThePotentialJump.Inventory
{
    public class WeightDropRegion : DropRegion
    {
        private HashSet<Droppable> droppables = new HashSet<Droppable>();
        public override void DropItem(Vector3 position, Droppable dropObject)
        {
            Debug.Log("dropped");
            if(dropObject is WeightDroppable)
            {
                var droppable = Instantiate(dropObject, position, Quaternion.identity, this.transform);
                droppables.Add(droppable);
            }
        }
    }

}