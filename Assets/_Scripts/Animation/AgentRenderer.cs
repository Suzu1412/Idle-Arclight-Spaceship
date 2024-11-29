using UnityEngine;
using DG.Tweening;

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

    private void OnEnable()
    {
        Agent.OnChangeFacingDirection += FaceDirection;
    }

    private void OnDisable()
    {
        SpriteRenderer.DOKill();
        Agent.OnChangeFacingDirection -= FaceDirection;
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
        SpriteRenderer.DOFade(targetAlpha, duration).SetEase(Ease.Linear);
    }

    public void ChangeSpriteColor(float r, float g, float b, float a)
    {
        _spriteTargetColor.r = r;
        _spriteTargetColor.g = g;
        _spriteTargetColor.b = b;
        _spriteTargetColor.a = a;

        SpriteRenderer.color = _spriteTargetColor;
    }
}
