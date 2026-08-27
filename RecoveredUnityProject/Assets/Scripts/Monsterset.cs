using UnityEngine;

/// <summary>轻量级怪物生命组件；生命归零时销毁对象。</summary>
public class Monsterset : MonoBehaviour
{
	[Min(1)]
	public int health;
	public int damage;

	public void TakeDamage(int damage)
	{
		health = Mathf.Max(0, health - Mathf.Max(0, damage));
		if (health == 0)
		{
			Destroy(gameObject);
		}
	}
}
