using System;
using System.Collections;
using UnityEngine;

public class CombatManager : Singleton<CombatManager>
{
    [SerializeField] private GameObjectGameEventListener _playerGOListener;
    [SerializeField] private Agent _player;
    [SerializeField] private GameObject _enemyPrefab;
    private Agent _enemy;
    [SerializeField] private Transform _playerCombatPosition;
    [SerializeField] private Transform _enemyPosition;
    private bool _isPlayerReady = false;

    protected void OnEnable()
    {
        GameManager.Instance.OnStateChanged += HandleCombatState;
        _playerGOListener.Register(SetPlayer);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnStateChanged -= HandleCombatState;
        _playerGOListener.DeRegister(SetPlayer);
    }

    private void SetPlayer(GameObject player)
    {
        _player = player.GetComponentInChildren<Agent>();
    }

    public void HandleCombatState(GameStateType state)
    {
        switch (state)
        {
            case GameStateType.StartCombat:
                SpawnEnemy();
                StartCoroutine(MoveAgentToPosition(_enemy, _enemyPosition.position));
                StartCoroutine(MoveAgentToPosition(_player, _playerCombatPosition.position));
                break;
        }
    }

    private void SpawnEnemy()
    {
        _enemy = Instantiate(_enemyPrefab).GetComponentInChildren<Agent>();
        _enemy.transform.position = new Vector3(0f, 3f, 0f);
    }

    private IEnumerator MoveAgentToPosition(Agent agent, Vector3 position)
    {
        Vector2 direction = Vector2.zero;
        bool isAgentReady = false;

        while (!isAgentReady)
        {
            direction = (position - agent.transform.position).normalized;
            agent.Input.CallOnMovementInput(direction);

            if (Vector3.Distance(position, agent.transform.position) < 0.1f)
            {
                agent.Input.CallOnMovementInput(Vector2.zero);
                agent.transform.position = position;
                isAgentReady = true;
            }

            yield return null;
        }

        
    }
}
