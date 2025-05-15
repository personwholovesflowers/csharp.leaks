using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000040 RID: 64
public class PlayerLoadout : MonoBehaviour, IPlaceholdableComponent
{
	// Token: 0x0600013D RID: 317 RVA: 0x00006B8C File Offset: 0x00004D8C
	private void Start()
	{
		if (!this.forceStartLoadout || SceneHelper.IsPlayingCustom)
		{
			return;
		}
		this.SetLoadout();
	}

	// Token: 0x0600013E RID: 318 RVA: 0x00006BA4 File Offset: 0x00004DA4
	public void WillReplace(GameObject oldObject, GameObject newObject, bool isSelfBeingReplaced)
	{
		if (!isSelfBeingReplaced || !this.forceStartLoadout)
		{
			return;
		}
		PlayerLoadoutTarget component = newObject.GetComponent<PlayerLoadoutTarget>();
		if (!component)
		{
			return;
		}
		component.CommitLoadout(this.loadout);
	}

	// Token: 0x0600013F RID: 319 RVA: 0x00006BD9 File Offset: 0x00004DD9
	public void SetLoadout()
	{
		MonoSingleton<GunSetter>.Instance.forcedLoadout = this.loadout;
		MonoSingleton<GunSetter>.Instance.ResetWeapons(false);
		MonoSingleton<FistControl>.Instance.forcedLoadout = this.loadout;
		MonoSingleton<FistControl>.Instance.ResetFists();
	}

	// Token: 0x04000128 RID: 296
	[FormerlySerializedAs("forceLoadout")]
	public bool forceStartLoadout;

	// Token: 0x04000129 RID: 297
	public ForcedLoadout loadout;
}
