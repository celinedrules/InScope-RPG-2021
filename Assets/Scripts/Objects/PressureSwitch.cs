using System;
using UnityEngine;

namespace Objects
{
    public class PressureSwitch : ShowHideObject
    {
        [SerializeField] private bool active;
        [SerializeField] private bool isPressure;
        [SerializeField] private Sprite activeSprite;
        [SerializeField] private Sprite inactiveSprite;

        private SpriteRenderer spriteRenderer;

        protected override void Clear()
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();

            if (active)
                ActivateSwitch();
        }

        protected override void Update()
        {
            if (targetGameObject == null)
                return;

            switch (action)
            {
                case TargetAction.Show:
                    targetGameObject.SetActive(active);
                    break;
                case TargetAction.Hide:
                    targetGameObject.SetActive(!active);
                    break;
                case TargetAction.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ActivateSwitch()
        {
            active = true;
            spriteRenderer.sprite = activeSprite;
        }

        private void DeactivateSwitch()
        {
            active = false;
            spriteRenderer.sprite = inactiveSprite;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") || other.CompareTag("Pushable"))
                ActivateSwitch();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player") && !other.CompareTag("Pushable"))
                return;

            if (isPressure)
                DeactivateSwitch();
        }
    }
}