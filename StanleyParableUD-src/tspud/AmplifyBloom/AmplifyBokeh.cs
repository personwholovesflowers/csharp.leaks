using System;
using System.Collections.Generic;
using UnityEngine;

namespace AmplifyBloom
{
	// Token: 0x0200032D RID: 813
	[Serializable]
	public sealed class AmplifyBokeh : IAmplifyItem, ISerializationCallbackReceiver
	{
		// Token: 0x060014A7 RID: 5287 RVA: 0x0006E464 File Offset: 0x0006C664
		public AmplifyBokeh()
		{
			this.m_bokehOffsets = new List<AmplifyBokehData>();
			this.CreateBokehOffsets(ApertureShape.Hexagon);
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x0006E4BC File Offset: 0x0006C6BC
		public void Destroy()
		{
			for (int i = 0; i < this.m_bokehOffsets.Count; i++)
			{
				this.m_bokehOffsets[i].Destroy();
			}
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x0006E4F0 File Offset: 0x0006C6F0
		private void CreateBokehOffsets(ApertureShape shape)
		{
			this.m_bokehOffsets.Clear();
			switch (shape)
			{
			case ApertureShape.Square:
				this.m_bokehOffsets.Add(new AmplifyBokehData(this.CalculateBokehSamples(8, this.m_offsetRotation)));
				this.m_bokehOffsets.Add(new AmplifyBokehData(this.CalculateBokehSamples(8, this.m_offsetRotation + 90f)));
				return;
			case ApertureShape.Hexagon:
				this.m_bokehOffsets.Add(new AmplifyBokehData(this.CalculateBokehSamples(8, this.m_offsetRotation)));
				this.m_bokehOffsets.Add(new AmplifyBokehData(this.CalculateBokehSamples(8, this.m_offsetRotation - 75f)));
				this.m_bokehOffsets.Add(new AmplifyBokehData(this.CalculateBokehSamples(8, this.m_offsetRotation + 75f)));
				return;
			case ApertureShape.Octagon:
				this.m_bokehOffsets.Add(new AmplifyBokehData(this.CalculateBokehSamples(8, this.m_offsetRotation)));
				this.m_bokehOffsets.Add(new AmplifyBokehData(this.CalculateBokehSamples(8, this.m_offsetRotation + 65f)));
				this.m_bokehOffsets.Add(new AmplifyBokehData(this.CalculateBokehSamples(8, this.m_offsetRotation + 90f)));
				this.m_bokehOffsets.Add(new AmplifyBokehData(this.CalculateBokehSamples(8, this.m_offsetRotation + 115f)));
				return;
			default:
				return;
			}
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x0006E648 File Offset: 0x0006C848
		private Vector4[] CalculateBokehSamples(int sampleCount, float angle)
		{
			Vector4[] array = new Vector4[sampleCount];
			float num = 0.017453292f * angle;
			float num2 = (float)Screen.width / (float)Screen.height;
			Vector4 vector = new Vector4(this.m_bokehSampleRadius * Mathf.Cos(num), this.m_bokehSampleRadius * Mathf.Sin(num));
			vector.x /= num2;
			for (int i = 0; i < sampleCount; i++)
			{
				float num3 = (float)i / ((float)sampleCount - 1f);
				array[i] = Vector4.Lerp(-vector, vector, num3);
			}
			return array;
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x0006E6D4 File Offset: 0x0006C8D4
		public void ApplyBokehFilter(RenderTexture source, Material material)
		{
			for (int i = 0; i < this.m_bokehOffsets.Count; i++)
			{
				this.m_bokehOffsets[i].BokehRenderTexture = AmplifyUtils.GetTempRenderTarget(source.width, source.height);
			}
			material.SetVector(AmplifyUtils.BokehParamsId, this.m_bokehCameraProperties);
			for (int j = 0; j < this.m_bokehOffsets.Count; j++)
			{
				for (int k = 0; k < 8; k++)
				{
					material.SetVector(AmplifyUtils.AnamorphicGlareWeightsStr[k], this.m_bokehOffsets[j].Offsets[k]);
				}
				Graphics.Blit(source, this.m_bokehOffsets[j].BokehRenderTexture, material, 27);
			}
			for (int l = 0; l < this.m_bokehOffsets.Count - 1; l++)
			{
				material.SetTexture(AmplifyUtils.AnamorphicRTS[l], this.m_bokehOffsets[l].BokehRenderTexture);
			}
			source.DiscardContents();
			Graphics.Blit(this.m_bokehOffsets[this.m_bokehOffsets.Count - 1].BokehRenderTexture, source, material, 28 + (this.m_bokehOffsets.Count - 2));
			for (int m = 0; m < this.m_bokehOffsets.Count; m++)
			{
				AmplifyUtils.ReleaseTempRenderTarget(this.m_bokehOffsets[m].BokehRenderTexture);
				this.m_bokehOffsets[m].BokehRenderTexture = null;
			}
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x0006E83E File Offset: 0x0006CA3E
		public void OnAfterDeserialize()
		{
			this.CreateBokehOffsets(this.m_apertureShape);
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x00005444 File Offset: 0x00003644
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x060014AE RID: 5294 RVA: 0x0006E84C File Offset: 0x0006CA4C
		// (set) Token: 0x060014AF RID: 5295 RVA: 0x0006E854 File Offset: 0x0006CA54
		public ApertureShape ApertureShape
		{
			get
			{
				return this.m_apertureShape;
			}
			set
			{
				if (this.m_apertureShape != value)
				{
					this.m_apertureShape = value;
					this.CreateBokehOffsets(value);
				}
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060014B0 RID: 5296 RVA: 0x0006E86D File Offset: 0x0006CA6D
		// (set) Token: 0x060014B1 RID: 5297 RVA: 0x0006E875 File Offset: 0x0006CA75
		public bool ApplyBokeh
		{
			get
			{
				return this.m_isActive;
			}
			set
			{
				this.m_isActive = value;
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060014B2 RID: 5298 RVA: 0x0006E87E File Offset: 0x0006CA7E
		// (set) Token: 0x060014B3 RID: 5299 RVA: 0x0006E886 File Offset: 0x0006CA86
		public bool ApplyOnBloomSource
		{
			get
			{
				return this.m_applyOnBloomSource;
			}
			set
			{
				this.m_applyOnBloomSource = value;
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x060014B4 RID: 5300 RVA: 0x0006E88F File Offset: 0x0006CA8F
		// (set) Token: 0x060014B5 RID: 5301 RVA: 0x0006E897 File Offset: 0x0006CA97
		public float BokehSampleRadius
		{
			get
			{
				return this.m_bokehSampleRadius;
			}
			set
			{
				this.m_bokehSampleRadius = value;
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x060014B6 RID: 5302 RVA: 0x0006E8A0 File Offset: 0x0006CAA0
		// (set) Token: 0x060014B7 RID: 5303 RVA: 0x0006E8A8 File Offset: 0x0006CAA8
		public float OffsetRotation
		{
			get
			{
				return this.m_offsetRotation;
			}
			set
			{
				this.m_offsetRotation = value;
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x060014B8 RID: 5304 RVA: 0x0006E8B1 File Offset: 0x0006CAB1
		// (set) Token: 0x060014B9 RID: 5305 RVA: 0x0006E8B9 File Offset: 0x0006CAB9
		public Vector4 BokehCameraProperties
		{
			get
			{
				return this.m_bokehCameraProperties;
			}
			set
			{
				this.m_bokehCameraProperties = value;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x060014BA RID: 5306 RVA: 0x0006E8C2 File Offset: 0x0006CAC2
		// (set) Token: 0x060014BB RID: 5307 RVA: 0x0006E8CF File Offset: 0x0006CACF
		public float Aperture
		{
			get
			{
				return this.m_bokehCameraProperties.x;
			}
			set
			{
				this.m_bokehCameraProperties.x = value;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060014BC RID: 5308 RVA: 0x0006E8DD File Offset: 0x0006CADD
		// (set) Token: 0x060014BD RID: 5309 RVA: 0x0006E8EA File Offset: 0x0006CAEA
		public float FocalLength
		{
			get
			{
				return this.m_bokehCameraProperties.y;
			}
			set
			{
				this.m_bokehCameraProperties.y = value;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060014BE RID: 5310 RVA: 0x0006E8F8 File Offset: 0x0006CAF8
		// (set) Token: 0x060014BF RID: 5311 RVA: 0x0006E905 File Offset: 0x0006CB05
		public float FocalDistance
		{
			get
			{
				return this.m_bokehCameraProperties.z;
			}
			set
			{
				this.m_bokehCameraProperties.z = value;
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060014C0 RID: 5312 RVA: 0x0006E913 File Offset: 0x0006CB13
		// (set) Token: 0x060014C1 RID: 5313 RVA: 0x0006E920 File Offset: 0x0006CB20
		public float MaxCoCDiameter
		{
			get
			{
				return this.m_bokehCameraProperties.w;
			}
			set
			{
				this.m_bokehCameraProperties.w = value;
			}
		}

		// Token: 0x040010CA RID: 4298
		private const int PerPassSampleCount = 8;

		// Token: 0x040010CB RID: 4299
		[SerializeField]
		private bool m_isActive;

		// Token: 0x040010CC RID: 4300
		[SerializeField]
		private bool m_applyOnBloomSource;

		// Token: 0x040010CD RID: 4301
		[SerializeField]
		private float m_bokehSampleRadius = 0.5f;

		// Token: 0x040010CE RID: 4302
		[SerializeField]
		private Vector4 m_bokehCameraProperties = new Vector4(0.05f, 0.018f, 1.34f, 0.18f);

		// Token: 0x040010CF RID: 4303
		[SerializeField]
		private float m_offsetRotation;

		// Token: 0x040010D0 RID: 4304
		[SerializeField]
		private ApertureShape m_apertureShape = ApertureShape.Hexagon;

		// Token: 0x040010D1 RID: 4305
		private List<AmplifyBokehData> m_bokehOffsets;
	}
}
