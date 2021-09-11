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
    public class LootButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI title;
        private LootWindow lootWindow;

        public Image Icon => icon;
        public TextMeshProUGUI Title => title;
        public Item Loot { get; set; }

        private void Awake()
        {
            lootWindow = GetComponentInParent<LootWindow>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            UiManager.Instance.ShowTooltip(new Vector2(1, 0), transform.position, Loot);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UiManager.Instance.HideTooltip();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (InventoryScript.Instance.AddItem(Loot))
            {
                gameObject.SetActive(false);
                lootWindow.TakeLoot(Loot);
                UiManager.Instance.HideTooltip();
            }
        }
    }
}