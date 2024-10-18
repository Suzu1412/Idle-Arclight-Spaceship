using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class TestTouch : MonoBehaviour
{
    private Camera _mainCam;
    private Vector3 _offset;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
        if (Touch.activeTouches.Count == 0 ||
            Touch.activeTouches[0].finger.index != 0)
        {
            return;
        }

        Touch myTouch = Touch.activeTouches[0];
        Vector3 touchPos = myTouch.screenPosition;
        touchPos = _mainCam.ScreenToWorldPoint(touchPos);

        if (Touch.activeTouches[0].phase == TouchPhase.Began)
        {
            _offset = touchPos - transform.position;
        }

        if (Touch.activeTouches[0].phase == TouchPhase.Moved)
        {
            transform.position = new Vector3(touchPos.x - _offset.x, touchPos.y - _offset.y, 0f);
        }
        if (Touch.activeTouches[0].phase == TouchPhase.Stationary)
        {
            transform.position = new Vector3(touchPos.x - _offset.x, touchPos.y - _offset.y, 0f);
        }
    }
}
