using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dialogue
{
    public class DialogueButton : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private Image selectIcon;

        [SerializeField] private Image cancelIcon;

        public bool IsCancelButton
        {
            set => cancelIcon.enabled = value;
        }
    
        public void OnSelect(BaseEventData eventData)
        {
            selectIcon.enabled = true;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            selectIcon.enabled = false;
        }
    }
}
