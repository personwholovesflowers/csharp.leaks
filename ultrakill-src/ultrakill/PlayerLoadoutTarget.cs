using System;
using UnityEngine;

// Token: 0x02000045 RID: 69
public class PlayerLoadoutTarget : MonoBehaviour
{
	// Token: 0x06000144 RID: 324 RVA: 0x00006C10 File Offset: 0x00004E10
	public void CommitLoadout(ForcedLoadout loadout)
	{
		Debug.Log(string.Format("Setting loadout on {0}", loadout));
		MonoSingleton<GunSetter>.Instance.forcedLoadout = loadout;
		MonoSingleton<GunSetter>.Instance.ResetWeapons(false);
		MonoSingleton<FistControl>.Instance.forcedLoadout = loadout;
		MonoSingleton<FistControl>.Instance.ResetFists();
	}
}
