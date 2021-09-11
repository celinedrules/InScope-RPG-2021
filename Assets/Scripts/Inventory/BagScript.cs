using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEngine;

namespace Inventory
{
    public class BagScript : MonoBehaviour
    {
        [SerializeField] private GameObject slotPrefab;

        private CanvasGroup canvasGroup;

        public bool IsOpen => canvasGroup.alpha > 0;

        public List<SlotScript> Slots { get; } = new List<SlotScript>();
        public int EmptySlotCount { get { return Slots.Count(slot => slot.IsEmpty); } }

        private void Awake() => canvasGroup = GetComponentInParent<CanvasGroup>();

        public List<Item> GetItems() => Slots.Where(slot => !slot.IsEmpty).SelectMany(slot => slot.Items).ToList();
    
        public void Clear()
        {
            foreach (SlotScript slot in Slots)
                slot.Clear();
        }
    
        public void AddSlots(int slotCount)
        {
            for (int i = 0; i < slotCount; i++)
            {
                SlotScript slot = Instantiate(slotPrefab, transform).GetComponent<SlotScript>();
                slot.Bag = this;
                Slots.Add(slot);
            }
        }

        public bool AddItem(Item item)
        {
            foreach (var slot in Slots.Where(slot => slot.IsEmpty))
            {
                slot.AddItem(item);

                return true;
            }

            return false;
        }
    
        public void OpenClose()
        {
            canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
            canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts != true;
        }
    }
}