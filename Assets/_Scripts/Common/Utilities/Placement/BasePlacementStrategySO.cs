using System.Net.NetworkInformation;
using UnityEngine;

public abstract class BasePlacementStrategySO : ScriptableObject
{
    public abstract Vector3 SetPosition(Vector3 origin);
}
