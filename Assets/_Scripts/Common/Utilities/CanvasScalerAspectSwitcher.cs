using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(CanvasScaler))]
public class CanvasScalerAspectSwitcher : MonoBehaviour
{
    CanvasScaler canvasScaler;
    RectTransform rectTransform;

    void OnEnable() => OnRectTransformDimensionsChange();

    void OnRectTransformDimensionsChange()
    {
        Debug.Log("is working");

        if (!canvasScaler) canvasScaler = GetComponent<CanvasScaler>();
        if (!rectTransform) rectTransform = GetComponent<RectTransform>();

        var resolution = rectTransform.sizeDelta;
        var aspect = resolution.x / resolution.y;

        var min = canvasScaler.referenceResolution;
        var minAspect = min.x / min.y;

        canvasScaler.matchWidthOrHeight = aspect < minAspect ? 0 : 1;
    }
}