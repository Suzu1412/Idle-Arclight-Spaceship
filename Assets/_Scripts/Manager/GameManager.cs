using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{



    #region Events
    public event UnityAction<GameStateType> OnStateChanged;


    #endregion

    protected override void Awake()
    {
        base.Awake();
    }


}
