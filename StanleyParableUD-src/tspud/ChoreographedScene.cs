using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200009D RID: 157
public class ChoreographedScene : HammerEntity
{
	// Token: 0x060003D0 RID: 976 RVA: 0x00018808 File Offset: 0x00016A08
	private void Start()
	{
		for (int i = 0; i < this.choreoEvents.Count; i++)
		{
			this.choreoEvents[i].owner = this;
		}
		if (this.choreoEvents.Count >= 1)
		{
			this.choreoEvents[0].PreloadAudioClips();
		}
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x00005444 File Offset: 0x00003644
	private void AutoToken()
	{
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x0001885C File Offset: 0x00016A5C
	public void Input_Start()
	{
		if (this.killed || !this.isEnabled)
		{
			return;
		}
		Singleton<ChoreoMaster>.Instance.BeginEvents(this.choreoEvents, this.interrupt, this.spawnflag49);
		base.FireOutput(Outputs.OnStart);
	}

	// Token: 0x060003D3 RID: 979 RVA: 0x00018894 File Offset: 0x00016A94
	public void FinishedEvent(ChoreographedEvent e)
	{
		if (e.Clip == this.choreoEvents[this.choreoEvents.Count - 1].Clip)
		{
			base.FireOutput(Outputs.OnCompletion);
			return;
		}
		if (this.cancelled)
		{
			base.FireOutput(Outputs.OnCanceled);
		}
	}

	// Token: 0x060003D4 RID: 980 RVA: 0x000188E4 File Offset: 0x00016AE4
	public void Cancelled()
	{
		this.cancelled = true;
	}

	// Token: 0x060003D5 RID: 981 RVA: 0x000188ED File Offset: 0x00016AED
	public override void Input_Kill()
	{
		Singleton<ChoreoMaster>.Instance.DropOwnedEvents(this);
		this.killed = true;
		base.Input_Kill();
	}

	// Token: 0x040003D1 RID: 977
	public bool spawnflag49;

	// Token: 0x040003D2 RID: 978
	public ChoreographedScene.InteruptBehaviour interrupt;

	// Token: 0x040003D3 RID: 979
	public List<ChoreographedEvent> choreoEvents = new List<ChoreographedEvent>();

	// Token: 0x040003D4 RID: 980
	private bool cancelled;

	// Token: 0x040003D5 RID: 981
	private bool killed;

	// Token: 0x040003D6 RID: 982
	[InspectorButton("AutoToken", null)]
	public bool doesNothingRightNow;

	// Token: 0x040003D7 RID: 983
	[Header("Sets 'Clip' variable in events in bulk (or creates new events if there aren't enough)")]
	public List<VoiceClip> williamDragNewVoiceClipsHere;

	// Token: 0x02000398 RID: 920
	public enum InteruptBehaviour
	{
		// Token: 0x04001329 RID: 4905
		StartImmediately,
		// Token: 0x0400132A RID: 4906
		WaitForFinish,
		// Token: 0x0400132B RID: 4907
		InterruptAtNextEvent,
		// Token: 0x0400132C RID: 4908
		CancelAtNextEvent
	}
}
