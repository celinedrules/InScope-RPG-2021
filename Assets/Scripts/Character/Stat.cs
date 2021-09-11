using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Character
{
    public class Stat : MonoBehaviour
    {
        [SerializeField] private float lerpSpeed;
        [SerializeField] private TextMeshProUGUI statValue;
        private Image content;
        private float currentValue;
        private float currentFill;

        public float MaxValue { get; private set; }

        public float CurrentValue
        {
            get => currentValue;
            set
            {
                if (value > MaxValue)
                    currentValue = MaxValue;
                else if (value < 0)
                    currentValue = 0;
                else
                    currentValue = value;

                currentFill = currentValue / MaxValue;

                if (statValue != null)
                    statValue.text = currentValue + " / " + MaxValue;
            }
        }

        private void Start()
        {
            content = GetComponent<Image>();
        }

        private void Update()
        {
            if (currentFill != content.fillAmount)
                content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }

        public void Init(float curValue, float maxValue)
        {
            if (content == null)
                content = GetComponent<Image>();

            MaxValue = maxValue;
            CurrentValue = curValue;
            content.fillAmount = CurrentValue / MaxValue;
        }
    }
}