using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;
using Cysharp.Text;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _loadingText;
    private Coroutine _moveTowardsTargetValue;
    private string _loading;
    private float _currentValue;

    [Header("Float Event Listener")]
    [SerializeField] private FloatGameEventListener OnLoadProgressEventListener;

    private void Start()
    {
        LoadingTextTranslate();
    }

    private void OnEnable()
    {
        _currentValue = 0;
        OnLoadProgressEventListener.Register(UpdateUIProgressBar);
    }

    private void OnDisable()
    {
        OnLoadProgressEventListener.DeRegister(UpdateUIProgressBar);
    }

    private async void LoadingTextTranslate()
    {
        var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("Tabla1", "Loading");
        await op.Task;

        if (op.IsDone)
        {
            _loading = op.Result;
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
            _loadingText.SetTextFormat("{0}... {1:F0}%", _loading, (_currentValue * 100));
            yield return null;
        }
    }
}
