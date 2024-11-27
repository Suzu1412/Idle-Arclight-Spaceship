using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private LocalizeStringEvent _localizedStringEvent;
    private LocalizedString _localizedString;
    private Coroutine _moveTowardsTargetValue;
    private string _loading;
    private float _currentValue;
    private FloatVariable _amount = null;

    [Header("Float Event Listener")]
    [SerializeField] private FloatGameEventListener OnLoadProgressEventListener;

    private void OnEnable()
    {
        _currentValue = 0;
        OnLoadProgressEventListener.Register(UpdateUIProgressBar);
    }

    private void OnDisable()
    {
        OnLoadProgressEventListener.DeRegister(UpdateUIProgressBar);
    }

    private void Awake()
    {
        _localizedString = _localizedStringEvent.StringReference;

        if (!_localizedString.TryGetValue("amount", out var variable))
        {
            _amount = new FloatVariable();
            _localizedString.Add("amount", _amount);
        }
        else
        {
            _amount = variable as FloatVariable;
        }
    }

    private void UpdateUIProgressBar(float targetValue)
    {
        if (_moveTowardsTargetValue != null) StopCoroutine(_moveTowardsTargetValue);
        _moveTowardsTargetValue = StartCoroutine(MoveTowardsTargetValueCoroutine(targetValue));
    }

    private IEnumerator MoveTowardsTargetValueCoroutine(float target)
    {
        var rate = (target - _currentValue) * 4;
        while (_currentValue != target)
        {
            _currentValue = Mathf.MoveTowards(_currentValue, target, rate * Time.deltaTime);
            _slider.value = _currentValue;
            if (_amount != null)
            {
                _amount.Value = Mathf.Round(_currentValue * 100);
            }
            yield return null;
        }
    }
}
