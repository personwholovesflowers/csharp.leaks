using System;
using UnityEngine;

// Token: 0x02000084 RID: 132
[RequireComponent(typeof(AudioReverbZone))]
public class ReverbController : ProfileInterpolator
{
	// Token: 0x0600032A RID: 810 RVA: 0x0001578C File Offset: 0x0001398C
	private void Awake()
	{
		base.SetupResetOnLevelLoad(true);
		this.reverbZone = base.GetComponent<AudioReverbZone>();
		ReverbVolume.OnEnterReverbVolume = (Action<ReverbVolume>)Delegate.Combine(ReverbVolume.OnEnterReverbVolume, new Action<ReverbVolume>(base.OnEnterVolume));
		ReverbVolume.OnExitReverbVolume = (Action<ReverbVolume>)Delegate.Combine(ReverbVolume.OnExitReverbVolume, new Action<ReverbVolume>(base.OnExitVolume));
		if (this.defaultProfile != null)
		{
			this.SetProfileInstant(this.defaultProfile);
		}
	}

	// Token: 0x0600032B RID: 811 RVA: 0x00015808 File Offset: 0x00013A08
	private void OnDestroy()
	{
		base.SetupResetOnLevelLoad(false);
		ReverbVolume.OnEnterReverbVolume = (Action<ReverbVolume>)Delegate.Remove(ReverbVolume.OnEnterReverbVolume, new Action<ReverbVolume>(base.OnEnterVolume));
		ReverbVolume.OnExitReverbVolume = (Action<ReverbVolume>)Delegate.Remove(ReverbVolume.OnExitReverbVolume, new Action<ReverbVolume>(base.OnExitVolume));
	}

	// Token: 0x0600032C RID: 812 RVA: 0x0001585C File Offset: 0x00013A5C
	public override void InterpolateToProfile(ProfileBase profile, float duration, AnimationCurve curve)
	{
		ReverbProfile reverbProfile = profile as ReverbProfile;
		if (this.interpolationRoutine != null)
		{
			base.StopCoroutine(this.interpolationRoutine);
			this.interpolationRoutine = null;
		}
		this.startData.Room = this.reverbZone.room;
		this.startData.RoomHF = this.reverbZone.roomHF;
		this.startData.RoomLF = this.reverbZone.roomLF;
		this.startData.DecayTime = this.reverbZone.decayTime;
		this.startData.DecayHFRatio = this.reverbZone.decayHFRatio;
		this.startData.Reflections = (float)this.reverbZone.reflections;
		this.startData.ReflectionsDelay = this.reverbZone.reflectionsDelay;
		this.startData.Reverb = (float)this.reverbZone.reverb;
		this.startData.ReverbDelay = this.reverbZone.reverbDelay;
		this.startData.HFReference = this.reverbZone.HFReference;
		this.startData.LFReference = this.reverbZone.LFReference;
		this.startData.Diffusion = this.reverbZone.diffusion;
		this.startData.Density = this.reverbZone.density;
		this.endData = reverbProfile.Data;
		this.interpolationRoutine = base.StartCoroutine(base.LinearInterpolation(duration, curve));
	}

	// Token: 0x0600032D RID: 813 RVA: 0x000159CC File Offset: 0x00013BCC
	public override void SetProfileInstant(ProfileBase profile)
	{
		ReverbProfile reverbProfile = profile as ReverbProfile;
		if (this.currentProfile != null)
		{
			this.previousProfile = this.currentProfile;
		}
		this.currentProfile = reverbProfile;
		this.endData = reverbProfile.Data;
		this.Interpolate(1f);
	}

	// Token: 0x0600032E RID: 814 RVA: 0x00015A18 File Offset: 0x00013C18
	public override void Interpolate(float lerpValue)
	{
		this.reverbZone.room = Convert.ToInt32(Mathf.Lerp((float)this.startData.Room, (float)this.endData.Room, lerpValue));
		this.reverbZone.roomHF = Convert.ToInt32(Mathf.Lerp((float)this.startData.RoomHF, (float)this.endData.RoomHF, lerpValue));
		this.reverbZone.roomLF = Convert.ToInt32(Mathf.Lerp((float)this.startData.RoomLF, (float)this.endData.RoomLF, lerpValue));
		this.reverbZone.decayTime = Mathf.Lerp(this.startData.DecayTime, this.endData.DecayTime, lerpValue);
		this.reverbZone.decayHFRatio = Mathf.Lerp(this.startData.DecayHFRatio, this.endData.DecayHFRatio, lerpValue);
		this.reverbZone.reflections = Convert.ToInt32(Mathf.Lerp(this.startData.Reflections, this.endData.Reflections, lerpValue));
		this.reverbZone.reflectionsDelay = Mathf.Lerp(this.startData.ReflectionsDelay, this.endData.ReflectionsDelay, lerpValue);
		this.reverbZone.reverb = Convert.ToInt32(Mathf.Lerp(this.startData.Reverb, this.endData.Reverb, lerpValue));
		this.reverbZone.reverbDelay = Mathf.Lerp(this.startData.ReverbDelay, this.endData.ReverbDelay, lerpValue);
		this.reverbZone.HFReference = Mathf.Lerp(this.startData.HFReference, this.endData.HFReference, lerpValue);
		this.reverbZone.LFReference = Mathf.Lerp(this.startData.LFReference, this.endData.LFReference, lerpValue);
		this.reverbZone.diffusion = Mathf.Lerp(this.startData.Diffusion, this.endData.Diffusion, lerpValue);
		this.reverbZone.density = Mathf.Lerp(this.startData.Density, this.endData.Density, lerpValue);
	}

	// Token: 0x0600032F RID: 815 RVA: 0x00015C40 File Offset: 0x00013E40
	public override void FeatherToProfile(ProfileBase profile, VolumeBase volume, AnimationCurve curve)
	{
		ReverbProfile reverbProfile = profile as ReverbProfile;
		this.startData.Room = this.reverbZone.room;
		this.startData.RoomHF = this.reverbZone.roomHF;
		this.startData.RoomLF = this.reverbZone.roomLF;
		this.startData.DecayTime = this.reverbZone.decayTime;
		this.startData.DecayHFRatio = this.reverbZone.decayHFRatio;
		this.startData.Reflections = (float)this.reverbZone.reflections;
		this.startData.ReflectionsDelay = this.reverbZone.reflectionsDelay;
		this.startData.Reverb = (float)this.reverbZone.reverb;
		this.startData.ReverbDelay = this.reverbZone.reverbDelay;
		this.startData.HFReference = this.reverbZone.HFReference;
		this.startData.LFReference = this.reverbZone.LFReference;
		this.startData.Diffusion = this.reverbZone.diffusion;
		this.startData.Density = this.reverbZone.density;
		this.endData = reverbProfile.Data;
		this.interpolationRoutine = base.StartCoroutine(base.DistanceBasedInterpolation(volume, curve));
	}

	// Token: 0x0400032B RID: 811
	private AudioReverbZone reverbZone;

	// Token: 0x0400032C RID: 812
	private ReverbProfile.ReverbData startData;

	// Token: 0x0400032D RID: 813
	private ReverbProfile.ReverbData endData;
}
