using System.Collections.Generic;
using UnityEngine;

/// <summary>敌人有限状态机的上下文，负责注册状态、更新当前状态和状态转换。</summary>
public class FSM : MonoBehaviour
{
	private IState currentState;
	private readonly Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();

	[Tooltip("由所有状态共享的敌人配置与运行数据")]
	public Parameter parameter = new Parameter();

	/// <summary>便于调试器和 UI 查看当前状态，但不暴露状态对象。</summary>
	public StateType CurrentState { get; private set; }

	private void Awake()
	{
		// 先缓存 Animator，再进入 Idle，避免 OnEnter 播放动画时出现空引用。
		parameter.animator = GetComponent<Animator>();
		states.Add(StateType.Idle, new IdleState(this));
		states.Add(StateType.Patrol, new PatrolState(this));
		states.Add(StateType.Chase, new ChaseState(this));
		states.Add(StateType.React, new ReactState(this));
		states.Add(StateType.Attack, new AttackState(this));
		states.Add(StateType.Hit, new HitState(this));
		states.Add(StateType.Death, new DeathState(this));
	}

	private void Start()
	{
		TransitionState(StateType.Idle);
	}

	private void Update()
	{
		currentState?.OnUpdate();
	}

	/// <summary>由受击组件调用，避免用全局按键让场景中的所有敌人同时受伤。</summary>
	public void RequestHit()
	{
		if (CurrentState != StateType.Death)
		{
			parameter.getHit = true;
		}
	}

	/// <summary>执行“退出旧状态 → 切换引用 → 进入新状态”的原子转换。</summary>
	public void TransitionState(StateType type)
	{
		if (!states.TryGetValue(type, out IState nextState) || ReferenceEquals(currentState, nextState))
		{
			return;
		}

		currentState?.OnExit();
		currentState = nextState;
		CurrentState = type;
		currentState.OnEnter();
	}

	public void FlipTo(Transform target)
	{
		if (target != null)
		{
			if (transform.position.x > target.position.x)
			{
				transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			else if (transform.position.x < target.position.x)
			{
				transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			parameter.target = other.transform;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			parameter.target = null;
		}
	}

	private void OnDrawGizmos()
	{
		if (parameter != null && parameter.attackPoint != null)
		{
			Gizmos.DrawWireSphere(parameter.attackPoint.position, parameter.attackArea);
		}
	}
}
