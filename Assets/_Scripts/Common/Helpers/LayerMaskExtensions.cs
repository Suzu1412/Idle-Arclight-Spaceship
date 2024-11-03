using UnityEngine;

public static class LayerMaskExtensions
{
    public static int GetLayer(this LayerMask layerMask)
    {
        return (int)Mathf.Log(layerMask.value, 2);
    }
}
