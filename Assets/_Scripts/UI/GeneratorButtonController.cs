using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class GeneratorButtonController : MonoBehaviour
{
    [SerializeField] [ReadOnly] private GeneratorSO _generator;
    [SerializeField] [ReadOnly] private int _index;
    [SerializeField] private Image _generatorIcon;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private TextMeshProUGUI _availablePriceText;
    [SerializeField] private TextMeshProUGUI _unavailablePriceText;
    [SerializeField] private TextMeshProUGUI _productionText;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _unavailableButton;

    public double Cost => _generator.Cost;

    public event UnityAction<int> OnBuyGeneratorClicked;

    private void OnEnable()
    {
        _buyButton.onClick.AddListener(HandleBuyButton);
        _unavailableButton.interactable = false;
    }

    private void OnDisable()
    {
        _buyButton.onClick.RemoveAllListeners();
    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    public void SetGenerator(GeneratorSO generator)
    {
        _generator = generator;
    }

    public void PrepareButton()
    {
        DisplayImage();
        DisplayAmountOwned();
        DisplayName();
        DisplayPriceText();
        DisplayProductionText();
    }

    public void ToggleBuyButton(bool val)
    {
        if (val)
        {
            _buyButton.gameObject.SetActive(true);
            _unavailableButton.gameObject.SetActive(false);
        }
        else
        {
            _buyButton.gameObject.SetActive(false);
            _unavailableButton.gameObject.SetActive(true);
        }
    }

    public void HandleBuyButton()
    {
        OnBuyGeneratorClicked?.Invoke(_index);
    }

    private void DisplayImage()
    {
        _generatorIcon.sprite = _generator.Image;
    }

    private void DisplayAmountOwned()
    {
        _amountText.text = _generator.AmountOwned.ToString();
    }

    private void DisplayName()
    {
        _nameText.text = _generator.Name.ToString();
    }

    private void DisplayPriceText()
    {
        _availablePriceText.text = _generator.CostFormatted.GetFormat();
        _unavailablePriceText.text = _generator.CostFormatted.GetFormat();
    }

    private void DisplayProductionText()
    {
        _productionText.text = $"{_generator.ProductionFormatted.GetFormat()} CPS";
    }


}
