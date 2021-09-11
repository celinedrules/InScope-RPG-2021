using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Environment
{
    public class Obstacle : MonoBehaviour, IComparable<Obstacle>
    {
        [SerializeField] private bool makeTransparent;
        public bool MakeTransparent => makeTransparent;
        
        public SpriteRenderer SpriteRenderer { get; private set; }
        public SortingGroup SortingGroup { get; private set; }
        private Color defaultColor;
        private Color fadedColor;
    
        private void Start()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
            SortingGroup = GetComponent<SortingGroup>();
            defaultColor = SpriteRenderer.color;
            fadedColor = defaultColor;
            fadedColor.a = 0.7f;
        }

        public int CompareTo(Obstacle other)
        {
            if (SpriteRenderer.sortingOrder > other.SpriteRenderer.sortingOrder)
                return 1;
        
            if (SpriteRenderer.sortingOrder < other.SpriteRenderer.sortingOrder)
                return -1;

            return 0;
        }

        public void FadeIn()
        {
            SpriteRenderer.color = defaultColor;
        }
    
        public void FadeOut()
        {
            SpriteRenderer.color = fadedColor;
        }
    }
}