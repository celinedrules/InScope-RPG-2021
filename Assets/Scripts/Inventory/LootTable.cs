using System.Collections.Generic;
using Items;
using UI;
using UnityEngine;

namespace Inventory
{
    public class LootTable : MonoBehaviour
    {
        [SerializeField] Loot[] loot;

        private List<Item> droppedItems = new List<Item>();
        private bool rollded;

        public void ShowLoot()
        {
            if (!rollded)
                RollLoot();
        
            LootWindow.Instance.CreatePages(droppedItems);
        }

        private void RollLoot()
        {
            foreach (Loot item in loot)
            {
                int roll = Random.Range(0, 100);

                if (roll <= item.DropChance)
                    droppedItems.Add(item.Item);
            }

            rollded = true;
        }
    }
}