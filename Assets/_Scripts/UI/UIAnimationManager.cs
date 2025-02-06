using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimationManager : Singleton<UIAnimationManager>
{
    // Slide From a Start position to Original position
    public void SlideFromStart(RectTransform target, Vector2 start, float duration)
        => StartCoroutine(SlideFromStartCoroutine(target, start, duration));

    // Slide from Original Position to Target
    public void SlideToTarget(RectTransform target, Vector2 to, float duration)
        => StartCoroutine(SlideToTargetCoroutine(target, to, duration));

    // Scale
    public void Scale(RectTransform target, Vector3 to, float duration)
        => StartCoroutine(ScaleCoroutine(target, to, duration));

    // Shake
    public void Shake(RectTransform target, float strength, float duration)
        => StartCoroutine(ShakeCoroutine(target, strength, duration));

    // Fade
    public void Fade(CanvasGroup canvasGroup, float to, float duration)
        => StartCoroutine(FadeCoroutine(canvasGroup, to, duration));

    // Rotate Smoothly rotates UI elements.
    public void Rotate(RectTransform target, float toAngle, float duration)
        => StartCoroutine(RotateCoroutine(target, toAngle, duration));

    // Pulse (breathing effect) Makes an element smoothly grow and shrink.
    public void Pulse(RectTransform target, float minScale, float maxScale, float duration)
        => StartCoroutine(PulseCoroutine(target, minScale, maxScale, duration));

    // Bounce effect Moves an element with an elastic effect.
    public void Bounce(RectTransform target, Vector2 to, float duration)
        => StartCoroutine(BounceCoroutine(target, to, duration));

    // Wobble Tilts the UI element back and forth.
    public void Wobble(RectTransform target, float strength, float duration)
        => StartCoroutine(WobbleCoroutine(target, strength, duration));

    // Change Color Fades a UI element’s color smoothly.
    public void ChangeColor(Graphic graphic, Color to, float duration)
        => StartCoroutine(ChangeColorCoroutine(graphic, to, duration));

    // Coroutine Implementations
    private IEnumerator SlideFromStartCoroutine(RectTransform target, Vector2 start, float duration)
    {
        Vector2 to = target.anchoredPosition;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            target.anchoredPosition = Vector2.LerpUnclamped(start, to, EaseOutQuad(t));
            yield return null;
        }
        target.anchoredPosition = to;
    }

    private IEnumerator SlideToTargetCoroutine(RectTransform target, Vector2 to, float duration)
    {
        Vector2 start = target.anchoredPosition;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            target.anchoredPosition = Vector2.LerpUnclamped(start, to, EaseOutQuad(t));
            yield return null;
        }
        target.anchoredPosition = to;
    }

    private IEnumerator ScaleCoroutine(RectTransform target, Vector3 to, float duration)
    {
        Vector3 start = target.localScale;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            target.localScale = Vector3.LerpUnclamped(start, to, EaseOutBack(t));
            yield return null;
        }
        target.localScale = to;
    }

    private IEnumerator ShakeCoroutine(RectTransform target, float strength, float duration)
    {
        Vector2 originalPos = target.anchoredPosition;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            target.anchoredPosition = originalPos + new Vector2(Random.Range(-strength, strength), Random.Range(-strength, strength));
            yield return null;
        }
        target.anchoredPosition = originalPos;
    }

    private IEnumerator FadeCoroutine(CanvasGroup canvasGroup, float to, float duration)
    {
        float start = canvasGroup.alpha;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            canvasGroup.alpha = Mathf.LerpUnclamped(start, to, EaseLinear(t));
            yield return null;
        }
        canvasGroup.alpha = to;
    }

    private IEnumerator RotateCoroutine(RectTransform target, float toAngle, float duration)
    {
        float start = target.eulerAngles.z;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            target.eulerAngles = new Vector3(0, 0, Mathf.LerpUnclamped(start, toAngle, EaseOutQuad(t)));
            yield return null;
        }
        target.eulerAngles = new Vector3(0, 0, toAngle);
    }

    private IEnumerator PulseCoroutine(RectTransform target, float minScale, float maxScale, float duration)
    {
        float elapsed = 0f;
        while (true) // Looping effect
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.PingPong(elapsed / duration, 1);
            float scale = Mathf.Lerp(minScale, maxScale, EaseOutQuad(t));
            target.localScale = Vector3.one * scale;
            yield return null;
        }
    }

    private IEnumerator BounceCoroutine(RectTransform target, Vector2 to, float duration)
    {
        Vector2 start = target.anchoredPosition;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            target.anchoredPosition = Vector2.LerpUnclamped(start, to, EaseOutBack(t));
            yield return null;
        }
        target.anchoredPosition = to;
    }

    private IEnumerator WobbleCoroutine(RectTransform target, float strength, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float angle = Mathf.Sin(elapsed * 10) * strength;
            target.eulerAngles = new Vector3(0, 0, angle);
            yield return null;
        }
        target.eulerAngles = Vector3.zero;
    }

    private IEnumerator ChangeColorCoroutine(Graphic graphic, Color to, float duration)
    {
        Color start = graphic.color;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            graphic.color = Color.LerpUnclamped(start, to, EaseLinear(t));
            yield return null;
        }
        graphic.color = to;
    }

    // Easing Functions
    private float EaseOutQuad(float t) => t * (2 - t);
    private float EaseOutBack(float t) => (--t) * t * (2.70158f * t + 1.70158f) + 1;
    private float EaseLinear(float t) => t;
}
