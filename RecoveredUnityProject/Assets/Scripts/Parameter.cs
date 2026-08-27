using System;
using UnityEngine;

[Serializable]
/// <summary>
/// 敌人状态机的共享运行参数。保留字段名以兼容原构建中的 Inspector 序列化数据。
/// </summary>
public class Parameter
{
	[Header("基础属性")]
	public int health = 3;
	public int damage = 1;
	public float moveSpeed = 2f;
	public float chaseSpeed = 3f;
	public float idleTime = 2f;

	[Header("移动范围")]
	public Transform[] patrolPoints;
	public Transform[] chasePoints;

	[Header("感知与攻击")]
	public Transform target;
	public LayerMask targetLayer;
	public Transform attackPoint;
	public float attackArea = 0.5f;

	[HideInInspector] public Animator animator;
	[HideInInspector] public bool getHit;
	[HideInInspector] public bool isHit;
}
