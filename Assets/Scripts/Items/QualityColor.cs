using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public  enum Quality {Common, Uncommon, Rare, Epic}
    
    public static  class QualityColor
    {
        public static Dictionary<Quality, string> Colors { get; } = new Dictionary<Quality, string>()
        {
            {Quality.Common, "#" + ColorUtility.ToHtmlStringRGBA(Color.gray)},
            {Quality.Uncommon, "#" + ColorUtility.ToHtmlStringRGBA(Color.green)},
            {Quality.Rare, "#" + ColorUtility.ToHtmlStringRGBA(Color.blue)},
            {Quality.Epic, "#" + ColorUtility.ToHtmlStringRGBA(Color.magenta)}
        };
    }
}