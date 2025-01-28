using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AgentAnimationFactory))]
[RequireComponent(typeof(Animator))]
public class AnimationManager : MonoBehaviour
{
    private Animator _animator;
    private AgentAnimationFactory _factory;
    private AnimationType _currentAnimationType;
    private AnimationType _previousAnimation;
    private readonly Dictionary<AnimationType, float> _animationDurationDictionary = new();

    public event UnityAction OnAnimationAction;
    public event UnityAction OnAnimationEnd;

    private float _animationSpeed = 1f;
    private Coroutine _playAnimationCoroutine;

    public Animator Animator => _animator != null ? _animator : _animator = GetComponent<Animator>();
    public AgentAnimationFactory AnimationFactory => _factory != null ? _factory : _factory = gameObject.GetOrAdd<AgentAnimationFactory>();

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.speed = _animationSpeed;
    }

    private void Start()
    {

    }

    /// <summary>
    /// Play the Animation
    /// </summary>
    /// <param name="animationType"></param>
    public void PlayAnimation(AnimationType animationType)
    {
        _previousAnimation = _currentAnimationType;
        Animator.Play(AnimationFactory.GetAnimation(animationType));
        _currentAnimationType = animationType;
    }

    public void PlayPreviousAnimation()
    {
        Animator.Play(AnimationFactory.GetAnimation(_previousAnimation));
    }

    public void Pause()
    {
        Animator.speed = 0f;
    }

    public void Resume()
    {
        Animator.speed = _animationSpeed;
    }


    public void SetAnimationSpeed(float animationSpeed)
    {
        _animationSpeed = animationSpeed;
    }

    public void CheckCurrentAnimation()
    {
        Debug.Log(_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"));
    }

    public AnimationType GetCurrentAnimationType()
    {
        return _currentAnimationType;
    }

    /// <summary>
    /// Must use AFTER: yield return new WaitForEndOfFrame(); to get correct values
    /// </summary>
    /// <returns></returns>
    public float GetAnimationDuration()
    {
        return Animator.GetCurrentAnimatorStateInfo(0).length;
    }

    public void InvokeAnimationAction()
    {
        OnAnimationAction?.Invoke();
    }

    public void InvokeAnimationEnd()
    {
        OnAnimationEnd?.Invoke();
    }
}
