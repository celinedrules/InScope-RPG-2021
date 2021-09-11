using System;
using System.Collections.Generic;
using Dialogue;
using UI;
using Unity.VisualScripting;
using UnityEngine;


namespace Objects
{
    public class Sign : MonoBehaviour, IInteractable
    {
        [SerializeField] private string message;
        [SerializeField] private bool centerText;
        private PlayerConversant playerConversant;
        private Dialogue.Dialogue dialogue;

        public bool CenterText
        {
            get => centerText;
            set => centerText = value;
        }
        
        private void Awake()
        {
            dialogue = ScriptableObject.CreateInstance<Dialogue.Dialogue>();
            DialogueNode node = ScriptableObject.CreateInstance<DialogueNode>();
            node.Text = message;
            dialogue.Nodes.Add(node);
            dialogue.RootNode.Text = message;
        }

        public void Interact()
        {
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            playerConversant.StartDialogue(this, dialogue);
        }

        public void StopInteract()
        {
            
        }
    }
}
