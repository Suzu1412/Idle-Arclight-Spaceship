using System.Collections.Generic;
using UnityEngine;
using System;

public class AgentAnimationFactory : MonoBehaviour
{
    private readonly Dictionary<AnimationType, int> _animationDictionary = new();

    internal int GetAnimation(AnimationType animationType)
    {
        if (_animationDictionary.TryGetValue(animationType, out var animation))
        {
            return animation;
        }

        _animationDictionary.Add(animationType, Animator.StringToHash(Enum.GetName(typeof(AnimationType),animationType)));
        return _animationDictionary.GetValueOrDefault(animationType);
    }
}
