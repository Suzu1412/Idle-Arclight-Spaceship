using System.Collections;
using UnityEngine;

public class UIAnimationManager : Singleton<UIAnimationManager>
{
    /// <summary>
    /// Moves a UI element from startPosition to targetPosition.
    /// </summary>
    public IEnumerator Slide(RectTransform uiElement, float? startX, float? startY, float duration)
    {
        Vector2 startPos = uiElement.anchoredPosition;
        Vector2 targetPos = startPos;

        if (startX.HasValue) startPos.x = startX.Value;
        if (startY.HasValue) startPos.y = startY.Value;

        uiElement.anchoredPosition = startPos;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsedTime / duration);
            uiElement.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        uiElement.anchoredPosition = targetPos;
    }

    /// <summary>
    /// Fades a UI element in or out using CanvasGroup.
    /// </summary>
    public IEnumerator Fade(CanvasGroup canvasGroup, bool fadeIn, float speed)
    {
        float targetAlpha = fadeIn ? 1f : 0f;
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * speed;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }

    /// <summary>
    /// Makes a UI element pop in and return to its normal size.
    /// </summary>
    public IEnumerator ScalePop(RectTransform uiElement, float popScale, float speed)
    {
        Vector3 originalScale = uiElement.localScale;
        Vector3 targetScale = originalScale * popScale;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * speed;
            float t = Mathf.SmoothStep(0, 1, elapsedTime);
            uiElement.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * speed;
            float t = Mathf.SmoothStep(1, 0, elapsedTime);
            uiElement.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }

        uiElement.localScale = originalScale;
    }

    /// <summary>
    /// Shakes a UI element for feedback.
    /// </summary>
    public IEnumerator Shake(RectTransform uiElement, float shakeAmount, float duration)
    {
        Vector2 originalPos = uiElement.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float offsetX = Random.Range(-shakeAmount, shakeAmount);
            float offsetY = Random.Range(-shakeAmount, shakeAmount);
            uiElement.anchoredPosition = originalPos + new Vector2(offsetX, offsetY);
            yield return null;
        }

        uiElement.anchoredPosition = originalPos;
    }

    /// <summary>
    /// Combines multiple animations in a sequence.
    /// </summary>
    public void AnimateUI(RectTransform uiElement, CanvasGroup canvasGroup, Vector2 startPos, Vector2 endPos, float popScale, float speed)
    {
        StartCoroutine(CombinedAnimation(uiElement, canvasGroup, startPos, endPos, popScale, speed));
    }

    private IEnumerator CombinedAnimation(RectTransform uiElement, CanvasGroup canvasGroup, Vector2 startPos, Vector2 endPos, float popScale, float speed)
    {
        //yield return StartCoroutine(Slide(uiElement, startPos, endPos, speed));
        yield return StartCoroutine(Fade(canvasGroup, true, speed));
        yield return StartCoroutine(ScalePop(uiElement, popScale, speed));
    }
}
