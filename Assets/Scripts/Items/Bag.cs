using Buttons;
using Inventory;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Items
{
    public class Bag : Item, IUsable
    {
        [PropertyOrder(-1)]
        [SerializeField] private GameObject bagPrefab;

        [SerializeField] private int slots;

        public BagScript BagScript { get; set; }
        public BagButton BagButton { get; set; }

        public int Slots => slots;

        public void Initialize(int numSlots) => slots = numSlots;

        public void ResizeBag(int numSlotsToAdd)
        {
            slots += numSlotsToAdd;
            BagScript.AddSlots(numSlotsToAdd);
        }

        public void Use()
        {
            Remove();
            BagScript = Instantiate(bagPrefab, InventoryScript.Instance.transform).GetComponentInChildren<BagScript>();
            BagScript.AddSlots(slots);

            InventoryScript.Instance.AddBag(this);
        }

        public override string GetDescription()
        {
            return base.GetDescription() + $"\n{slots} slot bag";
        }
    }
}