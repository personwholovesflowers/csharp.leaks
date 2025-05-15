using System;
using System.Collections.Generic;
using plog;

// Token: 0x02000257 RID: 599
public class HurtCooldownCollection
{
	// Token: 0x06000D3A RID: 3386 RVA: 0x00064A1C File Offset: 0x00062C1C
	public bool TryHurtCheckEnemy(EnemyIdentifier eid, bool autoUpdate = true)
	{
		TimeSince timeSince;
		if (this.timeSinceHurtEnemies.TryGetValue(eid, out timeSince))
		{
			if (timeSince < 0.5f)
			{
				return false;
			}
			if (autoUpdate)
			{
				this.timeSinceHurtEnemies[eid] = 0f;
			}
		}
		else if (autoUpdate)
		{
			this.timeSinceHurtEnemies.Add(eid, 0f);
		}
		return true;
	}

	// Token: 0x06000D3B RID: 3387 RVA: 0x00064A7D File Offset: 0x00062C7D
	public void ResetEnemyCooldown(EnemyIdentifier eid)
	{
		this.timeSinceHurtEnemies.Remove(eid);
	}

	// Token: 0x06000D3C RID: 3388 RVA: 0x00064A8C File Offset: 0x00062C8C
	public bool TryHurtCheckPlayer(bool autoUpdate = true)
	{
		if (this.timeSinceHurtPlayer != null)
		{
			TimeSince? timeSince = this.timeSinceHurtPlayer;
			float? num = ((timeSince != null) ? new float?(timeSince.GetValueOrDefault()) : null);
			float num2 = 0.5f;
			if ((num.GetValueOrDefault() < num2) & (num != null))
			{
				return false;
			}
			if (autoUpdate)
			{
				this.timeSinceHurtPlayer = new TimeSince?(0f);
			}
		}
		else if (autoUpdate)
		{
			this.timeSinceHurtPlayer = new TimeSince?(0f);
		}
		return true;
	}

	// Token: 0x06000D3D RID: 3389 RVA: 0x00064B22 File Offset: 0x00062D22
	public void ResetPlayerCooldown()
	{
		this.timeSinceHurtPlayer = null;
	}

	// Token: 0x06000D3E RID: 3390 RVA: 0x00064B30 File Offset: 0x00062D30
	public bool TryHurtCheckFlammable(Flammable flammable, bool autoUpdate = true)
	{
		TimeSince timeSince;
		if (this.timeSinceHurtFlammables.TryGetValue(flammable, out timeSince))
		{
			if (timeSince < 0.5f)
			{
				return false;
			}
			if (autoUpdate)
			{
				this.timeSinceHurtFlammables[flammable] = 0f;
			}
		}
		else if (autoUpdate)
		{
			this.timeSinceHurtFlammables.Add(flammable, 0f);
		}
		return true;
	}

	// Token: 0x06000D3F RID: 3391 RVA: 0x00064B91 File Offset: 0x00062D91
	public void ResetFlammableCooldown(Flammable flammable)
	{
		this.timeSinceHurtFlammables.Remove(flammable);
	}

	// Token: 0x040011CD RID: 4557
	private static readonly Logger Log = new Logger("HurtCooldownCollection");

	// Token: 0x040011CE RID: 4558
	private const float HurtDelay = 0.5f;

	// Token: 0x040011CF RID: 4559
	private readonly Dictionary<EnemyIdentifier, TimeSince> timeSinceHurtEnemies = new Dictionary<EnemyIdentifier, TimeSince>();

	// Token: 0x040011D0 RID: 4560
	private readonly Dictionary<Flammable, TimeSince> timeSinceHurtFlammables = new Dictionary<Flammable, TimeSince>();

	// Token: 0x040011D1 RID: 4561
	private TimeSince? timeSinceHurtPlayer;
}
