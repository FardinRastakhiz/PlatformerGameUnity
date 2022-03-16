using System;
using ThePotentialJump.Gameplay;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ThePotentialJump.Inventory
{

    public abstract class DropRegion : Replacable, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event EventHandler Dropped;
        public event EventHandler Entered;
        public event EventHandler Exitted;
        protected Droppable droppable = null;
        [SerializeField] protected float minHeight;
        public void OnDrop(PointerEventData eventData)
        {
            var droppedObject = eventData.pointerDrag;
            if (droppedObject == null) return;
            var inventoryCell = droppedObject.GetComponent<InventoryCell>();
            if (inventoryCell == null || inventoryCell.Count == 0) return;
            Dropped?.Invoke(this, null);
            if (inventoryCell.Content.DropItame != null && droppable == null)
            {
                DropItem(eventData.pointerCurrentRaycast.worldPosition, inventoryCell.Content.DropItame);
                inventoryCell.RemoveItem(1);
            }
        }

        internal void SetMinHeight(float baseFloorHeight)
        {
            minHeight = baseFloorHeight - 5.0f;
        }

        public abstract void DropItem(Vector3 position, Droppable dropObject);

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null) return;
            Entered?.Invoke(this, null);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null) return;
            Exitted?.Invoke(this, null);
        }
    }
}