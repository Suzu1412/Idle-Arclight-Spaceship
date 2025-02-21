using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorShopManager : MonoBehaviour
{
    [SerializeField] private GameObject shopPanel; // Shop UI Panel
    [SerializeField] private Transform shopContent; // ScrollView Content
    [SerializeField] private GameObject buttonPrefab; // Shop button prefab
    [SerializeField] private ScrollRect scrollRect; // ScrollRect for visibility checks

    [Header("Data")]
    [SerializeField] private GeneratorDatabaseSO generators;
    [SerializeField] private CurrencyDataSO currencyData;
    [Header("Events")]
    [SerializeField] private VoidGameEventBinding OnCurrencyChangedEventBinding;


    private Queue<GeneratorButton> buttonPool = new Queue<GeneratorButton>();
    private List<GeneratorButton> activeButtons = new List<GeneratorButton>();

    private int visibleItemCount;
    private float buttonHeight;

    private void Start()
    {
        //shopPanel.SetActive(false); // Hide shop initially
        buttonHeight = buttonPrefab.GetComponent<RectTransform>().rect.height;
        visibleItemCount = Mathf.CeilToInt(scrollRect.viewport.rect.height / buttonHeight) + 2; // Extra buffer
    }

    private void OnEnable()
    {
        OnCurrencyChangedEventBinding.Bind(UpdateShopButtons, this);
        scrollRect.onValueChanged.AddListener(_ => RecycleButtons());
        PopulateShop();
    }

    private void OnDisable()
    {
        OnCurrencyChangedEventBinding.Unbind(UpdateShopButtons, this);
        scrollRect.onValueChanged.RemoveListener(_ => RecycleButtons());
    }

    public void ToggleGeneratorShop()
    {
        shopPanel.SetActive(!shopPanel.activeSelf);
        if (shopPanel.activeSelf) PopulateShop();
    }

    private void PopulateShop()
    {
        // Clear any existing active buttons
        foreach (var button in activeButtons)
        {
            buttonPool.Enqueue(button);
            button.gameObject.SetActive(false);
        }
        activeButtons.Clear();

        // Instantiate only visible items
        for (int i = 0; i < visibleItemCount && i < generators.GeneratorDictionary.Count; i++)
        {
            var button = GetPooledButton();
            button.Initialize(generators.GeneratorDictionary[i], currencyData);
            PositionButton(button, i);
            activeButtons.Add(button);
        }
    }

    private void UpdateShopButtons()
    {
        foreach (var button in activeButtons)
        {
            button.UpdateButtonState(currencyData.TotalCurrency);
        }
    }

    private void RecycleButtons()
    {
        float contentY = shopContent.localPosition.y;

        for (int i = 0; i < activeButtons.Count; i++)
        {
            GeneratorButton button = activeButtons[i];
            int newIndex = Mathf.FloorToInt(contentY / buttonHeight) + i;

            if (newIndex >= 0 && newIndex < generators.GeneratorDictionary.Count)
            {
                button.Initialize(generators.GeneratorDictionary[newIndex], currencyData);
                PositionButton(button, newIndex);
            }
            else
            {
                button.gameObject.SetActive(false);
                buttonPool.Enqueue(button);
                activeButtons.RemoveAt(i);
                i--;
            }
        }

        // Fill missing visible slots
        while (activeButtons.Count < visibleItemCount && activeButtons.Count < generators.GeneratorDictionary.Count)
        {
            int newIndex = activeButtons.Count;
            var button = GetPooledButton();
            button.Initialize(generators.GeneratorDictionary[newIndex], currencyData);
            PositionButton(button, newIndex);
            activeButtons.Add(button);
        }
    }

    private GeneratorButton GetPooledButton()
    {
        if (buttonPool.Count > 0)
        {
            var reusedButton = buttonPool.Dequeue();
            reusedButton.gameObject.SetActive(true);
            return reusedButton;
        }
        else
        {
            var newButton = Instantiate(buttonPrefab, shopContent).GetComponent<GeneratorButton>();
            return newButton;
        }
    }

    private void PositionButton(GeneratorButton button, int index)
    {
        RectTransform rect = button.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0, -index * buttonHeight);
    }
}
