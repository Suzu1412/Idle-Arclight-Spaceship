using UnityEngine;

public class AspectRatioUtility : MonoBehaviour
{
    [SerializeField] private Orientation _orientation = Orientation.Landscape;

    private float targetAspectRatio; // The desired aspect ratio, e.g., 16:9
    private Camera _camera;


    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void SetCameraAspect()
    {
        float windowAspect = 0;
        switch (_orientation)
        {
            case Orientation.Landscape:
                targetAspectRatio = 16f / 9f;
                windowAspect = (float)Screen.width / Screen.height;
                break;

            case Orientation.Portrait:
                targetAspectRatio = 9f / 16f;
                windowAspect = (float)Screen.height / Screen.width;
                break;
        }

        float scaleHeight = windowAspect / targetAspectRatio;

        if (scaleHeight < 1.0f)
        {
            // Letterboxing
            Rect rect = _camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            _camera.rect = rect;
        }
        else
        {
            // Pillarboxing
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = _camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            _camera.rect = rect;
        }
    }

    private void Update()
    {
        SetCameraAspect();

    }
}

public enum Orientation
{
    Landscape,
    Portrait
}