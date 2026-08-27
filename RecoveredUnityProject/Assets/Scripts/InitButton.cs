using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>键盘或手柄导航丢失焦点时，恢复到最近一次选择的 UI 元素。</summary>
public class InitButton : MonoBehaviour
{
	private GameObject lastSelect;

	private void Start()
	{
		lastSelect = EventSystem.current != null ? EventSystem.current.currentSelectedGameObject : null;
	}

	private void Update()
	{
		if (EventSystem.current == null)
		{
			return;
		}

		if (EventSystem.current.currentSelectedGameObject == null && lastSelect != null)
		{
			EventSystem.current.SetSelectedGameObject(lastSelect);
		}
		else
		{
			lastSelect = EventSystem.current.currentSelectedGameObject;
		}
	}
}
