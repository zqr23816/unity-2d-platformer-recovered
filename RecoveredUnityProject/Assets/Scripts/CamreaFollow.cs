using UnityEngine;

/// <summary>在边界内平滑跟随目标。类名保留原拼写以兼容 Unity 场景引用。</summary>
public class CamreaFollow : MonoBehaviour
{
	[Tooltip("需要跟随的玩家 Transform")]
	public Transform target;
	[Range(0f, 1f)]
	public float smoothing;
	public Vector2 minPosition;
	public Vector2 maxPosition;

	private void LateUpdate()
	{
		if (target != null)
		{
			Vector3 position = target.position;
			position.x = Mathf.Clamp(position.x, minPosition.x, maxPosition.x);
			position.y = Mathf.Clamp(position.y, minPosition.y, maxPosition.y);
			// 摄像机保留自身 Z 轴，避免逐帧插值到玩家所在平面后看不到场景。
			position.z = transform.position.z;
			transform.position = Vector3.Lerp(transform.position, position, smoothing);
		}
	}

	public void SetCamPosLimit(Vector2 minPos, Vector2 maxPos)
	{
		minPosition = minPos;
		maxPosition = maxPos;
	}
}
