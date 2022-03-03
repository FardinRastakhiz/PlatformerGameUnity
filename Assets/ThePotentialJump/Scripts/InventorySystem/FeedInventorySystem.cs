using UnityEngine;

namespace ThePotentialJump.Inventory
{
    public class FeedInventorySystem : MonoBehaviour
    {
        [SerializeField] private InventorySystem inventorySystem;
        [SerializeField] private InventoryItem[] inventoryItems;

        private void Start()
        {
            for (int i = 0; i < inventoryItems.Length; i++)
                StartCoroutine(inventorySystem.AddItem(inventoryItems[i]));
        }
    }

}