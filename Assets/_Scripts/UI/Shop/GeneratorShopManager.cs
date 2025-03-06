using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorShopManager : MonoBehaviour
{
    [SerializeField] private GameObject shopPanel; // Shop UI Panel
    [SerializeField] private RectTransform contentPanel; // The parent that holds the buttons
    [SerializeField] private GameObject buttonPrefab; // Shop button prefab
    [SerializeField] private ScrollRect scrollRect; // ScrollRect for visibility checks
    [SerializeField] private int visibleItemCount; // Number of visible buttons at a time

    [Header("Data")]
    [SerializeField] private GeneratorDatabaseSO generators;
    [SerializeField] private CurrencyDataSO currencyData;
    [SerializeField] private IntVariableSO _amountToBuy;

    [Header("Event Binding")]
    [SerializeField] private IntGameEvent OnChangeBuyAmount;

    [Header("Generator UI")]
    [SerializeField] private Button _generatorShopDefaultButton;
    [SerializeField] private Image _buyButton1Image;
    [SerializeField] private Image _buyButton2Image;
    [SerializeField] private Image _buyButton3Image;
    [SerializeField] private Image _buyButton4Image;
    [SerializeField] private Sprite _buttonAmountChecked;
    [SerializeField] private Sprite _buttonAmountUnchecked;

    private Queue<GameObject> buttonPool = new Queue<GameObject>();
    private List<GameObject> activeButtons = new List<GameObject>(); // Track active buttons

    private float itemHeight;
    private int totalItems; // Total number of generators


    private void Start()
    {
        totalItems = GetTotalGenerators();
        itemHeight = buttonPrefab.GetComponent<RectTransform>().rect.height;
        visibleItemCount = Mathf.CeilToInt(scrollRect.GetComponent<RectTransform>().rect.height / itemHeight) + 2;

        scrollRect.onValueChanged.AddListener(OnScroll);
        InitializePool();
    }

    private void OnEnable()
    {
        ChangeBuyAmount(_amountToBuy.Value);
    }

    public void ChangeBuyAmount(int amount)
    {
        _amountToBuy.Value = amount;
        OnChangeBuyAmount.RaiseEvent(amount, this);
        _buyButton1Image.sprite = (amount == 1) ? _buttonAmountChecked : _buttonAmountUnchecked;
        _buyButton2Image.sprite = (amount == 5) ? _buttonAmountChecked : _buttonAmountUnchecked;
        _buyButton3Image.sprite = (amount == 10) ? _buttonAmountChecked : _buttonAmountUnchecked;
        _buyButton4Image.sprite = (amount == -1) ? _buttonAmountChecked : _buttonAmountUnchecked;
    }


    private void OnScroll(Vector2 position)
    {
        RefreshVisibleItems();
    }

    private int GetTotalGenerators()
    {
        return generators.GeneratorDictionary.Count;
    }

    private void InitializePool()
    {
        for (int i = 0; i < visibleItemCount; i++)
        {
            GameObject newItem = Instantiate(buttonPrefab, contentPanel);
            buttonPool.Enqueue(newItem);
            newItem.SetActive(false);
        }

        contentPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, totalItems * itemHeight);
        RefreshVisibleItems();
    }

    private void RefreshVisibleItems()
    {
        float scrollY = contentPanel.GetComponent<RectTransform>().anchoredPosition.y;
        int startIndex = Mathf.FloorToInt(scrollY / itemHeight);
        startIndex = Mathf.Clamp(startIndex, 0, totalItems - visibleItemCount);
        while (activeButtons.Count > 0)
        {
            GameObject item = activeButtons[0];
            activeButtons.RemoveAt(0);
            item.SetActive(false);
            buttonPool.Enqueue(item);
        }

        for (int i = 0; i < visibleItemCount && startIndex + i < totalItems; i++)
        {
            GameObject item = buttonPool.Dequeue();
            item.SetActive(true);
            activeButtons.Add(item);

            RectTransform itemRect = item.GetComponent<RectTransform>();
            itemRect.anchoredPosition = new Vector2(0, -((startIndex + i) * itemHeight));

            UpdateButtonData(item, startIndex + i);
        }
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
