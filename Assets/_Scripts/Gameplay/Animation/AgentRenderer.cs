using System.Collections;
using UnityEngine;

public class AgentRenderer : MonoBehaviour, IAgentRenderer
{
    private Color _spriteOriginalColor;
    private Color _spriteTargetColor;
    private Vector2 _facingDirection = new(1f, 1f);
    private float _rotationOffset;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private FacingDirectionType _facingDirectionType;
    private IAgent _agent;
    public IAgent Agent => _agent ??= _agent = GetComponentInParent<IAgent>();
    public SpriteRenderer SpriteRenderer => _spriteRenderer != null ? _spriteRenderer : _spriteRenderer = GetComponent<SpriteRenderer>();
    public Vector2 FacingDirection => _facingDirection;

    private Coroutine _changeAlphaCoroutine;

    private void OnEnable()
    {
        Agent.OnChangeFacingDirection += FaceDirection;
        SetSpriteFacingDirection();
    }

    private void OnDisable()
    {
        Agent.OnChangeFacingDirection -= FaceDirection;
        StopAllCoroutines();
    }

    private void SetSpriteFacingDirection()
    {
        switch (_facingDirectionType)
        {
            case FacingDirectionType.Up:
                _rotationOffset = 90f;
                break;

                case FacingDirectionType.Down:
                _rotationOffset = 270f;
                break;
                case FacingDirectionType.Left:
                    _rotationOffset = 180f;

                break;

                case FacingDirectionType.Right:
                    _rotationOffset = 0f;

                break;
        }
    }

    private void FaceDirection(Vector2 input)
    {
        if (input.y < 0)
        {
            _facingDirection.y = -1f;
            transform.localScale = new Vector3(
                1,
                -1 * Mathf.Abs(transform.parent.localScale.y),
                transform.localScale.z);
        }
        else if (input.y > 0)
        {
            _facingDirection.y = 1f;
            transform.localScale = new Vector3(1,
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

    public void RotateSpriteToDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        angle += _rotationOffset;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
