using UnityEngine;
using UnityEngine.UI;

/// <summary>把玩家生命值同步到填充图片和数值文本。</summary>
public class HealthBar : MonoBehaviour
{
	[Tooltip("显示为 当前生命/最大生命 的文本")]
	public Text healthText;

	public static int HealthCurrent;

	public static int HealthMax;

	private Image healthBar;

	private void Start()
	{
		healthBar = GetComponent<Image>();
	}

	private void Update()
	{
		int safeMax = Mathf.Max(1, HealthMax);
		healthBar.fillAmount = Mathf.Clamp01((float)HealthCurrent / safeMax);
		if (healthText != null)
		{
			healthText.text = HealthCurrent + "/" + HealthMax;
		}
	}
}
