using System;
using UnityEngine;

// Token: 0x020001A1 RID: 417
public class EnviroParticle : MonoBehaviour
{
	// Token: 0x0600086B RID: 2155 RVA: 0x0003A2F8 File Offset: 0x000384F8
	private void Start()
	{
		this.part = base.GetComponent<ParticleSystem>();
		if (MonoSingleton<PrefsManager>.Instance.GetBoolLocal("disableEnvironmentParticles", false) && this.part.isPlaying && !this.stopped)
		{
			this.stopped = true;
			this.part.Stop();
			this.part.Clear();
		}
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x0003A355 File Offset: 0x00038555
	private void OnEnable()
	{
		this.CheckEnviroParticles();
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x0003A37D File Offset: 0x0003857D
	private void OnDisable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x0003A39F File Offset: 0x0003859F
	private void OnPrefChanged(string key, object value)
	{
		if (key == "disableEnvironmentParticles")
		{
			this.CheckEnviroParticles();
		}
	}

	// Token: 0x0600086F RID: 2159 RVA: 0x0003A3B4 File Offset: 0x000385B4
	public void CheckEnviroParticles()
	{
		if (this.part == null)
		{
			this.part = base.GetComponent<ParticleSystem>();
		}
		if (MonoSingleton<PrefsManager>.Instance.GetBoolLocal("disableEnvironmentParticles", false) && this.part.isPlaying && !this.stopped)
		{
			this.stopped = true;
			this.part.Stop();
			this.part.Clear();
			return;
		}
		if (this.stopped)
		{
			this.stopped = false;
			this.part.Play();
		}
	}

	// Token: 0x04000B38 RID: 2872
	private ParticleSystem part;

	// Token: 0x04000B39 RID: 2873
	[HideInInspector]
	public bool stopped;
}
