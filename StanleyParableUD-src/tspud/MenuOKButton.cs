using System;
using UnityEngine;

// Token: 0x02000118 RID: 280
public class MenuOKButton : MenuToggleButton
{
	// Token: 0x060006BA RID: 1722 RVA: 0x00023FA8 File Offset: 0x000221A8
	public override void OnClick(Vector3 point = default(Vector3))
	{
		base.OnClick(point);
		if (this.original)
		{
			for (int i = 0; i < this.toSave.Length; i++)
			{
				this.toSave[i].SaveChange();
			}
			Singleton<GameMaster>.Instance.WriteAllPrefs();
		}
	}

	// Token: 0x04000709 RID: 1801
	public MenuButton[] toSave;
}
