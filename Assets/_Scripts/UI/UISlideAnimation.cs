using UnityEngine;

public class UISlideAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private VoidGameEventListener OnStartGameEventListener;
    [SerializeField] private float _duration = 2f;
    [SerializeField] private float _startPositionX = 0;
    [SerializeField] private float _startPositionY = 0;


    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        OnStartGameEventListener.Register(AnimateUI);
    }

    void OnDisable()
    {
        OnStartGameEventListener.DeRegister(AnimateUI);
    }

    private void AnimateUI()
    {
        UIAnimationManager.Instance.StartCoroutine(UIAnimationManager.Instance.Slide(_rectTransform, null, -500f, _duration));
    }
}
