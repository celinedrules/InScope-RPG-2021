using System;
using Items;
using UnityEngine;

namespace Inventory
{
    [Serializable]
    public class Loot
    {
        [SerializeField] private Item item;
        [SerializeField] private float dropChance;

        public Item Item => item;
        public float DropChance => dropChance;
    }
}