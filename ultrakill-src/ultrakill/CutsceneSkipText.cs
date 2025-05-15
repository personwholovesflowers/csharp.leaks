using System;
using TMPro;

// Token: 0x020000FD RID: 253
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class CutsceneSkipText : MonoSingleton<CutsceneSkipText>
{
	// Token: 0x060004E5 RID: 1253 RVA: 0x0002144F File Offset: 0x0001F64F
	private void Start()
	{
		this.txt = base.GetComponent<TMP_Text>();
		this.Hide();
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x00021463 File Offset: 0x0001F663
	public void Show()
	{
		this.txt.enabled = true;
	}

	// Token: 0x060004E7 RID: 1255 RVA: 0x00021471 File Offset: 0x0001F671
	public void Hide()
	{
		this.txt.enabled = false;
	}

	// Token: 0x040006AA RID: 1706
	private TMP_Text txt;
}
