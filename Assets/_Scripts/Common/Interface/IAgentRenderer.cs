using UnityEngine;

public interface IAgentRenderer
{
    SpriteRenderer Sprite { get; }
    void TransparencyOverTime(float targetAlpha, float duration);
    void ChangeSpriteColor(float r, float g, float b, float a);
}
