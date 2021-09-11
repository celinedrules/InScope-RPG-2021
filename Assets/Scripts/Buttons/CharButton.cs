using Character;
using Items;
using Managers;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Buttons
{
    public class CharButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private ArmorType armorType;
        [SerializeField] private CutoutMask icon;
        [SerializeField] private GearSlots gearSlots;

        private Armor equippedArmor;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (equippedArmor != null)
            {
                UiManager.Instance.ShowTooltip(new Vector2(0, 0), transform.position, equippedArmor);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UiManager.Instance.HideTooltip();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (HandScript.Instance.Movable is Armor)
                {
                    Armor tmp = (Armor)HandScript.Instance.Movable;

                    if (tmp.Type == armorType)
                    {
                        EquipArmor(tmp);
                    }

                    UiManager.Instance.RefreshTooltip(tmp);
                }
                else if (HandScript.Instance.Movable == null && equippedArmor != null)
                {
                    HandScript.Instance.TakeMovable(equippedArmor);
                    CharacterPanel.Instance.SelectedButton = this;
                    icon.color = Color.gray;
                }
            }
        }

        public void EquipArmor(Armor armorToEquip)
        {
            if (equippedArmor != null)
            {
                if (equippedArmor != armorToEquip)
                    armorToEquip.Slot.AddItem(equippedArmor);

                UiManager.Instance.RefreshTooltip(equippedArmor);
            }
            else
            {
                UiManager.Instance.HideTooltip();
            }

            icon.enabled = true;
            icon.sprite = armorToEquip.Icon;
            icon.color = Color.white;
            equippedArmor = armorToEquip;
            equippedArmor.CharButton = this;
            equippedArmor.Remove();

            if (HandScript.Instance.Movable == (equippedArmor as IMovable))
                HandScript.Instance.Drop();

            if (gearSlots != null)
                gearSlots.Equip(equippedArmor);
        }

        public void DeEquipArmor(Armor armor)
        {
            icon.color = Color.white;
            icon.enabled = false;
            equippedArmor.CharButton = null;
            equippedArmor = null;

            if (gearSlots != null)
                gearSlots.DeEquip(armor);
        }
    }
}