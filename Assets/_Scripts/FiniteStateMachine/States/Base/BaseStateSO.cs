using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStateSO : ScriptableObject
{
    public abstract BaseState State { get; } 
}