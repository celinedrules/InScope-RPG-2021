using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEngine;

namespace Character
{
    public class GearSlots : MonoBehaviour
    {
        [SerializeField] private GameObject downDirection;
        [SerializeField] private GameObject upDirection;
        [SerializeField] private GameObject leftDirection;
        [SerializeField] private GameObject rightDirection;
        [SerializeField] private Sprite emptySprite;
    
        private SpriteRenderer[] cachedUpSpriteRenderers;
        private SpriteRenderer[] cachedLeftSpriteRenderers;
        private SpriteRenderer[] cachedDownSpriteRenderers;
        private SpriteRenderer[] cachedRightSpriteRenderers;
        
        private void Awake() => UpdateSpriteRenderers();

    public void Equip(Armor armor)
    {
        UpdateSpritesForDirection(cachedDownSpriteRenderers, Character.FacingDirection.Down, armor, true);
        UpdateSpritesForDirection(cachedLeftSpriteRenderers, Character.FacingDirection.Left, armor, true);
        UpdateSpritesForDirection(cachedRightSpriteRenderers, Character.FacingDirection.Right, armor, true);
        UpdateSpritesForDirection(cachedUpSpriteRenderers, Character.FacingDirection.Up, armor, true);
    }

    public void DeEquip(Armor armor)
    {
        UpdateSpritesForDirection(cachedDownSpriteRenderers, Character.FacingDirection.Down, armor, false);
        UpdateSpritesForDirection(cachedLeftSpriteRenderers, Character.FacingDirection.Left, armor, false);
        UpdateSpritesForDirection(cachedRightSpriteRenderers, Character.FacingDirection.Right, armor, false);
        UpdateSpritesForDirection(cachedUpSpriteRenderers, Character.FacingDirection.Up, armor, false);
    }

    private SpriteRenderer GetSpriteRendererBySlotName(IEnumerable<SpriteRenderer> cachedSpriteRenderers, string slotName) =>
        cachedSpriteRenderers.FirstOrDefault(spriteRenderer => spriteRenderer.transform.parent.name == slotName);
    
    private void UpdateSpriteRenderers()
    {
        UpdateCachedSpriteRenderers(downDirection, ref cachedDownSpriteRenderers);
        UpdateCachedSpriteRenderers(upDirection, ref cachedUpSpriteRenderers);
        UpdateCachedSpriteRenderers(leftDirection, ref cachedLeftSpriteRenderers);

        if (rightDirection != leftDirection)
            UpdateCachedSpriteRenderers(rightDirection, ref cachedRightSpriteRenderers);
    }

    private void UpdateCachedSpriteRenderers(GameObject directionGameObject, ref SpriteRenderer[] cachedSpriteRenderers)
    {
        if (directionGameObject == null)
            return;

        if (cachedSpriteRenderers == null || cachedSpriteRenderers.Length == 0)
            cachedSpriteRenderers = directionGameObject.GetComponentsInChildren<SpriteRenderer>(true);
    }

