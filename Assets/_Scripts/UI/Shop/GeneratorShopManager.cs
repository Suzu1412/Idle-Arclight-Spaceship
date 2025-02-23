using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorShopManager : MonoBehaviour
{
    [SerializeField] private GameObject shopPanel; // Shop UI Panel
    [SerializeField] private RectTransform contentPanel; // The parent that holds the buttons
    [SerializeField] private GameObject buttonPrefab; // Shop button prefab
    [SerializeField] private ScrollRect scrollRect; // ScrollRect for visibility checks
    [SerializeField] private int visibleCount = 7; // Number of visible buttons at a time

    [Header("Data")]
    [SerializeField] private GeneratorDatabaseSO generators;
    [SerializeField] private CurrencyDataSO currencyData;
    [Header("Events")]
    [SerializeField] private VoidGameEventBinding OnCurrencyChangedEventBinding;


    private Queue<GameObject> buttonPool = new Queue<GameObject>();
    private List<GameObject> activeButtons = new List<GameObject>(); // Track active buttons

    private float itemHeight;
    private float contentHeight;
    private int firstVisibleIndex = 0; // Tracks the first visible generator
    private int totalItems; // Total number of generators


    private void Start()
    {
        totalItems = generators.GeneratorDictionary.Count;

        if (totalItems == 0) return; // Avoid issues if the dictionary is empty

        itemHeight = buttonPrefab.GetComponent<RectTransform>().sizeDelta.y;
        contentHeight = totalItems * itemHeight;

        // Fix 1: Ensure Scrollbar Covers Full Area
        contentPanel.sizeDelta = new Vector2(contentPanel.sizeDelta.x, contentHeight);

        // Calculate how many buttons should be visible at once (plus a buffer)
        int visibleCount = Mathf.CeilToInt(scrollRect.viewport.rect.height / itemHeight) + 2;

        for (int i = 0; i < visibleCount; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, contentPanel);
            buttonPool.Enqueue(newButton);
            activeButtons.Add(newButton);
            UpdateButtonPosition(newButton, i);
            UpdateButtonData(newButton, i);
        }

        scrollRect.onValueChanged.AddListener(OnScroll);
    }

    private void OnScroll(Vector2 scrollPosition)
    {
        float scrollY = contentPanel.anchoredPosition.y;
        int newFirstVisibleIndex = Mathf.FloorToInt(scrollY / itemHeight);

        if (newFirstVisibleIndex != firstVisibleIndex)
        {
            firstVisibleIndex = newFirstVisibleIndex;

            for (int i = 0; i < activeButtons.Count; i++)
            {
                int newIndex = firstVisibleIndex + i;
                if (newIndex < totalItems && newIndex >= 0)
                {
                    UpdateButtonPosition(activeButtons[i], newIndex);
                    UpdateButtonData(activeButtons[i], newIndex);
                }
                else
                {
                    activeButtons[i].SetActive(false);
                }
            }
        }
    }

    private void UpdateButtonPosition(GameObject button, int index)
    {
        RectTransform rect = button.GetComponent<RectTransform>();

        // Ensure X position is centered if needed
        float xPos = rect.sizeDelta.x / 2; // Adjust if you need it centered (maybe rect.sizeDelta.x / 2?)

        // Ensure buttons are positioned correctly along Y-axis
        rect.anchoredPosition = new Vector2(xPos, -index * itemHeight);

        button.SetActive(true);
    }

    private void UpdateButtonData(GameObject button, int index)
    {
        if (generators.GeneratorDictionary.TryGetValue(index, out var generatorData))
        {
            button.GetComponent<GeneratorButton>().Initialize(generatorData, currencyData);
        }
        else
        {
            button.SetActive(false); // Hide button if no valid generator
        }
    }
}
