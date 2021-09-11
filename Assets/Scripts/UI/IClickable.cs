using TMPro;
using UnityEngine.UI;

namespace UI
{
    public interface IClickable
    {
        Image Icon { get; set; }
        int Count { get; }
        TextMeshProUGUI StackSizeText { get; }
    }
}