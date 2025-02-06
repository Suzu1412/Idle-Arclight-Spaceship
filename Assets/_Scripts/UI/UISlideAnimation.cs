using UnityEngine;

public class UISlideAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private float _duration = 2f;
    [SerializeField] private float _startPositionX = 0;
    [SerializeField] private float _startPositionY = 0;

    void Awake()
    {
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
        Vector2 startPosition = new(_startPositionX - _rectTransform.anchoredPosition.x,_startPositionY - _rectTransform.anchoredPosition.y);

        UIAnimationManager.Instance.SlideFromStart(_rectTransform, startPosition, 3f);
    }
}