    private void UpdateSpritesForDirection(SpriteRenderer[] cachedSpriteRenderers, Character.FacingDirection direction, Armor armor, bool equip)
    {
        if (cachedSpriteRenderers == null || cachedSpriteRenderers.Length <= 0)
            return;

        switch (armor.Type)
        {
            case ArmorType.Head:
                SpriteRenderer headwearSlotSr = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__headwear slot");
                UpdateSlots(headwearSlotSr, direction, armor, equip);
                break;
            case ArmorType.Chest:
                SpriteRenderer chestSlotSr = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__chest slot");
                SpriteRenderer bodySlotSr = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__body slot");
                UpdateSlots(new[]{chestSlotSr, bodySlotSr}, direction, armor, equip);
                break;
            case ArmorType.Arms:
            {
                SpriteRenderer leftUpperArmSlotSr = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__left upper arm slot");
                SpriteRenderer rightUpperArmSlotSr = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__right upper arm slot");
                SpriteRenderer leftLowerArmSlotSr = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__left lower arm slot");
                SpriteRenderer rightLowerArmSlotSr = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__right lower arm slot");
                UpdateSlots(new[] {leftUpperArmSlotSr, rightUpperArmSlotSr, leftLowerArmSlotSr, rightLowerArmSlotSr}, direction, armor, equip);
                break;
            }
            case ArmorType.Hands:
                SpriteRenderer leftHandSlotSr = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__left hand slot");
                SpriteRenderer rightHandSlotSr = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__right hand slot");
                UpdateSlots(new[] {leftHandSlotSr, rightHandSlotSr}, direction, armor, equip);
                break;
            case ArmorType.Legs:
                SpriteRenderer leftUpperLegSlotSr = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__left upper leg slot");
                SpriteRenderer rightUpperLegSlotSr = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__right upper leg slot");
                SpriteRenderer leftLowerLegSlotSr = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__left lower leg slot");
                SpriteRenderer rightLowerLegSlotSr = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__right lower leg slot");
                UpdateSlots(new[] {leftUpperLegSlotSr, rightUpperLegSlotSr, leftLowerLegSlotSr, rightLowerLegSlotSr}, direction, armor, equip);
                break;
            case ArmorType.Feet:
                SpriteRenderer leftFootSlotSr = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__left foot slot");
                SpriteRenderer rightFootSlotSr = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__right foot slot");
                UpdateSlots(new[] {leftFootSlotSr, rightFootSlotSr}, direction, armor, equip);
                break;
            case ArmorType.MainHand:
                SpriteRenderer swingWeaponSlotSr = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__swing weapon slot");
                UpdateSlots(swingWeaponSlotSr, direction, armor, equip);
                break;
            case ArmorType.OffHand:
                break;
            case ArmorType.TwoHand:
                break;
            case ArmorType.Accessory:
                break;
            case ArmorType.Belt:
                SpriteRenderer beltSlotSr = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__belt slot");
                UpdateSlots(beltSlotSr, direction, armor, equip);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateSlots(SpriteRenderer[] spriteRenderer, Character.FacingDirection direction, Armor armor, bool equip)
    {
        switch (direction)
        {
            case Character.FacingDirection.Down:
                UpdateSprite(spriteRenderer[0], equip is true ? armor.Sprites[0] : emptySprite, armor.Color);
                UpdateSprite(spriteRenderer[1], equip is true ? armor.Sprites[4] : emptySprite, armor.Color);

                if (spriteRenderer.Length == 4)
                {
                    UpdateSprite(spriteRenderer[2], equip is true ? armor.Sprites[8] : emptySprite, armor.Color);
                    UpdateSprite(spriteRenderer[3], equip is true ? armor.Sprites[12] : emptySprite, armor.Color);
                }
                break;
            case Character.FacingDirection.Left:
                UpdateSprite(spriteRenderer[0], equip is true ? armor.Sprites[1] : emptySprite, armor.Color);
                UpdateSprite(spriteRenderer[1], equip is true ? armor.Sprites[5] : emptySprite, armor.Color);
                
                if (spriteRenderer.Length == 4)
                {
                    UpdateSprite(spriteRenderer[2], equip is true ? armor.Sprites[9] : emptySprite, armor.Color);
                    UpdateSprite(spriteRenderer[3], equip is true ? armor.Sprites[13] : emptySprite, armor.Color);
                }
                break;
            case Character.FacingDirection.Right:
                UpdateSprite(spriteRenderer[0], equip is true ? armor.Sprites[2] : emptySprite, armor.Color);
                UpdateSprite(spriteRenderer[1], equip is true ? armor.Sprites[6] : emptySprite, armor.Color);
                
                if (spriteRenderer.Length == 4)
                {
                    UpdateSprite(spriteRenderer[2], equip is true ? armor.Sprites[10] : emptySprite, armor.Color);
                    UpdateSprite(spriteRenderer[3], equip is true ? armor.Sprites[14] : emptySprite, armor.Color);
                }
                break;
            case Character.FacingDirection.Up:
                UpdateSprite(spriteRenderer[0], equip is true ? armor.Sprites[3] : emptySprite, armor.Color);
                UpdateSprite(spriteRenderer[1], equip is true ? armor.Sprites[7] : emptySprite, armor.Color);
                
                if (spriteRenderer.Length == 4)
                {
                    UpdateSprite(spriteRenderer[2], equip is true ? armor.Sprites[11] : emptySprite, armor.Color);
                    UpdateSprite(spriteRenderer[3], equip is true ? armor.Sprites[15] : emptySprite, armor.Color);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }
    
    private void UpdateSlots(SpriteRenderer spriteRenderer, Character.FacingDirection direction, Armor armor, bool equip)
    {
        switch (direction)
        {
            case Character.FacingDirection.Down:
                UpdateSprite(spriteRenderer, equip is true ? armor.Sprites[0] : emptySprite, armor.Color);
                break;
            case Character.FacingDirection.Left:
                UpdateSprite(spriteRenderer, equip is true ? armor.Sprites[1] : emptySprite, armor.Color);
                break;
            case Character.FacingDirection.Right:
                UpdateSprite(spriteRenderer, equip is true ? armor.Sprites[2] : emptySprite, armor.Color);
                break;
            case Character.FacingDirection.Up:
                UpdateSprite(spriteRenderer, equip is true ? armor.Sprites[3] : emptySprite, armor.Color);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    private void UpdateSprite(SpriteRenderer spriteRenderer, Sprite sprite, Color color)
    {
        if (spriteRenderer == null)
            return;

        spriteRenderer.color = color;

        if (sprite != null)
            spriteRenderer.sprite = sprite;
    }
    }
}
