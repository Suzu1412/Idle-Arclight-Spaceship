using UnityEngine;
using UnityEngine.Events;
public interface IAgentAnimation
{
    public event UnityAction OnAnimationAction;
    public event UnityAction OnAnimationEnd;

    void PlayAnimation(AnimationType animationType);

    void Pause();

    void Resume();

    void PlayPreviousAnimation();

    void SetAnimationSpeed(float animationSpeed);

    void CheckCurrentAnimation();

    AnimationType GetCurrentAnimationType();

    float GetAnimationDuration();

    void InvokeAnimationAction();

    void InvokeAnimationEnd();
}
