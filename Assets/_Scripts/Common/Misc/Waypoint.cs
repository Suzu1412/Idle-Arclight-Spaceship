using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private Color _pathColor = Color.cyan;
    Transform[] _pathArray;
    [SerializeField] private List<Transform> _pathObjectList = new();

    public List<Transform> PathObjectList => _pathObjectList;

    private void OnEnable()
    {
        LoadWaypoints();
    }

    public void LoadWaypoints()
    {
        _pathObjectList.Clear();

        _pathArray = GetComponentsInChildren<Transform>();

        foreach (var transform in _pathArray)
        {
            if (transform != this.transform)
            {
                _pathObjectList.Add(transform);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _pathColor;

        for (int i = 0; i < _pathObjectList.Count; i++)
        {
            Vector2 currentPosition = _pathObjectList[i].position;
            if (i > 0)
            {
                Vector2 previousPosition = _pathObjectList[i - 1].position;
                Gizmos.DrawLine(previousPosition, currentPosition);
            }
            Gizmos.DrawWireSphere(currentPosition, 0.1f);
        }
    }
}
