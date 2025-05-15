using System;
using System.Linq;
using Sandbox.Arm;
using UnityEngine;

// Token: 0x0200025C RID: 604
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class IconManager : MonoSingleton<IconManager>
{
	// Token: 0x1700012A RID: 298
	// (get) Token: 0x06000D57 RID: 3415 RVA: 0x00065C0B File Offset: 0x00063E0B
	public int CurrentIconPackId
	{
		get
		{
			if (!this.prefFetched)
			{
				return this.FetchSavedPref();
			}
			return this.currentIconPack;
		}
	}

	// Token: 0x1700012B RID: 299
	// (get) Token: 0x06000D58 RID: 3416 RVA: 0x00065C22 File Offset: 0x00063E22
	public CheatAssetObject CurrentIcons
	{
		get
		{
			return this.iconPacks[this.CurrentIconPackId];
		}
	}

	// Token: 0x06000D59 RID: 3417 RVA: 0x00065C31 File Offset: 0x00063E31
	protected override void OnEnable()
	{
		base.OnEnable();
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06000D5A RID: 3418 RVA: 0x00065C59 File Offset: 0x00063E59
	private void OnDisable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06000D5B RID: 3419 RVA: 0x00065C7C File Offset: 0x00063E7C
	private void OnPrefChanged(string key, object value)
	{
		if (key == "iconPack" && value is int)
		{
			int num = (int)value;
			this.SetIconPack(num, false);
			this.Reload();
		}
	}

	// Token: 0x06000D5C RID: 3420 RVA: 0x00065CB3 File Offset: 0x00063EB3
	private int FetchSavedPref()
	{
		this.prefFetched = true;
		this.currentIconPack = MonoSingleton<PrefsManager>.Instance.GetInt("iconPack", 0);
		return this.currentIconPack;
	}

	// Token: 0x06000D5D RID: 3421 RVA: 0x00065CD8 File Offset: 0x00063ED8
	public string[] AvailableIconPacks()
	{
		if (this.iconPacks == null)
		{
			return Array.Empty<string>();
		}
		return (from ip in this.iconPacks
			where ip != null
			select ip.name).ToArray<string>();
	}

	// Token: 0x06000D5E RID: 3422 RVA: 0x00065D46 File Offset: 0x00063F46
	public void SetIconPack(int pack, bool setPref = true)
	{
		Debug.Log("Selecting icon pack " + pack.ToString());
		this.currentIconPack = pack;
		if (setPref)
		{
			MonoSingleton<PrefsManager>.Instance.SetInt("iconPack", pack);
		}
	}

	// Token: 0x06000D5F RID: 3423 RVA: 0x00065D78 File Offset: 0x00063F78
	public void Reload()
	{
		MonoSingleton<CheatsManager>.Instance.RebuildIcons();
		MonoSingleton<CheatsManager>.Instance.RebuildMenu();
		if (MonoSingleton<SpawnMenu>.Instance)
		{
			MonoSingleton<SpawnMenu>.Instance.RebuildIcons();
			MonoSingleton<SpawnMenu>.Instance.RebuildMenu();
		}
		if (MonoSingleton<SandboxArm>.Instance)
		{
			MonoSingleton<SandboxArm>.Instance.ReloadIcon();
		}
	}

	// Token: 0x040011F4 RID: 4596
	[SerializeField]
	private CheatAssetObject[] iconPacks;

	// Token: 0x040011F5 RID: 4597
	private int currentIconPack;

	// Token: 0x040011F6 RID: 4598
	private bool prefFetched;
}
