using System.Collections.Generic;
using Buttons;
using Items;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LootWindow : MonoBehaviour
    {
        private static LootWindow instance;
    
    [SerializeField] private LootButton[] lootButtons;
    // Debug
    [SerializeField] private Item[] items;
    [SerializeField] private TextMeshProUGUI pageNumber;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject previousButton;
    
    private CanvasGroup canvasGroup;
    private List<Page> pages = new List<Page>();
    private List<Item> droppedLoot = new List<Item>();
    private int pageIndex = 0;

    public static LootWindow Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<LootWindow>();

            return instance;
        }
    }

    public bool IsOpen => canvasGroup.alpha > 0;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        // // Debug Only
        // var tmp = items.ToList();
        //
        // CreatePages(tmp);
    }

    public void CreatePages(List<Item> itemList)
    {
        if (!IsOpen)
        {
            Page page = new Page();
            droppedLoot = itemList;

            for (int i = 0; i < itemList.Count; i++)
            {
                page.Add(itemList[i]);

                if (page.Count == 4 || i == itemList.Count - 1)
                {
                    pages.Add(page);
                    page = new Page();
                }
            }

            AddLoot();
            Open();
        }
    }

    private void AddLoot()
    {
        if (pages.Count > 0)
        {
            pageNumber.text = pageIndex + 1 + "/" + pages.Count;
            
            previousButton.SetActive(pageIndex > 0);
            nextButton.SetActive(pages.Count > 1 && pageIndex < pages.Count - 1);
            
            for (int i = 0; i < pages[pageIndex].Count; i++)
            {
                if (pages[pageIndex][i] != null)
                {
                    lootButtons[i].Icon.sprite = pages[pageIndex][i].Icon;
                    lootButtons[i].Loot = pages[pageIndex][i];
                    lootButtons[i].gameObject.SetActive(true);
                    lootButtons[i].Title.text =
                        string.Format(
                            $"<color={QualityColor.Colors[pages[pageIndex][i].Quality]}>{pages[pageIndex][i].Title}</color>");
                }
            }
        }
    }

    public void ClearButtons()
    {
        foreach (var lootButton in lootButtons)
        {
            lootButton.gameObject.SetActive(false);
        }
    }
    
    [UsedImplicitly]
    public void NextPage()
    {
        if (pageIndex < pages.Count - 1)
        {
            pageIndex++;
            ClearButtons();
            AddLoot();
        }
    }

    [UsedImplicitly]
    public void PreviousPage()
    {
        if (pageIndex > 0)
        {
            pageIndex--;
            ClearButtons();
            AddLoot();
        }
    }

    public void TakeLoot(Item loot)
    {
        pages[pageIndex].Remove(loot);
        droppedLoot.Remove(loot);

        if (pages[pageIndex].Count == 0)
        {
            pages.Remove(pages[pageIndex]);

            if (pageIndex == pages.Count && pageIndex > 0)
            {
                pageIndex--;
            }
            
            AddLoot();
        }
    }

    public void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    [UsedImplicitly]
    public void Close()
    {
        pages.Clear();
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        ClearButtons();
    }
    }
}