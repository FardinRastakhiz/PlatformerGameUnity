using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ThePotentialJump.Inventory
{
    public class DropRegion : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            var droppedObject = eventData.pointerDrag;
            var inventoryCell = droppedObject.GetComponent<InventoryCell>();
            if (inventoryCell == null) return;
            inventoryCell.RemoveItem(1);
        }
    }

}