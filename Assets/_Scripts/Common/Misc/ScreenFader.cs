using Mono.Cecil.Cil;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] private FloatGameEventListener OnScreenFadeOutEventListener;
    [SerializeField] private FloatGameEventListener OnScreenFadeInEventListener;

    private Image _fadeImage;      // The Image component that will cover the screen
    private Canvas _canvas;

    private void Awake()
    {
        _fadeImage = GetComponent<Image>();

        
    }

    private void OnEnable()
    {
        OnScreenFadeOutEventListener.Register(FadeOut);
        OnScreenFadeInEventListener.Register(FadeIn);
    }

    private void OnDisable()
    {
        OnScreenFadeOutEventListener.DeRegister(FadeOut);
        OnScreenFadeInEventListener.DeRegister(FadeIn);
    }

    private void FadeOut(float duration)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }

    private void FadeIn(float duration)
    {
        StartCoroutine(FadeInCoroutine(duration));
    }


    // Coroutine to fade out the screen
    private IEnumerator FadeOutCoroutine(float duration)
    {
        float timeElapsed = 0f;

        // Make sure the image is completely transparent at the beginning
        Color startColor = _fadeImage.color;
        startColor.a = 0f;
        _fadeImage.color = startColor;

        // Make the Image active if it's not already
        _fadeImage.enabled = true;

        // Fade to fully opaque (alpha = 1) over the duration of time
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(timeElapsed / duration);
            _fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        // Ensure the image is fully opaque at the end
        _fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, 1f);
        _fadeImage.enabled = false;

    }

    // Coroutine to fade in the screen
    private IEnumerator FadeInCoroutine(float duration)
    {
        float timeElapsed = 0f;

        // Make sure the image is completely dark at the beginning
        Color startColor = _fadeImage.color;
        startColor.a = 1f;
        _fadeImage.color = startColor;

        // Make the Image active if it's not already
        _fadeImage.enabled = true;

        // Fade to fully opaque (alpha = 1) over the duration of time
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - timeElapsed / duration);
            _fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        // Ensure the image is fully opaque at the end
        _fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        _fadeImage.enabled = false;
    }

}
