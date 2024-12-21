using System.Collections;
using UnityEngine;

public class AgentRenderer : MonoBehaviour, IAgentRenderer
{
    private Color _spriteOriginalColor;
    private Color _spriteTargetColor;
    private Vector2 _facingDirection = new(1f, 1f);
    private SpriteRenderer _spriteRenderer;
    private IAgent _agent;
    public IAgent Agent => _agent ??= _agent = GetComponentInParent<IAgent>();
    public SpriteRenderer SpriteRenderer => _spriteRenderer != null ? _spriteRenderer : _spriteRenderer = GetComponent<SpriteRenderer>();
    public Vector2 FacingDirection => _facingDirection;

    private Coroutine _changeAlphaCoroutine;

    private void OnEnable()
    {
        Agent.OnChangeFacingDirection += FaceDirection;
    }

    private void OnDisable()
    {
        Agent.OnChangeFacingDirection -= FaceDirection;
        StopAllCoroutines();
    }

    private void FaceDirection(Vector2 input)
    {
        if (input.y < 0)
        {
            _facingDirection.y = -1f;
            transform.parent.localScale = new Vector3(
                1,
                -1 * Mathf.Abs(transform.parent.localScale.y),
                transform.localScale.z);
        }
        else if (input.y > 0)
        {
            _facingDirection.y = 1f;
            transform.parent.localScale = new Vector3(1,
                Mathf.Abs(transform.parent.localScale.y),
                transform.localScale.z);
        }
    }

    public void TransparencyOverTime(float targetAlpha, float duration)
    {
        if (_changeAlphaCoroutine != null) StopCoroutine(_changeAlphaCoroutine);
        _changeAlphaCoroutine = StartCoroutine(ChangeAlphaCoroutine(targetAlpha, duration));
    }

    public void ChangeSpriteColor(float r, float g, float b, float a)
    {
        _spriteTargetColor.r = r;
        _spriteTargetColor.g = g;
        _spriteTargetColor.b = b;
        _spriteTargetColor.a = a;

        SpriteRenderer.color = _spriteTargetColor;
    }

    private IEnumerator ChangeAlphaCoroutine(float targetAlpha, float duration)
    {
        Color originalColor = SpriteRenderer.color;
        float startAlpha = originalColor.a;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            SpriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            yield return null; // Wait for the next frame
        }

        // Ensure the final alpha is set
        SpriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, targetAlpha);
    }
}
