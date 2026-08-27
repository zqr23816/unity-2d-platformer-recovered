using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>玩家离开关卡出口触发器后加载构建列表中的下一场景。</summary>
public class DoortoNextScene : MonoBehaviour
{
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player") && other is BoxCollider2D)
		{
			int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
			if (nextIndex < SceneManager.sceneCountInBuildSettings)
			{
				SceneManager.LoadScene(nextIndex);
			}
			else
			{
				Debug.LogWarning("已到达最后一个关卡，未配置下一场景。");
			}
		}
	}
}
