using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.UI;

// Token: 0x0200047A RID: 1146
internal sealed class TextOverride : MonoBehaviour
{
	// Token: 0x06001A3F RID: 6719 RVA: 0x000D862C File Offset: 0x000D682C
	private void Awake()
	{
		Text text;
		TMP_Text tmp_Text;
		if (base.TryGetComponent<Text>(out text))
		{
			this.m_TextComponent = text;
		}
		else if (base.TryGetComponent<TMP_Text>(out tmp_Text))
		{
			this.m_TextComponent = tmp_Text;
		}
		if (string.IsNullOrEmpty(this.m_KeyboardText))
		{
			this.m_KeyboardText = this.GetText();
		}
	}

	// Token: 0x06001A40 RID: 6720 RVA: 0x000D8678 File Offset: 0x000D6878
	private void Update()
	{
		if (!string.IsNullOrEmpty(this.m_DualShockText) && MonoSingleton<InputManager>.Instance.LastButtonDevice is DualShockGamepad)
		{
			this.SetText(this.m_DualShockText);
			return;
		}
		if (MonoSingleton<InputManager>.Instance.LastButtonDevice is Gamepad)
		{
			this.SetText(this.m_GenericText);
			return;
		}
		this.SetText(this.m_KeyboardText);
	}

	// Token: 0x06001A41 RID: 6721 RVA: 0x000D86DA File Offset: 0x000D68DA
	private void SetText(string value)
	{
		if (this.m_TextComponent is Text)
		{
			((Text)this.m_TextComponent).text = value;
			return;
		}
		if (this.m_TextComponent is TMP_Text)
		{
			((TMP_Text)this.m_TextComponent).text = value;
		}
	}

	// Token: 0x06001A42 RID: 6722 RVA: 0x000D8719 File Offset: 0x000D6919
	private string GetText()
	{
		if (this.m_TextComponent is Text)
		{
			return ((Text)this.m_TextComponent).text;
		}
		if (this.m_TextComponent is TMP_Text)
		{
			return ((TMP_Text)this.m_TextComponent).text;
		}
		return null;
	}

	// Token: 0x040024C4 RID: 9412
	private Component m_TextComponent;

	// Token: 0x040024C5 RID: 9413
	[TextArea]
	[SerializeField]
	private string m_KeyboardText;

	// Token: 0x040024C6 RID: 9414
	[TextArea]
	[SerializeField]
	private string m_GenericText;

	// Token: 0x040024C7 RID: 9415
	[TextArea]
	[SerializeField]
	private string m_DualShockText;
}
