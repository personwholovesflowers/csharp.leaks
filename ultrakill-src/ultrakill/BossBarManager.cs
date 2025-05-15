using System;
using System.Collections.Generic;
using plog;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200008C RID: 140
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class BossBarManager : MonoSingleton<BossBarManager>
{
	// Token: 0x060002AF RID: 687 RVA: 0x0000F988 File Offset: 0x0000DB88
	public void UpdateBossBar(BossHealthBar bossBar)
	{
		if (bossBar.source != null && bossBar.source.Dead)
		{
			return;
		}
		int bossBarId = bossBar.bossBarId;
		IEnemyHealthDetails source = bossBar.source;
		if (!this.bossBars.ContainsKey(bossBarId))
		{
			this.CreateBossBar(bossBar);
		}
		BossHealthBarTemplate bossHealthBarTemplate = this.bossBars[bossBarId];
		bossHealthBarTemplate.UpdateState(source);
		if (bossBar.secondaryBar)
		{
			bossHealthBarTemplate.UpdateSecondaryBar(bossBar);
		}
		else
		{
			bossHealthBarTemplate.ResetSecondaryBar();
		}
		if (bossHealthBarTemplate.bossNameText.text != bossBar.bossName.ToUpper())
		{
			bossHealthBarTemplate.ChangeName(bossBar.bossName.ToUpper());
		}
		if (!source.Dead)
		{
			this.bossBarLastUpdated[bossBarId] = 0f;
		}
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x0000FA48 File Offset: 0x0000DC48
	public void ExpireImmediately(int bossBarId)
	{
		BossHealthBarTemplate bossHealthBarTemplate;
		if (this.bossBars.TryGetValue(bossBarId, out bossHealthBarTemplate))
		{
			BossBarManager.Log.Info(string.Format("Immediately removing boss bar {0} ({1})", bossHealthBarTemplate.bossNameText.text, bossBarId), null, null, null);
			this.bossBarLastUpdated[bossBarId] = 3f;
		}
	}

	// Token: 0x060002B1 RID: 689 RVA: 0x0000FAA4 File Offset: 0x0000DCA4
	private void CreateBossBar(BossHealthBar bossBar)
	{
		BossBarManager.Log.Info(string.Format("Creating Boss Bar for {0} ({1})", bossBar.bossName, bossBar.bossBarId), null, null, null);
		BossHealthBarTemplate bossHealthBarTemplate = Object.Instantiate<BossHealthBarTemplate>(this.template, this.containerRect);
		bossHealthBarTemplate.Initialize(bossBar, this.layers);
		bossHealthBarTemplate.UpdateState(bossBar.source);
		this.bossBars.Add(bossBar.bossBarId, bossHealthBarTemplate);
		if (this.bossBarsVisible)
		{
			this.RecalculateStretch();
			return;
		}
		bossHealthBarTemplate.SetVisible(false);
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x0000FB2C File Offset: 0x0000DD2C
	private void Update()
	{
		while (this.bossBarsToRemove.Count > 0)
		{
			int num = this.bossBarsToRemove.Dequeue();
			BossBarManager.Log.Info(string.Format("Removing Expired Boss Bar for {0} ({1})", this.bossBars[num].bossNameText.text, num), null, null, null);
			Object.Destroy(this.bossBars[num].gameObject);
			this.bossBars.Remove(num);
			this.bossBarLastUpdated.Remove(num);
			this.RecalculateStretch();
		}
		foreach (KeyValuePair<int, BossHealthBarTemplate> keyValuePair in this.bossBars)
		{
			int key = keyValuePair.Key;
			if (this.bossBarLastUpdated[key] > 3f && !this.bossBarsToRemove.Contains(key))
			{
				this.bossBarsToRemove.Enqueue(key);
			}
		}
		if (this.bossBarsVisible == HideUI.Active)
		{
			this.bossBarsVisible = !HideUI.Active;
			this.RefreshVisibility();
		}
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x0000FC5C File Offset: 0x0000DE5C
	private void RecalculateStretch()
	{
		float num = 1f;
		if (this.bossBars.Count > 2)
		{
			num = this.baseOverflowedSize - (float)(this.bossBars.Count - 2) * this.overflowShrinkFactor;
		}
		num = Mathf.Max(this.minimumSize, num);
		this.containerRect.localScale = new Vector3(1f, num, 1f);
		foreach (BossHealthBarTemplate bossHealthBarTemplate in this.bossBars.Values)
		{
			bossHealthBarTemplate.ScaleChanged(num);
		}
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x0000FD0C File Offset: 0x0000DF0C
	private void RefreshVisibility()
	{
		foreach (BossHealthBarTemplate bossHealthBarTemplate in this.bossBars.Values)
		{
			if (!(bossHealthBarTemplate == null))
			{
				bossHealthBarTemplate.SetVisible(this.bossBarsVisible);
			}
		}
		if (this.bossBarsVisible)
		{
			this.RecalculateStretch();
		}
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x0000FD80 File Offset: 0x0000DF80
	public void ForceLayoutRebuild()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.containerRect);
	}

	// Token: 0x04000333 RID: 819
	private static readonly global::plog.Logger Log = new global::plog.Logger("BossBarManager");

	// Token: 0x04000334 RID: 820
	[SerializeField]
	private float overflowShrinkFactor = 0.14f;

	// Token: 0x04000335 RID: 821
	[SerializeField]
	private float minimumSize = 0.3f;

	// Token: 0x04000336 RID: 822
	[SerializeField]
	private float baseOverflowedSize = 0.82f;

	// Token: 0x04000337 RID: 823
	[Space]
	[SerializeField]
	private RectTransform containerRect;

	// Token: 0x04000338 RID: 824
	[SerializeField]
	private BossHealthBarTemplate template;

	// Token: 0x04000339 RID: 825
	[SerializeField]
	private SliderLayer[] layers;

	// Token: 0x0400033A RID: 826
	private readonly Dictionary<int, BossHealthBarTemplate> bossBars = new Dictionary<int, BossHealthBarTemplate>();

	// Token: 0x0400033B RID: 827
	private readonly Dictionary<int, TimeSince> bossBarLastUpdated = new Dictionary<int, TimeSince>();

	// Token: 0x0400033C RID: 828
	private readonly Queue<int> bossBarsToRemove = new Queue<int>();

	// Token: 0x0400033D RID: 829
	private bool bossBarsVisible = true;

	// Token: 0x0400033E RID: 830
	private const float BossBarTimeToExpire = 3f;
}
