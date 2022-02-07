using UnityEngine;
using UnityEngine.UI;

namespace ThePotentialJump.Inventory
{

    [System.Serializable]
    public class InventoryItem
    {
        public InventoryItem()
        {

        }

        public InventoryItem(InventoryItem item)
        {
            Count = item.Count;
            ItemIcon = item.itemIcon;
            Name = item.Name;
            DropItame = item.DropItame;
        }

        [SerializeField] private string name;
        [SerializeField] private Sprite itemIcon;
        [SerializeField] private int count;
        [SerializeField] private Droppable dropItame;
        public string Name { get => name; set => name = value; }
        public Sprite ItemIcon { get => itemIcon; set => itemIcon = value; }
        public int Count { get => count; set => count = value; }
        public Droppable DropItame { get => dropItame; set => dropItame = value; }
    }
}