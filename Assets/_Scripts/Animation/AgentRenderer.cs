using UnityEngine;
using DG.Tweening;

public class AgentRenderer : MonoBehaviour, IAgentRenderer
{
    private Color _spriteOriginalColor;
    private Color _spriteTargetColor;
    private Vector2 _facingDirection = new(1f, 1f);
    private SpriteRenderer _sprite;
    private IAgent _agent;
    public IAgent Agent => _agent ??= _agent = GetComponentInParent<IAgent>();
    public SpriteRenderer Sprite => _sprite != null ? _sprite : _sprite = GetComponent<SpriteRenderer>();
    public Vector2 FacingDirection => _facingDirection;

    private void OnEnable()
    {
        Agent.OnChangeFacingDirection += FaceDirection;
    }

    private void OnDisable()
    {
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
        Sprite.DOFade(targetAlpha, duration).SetEase(Ease.Linear);
    }

    public void ChangeSpriteColor(float r, float g, float b, float a)
    {
        _spriteTargetColor.r = r;
        _spriteTargetColor.g = g;
        _spriteTargetColor.b = b;
        _spriteTargetColor.a = a;

        Sprite.color = _spriteTargetColor;
    }
}
