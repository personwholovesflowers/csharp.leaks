using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace StanleyUI.Setters
{
	// Token: 0x0200020F RID: 527
	[RequireComponent(typeof(StanleyMenuSlider))]
	public class VolumeSlider : GenericSlider
	{
		// Token: 0x06000C1A RID: 3098 RVA: 0x0003627F File Offset: 0x0003447F
		public override void SetValue(float val100)
		{
			this.SetValueSilent(val100);
			base.StartCoroutine(this.PlaySound());
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x00036295 File Offset: 0x00034495
		public void SetValueSilent(float val100)
		{
			base.SetValue(val100);
			this.mixer.SetFloat(this.mixerVariableName, this.VolumeFunction(Mathf.Lerp(0.001f, 1f, val100 / 100f), -80f));
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x000362D1 File Offset: 0x000344D1
		private IEnumerator Start()
		{
			this.isPlaying = true;
			yield return new WaitForSecondsRealtime(1f);
			this.isPlaying = false;
			yield break;
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x000362E0 File Offset: 0x000344E0
		private void OnEnable()
		{
			base.StartCoroutine(this.Start());
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x000362EF File Offset: 0x000344EF
		private void OnDisable()
		{
			this.isPlaying = false;
			base.StopAllCoroutines();
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x000362FE File Offset: 0x000344FE
		public void ForceStop()
		{
			this.timer = this.playtime;
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x0003630C File Offset: 0x0003450C
		private IEnumerator PlaySound()
		{
			if (!base.isActiveAndEnabled || this.isPlaying)
			{
				this.timer = 0f;
				yield break;
			}
			this.isPlaying = true;
			if (this.playWhileValueChange != null)
			{
				this.playWhileValueChange.Play();
				float playTimeToWait = this.playtime;
				if (this.autoStopBehaviour == VolumeSlider.AutoStopBehaviour.AfterClipPlayed)
				{
					this.playtime = this.playWhileValueChange.clip.length - this.playWhileValueChange.time;
				}
				this.timer = 0f;
				while (this.timer < playTimeToWait)
				{
					this.timer += Time.unscaledDeltaTime;
					yield return null;
				}
				this.playWhileValueChange.Stop();
				this.isPlaying = false;
				yield break;
			}
			yield break;
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x0003631B File Offset: 0x0003451B
		private float VolumeFunctionFake(float x)
		{
			return Mathf.Log(x + 1f) / Mathf.Log(2f);
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x00036334 File Offset: 0x00034534
		private float VolumeFunction(float x, float lowDb = -80f)
		{
			return -lowDb * Mathf.Log10(x) / 2f;
		}

		// Token: 0x04000B76 RID: 2934
		public AudioMixer mixer;

		// Token: 0x04000B77 RID: 2935
		public string mixerVariableName;

		// Token: 0x04000B78 RID: 2936
		public AudioSource playWhileValueChange;

		// Token: 0x04000B79 RID: 2937
		public float playtime = 100000f;

		// Token: 0x04000B7A RID: 2938
		public VolumeSlider.AutoStopBehaviour autoStopBehaviour;

		// Token: 0x04000B7B RID: 2939
		private bool isPlaying;

		// Token: 0x04000B7C RID: 2940
		private float timer = 100000f;

		// Token: 0x02000420 RID: 1056
		public enum AutoStopBehaviour
		{
			// Token: 0x04001568 RID: 5480
			AfterSetPlayTime,
			// Token: 0x04001569 RID: 5481
			AfterClipPlayed
		}
	}
}
