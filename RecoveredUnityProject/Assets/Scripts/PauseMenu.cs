using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>控制暂停面板、全局时间缩放和返回主菜单。</summary>
public class PauseMenu : MonoBehaviour
{
	public static bool GameIsPaused;

	public GameObject pauseMenuUI;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (GameIsPaused)
			{
				Resume();
			}
			else
			{
				Pause();
			}
		}
	}

	public void Resume()
	{
		pauseMenuUI.SetActive(value: false);
		Time.timeScale = 1f;
		GameIsPaused = false;
	}

	public void Pause()
	{
		pauseMenuUI.SetActive(value: true);
		Time.timeScale = 0f;
		GameIsPaused = true;
	}

	public void MainMenu()
	{
		GameIsPaused = false;
		Time.timeScale = 1f;
		SceneManager.LoadScene("Menu");
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	private void OnDisable()
	{
		// 切换场景或禁用菜单时，保证不会把全局时间永久留在暂停状态。
		if (GameIsPaused)
		{
			Time.timeScale = 1f;
			GameIsPaused = false;
		}
	}
}
