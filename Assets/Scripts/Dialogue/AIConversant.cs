using System.Collections.Generic;
using Character;
using Control;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {
        [SerializeField] private string conversantName;
        [SerializeField] private List<Dialogue> dialogues = new List<Dialogue>();

        private int currentDialogue;

        public string ConversantName => conversantName;

        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }

        public void StartDialogue(PlayerConversant callingController)
        {
            if(dialogues.Count ==0)
                return;
            
            callingController.StartDialogue(this, dialogues[currentDialogue]);
            
            if (currentDialogue < dialogues.Count - 1)
                currentDialogue++;
            else
                currentDialogue = 0;
        }
        
        public bool HandleRaycast(Player callingController)
        {
            if (dialogues.Count == 0)
                return false;

            Debug.Log(Mouse.current.leftButton.wasPressedThisFrame);
            
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                callingController.GetComponent<PlayerConversant>().StartDialogue(this, dialogues[currentDialogue]);
                
                if (currentDialogue < dialogues.Count - 1)
                    currentDialogue++;
                else
                    currentDialogue = 0;
            }



            return true;
        }
    }
}