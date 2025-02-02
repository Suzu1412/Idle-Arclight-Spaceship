using UnityEngine;

[CreateAssetMenu(fileName = "ChaserBossFirstPhaseStateSO", menuName = "Scriptable Objects/FSM/Boss/Chaser/ChaserBossFirstPhaseStateSO")]
public class ChaserBossFirstPhaseStateSO : StateSO<ChaserBossContext>
{
	[SerializeField] private float _waitTime = 2f;
	[SerializeField] private AttackPatternSO _attackPattern;

	public override float EvaluateUtility(ChaserBossContext context)
	{
		return context.HealthPercent > 0 ? _highestUtility : 0f;
	}

	public override void OnEnter(ChaserBossContext context)
	{
		context.ChaserAttackTimesDone = 0;
		context.WaitTimer = Time.time;
		context.ChaseState = ChaserBossContext.ChaseMoveState.Wait;
		(context.Agent.AttackSystem as AttackSystem).SetAttackPattern(_attackPattern);
	}

	public override void OnExit(ChaserBossContext context)
	{
	}

	public override void OnFixedUpdate(ChaserBossContext context)
	{
		context.Move();
	}

	public override void OnUpdate(ChaserBossContext context)
	{
		CurrentMove(context);
	}

	private void CurrentMove(ChaserBossContext context)
	{
		switch (context.ChaseState)
		{
			case ChaserBossContext.ChaseMoveState.Move:
				MoveToTarget(context);
				break;

			case ChaserBossContext.ChaseMoveState.Attack:
				AttackTarget(context);
				break;

			case ChaserBossContext.ChaseMoveState.Wait:
				Wait(context);
				break;
		}
	}

	private void MoveToTarget(ChaserBossContext context)
	{
		var playerPos = new Vector2(context.Target.position.x, context.Target.position.y + 2f);
		var direction = context.Transform.position.GetDirectionTo(playerPos);
		context.Agent.Input.CallOnMovementInput(direction);

		if (context.Transform.position.GetSquaredDistanceTo(playerPos) < 0.1f)
		{
			// Movement done
			context.ChaseState = ChaserBossContext.ChaseMoveState.Attack;
			context.Agent.Input.CallOnMovementInput(Vector2.zero);
		}
	}

	private void AttackTarget(ChaserBossContext context)
	{
		context.Agent.Input.CallOnAttack(true);
		// Attack End
		context.WaitTimer = Time.time;
		context.ChaseState = ChaserBossContext.ChaseMoveState.Wait;
	}

	private void Wait(ChaserBossContext context)
	{
		context.Agent.Input.CallOnMovementInput(Vector2.zero);

		if (Time.time > context.WaitTimer + _waitTime)
		{
			// Repeat 3 times, after that the move is not selected. 
			if (context.ChaserAttackTimesDone < 3)
			{
				context.ChaserAttackTimesDone++;
				context.ChaseState = ChaserBossContext.ChaseMoveState.Move;
			}
			else
			{
				Debug.Log("Move Finished");
			}
		}
	}
}