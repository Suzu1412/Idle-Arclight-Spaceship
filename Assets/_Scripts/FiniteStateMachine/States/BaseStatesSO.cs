using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStatesSO : ScriptableObject
{
    public abstract List<IState> States { get; }
}
