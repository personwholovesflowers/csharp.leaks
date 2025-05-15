using System;
using TMPro;
using UnityEngine;

// Token: 0x02000166 RID: 358
public class PlayerPrefsTest : MonoBehaviour
{
	// Token: 0x0600085D RID: 2141 RVA: 0x00027DFB File Offset: 0x00025FFB
	private void Start()
	{
		if (PlatformPlayerPrefs.HasKey(this.textKey))
		{
			this.text = PlatformPlayerPrefs.GetString(this.textKey);
			this.testText.text = this.text;
		}
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x00027E2C File Offset: 0x0002602C
	public void ChangeTextUp()
	{
		this.text = "Pressed up";
		PlatformPlayerPrefs.SetString(this.textKey, this.text);
		this.testText.text = this.text;
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x00027E5B File Offset: 0x0002605B
	public void ChangeTextDown()
	{
		this.text = "Pressed down";
		PlatformPlayerPrefs.SetString(this.textKey, this.text);
		this.testText.text = this.text;
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x00027E8A File Offset: 0x0002608A
	public void DeleteTextPref()
	{
		this.text = "Text was deleted";
		PlatformPlayerPrefs.DeleteKey(this.textKey);
		this.testText.text = this.text;
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x00027EB4 File Offset: 0x000260B4
	public void Update()
	{
		if (Singleton<GameMaster>.Instance.stanleyActions.MoveForward.WasPressed)
		{
			this.ChangeTextUp();
		}
		if (Singleton<GameMaster>.Instance.stanleyActions.MoveBackward.WasPressed)
		{
			this.ChangeTextDown();
		}
		if (Singleton<GameMaster>.Instance.stanleyActions.MoveRight.WasPressed)
		{
			this.DeleteTextPref();
		}
	}

	// Token: 0x04000833 RID: 2099
	private string textKey = "text";

	// Token: 0x04000834 RID: 2100
	private string text;

	// Token: 0x04000835 RID: 2101
	[SerializeField]
	private TextMeshProUGUI testText;
}
