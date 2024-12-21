using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
    private StringVariable _amountVariable;
    private LocalizedString _gemsLocalized;
    private LocalizedString _shopLocalized;
    private Coroutine _animateCoroutine;
    private Coroutine _returnToPoolCoroutine;

    [SerializeField] private float _easeDuration = 0.4f;
    public ObjectPooler Pool => _pool = _pool != null ? _pool : _pool = gameObject.GetOrAdd<ObjectPooler>();

    private void Awake()
    {
        _parentRectTransform = transform.GetComponent<RectTransform>();

        _localizedString = _localizedStringEvent.StringReference;

        GetMultiplierVariable();
        GetDurationVariable();
        GetAmountVariable();

    }

    private void Start()
    {
        if (_parentRectTransform == null)
        {
            _parentRectTransform = transform.GetComponent<RectTransform>();

        }
    }

    private void OnDisable()
    {
        transform.DOKill();
        StopAllCoroutines();
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

    private void GetAmountVariable()
    {
        if (!_localizedString.TryGetValue("amount", out var variable))
        {
            _amountVariable = new StringVariable();
            _localizedString.Add("amount", _multiplierVariable);
        }
        else
        {
            _amountVariable = variable as StringVariable;
        }
    }


    public void SetRandomEvent(BaseRandomEventSO randomEvent)
    {
        _image.sprite = randomEvent.Image;
        _duration = randomEvent.Duration;
        RandomEventDescription(randomEvent);
        _updateTimerCoroutine = StartCoroutine(UpdateTimerCoroutine(randomEvent));
        transform.DOLocalMoveX(_openPosition.x, _easeDuration).SetEase(Ease.InOutSine);
    }

    private void RandomEventDescription(BaseRandomEventSO randomEvent)
    {
        _localizedStringEvent.StringReference.SetReference(_table, randomEvent.Description);
        _multiplierVariable.Value = randomEvent.Multiplier;
        _localizedStringEvent.RefreshString();
    }

    public void SetShopMessage(INotification notification)
    {
        _image.sprite = notification.Sprite;
        transform.localPosition = _closePosition;

        _localizedStringEvent.StringReference.SetReference(_table, notification.Message);
        _localizedStringEvent.RefreshString();

        if (_returnToPoolCoroutine != null) StopCoroutine(_returnToPoolCoroutine);
        _returnToPoolCoroutine = StartCoroutine(ReturnToPoolCoroutine(5f));

    }

    public void SetOfflineMessage(INotification notification)
    {
        _localizedStringEvent.StringReference.SetReference(_table, notification.Message);

        _amountVariable.Value = notification.Amount;
        _localizedStringEvent.RefreshString();

        if (_returnToPoolCoroutine != null) StopCoroutine(_returnToPoolCoroutine);
        _returnToPoolCoroutine = StartCoroutine(ReturnToPoolCoroutine(5f));

    }


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
        if (_returnToPoolCoroutine != null) StopCoroutine(_returnToPoolCoroutine);
        _returnToPoolCoroutine = StartCoroutine(ReturnToPoolCoroutine(_easeDuration));
    }

    private IEnumerator ReturnToPoolCoroutine(float delay)
    {
        yield return Helpers.GetWaitForSeconds(delay);
        ObjectPoolFactory.ReturnToPool(Pool);
    }
}
