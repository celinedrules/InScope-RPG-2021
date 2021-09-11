using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Objects;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.iOS;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] private string playerName;

        public event Action OnConversationUpdated;

        private Dialogue currentDialogue;
        private DialogueNode currentNode;
        private AIConversant currentConversant;

        public string Text => currentNode == null ? "" : currentNode.Text;

        public bool IsChoosing { get; private set; }
        public bool IsActive => currentDialogue != null;

        public string PlayerName => playerName;

        public void StartDialogue(Sign sign, Dialogue newDialogue)
        {
            DialogueUI dialogueUI = FindObjectOfType<DialogueUI>(true);
            
            if (sign.CenterText)
            {
                dialogueUI.AiText.verticalAlignment = VerticalAlignmentOptions.Middle;
                dialogueUI.AiText.horizontalAlignment = HorizontalAlignmentOptions.Center;
            }
            else
            {
                dialogueUI.AiText.verticalAlignment = VerticalAlignmentOptions.Top;
                dialogueUI.AiText.horizontalAlignment = HorizontalAlignmentOptions.Left;
            }

            currentDialogue = newDialogue;
            currentNode = currentDialogue.RootNode;
            currentNode.IsSelected = true;
            TriggerEnterAction();
            OnConversationUpdated?.Invoke();
        }
        
        public void StartDialogue(AIConversant newConversant, Dialogue newDialogue)
        {
            DialogueUI dialogueUI = FindObjectOfType<DialogueUI>(true);
            
            dialogueUI.AiText.verticalAlignment = VerticalAlignmentOptions.Top;
            dialogueUI.AiText.horizontalAlignment = HorizontalAlignmentOptions.Left;
            
            currentConversant = newConversant;
            currentDialogue = newDialogue;
            currentNode = currentDialogue.RootNode;
            currentNode.IsSelected = true;

            TriggerEnterAction();
            OnConversationUpdated?.Invoke();
        }

        private void Quit()
        {
            currentDialogue = null;

            TriggerExitAction();

            currentNode = null;
            currentConversant = null;
            IsChoosing = false;
            OnConversationUpdated?.Invoke();
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            currentNode.IsSelected = false;

            return FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode));
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            currentNode = chosenNode;
            TriggerEnterAction();
            IsChoosing = false;

            Next();

            if (currentNode != null)
                currentNode.IsSelected = true;
        }

        public void Next()
        {
            if (!HasNext())
            {
                Quit();
                return;
            }

            foreach (var dialogueNode in currentDialogue.Nodes)
                dialogueNode.IsSelected = false;

            int numPlayerResponses = FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode)).Count();

            if (numPlayerResponses > 0)
            {
                IsChoosing = true;

                TriggerExitAction();
                OnConversationUpdated?.Invoke();

                return;
            }

            var children = FilterOnCondition(currentDialogue.GetAIChildren(currentNode)).ToArray();
            int randomIndex = Random.Range(0, children.Length);

            TriggerExitAction();

            currentNode = children[randomIndex];

            TriggerEnterAction();
            OnConversationUpdated?.Invoke();
        }

        public bool HasNext() => FilterOnCondition(currentDialogue.GetAllChildren(currentNode)).Any();
        private IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNodes) => inputNodes.Where(node => node.CheckConditions(GetEvaluators()));
        private IEnumerable<IPredicateEvaluator> GetEvaluators() => GetComponents<IPredicateEvaluator>();
        
        private void TriggerEnterAction()
        {
            if (currentNode != null)
            {
                TriggerAction(currentNode.OnEnterAction);
            }
        }

        private void TriggerExitAction()
        {
            if (currentNode != null)
            {
                TriggerAction(currentNode.OnExitAction);
            }
        }

        private void TriggerAction(Actions action)
        {
            if (action == Actions.None)
                return;

            foreach (var trigger in currentConversant.GetComponents<DialogueTrigger>())
            {
                trigger.Trigger(action);
            }
        }

        public string GetConversantName() => IsChoosing
            ? currentDialogue.GetAllChildren(currentNode).ToArray()[0].Speaker
            : currentNode.Speaker;
    }
}