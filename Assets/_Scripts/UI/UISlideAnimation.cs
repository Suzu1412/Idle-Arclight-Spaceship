using UnityEngine;

public class UISlideAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private float _duration = 2f;
    [SerializeField] private float _startPositionX = 0;
    [SerializeField] private float _startPositionY = 0;
    [SerializeField] private UIAnimationManager _animationManager;

    void Awake()
    {
        if (_animationManager == null) _animationManager = FindAnyObjectByType<UIAnimationManager>();
        _rectTransform = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        AnimateUI();
    }

    void OnDisable()
    {
    }

    private void AnimateUI()
    {
        Vector2 startPosition = new(_startPositionX - _rectTransform.anchoredPosition.x, _startPositionY - _rectTransform.anchoredPosition.y);

        _animationManager.SlideFromStart(_rectTransform, startPosition, _duration);
    }
}
