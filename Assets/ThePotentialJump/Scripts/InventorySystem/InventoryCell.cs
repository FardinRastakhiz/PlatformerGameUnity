using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace ThePotentialJump.Inventory
{
    public class InventoryCell : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private TextMeshProUGUI itemCount;
        [SerializeField] private Image itemImage;
        [SerializeField] private GameObject focusObject;
        private Sprite emptyIcon;
        private int count = 0;
        [SerializeField]
        private int capacity = 1;

        public event EventHandler CellActivated;
        public InventorySystem Inventory { get; set; }

        private void Awake()
        {
            emptyIcon = itemImage.sprite;
            itemCount.text = "";
        }
        public InventoryItem Content { get; set; }
        public int Capacity { get => capacity; set => capacity = value; }
        public int Count { get => count; set => count = value; }

        public void SetItem(InventoryItem item, bool replace = false)
        {
            if (!replace && itemImage.sprite != emptyIcon) return;
            if ((Capacity > 0) && (Count + item.Count > Capacity)) return;
            Content = new InventoryItem(item);
            Count += Content.Count;
            itemImage.sprite = Content.ItemIcon;
            itemCount.text = Count.ToString();
        }
        public void RemoveItem(int count, bool removeCellIfEmpty = true)
        {
            Count -= count;
            if (Count <= 0)
            {
                emptyIcon = itemImage.sprite;
                itemCount.text = "";
                if (removeCellIfEmpty) Inventory.RemoveCell(Content.Name);
            }
            itemCount.text = Count.ToString();
        }

        public void MakeCellEmpty()
        {
            Count = 0;
            itemCount.text = "";
            itemImage.sprite = emptyIcon;
        }


        private bool dragBegan = false;
        private RectTransform draggingItem;
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Count == 0) return;
            ActivateCell();
            var draggingItemImage = Instantiate(itemImage, transform);
            draggingItemImage.maskable = false;
            draggingItem = draggingItemImage.rectTransform;
            UpdateDraggingItemPosition();
            dragBegan = true;
            print("started dragging");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ActivateCell();
            print("started clicking");
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!dragBegan) return;
            UpdateDraggingItemPosition();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!dragBegan) return;
            dragBegan = false;
            Destroy(draggingItem.gameObject);
        }


        private void UpdateDraggingItemPosition()
        {
            var mousePosition = Input.mousePosition;
            draggingItem.position = mousePosition;
        }

        public void ActivateCell()
        {
            CellActivated?.Invoke(this, null);
            focusObject?.SetActive(true);
        }

        public void DeactivateCell()
        {
            focusObject?.SetActive(false);
        }
    }
}