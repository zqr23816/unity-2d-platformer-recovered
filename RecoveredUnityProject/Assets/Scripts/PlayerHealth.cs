using UnityEngine;

/// <summary>管理玩家受伤、击退、死亡动画以及生命 UI 同步。</summary>
public class PlayerHealth : MonoBehaviour
{
	[Header("生命值")]
	[Min(1)]
	public int health;
	[Header("受击击退速度")]
	public float speed;

	public bool isHit;
	public bool isDead;

	private Vector2 direction;
	private Animator animator;
	private Rigidbody2D rigidbody;
	private bool hasPlayedDieAnimation;

	private void Start()
	{
		HealthBar.HealthMax = health;
		HealthBar.HealthCurrent = health;
		animator = GetComponent<Animator>();
		rigidbody = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (isHit)
		{
			rigidbody.velocity = direction * speed;
			if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f)
			{
				isHit = false;
			}
		}
		if (health <= 0 && !hasPlayedDieAnimation)
		{
			isDead = true;
			animator.Play("die");
			health = 0;
			hasPlayedDieAnimation = true;
			Destroy(gameObject, 6f);
		}
	}

	/// <summary>施加指定方向的击退和伤害。保留单参数重载以兼容旧调用。</summary>
	public void GetHit(Vector2 direction)
	{
		GetHit(direction, 1);
	}

	public void GetHit(Vector2 direction, int damage)
	{
		if (isDead || hasPlayedDieAnimation)
		{
			return;
		}

		this.direction = direction.normalized;
		isHit = true;
		animator.SetTrigger("Hit");
		health = Mathf.Max(0, health - Mathf.Max(0, damage));
		HealthBar.HealthCurrent = health;
	}

	public void GetDead()
	{
		isDead = true;
		health = 0;
		HealthBar.HealthCurrent = 0;
	}
}
