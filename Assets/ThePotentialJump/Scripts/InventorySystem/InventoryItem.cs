using UnityEngine;
using UnityEngine.UI;

namespace ThePotentialJump.Inventory
{

    [System.Serializable]
    public class InventoryItem
    {

        [SerializeField] private string name;
        [SerializeField] private Sprite itemIcon;
        [SerializeField] private int count;
        public string Name { get => name; set => name = value; }
        public Sprite ItemIcon { get => itemIcon; set => itemIcon = value; }
        public int Count { get => count; set => count = value; }
    }
}