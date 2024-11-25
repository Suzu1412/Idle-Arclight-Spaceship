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
    [SerializeField] private Image _image;
    private float _duration;
    private FloatVariable _multiplier;
    [SerializeField] private float _easeDuration = 0.4f;
    public ObjectPooler Pool => _pool = _pool != null ? _pool : gameObject.GetOrAdd<ObjectPooler>();

    private void Awake()
    {
        _parentRectTransform = transform.GetComponent<RectTransform>();

        _localizedString = _localizedStringEvent.StringReference;

        if (!_localizedString.TryGetValue("multiplier", out var variable))
        {
            _multiplier = new FloatVariable();
            _localizedString.Add("multiplier", _multiplier);
        }
        else
        {
            _multiplier = variable as FloatVariable;
        }

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
        _localizedStringEvent.StringReference.SetReference("Tabla1", randomEvent.Description);
        _multiplier.Value = randomEvent.Multiplier;
        _localizedStringEvent.RefreshString();
    }


    private IEnumerator UpdateTimerCoroutine(BaseRandomEventSO randomEvent)
    {
        while (_duration > 0)
        {
            _duration -= 1f;
            //_detailsText.SetTextFormat(format: "{0} {1:00}s", _description, _duration);

            yield return Helpers.GetWaitForSeconds(1f);
        }

        _closePosition = new Vector2(_parentRectTransform.rect.width, transform.position.y);
        transform.DOLocalMoveX(_closePosition.x, _easeDuration).SetEase(Ease.InOutSine);
        yield return Helpers.GetWaitForSeconds(_easeDuration);
        ObjectPoolFactory.ReturnToPool(Pool);
    }
}
