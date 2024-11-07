using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _loadingText;
    private Coroutine _moveTowardsTargetValue;
    private float _currentValue;

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
            _loadingText.text = "Loading... " + (_currentValue * 100).ToString("F0") + "%";
            yield return null;
        }
    }
}
