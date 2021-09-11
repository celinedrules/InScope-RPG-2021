using System.Collections;
using UnityEngine;

namespace Dialogue
{
    public class BetterEventHandler : MonoBehaviour
    {
        private Dialogue dialogue;
        
        public BetterEvent EventHandler { get; set; }

        public Dialogue Dialogue
        {
            set => dialogue = value;
        }
        
        public void InitiateAction()
        {
            StartCoroutine(InDialogueBuffer());
        }

        private IEnumerator InDialogueBuffer()
        {
            yield return new WaitForSeconds(.01f);
     
            EventHandler.Invoke();
            // DialogueManager.CloseOptions();
            // DialogueManager.InDialogue = false;
            //
            // if (dialogue != null)
            //     DialogueManager.EnqueueDialogue(dialogue);
            // else
            //     DialogueManager.dialogueCanvas.alpha = 0;
        }
    }
}
