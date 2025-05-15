using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000195 RID: 405
public class SnapshotManager : MonoBehaviour
{
	// Token: 0x06000945 RID: 2373 RVA: 0x0002B925 File Offset: 0x00029B25
	private void Start()
	{
		if (this.FirstSnapshot != null)
		{
			this.TransitionToSnapshot(this.FirstSnapshot, 0f);
		}
	}

	// Token: 0x06000946 RID: 2374 RVA: 0x0002B946 File Offset: 0x00029B46
	public void TransitionToSnapshot(AudioMixerSnapshot snapshot)
	{
		this.Mixer.FindSnapshot(snapshot.name).TransitionTo(this.transitionTime);
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x0002B964 File Offset: 0x00029B64
	public void TransitionToSnapshot(AudioMixerSnapshot snapshot, float customTransitionTime)
	{
		this.Mixer.FindSnapshot(snapshot.name).TransitionTo(customTransitionTime);
	}

	// Token: 0x06000948 RID: 2376 RVA: 0x0002B980 File Offset: 0x00029B80
	public void RaiseIntensityLevel()
	{
		this.intensityIndex++;
		if (this.intensityIndex >= this.IntensitySnapshots.Length)
		{
			this.intensityIndex = this.IntensitySnapshots.Length - 1;
			return;
		}
		this.TransitionToSnapshot(this.IntensitySnapshots[this.intensityIndex]);
	}

	// Token: 0x06000949 RID: 2377 RVA: 0x0002B9CF File Offset: 0x00029BCF
	public void ChangeTransitionTime(float newTime)
	{
		this.transitionTime = newTime;
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x0002B9D8 File Offset: 0x00029BD8
	public void LowerIntensityLevel()
	{
		this.intensityIndex--;
		if (this.intensityIndex < 0)
		{
			this.intensityIndex = 0;
			return;
		}
		this.TransitionToSnapshot(this.IntensitySnapshots[this.intensityIndex]);
	}

	// Token: 0x0400091D RID: 2333
	public AudioMixer Mixer;

	// Token: 0x0400091E RID: 2334
	public AudioMixerSnapshot FirstSnapshot;

	// Token: 0x0400091F RID: 2335
	public float transitionTime = 0.3f;

	// Token: 0x04000920 RID: 2336
	public AudioMixerSnapshot[] IntensitySnapshots;

	// Token: 0x04000921 RID: 2337
	private int intensityIndex;
}
