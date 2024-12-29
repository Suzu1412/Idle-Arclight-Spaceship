using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(CinemachineCamera))]
public class DynamicCameraBoundsManager : MonoBehaviour
{
    public PolygonCollider2D boundingShape; // Assign your bounding shape here
    private CinemachineConfiner2D confiner;
    private CinemachineCamera virtualCamera;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineCamera>();
        confiner = GetComponent<CinemachineConfiner2D>();

        if (confiner != null && boundingShape != null)
        {
            // Assign the bounding shape to the confiner
            confiner.BoundingShape2D = boundingShape;

            // Adjust the camera size
            AdjustOrthographicSize();
        }
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

    void Update()
    {
        // Recalculate orthographic size on resolution or aspect ratio change
        AdjustOrthographicSize();
    }
}
