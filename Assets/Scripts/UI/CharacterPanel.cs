using System;
using Buttons;
using Items;
using UnityEngine;

namespace UI
{
    public class CharacterPanel : MonoBehaviour
    {
        private static CharacterPanel instance;

    public static CharacterPanel Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<CharacterPanel>();

            return instance;
        }
    }
    
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CharButton head;
    [SerializeField] private CharButton chest;
    [SerializeField] private CharButton offHand;
    [SerializeField] private CharButton legs;
    [SerializeField] private CharButton belt;
    [SerializeField] private CharButton feet;
    [SerializeField] private CharButton arms;
    [SerializeField] private CharButton hands;
    [SerializeField] private CharButton mainHand;
    [SerializeField] private CharButton accessory1;
    [SerializeField] private CharButton accessory2;
    [SerializeField] private CharButton accessory3;

    public CharButton SelectedButton { get; set; }
    
    public void OpenClose()
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts != true;
    }

    public void EquipArmor(Armor armor)
    {
        switch (armor.Type)
        {
            case ArmorType.Head:
                head.EquipArmor(armor);
                break;
            case ArmorType.Hands:
                hands.EquipArmor(armor);
                break;
            case ArmorType.Chest:
                chest.EquipArmor(armor);
                break;
            case ArmorType.Feet:
                feet.EquipArmor(armor);
                break;
            case ArmorType.MainHand:
                mainHand.EquipArmor(armor);
                break;
            case ArmorType.OffHand:
                offHand.EquipArmor(armor);
                break;
            case ArmorType.TwoHand:
                //twoHand.EquipArmor(armor);
                break;
            case ArmorType.Legs:
                legs.EquipArmor(armor);
                break;
            case ArmorType.Accessory:
                //accessory1.EquipArmor(armor);
                break;
            case ArmorType.Arms:
                arms.EquipArmor(armor);
                break;
            case ArmorType.Belt:
                belt.EquipArmor(armor);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    }
}