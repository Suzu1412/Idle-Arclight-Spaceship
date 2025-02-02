using System.Collections;
using UnityEngine;

public class FadeInvisibleWall : MonoBehaviour
{
    public string playerTag = "Player"; // Tag of the player object
    public float _fadeDuration = 1f; // Duration of the fade effect
    private SpriteRenderer _spriteRenderer; // SpriteRenderer of the target object
    private Coroutine _fadeCoroutine;

    private void Start()
    {
        // Get the SpriteRenderer of the target object
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer != null && gameObject.activeSelf)
            {
                // Ensure the sprite starts transparent
                SetSpriteAlpha(0f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) && _spriteRenderer != null)
        {
            StartFade(1f); // Fade in
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) && _spriteRenderer != null)
        {
            StartFade(0f); // Fade out
        }
    }

    private void StartFade(float targetAlpha)
    {
        // Stop any ongoing fade to avoid overlap
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }
        _fadeCoroutine = StartCoroutine(FadeTo(targetAlpha));
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        float startAlpha = _spriteRenderer.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / _fadeDuration);
            SetSpriteAlpha(alpha);
            yield return null;
        }

        SetSpriteAlpha(targetAlpha);
    }

    private void SetSpriteAlpha(float alpha)
    {
        if (_spriteRenderer != null)
        {
            Color color = _spriteRenderer.color;
            color.a = alpha;
            _spriteRenderer.color = color;
        }
    }
}
