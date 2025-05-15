using System;
using UnityEngine;

namespace Nest.Components
{
	// Token: 0x02000246 RID: 582
	public class AudioInput : NestInput
	{
		// Token: 0x06000DBC RID: 3516 RVA: 0x0003DC1C File Offset: 0x0003BE1C
		public override void Start()
		{
			base.Start();
			this._audioSource = base.GetComponent<AudioSource>();
			this.wLeftChannel = new float[1024];
			this.wRightChannel = new float[1024];
			this.Waveform = new float[1024];
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x0003DC6C File Offset: 0x0003BE6C
		private void Update()
		{
			if (this._audioSource.isPlaying)
			{
				this.AnalyzeSound();
				this.Value.TargetValue = this.Remap(this._decibels, (float)(-(float)this.DecibelLimit), 0f, 0f, 1f);
				base.Invoke();
			}
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x0003DCC0 File Offset: 0x0003BEC0
		private void AnalyzeSound()
		{
			this._audioSource.GetOutputData(this.wLeftChannel, 0);
			this._audioSource.GetOutputData(this.wRightChannel, 1);
			this.Waveform = this.CombineChannels(this.wLeftChannel, this.wRightChannel);
			float num = 0f;
			for (int i = 0; i < this.Waveform.Length; i++)
			{
				num += this.Waveform[i] * this.Waveform[i];
			}
			this._rmsValue = Mathf.Sqrt(num / 1024f);
			this._decibels = 20f * Mathf.Log10(this._rmsValue / this.DecibelReference);
			if (this._decibels < (float)(-(float)this.DecibelLimit))
			{
				this._decibels = (float)(-(float)this.DecibelLimit);
			}
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x0003DD84 File Offset: 0x0003BF84
		private float[] CombineChannels(float[] Left, float[] Right)
		{
			float[] array = new float[Left.Length];
			for (int i = 0; i < Left.Length; i++)
			{
				array[i] = (Left[i] + Right[i]) / 2f / 32768f;
			}
			return array;
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x0003DDBF File Offset: 0x0003BFBF
		public float Remap(float Value, float From1, float To1, float From2, float To2)
		{
			return (Value - From1) / (To1 - From1) * (To2 - From2) + From2;
		}

		// Token: 0x04000C4D RID: 3149
		public float DecibelReference = 0.1f;

		// Token: 0x04000C4E RID: 3150
		public int DecibelLimit = 160;

		// Token: 0x04000C4F RID: 3151
		private const int SampleCount = 1024;

		// Token: 0x04000C50 RID: 3152
		private AudioSource _audioSource;

		// Token: 0x04000C51 RID: 3153
		private float _rmsValue;

		// Token: 0x04000C52 RID: 3154
		private float _decibels;

		// Token: 0x04000C53 RID: 3155
		private float[] wLeftChannel;

		// Token: 0x04000C54 RID: 3156
		private float[] wRightChannel;

		// Token: 0x04000C55 RID: 3157
		private float[] Waveform;
	}
}
