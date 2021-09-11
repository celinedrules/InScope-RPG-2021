using System.Collections.Generic;
using Environment;
using UnityEngine;
using UnityEngine.Rendering;

namespace Character
{
    public class LayerSorter : MonoBehaviour
    {
        private SortingGroup sortingGroup;
        private readonly List<Obstacle> obstacles = new List<Obstacle>();

        private void Start() => sortingGroup = GetComponentInParent<SortingGroup>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            Obstacle obstacle = other.GetComponent<Obstacle>();

            if (obstacle == null)
                return;

            if (obstacle.MakeTransparent)
                obstacle.FadeOut();

            if (obstacles.Count == 0 || obstacle.SortingGroup.sortingOrder - 1 < sortingGroup.sortingOrder)
                sortingGroup.sortingOrder = obstacle.SortingGroup.sortingOrder - 1;

            obstacles.Add(obstacle);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Obstacle obstacle = other.GetComponent<Obstacle>();

            if (obstacle == null) return;

            if (obstacle.MakeTransparent)
                obstacle.FadeIn();
            
            obstacles.Remove(obstacle);

            if (obstacles.Count == 0)
            {
                sortingGroup.sortingOrder = 200;
            }
            else
            {
                obstacles.Sort();
                sortingGroup.sortingOrder = obstacles[0].SortingGroup.sortingOrder - 1;
            }
        }
    }
}