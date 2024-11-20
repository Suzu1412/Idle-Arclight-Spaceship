using Cysharp.Text;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotifyMessageUI : MonoBehaviour
{
    private ObjectPooler _pool;
    private Coroutine _updateTimerCoroutine;
    private RectTransform _parentRectTransform;
    private Vector2 _openPosition;
    private Vector2 _closePosition;
    [SerializeField] private TextMeshProUGUI _detailsText;
    [SerializeField] private Image _image;
    private float _duration;
    [SerializeField] private float _easeDuration = 0.4f;
    public ObjectPooler Pool => _pool = _pool != null ? _pool : gameObject.GetOrAdd<ObjectPooler>();

    private void Start()
    {
        _parentRectTransform = transform.GetComponent<RectTransform>();

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
        _updateTimerCoroutine = StartCoroutine(UpdateTimerCoroutine(randomEvent));
        transform.DOLocalMoveX(_openPosition.x, _easeDuration).SetEase(Ease.InOutSine);
    }


    private IEnumerator UpdateTimerCoroutine(BaseRandomEventSO randomEvent)
    {
        while (_duration > 0)
        {
            _duration -= 1f;
            _detailsText.SetTextFormat("{0} {1:00}s", randomEvent.Description, _duration);
            yield return new WaitForSeconds(1);
        }

        _closePosition = new Vector2(_parentRectTransform.rect.width, transform.position.y);
        transform.DOLocalMoveX(_closePosition.x, _easeDuration).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(_easeDuration);
        ObjectPoolFactory.ReturnToPool(Pool);
    }
}
