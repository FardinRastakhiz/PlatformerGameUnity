using ThePotentialJump.Gameplay;
using UnityEngine;

namespace ThePotentialJump.Inventory
{
    public abstract class Droppable : Replacable
    {
        public float MinHeight { get; set; }
    }
}