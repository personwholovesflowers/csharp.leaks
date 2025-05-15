using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000B5 RID: 181
public class EnvShake : HammerEntity
{
	// Token: 0x0600042E RID: 1070 RVA: 0x00019761 File Offset: 0x00017961
	private IEnumerator Shake()
	{
		fint32 startTime = Singleton<GameMaster>.Instance.GameTime;
		float endTime = startTime + this.duration;
		Transform cam = StanleyController.Instance.camTransform;
		while (Singleton<GameMaster>.Instance.GameTime < endTime)
		{
			float num = Mathf.InverseLerp(startTime, endTime, Singleton<GameMaster>.Instance.GameTime);
			float num2 = this.amplitude / 4f * MotionController.Instance.ScreenShakeAmplitudeMultiplier;
			if (Singleton<GameMaster>.Instance.IsRumbleEnabled && !GameMaster.PAUSEMENUACTIVE)
			{
				PlatformGamepad.PlayVibration(num2 * 0.25f);
			}
			else
			{
				PlatformGamepad.StopVibration();
			}
			num = Mathf.Clamp01(1f / Mathf.Pow(num, 0.5f) - 1f);
			if (!this.globalShake)
			{
				float num3 = Vector3.Distance(base.transform.position, StanleyController.Instance.stanleyTransform.position);
				num *= 1f - Mathf.InverseLerp(0f, this.radius, num3);
			}
			Quaternion quaternion = Quaternion.Euler(Random.Range(-num2, num2), Random.Range(-num2, num2), Random.Range(-num2, num2));
			cam.localRotation = Quaternion.Slerp(Quaternion.identity, quaternion, num);
			yield return new WaitForEndOfFrame();
		}
		PlatformGamepad.StopVibration();
		yield break;
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x00019770 File Offset: 0x00017970
	public void Input_StartShake()
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.Shake());
	}

	// Token: 0x06000430 RID: 1072 RVA: 0x00019785 File Offset: 0x00017985
	public void Input_StopShake()
	{
		base.StopAllCoroutines();
		StanleyController.Instance.camTransform.localRotation = Quaternion.identity;
		PlatformGamepad.StopVibration();
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x000197A6 File Offset: 0x000179A6
	public void Input_Amplitude(float amp)
	{
		this.amplitude = amp;
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x000197AF File Offset: 0x000179AF
	public void Input_Frequency(float f)
	{
		this.frequency = f;
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x000197B8 File Offset: 0x000179B8
	private void OnDestroy()
	{
		PlatformGamepad.StopVibration();
	}

	// Token: 0x04000425 RID: 1061
	public float amplitude = 4f;

	// Token: 0x04000426 RID: 1062
	public float duration = 1f;

	// Token: 0x04000427 RID: 1063
	public float frequency = 2.5f;

	// Token: 0x04000428 RID: 1064
	public float radius = 5f;

	// Token: 0x04000429 RID: 1065
	public bool globalShake;
}
