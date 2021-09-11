using System;
using Dialogue;
using Managers;
using UI;
using UnityEngine;

namespace Character
{
    public delegate void HealthChanged(float health);

    public delegate void CharacterRemoved();

    public class Npc : Character, IInteractable
    {
        public event HealthChanged HealthChanged;
        public event CharacterRemoved CharacterRemoved;

        [SerializeField] private Sprite portrait;

        private AIConversant aiConversant;

        public Sprite Portrait => portrait;

        private void Awake() => aiConversant = GetComponent<AIConversant>();

        public virtual Transform Select()
        {
            return hitBox;
        }

        public virtual void Deselect()
        {
            HealthChanged -= UiManager.Instance.UpdateTargetFrame;
            CharacterRemoved -= UiManager.Instance.HideTargetFrame;
        }

        protected void OnHealthChanged(float newHealth)
        {
            HealthChanged?.Invoke(newHealth);
        }

        public void OnCharacterRemoved()
        {
            CharacterRemoved?.Invoke();

            Destroy(gameObject);
        }

        public virtual void Interact()
        {
            if (aiConversant != null)
                aiConversant.StartDialogue(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>());
        }

        public virtual void StopInteract()
        {
        }
    }
}