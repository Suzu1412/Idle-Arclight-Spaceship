using UnityEngine;

public interface IAgentRenderer
{
    SpriteRenderer SpriteRenderer { get; }
    void TransparencyOverTime(float targetAlpha, float duration);
    void ChangeSpriteColor(float r, float g, float b, float a);

    void RotateSpriteToDirection(Vector2 direction);
}
