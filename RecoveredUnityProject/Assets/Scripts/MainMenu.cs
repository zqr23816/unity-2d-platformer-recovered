using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>主菜单按钮事件：开始游戏与退出程序。</summary>
public class MainMenu : MonoBehaviour
{
	public void PlayGame()
	{
		SceneManager.LoadScene(1);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
