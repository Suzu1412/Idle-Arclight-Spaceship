using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private IntGameEventListener _currentHealthListener;

    private void OnEnable()
    {
        _currentHealthListener.Register(UpdateHealth);
    }

    private void OnDisable()
    {
        _currentHealthListener.DeRegister(UpdateHealth);
    }

    private void UpdateHealth(int amount)
    {
        _text.text = "" + amount;
    }

}
