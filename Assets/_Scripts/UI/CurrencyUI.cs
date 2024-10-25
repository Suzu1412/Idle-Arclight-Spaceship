using UnityEngine;
using TMPro;

public class CurrencyUI : MonoBehaviour
{
    [Header("String Event Listener")]
    [SerializeField] private StringGameEventListener OnUpdateCurrencyTextListener;
    [SerializeField] private StringGameEventListener OnUpdateProductionTextListener;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _currencyText;
    [SerializeField] private TextMeshProUGUI _productionText;

    private void OnEnable()
    {
        OnUpdateCurrencyTextListener.Register(UpdateCurrencyText);
        OnUpdateProductionTextListener.Register(UpdateProductionText);
    }

    private void OnDisable()
    {
        OnUpdateCurrencyTextListener.DeRegister(UpdateCurrencyText);
        OnUpdateProductionTextListener.DeRegister(UpdateProductionText);
    }

    private void UpdateCurrencyText(string text)
    {
        _currencyText.text = text;
    }

    private void UpdateProductionText(string text)
    {
        _productionText.text = text + " CPS";
    }
}
