using UnityEngine;

namespace StateMachine
{
/// <summary>处理玩家移动、跳跃、朝向、三段连击和命中反馈。</summary>
public class PlayerMovement : MonoBehaviour
{
	[Header("连击")]
	[Tooltip("超过该时间未继续攻击，连击段数会重置")]
	public float interval = 2f;
	[Tooltip("攻击动画期间的水平位移速度")]
	public float attackSpeed;

	[Header("打击感")]
	public float shakeTime;
	[Tooltip("顿帧时长，单位为 60 FPS 下的帧数")]
	public int Pause;
	public float Strength;

	[Header("移动")]
	[SerializeField]
	private float moveSpeed;
	[SerializeField]
	private float jumpSpeed;
	[Range(0f, 0.3f)]
	[SerializeField]
	private float checkRadius = 0.2f;
	[SerializeField]
	private bool isGround = true;
	[SerializeField]
	private LayerMask layer;

	private int comboStep;
	private float comboTimer;
	private bool isAttack;
	private Rigidbody2D body;
	private Animator animator;
	private float inputX;

	private void Start()
	{
		body = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		inputX = Input.GetAxisRaw("Horizontal");
		isGround = Physics2D.OverlapCircle(transform.position, checkRadius, layer) != null;
		Move();
		Jump();
		Flip();
		Attack();
	}

	private void Flip()
	{
		if (inputX < 0f)
		{
			transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		else if (inputX > 0f)
		{
			transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	private void Jump()
	{
		if (isGround && Input.GetButtonDown("Jump"))
		{
			body.velocity = new Vector2(body.velocity.x, jumpSpeed);
			animator.SetTrigger("Jump");
		}
	}

	private void Move()
	{
		if (!isAttack)
		{
			body.velocity = new Vector2(inputX * moveSpeed, body.velocity.y);
		}
		else
		{
			body.velocity = new Vector2(transform.localScale.x * attackSpeed, body.velocity.y);
		}
		animator.SetBool("isGround", isGround);
		animator.SetFloat("Horizontal", body.velocity.x);
		animator.SetFloat("Vertical", body.velocity.y);
	}

	private void Attack()
	{
		if (Input.GetButtonDown("Attack") && !isAttack)
		{
			isAttack = true;
			comboStep++;
			if (comboStep > 3)
			{
				comboStep = 1;
			}
			comboTimer = interval;
			animator.SetTrigger("Attack");
			animator.SetInteger("ComboStep", comboStep);
		}
		if (comboTimer > 0f)
		{
			comboTimer -= Time.deltaTime;
			if (comboTimer <= 0f)
			{
				comboTimer = 0f;
				comboStep = 0;
			}
		}
	}

	public void AttackOver()
	{
		isAttack = false;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (isAttack && other.CompareTag("Enemy") && other.TryGetComponent(out Enemy enemy))
		{
			if (AttackSense.Instance != null)
			{
				AttackSense.Instance.HitPause(Pause);
				AttackSense.Instance.CameraShake(shakeTime, Strength);
			}

			Vector2 hitDirection = transform.localScale.x > 0f ? Vector2.right : Vector2.left;
			enemy.GetHit(hitDirection);
		}
	}
}
}
