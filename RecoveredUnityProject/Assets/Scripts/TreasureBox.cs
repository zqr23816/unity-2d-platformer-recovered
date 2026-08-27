using UnityEngine;

/// <summary>玩家进入交互范围后按 F 播放一次宝箱开启动画。</summary>
public class TreasureBox : MonoBehaviour
{
	private bool canOpen;

	private bool isOpened;

	private Animator anim;

	private void Start()
	{
		anim = GetComponent<Animator>();
		isOpened = false;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F) && canOpen && !isOpened && anim != null)
		{
			anim.SetTrigger("Opening");
			isOpened = true;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && other is BoxCollider2D)
		{
			canOpen = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player") && other is BoxCollider2D)
		{
			canOpen = false;
		}
	}
}
