using System;
using UnityEngine;

// Token: 0x020004B6 RID: 1206
public class ViewModelFlip : MonoBehaviour
{
	// Token: 0x06001BA7 RID: 7079 RVA: 0x000E52D0 File Offset: 0x000E34D0
	private void OnEnable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06001BA8 RID: 7080 RVA: 0x000E52F2 File Offset: 0x000E34F2
	private void OnDisable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06001BA9 RID: 7081 RVA: 0x000E5314 File Offset: 0x000E3514
	private void OnPrefChanged(string key, object value)
	{
		if (key == "weaponHoldPosition" && value is int)
		{
			int num = (int)value;
			if (num == 2)
			{
				this.Left();
				return;
			}
			this.Right();
		}
	}

	// Token: 0x06001BAA RID: 7082 RVA: 0x000E534E File Offset: 0x000E354E
	private void Start()
	{
		if (MonoSingleton<PrefsManager>.Instance.GetInt("weaponHoldPosition", 0) == 2)
		{
			this.Left();
			return;
		}
		this.Right();
	}

	// Token: 0x06001BAB RID: 7083 RVA: 0x000E5370 File Offset: 0x000E3570
	public void Left()
	{
		base.transform.localScale = new Vector3(-1f, 1f, 1f);
	}

	// Token: 0x06001BAC RID: 7084 RVA: 0x000E5391 File Offset: 0x000E3591
	public void Right()
	{
		base.transform.localScale = new Vector3(1f, 1f, 1f);
	}
}
