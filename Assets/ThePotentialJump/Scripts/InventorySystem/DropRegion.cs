using UnityEngine;
using UnityEngine.EventSystems;

namespace ThePotentialJump.Inventory
{

    public abstract class DropRegion : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            print("drop worked");
            var droppedObject = eventData.pointerDrag;
            var inventoryCell = droppedObject.GetComponent<InventoryCell>();
            if (inventoryCell == null || inventoryCell.Count == 0) return;
            if (inventoryCell.Content.DropItame != null)
            {
                DropItem(eventData.pointerCurrentRaycast.worldPosition, inventoryCell.Content.DropItame);
            }

            inventoryCell.RemoveItem(1);
        }

        public abstract void DropItem(Vector3 position, Droppable dropObject);
    }
}