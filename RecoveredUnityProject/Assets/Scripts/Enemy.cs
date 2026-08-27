using UnityEngine;

/// <summary>衔接敌人受击反馈、FSM 生命状态和与玩家接触时的伤害。</summary>
public class Enemy : MonoBehaviour
{
	[Header("受击击退")]
	public float speed;
	[Header("接触伤害")]
	public int damage;

	private Vector2 direction;
	private bool isHit;
	private Animator animator;
	private Animator hitAnimator;
	private Rigidbody2D rigidbody;
	private FSM stateMachine;

	private void Start()
	{
		animator = GetComponent<Animator>();
		hitAnimator = transform.childCount > 0 ? transform.GetChild(0).GetComponent<Animator>() : null;
		rigidbody = GetComponent<Rigidbody2D>();
		stateMachine = GetComponent<FSM>();
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
	}

	public void GetHit(Vector2 direction)
	{
		transform.localScale = new Vector3(-direction.x, 1f, 1f);
		isHit = true;
		this.direction = direction;
		animator.SetTrigger("Hit");
		if (hitAnimator != null)
		{
			hitAnimator.SetTrigger("Hit");
		}
		stateMachine?.RequestHit();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && other is BoxCollider2D &&
			other.TryGetComponent(out PlayerHealth health))
		{
			Vector2 knockback = transform.localScale.x > 0f ? Vector2.right : Vector2.left;
			health.GetHit(knockback, Mathf.Max(1, damage));
		}
	}
}
