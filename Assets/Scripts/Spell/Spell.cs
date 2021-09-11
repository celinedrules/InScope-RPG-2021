using Buttons;
using Character;
using UI;
using UnityEngine;

namespace Spell
{
    public class Spell : IUsable, IMovable, IDescribable
    {
        [SerializeField] private GameObject spellPrefab;
        [SerializeField] private Sprite icon;
        [SerializeField] private string name;
        [SerializeField] private string description;
        [SerializeField] private int damage;
        [SerializeField] private float speed;
        [SerializeField] private float castTime;
        [SerializeField] private Color barColor;

        public string Name => name;
        public int Damage => damage;
        public Sprite Icon => icon;

        public float Speed => speed;
        public float CastTime => castTime;
        public GameObject SpellPrefab => spellPrefab;
        public Color BarColor => barColor;
        Sprite IMovable.Icon => icon;

        public void Use()
        {
            Player.Instance.CastSpell(name);
        }

        public string GetDescription()
        {
            string color = ColorUtility.ToHtmlStringRGBA(Color.yellow);
            return string.Format($"{name}\nCast Time: {castTime} second(s)" + "" +
                                 $"\n<color=#{color}>{description}\nthat causes {damage} damage</color>");
        }
    }
}