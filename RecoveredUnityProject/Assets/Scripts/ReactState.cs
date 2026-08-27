using UnityEngine;

/// <summary>敌人发现玩家后的短暂警觉动作，结束后开始追击。</summary>
public class ReactState : IState
{
	private readonly FSM manager;
	private readonly Parameter parameter;

	public ReactState(FSM manager)
	{
		this.manager = manager;
		parameter = manager.parameter;
	}

	public void OnEnter()
	{
		parameter.animator.Play("React");
	}

	public void OnUpdate()
	{
		if (parameter.getHit)
		{
			manager.TransitionState(StateType.Hit);
			return;
		}
		if (parameter.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
		{
			manager.TransitionState(StateType.Chase);
		}
	}

	public void OnExit()
	{
	}
}
