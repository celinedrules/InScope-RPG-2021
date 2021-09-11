using System;
using Items;
using UI;
using UnityEngine;

namespace Environment
{
    public class Chest : MonoBehaviour, IInteractable
    {
        [SerializeField] private Item[] items;
        [SerializeField] private int numberOfSlots;
        [SerializeField] private GameObject closedState;
        [SerializeField] private GameObject openState;

        private bool isOpen;

        private void Awake() => items ??= new Item[numberOfSlots];

        public void AddItems()
        {
            if (items == null)
                return;
            
            foreach (Item item in items)
                item.Slot.AddItem(item);
        }

        public void StoreItems()
        {
            
        }
        
        public void Interact()
        {
            if (isOpen)
            {
                StopInteract();
            }

            else
            {
                AddItems();
                openState.SetActive(true);
                closedState.SetActive(false);
                isOpen = true;
            }
        }

        public void StopInteract()
        {
            if (isOpen)
            {
                StoreItems();
                isOpen = false;
            }
        }
    }
}
