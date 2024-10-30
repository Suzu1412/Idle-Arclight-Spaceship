using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private DoubleGameEventListener _totalCurrencyListener = default;
    [SerializeField] private TextMeshProUGUI _currencyText;

    private void OnEnable()
    {
        _totalCurrencyListener.Register(UpdateCurrencyText);
    }

    private void OnDisable()
    {
        _totalCurrencyListener.DeRegister(UpdateCurrencyText);
    }

    private void UpdateCurrencyText(double amount)
    {

        //_currencyText.text = FormatNumber.FormatDouble(amount);

    }



}
