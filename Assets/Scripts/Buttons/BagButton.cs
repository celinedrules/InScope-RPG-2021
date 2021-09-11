using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Buttons
{
    public class BagButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Sprite fullSprite;
        [SerializeField] private Sprite emptySprite;
        [SerializeField] private int bagIndex;
    
        private Bag bag;

        public Bag Bag
        {
            get => bag;
            set
            {
                GetComponent<Image>().sprite = value != null ? fullSprite : emptySprite;
                bag = value;
            }
        }

        public int BagIndex => bagIndex;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (bag != null)
                bag.BagScript.OpenClose();
        }
    }
}