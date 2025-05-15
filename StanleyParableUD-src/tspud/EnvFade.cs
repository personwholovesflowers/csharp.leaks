using System;
using UnityEngine;

// Token: 0x020000B4 RID: 180
public class EnvFade : HammerEntity
{
	// Token: 0x0600042A RID: 1066 RVA: 0x0001965C File Offset: 0x0001785C
	public void Input_Fade()
	{
		Singleton<GameMaster>.Instance.BeginFade(this.renderColor, this.inDuration, this.holdDuration, this.fadeFrom, this.stayOut);
		this.LogFade();
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x0001968C File Offset: 0x0001788C
	public void Input_FadeReverse()
	{
		Singleton<GameMaster>.Instance.BeginFade(this.renderColor, this.outDuration, this.holdDuration, true, false);
		this.LogFade();
	}

	// Token: 0x0600042C RID: 1068 RVA: 0x000196B4 File Offset: 0x000178B4
	private void LogFade()
	{
		string text = "";
		if (!this.fadeFrom)
		{
			text += "then ";
			if (this.stayOut)
			{
				text += "hold for all eternity";
				return;
			}
			if (this.holdDuration > 0f)
			{
				text = string.Concat(new object[] { text, "hold for", this.holdDuration, " seconds" });
				return;
			}
			text += "snap back to normal";
		}
	}

	// Token: 0x0400041F RID: 1055
	public Color renderColor = Color.black;

	// Token: 0x04000420 RID: 1056
	public float inDuration = 2f;

	// Token: 0x04000421 RID: 1057
	public float outDuration = 2f;

	// Token: 0x04000422 RID: 1058
	public float holdDuration;

	// Token: 0x04000423 RID: 1059
	public bool fadeFrom;

	// Token: 0x04000424 RID: 1060
	public bool stayOut;
}
