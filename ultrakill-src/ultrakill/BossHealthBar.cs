using System;
using plog;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200008F RID: 143
public class BossHealthBar : MonoBehaviour
{
	// Token: 0x060002BA RID: 698 RVA: 0x0000FE00 File Offset: 0x0000E000
	private void Awake()
	{
		this.source = base.GetComponent<IEnemyHealthDetails>();
		this.source.ForceGetHealth();
		if (this.healthLayers == null)
		{
			this.healthLayers = Array.Empty<HealthLayer>();
		}
		if (this.healthLayers.Length == 0)
		{
			this.healthLayers = new HealthLayer[1];
			this.healthLayers[0] = new HealthLayer
			{
				health = this.source.Health
			};
		}
		if (string.IsNullOrEmpty(this.bossName))
		{
			this.bossName = this.source.FullName;
		}
		if (this.bossBarId == 0)
		{
			this.bossBarId = base.GetInstanceID();
			return;
		}
		BossHealthBar.Log.Info(string.Format("Taking over boss bar {0}", this.bossBarId), null, null, null);
	}

	// Token: 0x060002BB RID: 699 RVA: 0x0000FEBF File Offset: 0x0000E0BF
	private void Start()
	{
		MonoSingleton<BossBarManager>.Instance.UpdateBossBar(this);
	}

	// Token: 0x060002BC RID: 700 RVA: 0x0000FECC File Offset: 0x0000E0CC
	private void OnEnable()
	{
		if (!this.source.Dead)
		{
			MusicManager musicManager = MonoSingleton<MusicManager>.Instance;
			if (musicManager == null)
			{
				musicManager = MonoSingleton<MusicManager>.Instance;
			}
			if (musicManager.useBossTheme)
			{
				musicManager.PlayBossMusic();
			}
		}
	}

	// Token: 0x060002BD RID: 701 RVA: 0x0000FF09 File Offset: 0x0000E109
	public void UpdateSecondaryBar(float value)
	{
		this.secondaryBarValue = value;
	}

	// Token: 0x060002BE RID: 702 RVA: 0x0000FF12 File Offset: 0x0000E112
	public void SetSecondaryBarColor(Color clr)
	{
		this.secondaryBarColor = clr;
	}

	// Token: 0x060002BF RID: 703 RVA: 0x0000FEBF File Offset: 0x0000E0BF
	private void Update()
	{
		MonoSingleton<BossBarManager>.Instance.UpdateBossBar(this);
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x0000FF1B File Offset: 0x0000E11B
	private void OnDisable()
	{
		this.DisappearBar();
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x0000FF23 File Offset: 0x0000E123
	public void DisappearBar()
	{
		if (MonoSingleton<BossBarManager>.Instance)
		{
			MonoSingleton<BossBarManager>.Instance.ExpireImmediately(this.bossBarId);
		}
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x0000FF41 File Offset: 0x0000E141
	public void ChangeName(string newName)
	{
		this.bossName = newName;
		MonoSingleton<BossBarManager>.Instance.UpdateBossBar(this);
	}

	// Token: 0x04000342 RID: 834
	private static readonly global::plog.Logger Log = new global::plog.Logger("BossHealthBar");

	// Token: 0x04000343 RID: 835
	[HideInInspector]
	public int bossBarId;

	// Token: 0x04000344 RID: 836
	[HideInInspector]
	public IEnemyHealthDetails source;

	// Token: 0x04000345 RID: 837
	public HealthLayer[] healthLayers;

	// Token: 0x04000346 RID: 838
	public string bossName;

	// Token: 0x04000347 RID: 839
	public bool secondaryBar;

	// Token: 0x04000348 RID: 840
	[FormerlySerializedAs("secondaryColor")]
	[SerializeField]
	public Color secondaryBarColor = Color.white;

	// Token: 0x04000349 RID: 841
	public float secondaryBarValue;
}
