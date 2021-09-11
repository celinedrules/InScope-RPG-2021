using Inventory;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class HandScript : MonoBehaviour
    {
        [SerializeField] private Vector2 offset;

        private static HandScript instance;

        private Image icon;

        public static HandScript Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<HandScript>();

                return instance;
            }
        }

        public IMovable Movable { get; set; }

        private void Start() => icon = GetComponent<Image>();

        private void Update()
        {
            icon.transform.position = Mouse.current.position.ReadValue() + offset;

            Mouse mouse = Mouse.current;
        
            // Fix this to use InputAsset
            if (mouse.leftButton.wasPressedThisFrame && !EventSystem.current.IsPointerOverGameObject() &&
                Instance.Movable != null)
            {
                DeleteItem();
            }
        }

        public void TakeMovable(IMovable movable)
        {
            Movable = movable;
            icon.sprite = Movable.Icon;
            icon.color = Color.white;
        }

        public IMovable Put()
        {
            IMovable tmp = Movable;
            Movable = null;
            icon.color = new Color(0, 0, 0, 0);

            return tmp;
        }

        public void Drop()
        {
            Movable = null;
            icon.color = new Color(0, 0, 0, 0);
            InventoryScript.Instance.FromSlot = null;
        }

        public void DeleteItem()
        {
            if (Movable is Item item)
            {
                if (item.Slot != null)
                    item.Slot.Clear();
                else if(item.CharButton != null)
                    item.CharButton.DeEquipArmor((Armor)item);
            }

            Drop();

            InventoryScript.Instance.FromSlot = null;
        }
    }
}