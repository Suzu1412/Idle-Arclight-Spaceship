using UnityEngine;

public class ChaserBossContext : StateContext
{
	public Vector2 BossSpawnPosition;
	public ChaserPatternState ChaseState;
	public float WaitTimer = 0f;
	public int Repetitions = 0; // Used to repeat The attack 3 times
    public bool HasMoveFinished;

	public float ChaseMoveUtility = 0f;
	public float WallAttackMoveUtility = 0f;
	public float RapidBurstUtility = 0f;

	public int ChaseMoveCycles = 0;
    public int WallAttackCycles = 0;
    public int RapidBurstCycles = 0;


    public enum MoveSelected
	{
		Wait,
		ChaseMove,

	}

	public enum ChaserPatternState
	{
		Move,
		Attack,
		Wait
	}
}

