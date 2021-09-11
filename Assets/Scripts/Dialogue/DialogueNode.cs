using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Misc;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Dialogue
{
    [Serializable]
    public struct Size
    {
        public float width;
        public float height;
    }

    [Serializable]
    public struct Position
    {
        public float x;
        public float y;
    }

    [HideMonoScript]
    [Serializable]
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] private string text;
        
        [SerializeField, ValueDropdown("GetAllConversants")]
        private string speaker;

        [FoldoutGroup("Actions")]
        [SerializeField] private Actions onEnterAction = Actions.None;
        
        [FoldoutGroup("Actions")]
        [SerializeField] private Actions onExitAction = Actions.None;
        // Note: Not sure I want to do it this way.  If I do, I have to drag the correct node onto the 
        // Note: DialogueTrigger event for the object giving the item
        // [FoldoutGroup("Actions")]
        // [ShowIf("@onExitAction == Actions.GiveItem")]
        // [SerializeField] private InventoryItem itemToGive;
        //
        // public void GiveItem()
        // {
        //     GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().AddToFirstEmptySlot(itemToGive, 1);
        // }

        [FoldoutGroup("Conditions")]
        [LabelText("Requirements for the node to be visible")]
        [ListDrawerSettings(Expanded = true, ShowItemCount = false)]
        [SerializeField] private List<Condition> conditions = new List<Condition>();
        // [ListDrawerSettings(Expanded = true, ShowItemCount = false, CustomAddFunction = "AddCondition")]
        // [SerializeField] private List<Condition> conditions = new();
        //
        // public void AddCondition()
        // {
        //     Condition c = new Condition(this);
        //     conditions.Add(c);
        // }

        [FoldoutGroup("Children")]
        [SerializeField, LabelText("Child Nodes"), ReadOnly]
        private List<string> children = new List<string>();

        [FoldoutGroup("Details")]
        [SerializeField, Indent()] private Position position;

        [FoldoutGroup("Details")]
        [SerializeField, ReadOnly, Indent()] private Size size;

        private bool isPlayerSpeaking;
        private Rect rect = new Rect(0, 0, 150, 40);
        private bool canDrag;

        public bool IsSelected { get; set; }
        public DialogueNode Parent { get; set; }
        public List<string> Children => children;
        public Vector2 outPos;

        public Vector2 DragOffset { get; set; }

        public Rect Rect
        {
            get => new Rect(position.x, position.y, size.width, size.height);
            set => rect = value;
        }

        public bool CanDrag
        {
            get => canDrag;
            set => canDrag = value;
        }

        public bool IsPlayerSpeaking
        {
            get => speaker == DialogueManager.PlayerName;
            set
            {
                Undo.RecordObject(this, "Change Dialogue Speaker");
                isPlayerSpeaking = value;
                //speaker = isPlayerSpeaking ? Actors.Player : Actors.SomeNpc;
                //isPlayerSpeaking = value;
                Utils.MarkAsDirty(this);
            }
        }

        public string Text
        {
            get => text;
            set
            {
                if (text == value)
                    return;

                Undo.RecordObject(this, "Update Dialogue Text");
                text = value;
                Utils.MarkAsDirty(this);
            }
        }

        public string Speaker
        {
            get => speaker;
            set => speaker = value;
        }

        public Actions OnEnterAction => onEnterAction;
        public Actions OnExitAction => onExitAction;

        // public List<Condition> Conditions
        // {
        //     get => conditions;
        //     set => conditions = value;
        // }

        //public bool CheckCondition(IEnumerable<IPredicateEvaluator> evaluators) => condition.Check(evaluators);
        public bool CheckConditions(IEnumerable<IPredicateEvaluator> evaluators) => conditions.All(condition => condition.Check(evaluators));
        
        private static List<ValueDropdownItem> GetAllConversants()
        {
            List<ValueDropdownItem> conversants = new List<ValueDropdownItem>();
            
            conversants.AddRange(FindObjectsOfType<PlayerConversant>()
                .Select(x => new ValueDropdownItem("[Player] " + x.PlayerName, x.PlayerName)));
            conversants.AddRange(FindObjectsOfType<AIConversant>()
                .Select(x => new ValueDropdownItem("[AI] " +x.ConversantName, x.ConversantName)));

            return conversants;
        }

        // private void ChangeActor()
        // {
        //     IsPlayerSpeaking = isPlayerSpeaking;
        // }

        public void AddChild(string childID)
        {
            Undo.RecordObject(this, "Add Dialogue Link");
            Children.Add(childID);
            Utils.MarkAsDirty(this);
        }

        public void RemoveChild(string childID)
        {
            Undo.RecordObject(this, "Remove Dialogue Link");
            Children.Remove(childID);
            Utils.MarkAsDirty(this);
        }

#if UNITY_EDITOR
        public void SetPosition(Vector2 newPosition, float gridSnap)
        {
            float x = Mathf.Floor(newPosition.x / gridSnap) * gridSnap;
            float y = Mathf.Floor(newPosition.y / gridSnap) * gridSnap;

            Undo.RecordObject(this, "Move Dialogue Node");
            rect.position = new Vector2(x, y);
            position.x = rect.position.x;
            position.y = rect.position.y;
            Utils.MarkAsDirty(this);
        }

        public void SetSize(float width, float height)
        {
            size.width = width;
            size.height = height;
        }

        public void SetSize(Rect newSize) => rect = newSize;
#endif
    }
}