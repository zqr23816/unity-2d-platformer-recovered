using UnityEngine;

/// <summary>播放死亡动画并延迟回收敌人对象。</summary>
public class DeathState : IState
{
	private readonly FSM manager;
	private readonly Parameter parameter;

	public DeathState(FSM manager)
	{
		this.manager = manager;
		parameter = manager.parameter;
	}

	public void OnEnter()
	{
		parameter.animator.Play("Dead");
		// 只安排一次销毁，避免原实现每帧重复调用 Destroy。
		Object.Destroy(manager.gameObject, 6f);
	}

	public void OnUpdate()
	{
	}

	public void OnExit()
	{
	}
}
