using System.Collections.Generic;
using System.Linq;
using Buttons;
using Items;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inventory
{
    public delegate void ItemCountChanged(Item item);
    
    public class InventoryScript : MonoBehaviour
    {
        public event ItemCountChanged ItemCountChanged;
    
    [SerializeField] private BagButton[] bagButtons;
    // Debug
    [SerializeField] private Item[] items;

    private Bag bag;
    private SlotScript fromSlot;
    
    private static InventoryScript instance;
    
    public static InventoryScript Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<InventoryScript>();

            return instance;
        }
    }

    public int EmptySlotCount => bag.BagScript.EmptySlotCount;
    public int TotalSlotCount => bag.BagScript.Slots.Count;
    public int FullSlotCount => TotalSlotCount - EmptySlotCount;

    public SlotScript FromSlot
    {
        get => fromSlot;
        set
        {
            fromSlot = value;
            
            if(fromSlot != null)
                fromSlot.Icon.color = Color.gray;
        }
    }

    private void Awake()
    {
        Bag newBag = (Bag) Instantiate(items[0]);
        newBag.Initialize(25);
        newBag.Use();
    }

    private void Update()
    {
        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            bag.ResizeBag(4);
            bagButtons[0].Bag = bag;
        }

        if (Keyboard.current.lKey.wasPressedThisFrame)
            AddItem((HealthPotion) Instantiate(items[1]));

        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            AddItem((Armor) Instantiate(items[2]));
            AddItem((Armor) Instantiate(items[3]));
            AddItem((Armor) Instantiate(items[4]));
            AddItem((Armor) Instantiate(items[5]));
            AddItem((Armor) Instantiate(items[6]));
            AddItem((Armor) Instantiate(items[7]));
            AddItem((Armor) Instantiate(items[8]));
            AddItem((Armor) Instantiate(items[9]));
            AddItem((Armor) Instantiate(items[10]));
            AddItem((Armor) Instantiate(items[11]));
            AddItem((Armor) Instantiate(items[12]));
        }
    }

    public void AddBag(Bag newBag)
    {
        foreach (var bagButton in bagButtons)
        {
            if (bagButton.Bag != null)
                continue;
            
            bagButton.Bag = newBag;
            bag = newBag;
            newBag.BagButton = bagButton;
            bag.BagScript.transform.SetSiblingIndex(bagButton.BagIndex);
            break;
        }
    }

    public bool AddItem(Item item)
    {
        if (item.StackSize > 0)
            if (PlaceInStack(item))
                return true;
        
        return PlaceInEmpty(item);
    }

    private bool PlaceInStack(Item item)
    {
        if (!bag.BagScript.Slots.Any(slot => slot.StackItem(item)))
            return false;
        
        OnItemCountChanged(item);
        
        return true;

    }

    private bool PlaceInEmpty(Item item)
    {
        if (bag.BagScript.AddItem(item))
        {
            OnItemCountChanged(item);
            return true;
        }

        return false;
    }
    
    public Stack<IUsable> GetUsables(IUsable type)
    {
        var usables = new Stack<IUsable>();

        foreach (var item in bag.BagScript.Slots.Where(slot => !slot.IsEmpty && slot.Item.GetType() == type.GetType()).SelectMany(slot => slot.Items))
            usables.Push(item as IUsable);

        return usables;
    }

    public void OnItemCountChanged(Item item) => ItemCountChanged?.Invoke(item);
    
    
    public void OpenClose() => bag.BagScript.OpenClose();
    }
}