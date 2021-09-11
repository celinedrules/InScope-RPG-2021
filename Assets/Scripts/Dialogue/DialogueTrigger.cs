using UltEvents;
using UnityEngine;

namespace Dialogue
{
    public enum Actions
    {
        GiveItem,
        GiveQuest,
        Attack,
        TestObjective,
        None
    }
    
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private Actions action;

        // [ShowIf("@action == Actions.GiveItem")]
        // [SerializeField] private InventoryItem itemToGive;
        [SerializeField] private UltEvent onTrigger;


        // Note: Not sure I want to do it this way.  This method should be somewhere else
        // public void GiveItem()
        // {
        //     GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().AddToFirstEmptySlot(itemToGive, 1);
        // }
        

        public void Trigger(Actions actionToTrigger)
        {
            if (actionToTrigger == action)
            {
                onTrigger?.Invoke();
            }
        }
    }
}