using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>将输入内容按时间戳追加到 TextMeshPro 聊天窗口。</summary>
public class ChatController : MonoBehaviour
{
	[Header("聊天面板引用")]
	public TMP_InputField ChatInputField;

	public TMP_Text ChatDisplayOutput;

	public Scrollbar ChatScrollbar;

	private void OnEnable()
	{
		if (ChatInputField != null)
		{
			ChatInputField.onSubmit.AddListener(AddToChatOutput);
		}
	}

	private void OnDisable()
	{
		if (ChatInputField != null)
		{
			ChatInputField.onSubmit.RemoveListener(AddToChatOutput);
		}
	}

	private void AddToChatOutput(string newText)
	{
		if (string.IsNullOrWhiteSpace(newText) || ChatInputField == null)
		{
			return;
		}

		ChatInputField.text = string.Empty;
		DateTime now = DateTime.Now;
		string text = "[<#FFFF80>" + now.Hour.ToString("d2") + ":" + now.Minute.ToString("d2") + ":" + now.Second.ToString("d2") + "</color>] " + newText;
		if (ChatDisplayOutput != null)
		{
			if (ChatDisplayOutput.text == string.Empty)
			{
				ChatDisplayOutput.text = text;
			}
			else
			{
				TMP_Text chatDisplayOutput = ChatDisplayOutput;
				chatDisplayOutput.text = chatDisplayOutput.text + "\n" + text;
			}
		}
		ChatInputField.ActivateInputField();
		if (ChatScrollbar != null)
		{
			ChatScrollbar.value = 0f;
		}
	}
}
