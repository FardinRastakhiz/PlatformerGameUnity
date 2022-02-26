using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ThePotentialJump.Inventory
{

    public class InventorySystem : MonoBehaviour
    {
        private Dictionary<string, InventoryCell> inventoryCells = new Dictionary<string, InventoryCell>();
        [SerializeField] private InventoryCell inventoryCellPrefab;
        [SerializeField] private RectTransform content;
        [SerializeField] private UnityEvent inventoryEmptied;
        public InventoryCell ActivatedCell { get; private set; }
        public IEnumerator AddItem(InventoryItem item)
        {
            yield return CreateCell((inventoryCell) =>
            {
                inventoryCell.SetItem(item);
                if (!inventoryCells.ContainsKey(item.Name))
                    inventoryCells.Add(item.Name, inventoryCell);
            });
        }

        public void RemoveItem(InventoryItem item, int count)
        {
            if (inventoryCells.ContainsKey(item.Name))
                inventoryCells[item.Name].RemoveItem(count);
        }

        public void RemoveCell(string cellName)
        {
            if (inventoryCells.ContainsKey(cellName))
            {
                var cell = inventoryCells[cellName];
                inventoryCells.Remove(cellName);
                Destroy(cell.gameObject);
                if (inventoryCells.Count == 0)
                    inventoryEmptied?.Invoke();
            }
        }

        private IEnumerator CreateCell(Action<InventoryCell> callback)
        {
            var inventoryCell = Instantiate(inventoryCellPrefab, content);
            while (inventoryCell == null)
                yield return null;
            Debug.Log(inventoryCell.gameObject.name);
            inventoryCell.Inventory = this;
            inventoryCell.CellActivated += OnCellActivated;
            callback(inventoryCell);
        }

        private void OnCellActivated(object sender, EventArgs e)
        {
            if (ActivatedCell != null) ActivatedCell.DeactivateCell();
            ActivatedCell = sender as InventoryCell;
        }
    }
}