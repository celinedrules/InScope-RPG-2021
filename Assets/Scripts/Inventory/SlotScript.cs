using System;
using Buttons;
using Items;
using Managers;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI stackSize;

        public ObservableStack<Item> Items { get; } = new ObservableStack<Item>();
        public bool IsEmpty => Items.Count == 0;
        public Item Item => !IsEmpty ? Items.Peek() : null;
        public bool IsFull => !IsEmpty && Count >= Item.StackSize;
        public int Count => Items.Count;
        public TextMeshProUGUI StackSizeText => stackSize;
        public BagScript Bag { get; set; }

        public Image Icon
        {
            get => icon;
            set => icon = value;
        }

        private void Awake()
        {
            Items.OnPop += UpdateSlot;
            Items.OnPush += UpdateSlot;
            Items.OnClear += UpdateSlot;
        }

        public bool AddItem(Item item)
        {
            Items.Push(item);
            icon.sprite = item.Icon;
            icon.color = Color.white;
            item.Slot = this;
            return true;
        }

        private bool AddItems(ObservableStack<Item> newItems)
        {
            if (!IsEmpty && newItems.Peek().GetType() != Item.GetType())
                return false;

            int count = newItems.Count;

            for (int i = 0; i < count; i++)
            {
                if (IsFull)
                    return false;

                AddItem(newItems.Pop());
            }

            return true;
        }

        public void RemoveItem(Item item)
        {
            if (!IsEmpty)
                InventoryScript.Instance.OnItemCountChanged(Items.Pop());
        }

        public void Clear()
        {
            if (Items.Count <= 0)
                return;

            InventoryScript.Instance.OnItemCountChanged(Items.Pop());
            Items.Clear();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left when InventoryScript.Instance.FromSlot == null && !IsEmpty:
                    if (HandScript.Instance.Movable != null)
                    {
                        if (HandScript.Instance.Movable is Armor)
                        {
                            if (Item is Armor && (Item as Armor).Type == (HandScript.Instance.Movable as Armor).Type)
                            {
                                (Item as Armor).Equip();
                                HandScript.Instance.Drop();
                            }
                        }
                    }
                    else
                    {
                        HandScript.Instance.TakeMovable(Item);
                        InventoryScript.Instance.FromSlot = this;
                    }

                    break;
                case PointerEventData.InputButton.Left:
                {
                    if (InventoryScript.Instance.FromSlot == null && IsEmpty)
                    {
                        if (HandScript.Instance.Movable is Armor)
                        {
                            Armor armor = (Armor)HandScript.Instance.Movable;
                            AddItem(armor);
                            CharacterPanel.Instance.SelectedButton.DeEquipArmor(armor);
                            HandScript.Instance.Drop();
                        }
                    }
                    else if (InventoryScript.Instance.FromSlot != null)
                    {
                        if (PutItemBack() || MergeItems(InventoryScript.Instance.FromSlot) ||
                            SwapItems(InventoryScript.Instance.FromSlot) ||
                            AddItems(InventoryScript.Instance.FromSlot.Items))
                        {
                            HandScript.Instance.Drop();
                            InventoryScript.Instance.FromSlot = null;
                        }
                    }

                    break;
                }
                case PointerEventData.InputButton.Right when HandScript.Instance.Movable == null:
                    UseItem();
                    break;
                case PointerEventData.InputButton.Right:
                    break;
                case PointerEventData.InputButton.Middle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!IsEmpty)
            {
                UiManager.Instance.ShowTooltip(new Vector2(1, 0), transform.position, Item);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UiManager.Instance.HideTooltip();
        }

        private void UseItem()
        {
            if (Item is IUsable)
                (Item as IUsable)?.Use();
            else // if(Item is Armor)
                (Item as Armor)?.Equip();
        }

        public bool StackItem(Item item)
        {
            if (IsEmpty || item.name != Item.name || Items.Count >= Item.StackSize)
                return false;

            Items.Push(item);
            item.Slot = this;

            return true;
        }

        private bool PutItemBack()
        {
            if (InventoryScript.Instance.FromSlot != this)
                return false;

            InventoryScript.Instance.FromSlot.Icon.color = Color.white;

            return true;
        }

        private bool SwapItems(SlotScript from)
        {
            if (IsEmpty)
                return false;

            if (from.Item.GetType() == Item.GetType() && from.Count + Count <= Item.StackSize)
                return false;

            var tmpFrom = new ObservableStack<Item>(from.Items);

            from.Items.Clear();
            from.AddItems(Items);
            Items.Clear();
            AddItems(tmpFrom);

            return true;
        }

        private bool MergeItems(SlotScript from)
        {
            if (IsEmpty)
                return false;

            if (from.Item.GetType() != Item.GetType() || IsFull)
                return false;

            int freeSlots = Item.StackSize - Count;
            int itemLeftToStack = from.Item.Slot.Count;

            for (int i = 0; i < freeSlots; i++)
            {
                if (itemLeftToStack <= 0)
                    continue;

                AddItem(from.Items.Pop());
                itemLeftToStack--;
            }

            return true;
        }

        private void UpdateSlot() => UiManager.Instance.UpdateStackSize(this);
    }
}