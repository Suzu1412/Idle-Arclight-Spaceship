using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITransition
{
    bool EvaluateCondition(IAgent agent);
}
