using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;


public static class Helpers
{
    public static readonly WaitForSecondsRealtime WaitOneSecond = new WaitForSecondsRealtime(1f);
    public static readonly WaitForSecondsRealtime WaitHalfSecond = new WaitForSecondsRealtime(0.5f);
    public static readonly WaitForSecondsRealtime WaitTwoSeconds = new WaitForSecondsRealtime(2f);

    public static Guid CreateGuidFromString(string input)
    {
        return new Guid(MD5.Create().ComputeHash(Encoding.Default.GetBytes(input)));
    }

    public static Vector2 ClampToScreen(VisualElement element, Vector2 targetPosition)
    {
        float x = Mathf.Clamp(targetPosition.x, 0, Screen.width - element.layout.width);
        float y = Mathf.Clamp(targetPosition.y, 0, Screen.height - element.layout.height);

        return new Vector2(x, y);
    }

    static readonly Dictionary<float, WaitForSeconds> WaitForSecondsDict = new(100, new FloatComparer());

    static readonly Dictionary<float, WaitForSecondsRealtime> WaitForRealTimeSecondsDict = new(100, new FloatComparer());

    /// <summary>
    /// Returns a WaitForSeconds object for the specified duration. </summary>
    /// <param name="seconds">The duration in seconds to wait.</param>
    /// <returns>A WaitForSeconds object.</returns>
    public static WaitForSeconds GetWaitForSeconds(float seconds)
    {
        if (seconds < 1f / Application.targetFrameRate) return null;

        if (WaitForSecondsDict.TryGetValue(seconds, out var forSeconds)) return forSeconds;

        var waitForSeconds = new WaitForSeconds(seconds);
        WaitForSecondsDict[seconds] = waitForSeconds;
        return waitForSeconds;
    }

    public static WaitForSecondsRealtime GetWaitForSecondsRealtime(float seconds)
    {
        if (seconds < 1f / Application.targetFrameRate) return null;

        if (WaitForRealTimeSecondsDict.TryGetValue(seconds, out var forSeconds)) return forSeconds;

        var waitForSeconds = new WaitForSecondsRealtime(seconds);
        WaitForRealTimeSecondsDict[seconds] = waitForSeconds;
        return waitForSeconds;
    }

    class FloatComparer : IEqualityComparer<float>
    {
        public bool Equals(float x, float y) => FloatComparison.TolerantEquals(x, y);
        public int GetHashCode(float obj) => obj.GetHashCode();
    }
}