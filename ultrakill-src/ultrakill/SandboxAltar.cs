using System;
using plog;
using Sandbox;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x020003A3 RID: 931
public class SandboxAltar : MonoBehaviour, IAlter, IAlterOptions<bool>, IAlterOptions<int>
{
	// Token: 0x06001557 RID: 5463 RVA: 0x000AE11D File Offset: 0x000AC31D
	private void Awake()
	{
		this.skullPrefab.SetActive(false);
	}

	// Token: 0x06001558 RID: 5464 RVA: 0x000AE12C File Offset: 0x000AC32C
	public void CreateSkull()
	{
		if (this.hasSkull || this.skull)
		{
			return;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.skullPrefab, this.defaultLocation.position, this.defaultLocation.rotation, this.defaultLocation.parent);
		gameObject.transform.localScale = this.skullPrefab.transform.localScale;
		this.skullPrefab.SetActive(false);
		gameObject.SetActive(true);
		this.skull = gameObject;
		this.hasSkull = true;
		if (this.altarTrigger != null)
		{
			this.altarTrigger.enabled = false;
		}
	}

	// Token: 0x06001559 RID: 5465 RVA: 0x000AE1D2 File Offset: 0x000AC3D2
	public void SetSkullActive(bool active)
	{
		SandboxAltar.Log.Info("Setting skull to " + active.ToString(), null, null, null);
		this.skullPrefab.SetActive(false);
		if (active)
		{
			this.CreateSkull();
			return;
		}
		this.RemoveSkull();
	}

	// Token: 0x0600155A RID: 5466 RVA: 0x000AE210 File Offset: 0x000AC410
	public void RemoveSkull()
	{
		if (!this.hasSkull)
		{
			return;
		}
		SandboxAltar.Log.Info("Deleting skull", null, null, null);
		Object.Destroy(this.skull);
		this.hasSkull = false;
		if (this.altarTrigger != null)
		{
			this.altarTrigger.enabled = true;
		}
	}

	// Token: 0x17000185 RID: 389
	// (get) Token: 0x0600155B RID: 5467 RVA: 0x000AE264 File Offset: 0x000AC464
	public string alterKey
	{
		get
		{
			return "ultrakill.sandbox.altar";
		}
	}

	// Token: 0x17000186 RID: 390
	// (get) Token: 0x0600155C RID: 5468 RVA: 0x000AE26B File Offset: 0x000AC46B
	public string alterCategoryName
	{
		get
		{
			return "Sandbox";
		}
	}

	// Token: 0x17000187 RID: 391
	// (get) Token: 0x0600155D RID: 5469 RVA: 0x000AE274 File Offset: 0x000AC474
	AlterOption<bool>[] IAlterOptions<bool>.options
	{
		get
		{
			return new AlterOption<bool>[]
			{
				new AlterOption<bool>
				{
					name = "Has Skull",
					key = "hasSkull",
					value = this.hasSkull,
					callback = new Action<bool>(this.SetSkullActive)
				}
			};
		}
	}

	// Token: 0x17000188 RID: 392
	// (get) Token: 0x0600155E RID: 5470 RVA: 0x000AE2C4 File Offset: 0x000AC4C4
	AlterOption<int>[] IAlterOptions<int>.options
	{
		get
		{
			return new AlterOption<int>[]
			{
				new AlterOption<int>
				{
					name = "Altar Type",
					key = "altarType",
					value = (int)this.altarType,
					type = typeof(AltarType),
					callback = delegate(int value)
					{
						if (value == (int)this.altarType)
						{
							return;
						}
						this.altarType = (AltarType)value;
						SandboxAltar.Log.Info(string.Format("Changing altar type to {0}", this.altarType), null, null, null);
						GameObject gameObject = Object.Instantiate<GameObject>(this.altars.altarPrefabs[value].ToAsset(), base.transform.position, base.transform.rotation, base.transform.parent);
						SandboxAltar component = gameObject.GetComponent<SandboxAltar>();
						component.altarType = this.altarType;
						component.SetSkullActive(this.hasSkull);
						MonoSingleton<SandboxAlterMenu>.Instance.alterInstance.OpenProp(gameObject.GetComponent<SandboxSpawnableInstance>());
						Object.Destroy(base.gameObject);
					}
				}
			};
		}
	}

	// Token: 0x04001DA2 RID: 7586
	private static readonly global::plog.Logger Log = new global::plog.Logger("SandboxAltar");

	// Token: 0x04001DA3 RID: 7587
	public AltarType altarType;

	// Token: 0x04001DA4 RID: 7588
	private AssetReference[] altarPrefabs;

	// Token: 0x04001DA5 RID: 7589
	[SerializeField]
	private GameObject skullPrefab;

	// Token: 0x04001DA6 RID: 7590
	[SerializeField]
	private Transform defaultLocation;

	// Token: 0x04001DA7 RID: 7591
	[SerializeField]
	private Collider altarTrigger;

	// Token: 0x04001DA8 RID: 7592
	[SerializeField]
	private Altars altars;

	// Token: 0x04001DA9 RID: 7593
	private bool hasSkull;

	// Token: 0x04001DAA RID: 7594
	private GameObject skull;
}
