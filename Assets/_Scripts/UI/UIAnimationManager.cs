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

    // Scale and return to original Scale
    public void ScalePop(RectTransform target, float scaleFactor, float duration, float holdTime)
    => StartCoroutine(ScalePopCoroutine(target, scaleFactor, duration, holdTime));

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
    public void Pulse(RectTransform target, float minScale, float maxScale, float duration, float pingPongDuration)
        => StartCoroutine(PulseCoroutine(target, minScale, maxScale, duration, pingPongDuration));

    // Bounce effect Moves an element with an elastic effect.
    public void Bounce(RectTransform target, Vector2 to, float duration)
        => StartCoroutine(BounceCoroutine(target, to, duration));

    // Wobble Tilts the UI element back and forth.
    public void Wobble(RectTransform target, float strength, float duration)
        => StartCoroutine(WobbleCoroutine(target, strength, duration));

    // Change Color Fades a UI element’s color smoothly.
    public void ChangeColor(Graphic graphic, Color to, float duration)
        => StartCoroutine(ChangeColorCoroutine(graphic, to, duration));

    // Add a Glow Effect to a Text or Image. Need to add an Outline First.
    public void GlowEffect(Outline outline, float maxAlpha, float duration)
        => StartCoroutine(GlowCoroutine(outline, maxAlpha, duration));

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

    private IEnumerator ScalePopCoroutine(RectTransform target, float scaleFactor, float duration, float holdTime)
    {
        Vector3 originalScale = target.localScale;
        Vector3 targetScale = originalScale * scaleFactor;

        float halfDuration = duration / 2f;
        float elapsed = 0f;

        // Scale Up
        while (elapsed < halfDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / halfDuration;
            target.localScale = Vector3.LerpUnclamped(originalScale, targetScale, EaseOutQuad(t));
            yield return null;
        }

        target.localScale = targetScale;

        // Hold at Max Size
        yield return new WaitForSecondsRealtime(holdTime);

        elapsed = 0f;

        // Scale Down
        while (elapsed < halfDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / halfDuration;
            target.localScale = Vector3.LerpUnclamped(targetScale, originalScale, EaseOutQuad(t));
            yield return null;
        }

        target.localScale = originalScale;
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

    private IEnumerator PulseCoroutine(RectTransform target, float minScale, float maxScale, float duration, float pingPongDuration)
    {
        float elapsed = 0f;
        target.localScale = Vector3.one * minScale;
        while (elapsed < duration) // Looping effect
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.PingPong(elapsed / duration, pingPongDuration);
            float scale = Mathf.Lerp(minScale, maxScale, EaseOutQuad(t));
            target.localScale = Vector3.one * scale;
            yield return null;
        }
        target.localScale = Vector3.one * minScale;
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

    private IEnumerator GlowCoroutine(Outline outline, float maxAlpha, float duration)
    {
        if (outline == null) yield break;

        Color originalColor = outline.effectColor;
        float originalAlpha = originalColor.a;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, maxAlpha);

        float halfDuration = duration / 2f;
        float elapsed = 0f;

        // Increase glow
        while (elapsed < halfDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / halfDuration;
            outline.effectColor = Color.Lerp(originalColor, targetColor, EaseOutQuad(t));
            yield return null;
        }

        elapsed = 0f;

        // Decrease glow
        while (elapsed < halfDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / halfDuration;
            outline.effectColor = Color.Lerp(targetColor, originalColor, EaseOutQuad(t));
            yield return null;
        }

        outline.effectColor = originalColor;
    }

    // Easing Functions
    private float EaseOutQuad(float t) => t * (2 - t);
    private float EaseOutBack(float t) => (--t) * t * (2.70158f * t + 1.70158f) + 1;
    private float EaseLinear(float t) => t;
}
