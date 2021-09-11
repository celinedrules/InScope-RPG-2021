using System.Collections.Generic;
using Inventory;
using Items;
using Managers;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Buttons
{
    public class ActionButton : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler,
        IPointerExitHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI stackSize;

        private Stack<IUsable> usables = new Stack<IUsable>();

        public Button Button { get; private set; }
        private IUsable Usable { get; set; }
        public int Count { get; private set; }
        public TextMeshProUGUI StackSizeText => stackSize;

        public Image Icon
        {
            get => icon;
            set => icon = value;
        }

        private void Start()
        {
            Button = GetComponent<Button>();
            Button.onClick.AddListener(OnClick);
            InventoryScript.Instance.ItemCountChanged += UpdateItemCount;
        }

        private void OnClick()
        {
            if (HandScript.Instance.Movable == null)
                Usable?.Use();

            if (usables != null && usables.Count > 0)
                usables.Peek().Use();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (HandScript.Instance.Movable != null && HandScript.Instance.Movable is IUsable)
                    SetUsable(HandScript.Instance.Movable as IUsable);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            IDescribable tmp = null;

            if (Usable is IDescribable usable)
            {
                tmp = usable;
                //UiManager.Instance.ShowTooltip(transform.position);
            }
            else if (usables.Count > 0)
            {
                //UiManager.Instance.ShowTooltip(transform.position);
            }

            if (tmp != null)
            {
                UiManager.Instance.ShowTooltip(new Vector2(1, 0), transform.position, tmp);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UiManager.Instance.HideTooltip();
        }

        private void SetUsable(IUsable usable)
        {
            if (usable is Item)
            {
                usables = InventoryScript.Instance.GetUsables(usable);
                InventoryScript.Instance.FromSlot.Icon.color = Color.white;
                InventoryScript.Instance.FromSlot = null;
            }
            else
            {
                Count = usables.Count;
                usables.Clear();
                Usable = usable;
            }

            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            Icon.sprite = HandScript.Instance.Put().Icon;
            Icon.color = Color.white;

            if (Count > 1)
            {
                UiManager.Instance.UpdateStackSize(this);
            }
            else if (Usable is Spell.Spell)
            {
                UiManager.Instance.ClearStackCount(this);
            }
        }

        private void UpdateItemCount(Item item)
        {
            if (!(item is IUsable usable) || usables.Count <= 0)
                return;

            if (usables.Peek().GetType() != usable.GetType())
                return;

            usables = InventoryScript.Instance.GetUsables(usable);
            Count = usables.Count;
            UiManager.Instance.UpdateStackSize(this);
        }
    }
}