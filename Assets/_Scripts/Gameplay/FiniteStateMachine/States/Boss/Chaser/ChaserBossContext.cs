using UnityEngine;

public class ChaserBossContext : StateContext
{
	public Vector2 BossSpawnPosition;
	public ChaseMoveState ChaseState;
	public float WaitTimer = 0f;
	public int ChaserAttackTimesDone = 0;

	public void WaitTimerTick()
	{
		WaitTimer += Time.deltaTime;
	}

	public enum MoveSelected
	{
		Wait,
		ChaseMove,

	}

	public enum ChaseMoveState
	{
		Move,
		Attack,
		Wait
	}
}

