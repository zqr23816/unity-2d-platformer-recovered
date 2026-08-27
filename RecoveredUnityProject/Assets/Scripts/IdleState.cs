using UnityEngine;

/// <summary>敌人待机状态：计时结束后巡逻，发现玩家时先做警觉反应。</summary>
public class IdleState : IState
{
	private readonly FSM manager;
	private readonly Parameter parameter;

	private float timer;

	public IdleState(FSM manager)
	{
		this.manager = manager;
		parameter = manager.parameter;
	}

	public void OnEnter()
	{
		timer = 0f;
		parameter.animator.Play("Idle");
	}

	public void OnUpdate()
	{
		timer += Time.deltaTime;
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
		if (timer >= parameter.idleTime)
		{
			manager.TransitionState(StateType.Patrol);
		}
	}

	public void OnExit()
	{
		timer = 0f;
	}
}
