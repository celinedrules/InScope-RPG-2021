using System;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;

namespace Items
{
    public enum ArmorType
    {
        Head = 1,
        Hands = 3,
        Chest = 5,
        Belt = 6,
        Arms = 7,
        Feet = 9,
        MainHand = 11,
        OffHand = 13,
        TwoHand = 15,
        Legs = 17,
        Accessory = 19
    }

    [CreateAssetMenu(fileName = "Armor", menuName = "Items/Armor", order = 2)]
    public class Armor : Item
    {
        [SerializeField, OnValueChanged("Reset")]
        private ArmorType armorType;

        [SerializeField] private int intellect;
        [SerializeField] private int strength;
        [SerializeField] private int stamina;

        [DetailedInfoBox("Sprite Order", "\nTotal Sprites 4:\n\n" +
                                         "  Down\n  Left\n  Right\n  Up\n\n" +
                                         "Total Sprites 8:\n\n" +
                                         "  Left Side Down\n  Left Side Left\n  Left Side Right\n  Left Side Up\n" +
                                         "  Right Side Down\n  Right Side Left\n  Right Side Right\n  Right Side Up\n\n" +
                                         "Total Sprites 16:\n\n" +
                                         "  Left Upper Side Down\n  Left Upper Side Left\n  Left Upper Side Right\n  Left Upper Side Up\n" +
                                         "  Right Upper Side Down\n  Right Upper Side Left\n  Right Upper Side Right\n  Right Upper Side Up\n" +
                                         "  Left Lower Side Down\n  Left Lower Side Left\n  Left Lower Side Right\n  Left Lower Side Up\n" +
                                         "  Right Lower Side Down\n  Right Lower Side Left\n  Right Lower Side Right\n  Right Lower Side Up")]
        [SerializeField] private Sprite[] sprites;

        [SerializeField] private Color color = Color.white;

        public ArmorType Type => armorType;

        public Sprite[] Sprites
        {
            get => sprites;
            set => sprites = value;
        }

        public Color Color
        {
            get => color;
            set => color = value;
        }

        private void Reset()
        {
            if (armorType == ArmorType.Feet || armorType == ArmorType.Hands || armorType == ArmorType.Chest)
            {
                if (sprites != null)
                {
                    if (sprites.Length <= 4)
                        Array.Resize(ref sprites, 8);
                }
                else
                {
                    sprites = new Sprite[8];
                }
            }
            else if (armorType == ArmorType.Arms || armorType == ArmorType.Legs)
            {
                if (sprites != null)
                {
                    if (sprites.Length <= 8)
                        Array.Resize(ref sprites, 16);
                }
                else
                {
                    sprites = new Sprite[16];
                }
            }
            else
            {
                if (sprites != null)
                {
                    if (sprites.Length == 8 || sprites.Length < 4)
                        Array.Resize(ref sprites, 4);
                }
                else
                {
                    sprites = new Sprite[4];
                }
            }
        }

        public override string GetDescription()
        {
            string stats = string.Empty;

            if (intellect > 0)
                stats += string.Format($"\n +{intellect} intellect");

            if (strength > 0)
                stats += string.Format($"\n +{strength} strength");

            if (stamina > 0)
                stats += string.Format($"\n +{stamina} stamina");

            return base.GetDescription() + stats;
        }

        public void Equip()
        {
            CharacterPanel.Instance.EquipArmor(this);
        }
    }
}