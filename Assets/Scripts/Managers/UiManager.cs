using System;
using Buttons;
using Character;
using Inventory;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Managers
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private GameObject targetFrame;
        [SerializeField] private Image portraitFrame;
        [SerializeField] private InputActionAsset inputActions;
        [SerializeField] private ActionButton[] actionButtons;
        [SerializeField] private CanvasGroup keybindCanvas;
        [SerializeField] private CanvasGroup spellBookCanvas;
        [SerializeField] private CharacterPanel characterPanel;
        [SerializeField] private GameObject tooltip;
        [SerializeField] private RectTransform tooltipRect;

        private GameObject[] keyBindButtons;
        private Stat healthStat;
        private TextMeshProUGUI tooltipText;

        private static UiManager instance;

        private InputAction action1;
        private InputAction action2;
        private InputAction action3;

        private InputAction keybindAction;
        private InputAction spellBookAction;
        private InputAction inventoryAction;
        private InputAction characterPanelAction;

        public static UiManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<UiManager>();

                return instance;
            }
        }

        private void Awake()
        {
            var gameplayActionMap = inputActions.FindActionMap("Default");
            var menuActionMap = inputActions.FindActionMap("Menus");

            action1 = gameplayActionMap.FindAction("Action 1");
            action2 = gameplayActionMap.FindAction("Action 2");
            action3 = gameplayActionMap.FindAction("Action 3");

            keybindAction = menuActionMap.FindAction("Keybinds");
            spellBookAction = menuActionMap.FindAction("SpellBook");
            inventoryAction = menuActionMap.FindAction("Inventory");
            characterPanelAction = menuActionMap.FindAction("CharacterPanel");

            action1.performed += PerformAction;
            action2.performed += PerformAction;
            action3.performed += PerformAction;

            keybindAction.performed += ShowMenu;
            spellBookAction.performed += ShowMenu;
            inventoryAction.performed += ShowMenu;
            characterPanelAction.performed += ShowMenu;

            keyBindButtons = GameObject.FindGameObjectsWithTag("Keybind");

            tooltipText = tooltip.GetComponentInChildren<TextMeshProUGUI>();
        }

        private void ShowMenu(InputAction.CallbackContext context)
        {
            switch (context.action.bindings[0].action)
            {
                case "Keybinds":
                    OpenClose(keybindCanvas);
                    break;
                case "SpellBook":
                    OpenClose(spellBookCanvas);
                    break;
                case "Inventory":
                    InventoryScript.Instance.OpenClose();
                    break;
                case "CharacterPanel":
                    characterPanel.OpenClose();
                    break;
            }
        }

        private void Start()
        {
            healthStat = targetFrame.GetComponentInChildren<Stat>();
        }

        private void OnEnable()
        {
            action1.Enable();
            action2.Enable();
            action3.Enable();

            keybindAction.Enable();
            spellBookAction.Enable();
            inventoryAction.Enable();
            characterPanelAction.Enable();
        }

        private void OnDisable()
        {
            action1.Disable();
            action2.Disable();
            action3.Disable();

            keybindAction.Disable();
            spellBookAction.Disable();
            inventoryAction.Disable();
            characterPanelAction.Disable();
        }

        private void PerformAction(InputAction.CallbackContext context) =>
            ClickActionButton(context.action.bindings[0].action);

        private void ClickActionButton(string buttonName) =>
            Array.Find(actionButtons, x => x.gameObject.name == buttonName).Button.onClick.Invoke();

        public void HideTargetFrame() => targetFrame.SetActive(false);
        public void UpdateTargetFrame(float health) => healthStat.CurrentValue = health;

        public void ShowTargetFrame(Npc target)
        {
            if (target == null)
                return;

            targetFrame.SetActive(true);
            healthStat.Init(target.Health.CurrentValue, target.Health.MaxValue);
            portraitFrame.sprite = target.Portrait;
            target.HealthChanged += UpdateTargetFrame;
            target.CharacterRemoved += HideTargetFrame;
        }

        public void UpdateKeyText(string key, string code)
        {
            TextMeshProUGUI tmp = Array.Find(keyBindButtons, x => x.name == key)
                .GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = code;
        }

        public void UpdateStackSize(IClickable clickable)
        {
            if (clickable.Count > 1)
            {
                clickable.StackSizeText.text = clickable.Count.ToString();
                clickable.StackSizeText.color = Color.white;
                clickable.Icon.color = Color.white;
            }
            else
            {
                ClearStackCount(clickable);
            }

            if (clickable.Count != 0)
                return;

            clickable.StackSizeText.color = new Color(0, 0, 0, 0);
            clickable.Icon.color = new Color(0, 0, 0, 0);
        }

        public void ClearStackCount(IClickable clickable)
        {
            clickable.StackSizeText.color = new Color(0, 0, 0, 0);
            clickable.Icon.color = Color.white;
        }

        private void OpenClose(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
            canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts != true;
        }

        public void ShowTooltip(Vector2 pivot, Vector2 position, IDescribable description)
        {
            tooltipRect.pivot = pivot;
            tooltip.SetActive(true);
            tooltip.transform.position = position;
            tooltipText.text = description.GetDescription();
        }

        public void HideTooltip()
        {
            tooltip.SetActive(false);
        }

        public void RefreshTooltip(IDescribable description)
        {
            tooltipText.text = description.GetDescription();
        }
    }
}