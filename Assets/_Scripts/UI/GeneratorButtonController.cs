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
    [SerializeField] private TextMeshProUGUI _buyText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _productionText;
    [SerializeField] private Color _buttonAvailableColor;
    [SerializeField] private Color _buttonUnvailableColor;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Sprite _buttonAvailableSelectedSprite;

    public double Cost => _generator.Cost;

    public event UnityAction<int> OnBuyGeneratorClicked;

    private void OnEnable()
    {
        _buyButton.onClick.AddListener(HandleBuyButton);
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
        //_buyButton.interactable = val;
        if (val)
        {
            _priceText.color = _buttonAvailableColor;
            _buyText.color = _buttonAvailableColor;
        }
        else
        {
            _priceText.color = _buttonUnvailableColor;
            _buyText.color = _buttonUnvailableColor;
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
        _priceText.text = _generator.CostFormatted.GetFormat();
        //_unavailablePriceText.text = _generator.CostFormatted.GetFormat();
    }

    private void DisplayProductionText()
    {
        _productionText.text = $"{_generator.ProductionFormatted.GetFormat()} CPS";
    }


}
