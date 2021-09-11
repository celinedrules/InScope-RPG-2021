using Buttons;
using Character;
using UnityEngine;

namespace Items
{
    public class HealthPotion : Item, IUsable
    {
        [SerializeField] private int health;

        public void Use()
        {
            if (!(Player.Instance.Health.CurrentValue < Player.Instance.Health.MaxValue))
                return;
        
            Remove();

            Player.Instance.Health.CurrentValue += health;
        }

        public override string GetDescription()
        {
            string color = ColorUtility.ToHtmlStringRGBA(Color.green);
            return base.GetDescription() + $"\n<color=#{color}>Use: Restores {health} health</color>";
        }
    }
}