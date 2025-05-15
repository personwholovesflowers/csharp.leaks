using System;
using UnityEngine;

namespace AmplifyBloom
{
	// Token: 0x02000338 RID: 824
	[Serializable]
	public class StarDefData
	{
		// Token: 0x0600151F RID: 5407 RVA: 0x0006FFA0 File Offset: 0x0006E1A0
		public StarDefData()
		{
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x0006FFED File Offset: 0x0006E1ED
		public void Destroy()
		{
			this.m_starLinesArr = null;
		}

		// Token: 0x06001521 RID: 5409 RVA: 0x0006FFF8 File Offset: 0x0006E1F8
		public StarDefData(StarLibType starType, string starName, int starLinesCount, int passCount, float sampleLength, float attenuation, float inclination, float rotation, float longAttenuation = 0f, float customIncrement = -1f)
		{
			this.m_starType = starType;
			this.m_starName = starName;
			this.m_passCount = passCount;
			this.m_sampleLength = sampleLength;
			this.m_attenuation = attenuation;
			this.m_starlinesCount = starLinesCount;
			this.m_inclination = inclination;
			this.m_rotation = rotation;
			this.m_customIncrement = customIncrement;
			this.m_longAttenuation = longAttenuation;
			this.CalculateStarData();
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x00070098 File Offset: 0x0006E298
		public void CalculateStarData()
		{
			if (this.m_starlinesCount == 0)
			{
				return;
			}
			this.m_starLinesArr = new StarLineData[this.m_starlinesCount];
			float num = ((this.m_customIncrement > 0f) ? this.m_customIncrement : (180f / (float)this.m_starlinesCount));
			num *= 0.017453292f;
			for (int i = 0; i < this.m_starlinesCount; i++)
			{
				this.m_starLinesArr[i] = new StarLineData();
				this.m_starLinesArr[i].PassCount = this.m_passCount;
				this.m_starLinesArr[i].SampleLength = this.m_sampleLength;
				if (this.m_longAttenuation > 0f)
				{
					this.m_starLinesArr[i].Attenuation = ((i % 2 == 0) ? this.m_longAttenuation : this.m_attenuation);
				}
				else
				{
					this.m_starLinesArr[i].Attenuation = this.m_attenuation;
				}
				this.m_starLinesArr[i].Inclination = num * (float)i;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06001523 RID: 5411 RVA: 0x00070187 File Offset: 0x0006E387
		// (set) Token: 0x06001524 RID: 5412 RVA: 0x0007018F File Offset: 0x0006E38F
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

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06001525 RID: 5413 RVA: 0x00070198 File Offset: 0x0006E398
		// (set) Token: 0x06001526 RID: 5414 RVA: 0x000701A0 File Offset: 0x0006E3A0
		public string StarName
		{
			get
			{
				return this.m_starName;
			}
			set
			{
				this.m_starName = value;
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06001527 RID: 5415 RVA: 0x000701A9 File Offset: 0x0006E3A9
		// (set) Token: 0x06001528 RID: 5416 RVA: 0x000701B1 File Offset: 0x0006E3B1
		public int StarlinesCount
		{
			get
			{
				return this.m_starlinesCount;
			}
			set
			{
				this.m_starlinesCount = value;
				this.CalculateStarData();
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06001529 RID: 5417 RVA: 0x000701C0 File Offset: 0x0006E3C0
		// (set) Token: 0x0600152A RID: 5418 RVA: 0x000701C8 File Offset: 0x0006E3C8
		public int PassCount
		{
			get
			{
				return this.m_passCount;
			}
			set
			{
				this.m_passCount = value;
				this.CalculateStarData();
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x0600152B RID: 5419 RVA: 0x000701D7 File Offset: 0x0006E3D7
		// (set) Token: 0x0600152C RID: 5420 RVA: 0x000701DF File Offset: 0x0006E3DF
		public float SampleLength
		{
			get
			{
				return this.m_sampleLength;
			}
			set
			{
				this.m_sampleLength = value;
				this.CalculateStarData();
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x0600152D RID: 5421 RVA: 0x000701EE File Offset: 0x0006E3EE
		// (set) Token: 0x0600152E RID: 5422 RVA: 0x000701F6 File Offset: 0x0006E3F6
		public float Attenuation
		{
			get
			{
				return this.m_attenuation;
			}
			set
			{
				this.m_attenuation = value;
				this.CalculateStarData();
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x0600152F RID: 5423 RVA: 0x00070205 File Offset: 0x0006E405
		// (set) Token: 0x06001530 RID: 5424 RVA: 0x0007020D File Offset: 0x0006E40D
		public float Inclination
		{
			get
			{
				return this.m_inclination;
			}
			set
			{
				this.m_inclination = value;
				this.CalculateStarData();
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06001531 RID: 5425 RVA: 0x0007021C File Offset: 0x0006E41C
		// (set) Token: 0x06001532 RID: 5426 RVA: 0x00070224 File Offset: 0x0006E424
		public float CameraRotInfluence
		{
			get
			{
				return this.m_rotation;
			}
			set
			{
				this.m_rotation = value;
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06001533 RID: 5427 RVA: 0x0007022D File Offset: 0x0006E42D
		public StarLineData[] StarLinesArr
		{
			get
			{
				return this.m_starLinesArr;
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06001534 RID: 5428 RVA: 0x00070235 File Offset: 0x0006E435
		// (set) Token: 0x06001535 RID: 5429 RVA: 0x0007023D File Offset: 0x0006E43D
		public float CustomIncrement
		{
			get
			{
				return this.m_customIncrement;
			}
			set
			{
				this.m_customIncrement = value;
				this.CalculateStarData();
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06001536 RID: 5430 RVA: 0x0007024C File Offset: 0x0006E44C
		// (set) Token: 0x06001537 RID: 5431 RVA: 0x00070254 File Offset: 0x0006E454
		public float LongAttenuation
		{
			get
			{
				return this.m_longAttenuation;
			}
			set
			{
				this.m_longAttenuation = value;
				this.CalculateStarData();
			}
		}

		// Token: 0x04001120 RID: 4384
		[SerializeField]
		private StarLibType m_starType;

		// Token: 0x04001121 RID: 4385
		[SerializeField]
		private string m_starName = string.Empty;

		// Token: 0x04001122 RID: 4386
		[SerializeField]
		private int m_starlinesCount = 2;

		// Token: 0x04001123 RID: 4387
		[SerializeField]
		private int m_passCount = 4;

		// Token: 0x04001124 RID: 4388
		[SerializeField]
		private float m_sampleLength = 1f;

		// Token: 0x04001125 RID: 4389
		[SerializeField]
		private float m_attenuation = 0.85f;

		// Token: 0x04001126 RID: 4390
		[SerializeField]
		private float m_inclination;

		// Token: 0x04001127 RID: 4391
		[SerializeField]
		private float m_rotation;

		// Token: 0x04001128 RID: 4392
		[SerializeField]
		private StarLineData[] m_starLinesArr;

		// Token: 0x04001129 RID: 4393
		[SerializeField]
		private float m_customIncrement = 90f;

		// Token: 0x0400112A RID: 4394
		[SerializeField]
		private float m_longAttenuation;
	}
}
