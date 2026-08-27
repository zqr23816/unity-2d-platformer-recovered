using System.Collections;
using UnityEngine;

/// <summary>集中管理攻击命中时的顿帧与摄像机震动反馈。</summary>
public class AttackSense : MonoBehaviour
{
	private static AttackSense instance;
	private bool isShake;

	public static AttackSense Instance
	{
		get => instance != null ? instance : (instance = FindObjectOfType<AttackSense>());
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	public void HitPause(int duration)
	{
		StartCoroutine(Pause(duration));
	}

	private IEnumerator Pause(int duration)
	{
		// duration 按 60 FPS 的帧数配置，实时等待不受 timeScale 影响。
		float time = Mathf.Max(0, duration) / 60f;
		float previousScale = Time.timeScale;
		Time.timeScale = 0f;
		yield return new WaitForSecondsRealtime(time);
		Time.timeScale = previousScale;
	}

	public void CameraShake(float duration, float strength)
	{
		if (!isShake)
		{
			StartCoroutine(Shake(duration, strength));
		}
	}

	private IEnumerator Shake(float duration, float strength)
	{
		isShake = true;
		Camera mainCamera = Camera.main;
		if (mainCamera == null)
		{
			isShake = false;
			yield break;
		}

		Transform camera = mainCamera.transform;
		Vector3 startPosition = camera.position;
		while (duration > 0f)
		{
			Vector2 offset = Random.insideUnitCircle * strength;
			camera.position = startPosition + new Vector3(offset.x, offset.y, 0f);
			duration -= Time.unscaledDeltaTime;
			yield return null;
		}
		camera.position = startPosition;
		isShake = false;
	}
}
