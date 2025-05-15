using System;

// Token: 0x02000116 RID: 278
public class MenuMasterVolSlider : MenuSlider
{
	// Token: 0x060006B2 RID: 1714 RVA: 0x00023F31 File Offset: 0x00022131
	private void OnEnable()
	{
		base.position = Singleton<GameMaster>.Instance.masterVolume;
	}

	// Token: 0x060006B3 RID: 1715 RVA: 0x00023F43 File Offset: 0x00022143
	protected override void Changed()
	{
		base.Changed();
	}

	// Token: 0x060006B4 RID: 1716 RVA: 0x00023F4B File Offset: 0x0002214B
	public override void SaveChange()
	{
		base.SaveChange();
		if (this.original)
		{
			Singleton<GameMaster>.Instance.SetMasterVolume(base.position);
		}
	}
}
