using System;
using UnityEngine;

namespace AmplifyBloom
{
	// Token: 0x0200032F RID: 815
	[Serializable]
	public class GlareDefData
	{
		// Token: 0x060014C2 RID: 5314 RVA: 0x0006E92E File Offset: 0x0006CB2E
		public GlareDefData()
		{
			this.m_customStarData = new StarDefData();
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x0006E948 File Offset: 0x0006CB48
		public GlareDefData(StarLibType starType, float starInclination, float chromaticAberration)
		{
			this.m_starType = starType;
			this.m_starInclination = starInclination;
			this.m_chromaticAberration = chromaticAberration;
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x060014C4 RID: 5316 RVA: 0x0006E96C File Offset: 0x0006CB6C
		// (set) Token: 0x060014C5 RID: 5317 RVA: 0x0006E974 File Offset: 0x0006CB74
		public StarLibType StarType
		{
			get
			{
				return this.m_starType;
			}
			set
			{
				this.m_starType = value;
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x060014C6 RID: 5318 RVA: 0x0006E97D File Offset: 0x0006CB7D
		// (set) Token: 0x060014C7 RID: 5319 RVA: 0x0006E985 File Offset: 0x0006CB85
		public float StarInclination
		{
			get
			{
				return this.m_starInclination;
			}
			set
			{
				this.m_starInclination = value;
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x060014C8 RID: 5320 RVA: 0x0006E98E File Offset: 0x0006CB8E
		// (set) Token: 0x060014C9 RID: 5321 RVA: 0x0006E99C File Offset: 0x0006CB9C
		public float StarInclinationDeg
		{
			get
			{
				return this.m_starInclination * 57.29578f;
			}
			set
			{
				this.m_starInclination = value * 0.017453292f;
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x060014CA RID: 5322 RVA: 0x0006E9AB File Offset: 0x0006CBAB
		// (set) Token: 0x060014CB RID: 5323 RVA: 0x0006E9B3 File Offset: 0x0006CBB3
		public float ChromaticAberration
		{
			get
			{
				return this.m_chromaticAberration;
			}
			set
			{
				this.m_chromaticAberration = value;
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060014CC RID: 5324 RVA: 0x0006E9BC File Offset: 0x0006CBBC
		// (set) Token: 0x060014CD RID: 5325 RVA: 0x0006E9C4 File Offset: 0x0006CBC4
		public StarDefData CustomStarData
		{
			get
			{
				return this.m_customStarData;
			}
			set
			{
				this.m_customStarData = value;
			}
		}

		// Token: 0x040010DD RID: 4317
		public bool FoldoutValue = true;

		// Token: 0x040010DE RID: 4318
		[SerializeField]
		private StarLibType m_starType;

		// Token: 0x040010DF RID: 4319
		[SerializeField]
		private float m_starInclination;

		// Token: 0x040010E0 RID: 4320
		[SerializeField]
		private float m_chromaticAberration;

		// Token: 0x040010E1 RID: 4321
		[SerializeField]
		private StarDefData m_customStarData;
	}
}
