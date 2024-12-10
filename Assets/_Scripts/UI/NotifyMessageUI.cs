using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

public class NotifyMessageUI : MonoBehaviour
{
    private ObjectPooler _pool;
    private Coroutine _updateTimerCoroutine;
    private RectTransform _parentRectTransform;
    private Vector2 _openPosition;
    private Vector2 _closePosition;
    private string _description;
    [SerializeField] private LocalizeStringEvent _localizedStringEvent;
    private LocalizedString _localizedString;
    [SerializeField] private string _table = "Tabla1";
    [SerializeField] private Image _image;
    private float _duration;
    private FloatVariable _multiplierVariable;
    private FloatVariable _durationVariable;
    [SerializeField] private float _easeDuration = 0.4f;
    public ObjectPooler Pool => _pool = _pool != null ? _pool : gameObject.GetOrAdd<ObjectPooler>();

    private void Awake()
    {
        _parentRectTransform = transform.GetComponent<RectTransform>();

        _localizedString = _localizedStringEvent.StringReference;

        GetMultiplierVariable();
        GetDurationVariable();

    }

    private void OnEnable()
    {
        if (_parentRectTransform == null)
        {
            _parentRectTransform = transform.GetComponent<RectTransform>();

        }
        _openPosition = Vector2.zero;
        transform.position = new Vector3(_parentRectTransform.rect.width, transform.position.y);
    }

    private void GetMultiplierVariable()
    {
        if (!_localizedString.TryGetValue("multiplier", out var variable))
        {
            _multiplierVariable = new FloatVariable();
            _localizedString.Add("multiplier", _multiplierVariable);
        }
        else
        {
            _multiplierVariable = variable as FloatVariable;
        }
    }

    private void GetDurationVariable()
    {
        if (!_localizedString.TryGetValue("duration", out var variable))
        {
            _durationVariable = new FloatVariable();
            _localizedString.Add("duration", _durationVariable);
        }
        else
        {
            _durationVariable = variable as FloatVariable;
        }
    }

    public void SetRandomEvent(BaseRandomEventSO randomEvent)
    {
        _image.sprite = randomEvent.Image;
        _duration = randomEvent.Duration;
        UpgradeDescription(randomEvent);
        _updateTimerCoroutine = StartCoroutine(UpdateTimerCoroutine(randomEvent));
        transform.DOLocalMoveX(_openPosition.x, _easeDuration).SetEase(Ease.InOutSine);
    }

    private void UpgradeDescription(BaseRandomEventSO randomEvent)
    {
        _localizedStringEvent.StringReference.SetReference(_table, randomEvent.Description);
        _multiplierVariable.Value = randomEvent.Multiplier;
        _localizedStringEvent.RefreshString();
    }

<<<<<<< Updated upstream
=======
    public async void SetShopMessage(INotification notification)
    {
        _closePosition = new Vector2(_parentRectTransform.rect.width, 0f);
        transform.localPosition = _closePosition;
        transform.DOLocalMoveX(_openPosition.x, _easeDuration).SetEase(Ease.InOutSine);

        _image.sprite = notification.Sprite;
        _localizedStringEvent.StringReference.SetReference(_table, notification.Message);
        _localizedStringEvent.RefreshString();

        await Awaitable.WaitForSecondsAsync(5f);
        transform.DOLocalMoveX(_closePosition.x, _easeDuration).SetEase(Ease.InOutSine);
        await Awaitable.WaitForSecondsAsync(_easeDuration);
        gameObject.SetActive(false);
    }

>>>>>>> Stashed changes

    private IEnumerator UpdateTimerCoroutine(BaseRandomEventSO randomEvent)
    {
        while (_duration > 0)
        {
            _duration -= 1f;
            _durationVariable.Value = _duration;

            yield return Helpers.GetWaitForSeconds(1f);
        }

        _closePosition = new Vector2(_parentRectTransform.rect.width, transform.position.y);
        transform.DOLocalMoveX(_closePosition.x, _easeDuration).SetEase(Ease.InOutSine);
        yield return Helpers.GetWaitForSeconds(_easeDuration);
        ObjectPoolFactory.ReturnToPool(Pool);
    }
}
