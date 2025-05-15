using System;

// Token: 0x02000117 RID: 279
public class MenuMusicVolSlider : MenuSlider
{
	// Token: 0x060006B6 RID: 1718 RVA: 0x00023F73 File Offset: 0x00022173
	private void OnEnable()
	{
		base.position = Singleton<GameMaster>.Instance.musicVolume;
	}

	// Token: 0x060006B7 RID: 1719 RVA: 0x00023F43 File Offset: 0x00022143
	protected override void Changed()
	{
		base.Changed();
	}

	// Token: 0x060006B8 RID: 1720 RVA: 0x00023F85 File Offset: 0x00022185
	public override void SaveChange()
	{
		base.SaveChange();
		if (this.original)
		{
			Singleton<GameMaster>.Instance.SetMusicVolume(base.position);
		}
	}
}
