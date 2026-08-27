using UnityEngine;
using UnityEngine.UI;

/// <summary>玩家进入告示牌范围后，按 F 显示配置的文本。</summary>
public class Sign : MonoBehaviour
{
	[Header("交互提示")]
	public GameObject dialogBox;
	public Text dialogBoxText;
	[TextArea(2, 6)]
	public string signText;

	private bool isPlayerInSign;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F) && isPlayerInSign && dialogBox != null)
		{
			if (dialogBoxText != null)
			{
				dialogBoxText.text = signText;
			}
			dialogBox.SetActive(value: true);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			isPlayerInSign = true;
			Debug.Log("进入招牌范围");
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			isPlayerInSign = false;
			if (dialogBox != null)
			{
				dialogBox.SetActive(value: false);
			}
		}
	}
}
