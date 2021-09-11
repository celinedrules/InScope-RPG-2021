using Dialogue;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace UI
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI aiText;
        [SerializeField] private Button nextButton;
        [SerializeField] private GameObject aiResponse;
        [SerializeField] private Transform choiceRoot;
        [SerializeField] private GameObject choicePrefab;
        [SerializeField] private TextMeshProUGUI conversantName;

        public TextMeshProUGUI AiText
        {
            get => aiText;
            set => aiText = value;
        }
        
        private PlayerConversant playerConversant;

        private void Start()
        {
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            playerConversant.OnConversationUpdated += UpdateUI;
            nextButton.onClick.AddListener(() => playerConversant.Next());
            
            UpdateUI();
        }

        private void UpdateUI()
        {
            gameObject.SetActive(playerConversant.IsActive);
            
            if(!playerConversant.IsActive)
                return;
            
            conversantName.text = playerConversant.GetConversantName();
            aiResponse.SetActive(!playerConversant.IsChoosing);
            choiceRoot.gameObject.SetActive(playerConversant.IsChoosing);

            if (playerConversant.IsChoosing)
            {
                BuildChoiceList();
            }
            else
            {
                aiText.text = playerConversant.Text;

                nextButton.GetComponentInChildren<TextMeshProUGUI>().text = !playerConversant.HasNext() ? "Close" : "Next";
            }
        }

        private void BuildChoiceList()
        {
            foreach (Transform item in choiceRoot)
                Destroy(item.gameObject);
            
            foreach (DialogueNode choiceNode in playerConversant.GetChoices())
            {
                choiceNode.IsSelected = true;
                GameObject choice = Instantiate(choicePrefab, choiceRoot);
                var text = choice.GetComponentInChildren<TextMeshProUGUI>();
                text.text = choiceNode.Text;

                choice.GetComponentInChildren<Button>().onClick
                    .AddListener(() => playerConversant.SelectChoice(choiceNode));
            }
        }
    }
}