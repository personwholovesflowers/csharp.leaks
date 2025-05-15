using System;

namespace Sandbox
{
	// Token: 0x0200055B RID: 1371
	[Serializable]
	public class EnemyRadianceConfig
	{
		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06001ED9 RID: 7897 RVA: 0x000FD5EC File Offset: 0x000FB7EC
		public bool damageEnabled
		{
			get
			{
				return this.enabled && this.tier > 0f && this.damageBuff > 0f;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06001EDA RID: 7898 RVA: 0x000FD612 File Offset: 0x000FB812
		public bool speedEnabled
		{
			get
			{
				return this.enabled && this.tier > 0f && this.speedBuff > 0f;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06001EDB RID: 7899 RVA: 0x000FD638 File Offset: 0x000FB838
		public bool healthEnabled
		{
			get
			{
				return this.enabled && this.tier > 0f && this.healthBuff > 0f;
			}
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x000FD65E File Offset: 0x000FB85E
		public EnemyRadianceConfig()
		{
		}

		// Token: 0x06001EDD RID: 7901 RVA: 0x000FD671 File Offset: 0x000FB871
		public EnemyRadianceConfig(EnemyIdentifier enemyId)
		{
			this.damageBuff = enemyId.damageBuffModifier;
			this.speedBuff = enemyId.speedBuffModifier;
			this.healthBuff = enemyId.healthBuffModifier;
		}

		// Token: 0x04002B46 RID: 11078
		public bool enabled;

		// Token: 0x04002B47 RID: 11079
		public float tier = 1f;

		// Token: 0x04002B48 RID: 11080
		public float damageBuff;

		// Token: 0x04002B49 RID: 11081
		public float speedBuff;

		// Token: 0x04002B4A RID: 11082
		public float healthBuff;
	}
}
