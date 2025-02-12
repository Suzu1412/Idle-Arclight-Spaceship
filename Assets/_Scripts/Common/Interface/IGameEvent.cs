using System.Collections.Generic;
using UnityEngine;

public interface IGameEvent
{
    void RaiseFromEditor(object value, UnityEngine.Object sender);
    bool HasParameterType { get; }
    System.Type ParameterType { get; }
    List<string> EventHistory { get; }
    Dictionary<UnityEngine.Object, int> SenderCounts { get; }
}
