using UnityEngine;

/// <summary>按配置的路径点循环巡逻，并响应玩家进入警戒范围。</summary>
public class PatrolState : IState
{
	private readonly FSM manager;
	private readonly Parameter parameter;

	private int patrolPosition;

	public PatrolState(FSM manager)
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
		if (StateGuards.TargetInsideChaseRange(parameter))
		{
			manager.TransitionState(StateType.React);
			return;
		}
		if (parameter.patrolPoints == null || parameter.patrolPoints.Length == 0)
		{
			manager.TransitionState(StateType.Idle);
			return;
		}

		Transform destination = parameter.patrolPoints[patrolPosition];
		manager.FlipTo(destination);
		manager.transform.position = Vector2.MoveTowards(
			manager.transform.position,
			destination.position,
			parameter.moveSpeed * Time.deltaTime);

		if (Vector2.Distance(manager.transform.position, destination.position) < 0.1f)
		{
			manager.TransitionState(StateType.Idle);
		}
	}

	public void OnExit()
	{
		if (parameter.patrolPoints != null && parameter.patrolPoints.Length > 0)
		{
			patrolPosition = (patrolPosition + 1) % parameter.patrolPoints.Length;
		}
	}
}
