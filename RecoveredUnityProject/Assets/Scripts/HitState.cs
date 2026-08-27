using UnityEngine;

/// <summary>结算一次敌人受击；生命归零时进入死亡，否则重新锁定玩家。</summary>
public class HitState : IState
{
	private readonly FSM manager;
	private readonly Parameter parameter;

	public HitState(FSM manager)
	{
		this.manager = manager;
		parameter = manager.parameter;
	}

	public void OnEnter()
	{
		parameter.animator.Play("Hit");
		parameter.health--;
	}

	public void OnUpdate()
	{
		if (parameter.health <= 0)
		{
			manager.TransitionState(StateType.Death);
			return;
		}
		if (parameter.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
		{
			GameObject player = GameObject.FindWithTag("Player");
			parameter.target = player != null ? player.transform : null;
			manager.TransitionState(parameter.target != null ? StateType.Chase : StateType.Idle);
		}
	}

	public void OnExit()
	{
		parameter.getHit = false;
	}
}
