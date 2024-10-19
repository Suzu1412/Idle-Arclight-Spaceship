using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameRulesSO _explorationRules;

    




    public event UnityAction<GameStateType> OnStateChanged;

    private void OnEnable()
    {
        
    }
}
