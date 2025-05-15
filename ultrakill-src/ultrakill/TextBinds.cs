using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000479 RID: 1145
public class TextBinds : MonoBehaviour
{
	// Token: 0x06001A3D RID: 6717 RVA: 0x000D848C File Offset: 0x000D668C
	private void OnEnable()
	{
		string text;
		if (this.input == "")
		{
			text = this.text1;
		}
		else
		{
			KeyCode keyCode = MonoSingleton<InputManager>.Instance.Inputs[this.input];
			string text2;
			if (keyCode == KeyCode.Mouse0)
			{
				text2 = "Left Mouse Button";
			}
			else if (keyCode == KeyCode.Mouse1)
			{
				text2 = "Right Mouse Button";
			}
			else if (keyCode == KeyCode.Mouse2)
			{
				text2 = "Middle Mouse Button";
			}
			else if (keyCode == KeyCode.Mouse3 || keyCode == KeyCode.Mouse4 || keyCode == KeyCode.Mouse5 || keyCode == KeyCode.Mouse6)
			{
				text2 = keyCode.ToString();
				string text3 = text2.Substring(text2.Length - 1, 1);
				text2 = text2.Substring(0, text2.Length - 1);
				text2 += (int.Parse(text3) + 1).ToString();
			}
			else
			{
				text2 = keyCode.ToString();
			}
			text2 = MonoSingleton<InputManager>.Instance.GetBindingString(this.input) ?? text2;
			text = this.text1 + text2 + this.text2;
		}
		text = text.Replace('$', '\n');
		if (!this.text && !this.textmp)
		{
			this.text = base.GetComponent<Text>();
			if (!this.text)
			{
				this.textmp = base.GetComponent<TMP_Text>();
			}
		}
		if (this.text)
		{
			this.text.text = text;
			return;
		}
		if (this.textmp)
		{
			this.textmp.text = text;
		}
	}

	// Token: 0x040024BF RID: 9407
	public string text1;

	// Token: 0x040024C0 RID: 9408
	public string input;

	// Token: 0x040024C1 RID: 9409
	public string text2;

	// Token: 0x040024C2 RID: 9410
	private Text text;

	// Token: 0x040024C3 RID: 9411
	private TMP_Text textmp;
}
