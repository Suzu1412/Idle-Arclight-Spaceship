using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

[RequireComponent(typeof(CinemachineCamera))]
public class DynamicCameraBoundsManager : MonoBehaviour
{
    public PolygonCollider2D boundingShape; // Assign your bounding shape here
    private CinemachineConfiner2D confiner;
    private CinemachineCamera virtualCamera;

    private int previousWidth;
    private int previousHeight;
    private ScreenOrientation previousOrientation;


    void Start()
    {
        // Initialize tracking variables
        previousWidth = Screen.width;
        previousHeight = Screen.height;
        previousOrientation = Screen.orientation;

        virtualCamera = GetComponent<CinemachineCamera>();
        confiner = GetComponent<CinemachineConfiner2D>();

        if (confiner != null && boundingShape != null)
        {
            // Assign the bounding shape to the confiner
            confiner.BoundingShape2D = boundingShape;

            // Adjust the camera size
            AdjustOrthographicSize();
        }

        StartCoroutine(TrackOrientationChange());
    }

    void AdjustOrthographicSize()
    {
        if (virtualCamera != null && virtualCamera.Lens.Orthographic)
        {
            Camera mainCamera = Camera.main;
            float screenAspect = (float)Screen.width / Screen.height;
            float boundingAspect = boundingShape.bounds.size.x / boundingShape.bounds.size.y;

            // Adjust orthographic size based on aspect ratio
            if (screenAspect >= boundingAspect)
            {
                virtualCamera.Lens.OrthographicSize = boundingShape.bounds.size.y / 2;
            }
            else
            {
                virtualCamera.Lens.OrthographicSize = boundingShape.bounds.size.x / (2 * screenAspect);
            }
        }
    }

    private IEnumerator TrackOrientationChange()
    {
        yield return Helpers.GetWaitForSeconds(0.5f);
        AdjustOrthographicSize();

        while (true)
        {
            yield return Helpers.GetWaitForSeconds(0.5f);

            // Check for screen size changes
            if (Screen.width != previousWidth || Screen.height != previousHeight)
            {
                previousWidth = Screen.width;
                previousHeight = Screen.height;

                Debug.Log($"Screen size changed: {Screen.width}x{Screen.height}");
                //OnScreenSizeChanged?.Invoke(); // Trigger the event
                AdjustOrthographicSize();
            }

            // Check for orientation changes
            if (Screen.orientation != previousOrientation)
            {
                previousOrientation = Screen.orientation;

                Debug.Log($"Orientation changed: {Screen.orientation}");
                //OnOrientationChanged?.Invoke(); // Trigger the event
                AdjustOrthographicSize();
            }
        }
    }
}
