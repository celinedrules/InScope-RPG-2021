using Buttons;
using Inventory;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;

namespace Items
{
    public abstract class Item : ScriptableObject, IMovable, IDescribable
    {
        [SerializeField, PreviewField(ObjectFieldAlignment.Left)]
        private Sprite icon;
        [SerializeField] private string title;
        [ShowIf("@GetType() != typeof(Armor)")]
        [SerializeField] private int stackSize;
        [SerializeField] private Quality quality;

        public Sprite Icon => icon;
        public int StackSize => stackSize;

        public SlotScript Slot { get; set; }
        public CharButton CharButton { get; set; }

        public Quality Quality => quality;

        public string Title
        {
            get => title;
            set => title = value;
        }

        public void Remove()
        {
            if (Slot != null)
            {
                Slot.RemoveItem(this);
                Slot = null;
            }
        }

        public virtual string GetDescription() => $"<color={QualityColor.Colors[Quality]}> {Title}</color>";
    }
}