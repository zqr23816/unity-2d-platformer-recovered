using UnityEngine;

/// <summary>播放一次攻击动作；受击优先于攻击结束后的追击转换。</summary>
public class AttackState : IState
{
	private readonly FSM manager;
	private readonly Parameter parameter;

	public AttackState(FSM manager)
	{
		this.manager = manager;
		parameter = manager.parameter;
	}

	public void OnEnter()
	{
		parameter.animator.Play("Attack");
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
