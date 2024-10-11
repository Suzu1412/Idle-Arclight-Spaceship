using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pickable/Score", fileName = "Score Pickable")]
public class ScorePickUpSO : ItemSO
{
    [SerializeField] private int _score = 10;
    [SerializeField] private IntGameEvent _scoreChannel = default; // Listen On GameManager

    public override void PickUp(IAgent agent)
    {
        _scoreChannel.RaiseEvent(_score);
        Debug.Log("pickeado");
    }
}
