using UnityEngine;

/// <summary>在警戒范围内追逐玩家，进入攻击圈后切换为攻击状态。</summary>
public class ChaseState : IState
{
	private readonly FSM manager;
	private readonly Parameter parameter;

	public ChaseState(FSM manager)
	{
		this.manager = manager;
		parameter = manager.parameter;
	}

	public void OnEnter()
	{
		parameter.animator.Play("Walk");
	}

	public void OnUpdate()
	{
		if (parameter.getHit)
		{
			manager.TransitionState(StateType.Hit);
			return;
		}
		if (!StateGuards.TargetInsideChaseRange(parameter))
		{
			manager.TransitionState(StateType.Idle);
			return;
		}

		manager.FlipTo(parameter.target);
		manager.transform.position = Vector2.MoveTowards(
			manager.transform.position,
			parameter.target.position,
			parameter.chaseSpeed * Time.deltaTime);

		if (parameter.attackPoint != null &&
			Physics2D.OverlapCircle(parameter.attackPoint.position, parameter.attackArea, parameter.targetLayer) != null)
		{
			manager.TransitionState(StateType.Attack);
		}
	}

	public void OnExit()
	{
	}
}
