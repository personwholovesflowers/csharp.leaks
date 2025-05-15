using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000083 RID: 131
public class ProfileInterpolator : MonoBehaviour
{
	// Token: 0x0600031B RID: 795 RVA: 0x000153C0 File Offset: 0x000135C0
	protected void SetupResetOnLevelLoad(bool status)
	{
		if (status)
		{
			SceneManager.sceneLoaded += this.SceneLoaded;
			return;
		}
		SceneManager.sceneLoaded -= this.SceneLoaded;
	}

	// Token: 0x0600031C RID: 796 RVA: 0x000153E8 File Offset: 0x000135E8
	private void SceneLoaded(Scene scene, LoadSceneMode mode)
	{
		this.ClearActiveElements();
	}

	// Token: 0x0600031D RID: 797 RVA: 0x000153F0 File Offset: 0x000135F0
	protected void ClearActiveElements()
	{
		this.enteredVolumes.Clear();
		this.activeVolume = null;
		if (this.interpolationRoutine != null)
		{
			base.StopCoroutine(this.interpolationRoutine);
			this.interpolationRoutine = null;
		}
	}

	// Token: 0x0600031E RID: 798 RVA: 0x00015420 File Offset: 0x00013620
	protected VolumeBase GetPriorityVolume()
	{
		int num = -1;
		int num2 = -1;
		for (int i = this.enteredVolumes.Count - 1; i > -1; i--)
		{
			if (this.enteredVolumes[i].Priority > num)
			{
				num = this.enteredVolumes[i].Priority;
				num2 = i;
			}
		}
		if (num2 > -1)
		{
			return this.enteredVolumes[num2];
		}
		return null;
	}

	// Token: 0x0600031F RID: 799 RVA: 0x00015484 File Offset: 0x00013684
	protected bool VolumeIsHigherPriority(VolumeBase volume)
	{
		int priority = volume.Priority;
		for (int i = 0; i < this.enteredVolumes.Count; i++)
		{
			if (this.enteredVolumes[i].Priority >= priority)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000320 RID: 800 RVA: 0x00005444 File Offset: 0x00003644
	public virtual void SetProfileInstant(ProfileBase profile)
	{
	}

	// Token: 0x06000321 RID: 801 RVA: 0x00005444 File Offset: 0x00003644
	public virtual void InterpolateToProfile(ProfileBase profile, float duration, AnimationCurve curve)
	{
	}

	// Token: 0x06000322 RID: 802 RVA: 0x00005444 File Offset: 0x00003644
	public virtual void Interpolate(float lerpValue)
	{
	}

	// Token: 0x06000323 RID: 803 RVA: 0x00005444 File Offset: 0x00003644
	public virtual void FeatherToProfile(ProfileBase profile, VolumeBase volume, AnimationCurve curve)
	{
	}

	// Token: 0x06000324 RID: 804 RVA: 0x00005444 File Offset: 0x00003644
	public virtual void LinearInterpolationComplete()
	{
	}

	// Token: 0x06000325 RID: 805 RVA: 0x000154C8 File Offset: 0x000136C8
	protected void OnEnterVolume(VolumeBase volume)
	{
		if (this.activeVolume == null || this.VolumeIsHigherPriority(volume))
		{
			if (this.currentProfile != null)
			{
				this.previousProfile = this.currentProfile;
			}
			this.currentProfile = volume.GetProfile();
			this.activeVolume = volume;
			VolumeBase.VolumePreservation preservation = volume.Preservation;
			if (preservation > VolumeBase.VolumePreservation.Preserve)
			{
				if (preservation - VolumeBase.VolumePreservation.FeatheredVolume <= 1)
				{
					this.FeatherToProfile(this.currentProfile, volume, volume.LerpCurve);
				}
			}
			else
			{
				this.InterpolateToProfile(this.currentProfile, volume.Duration, volume.LerpCurve);
			}
		}
		if (!this.enteredVolumes.Contains(volume))
		{
			this.enteredVolumes.Add(volume);
		}
	}

	// Token: 0x06000326 RID: 806 RVA: 0x00015574 File Offset: 0x00013774
	protected void OnExitVolume(VolumeBase volume)
	{
		this.enteredVolumes.Remove(volume);
		if (this.activeVolume == volume)
		{
			if (this.activeVolume.Preservation == VolumeBase.VolumePreservation.DiscardWhenLeaving || this.activeVolume.Preservation == VolumeBase.VolumePreservation.Preserve)
			{
				VolumeBase priorityVolume = this.GetPriorityVolume();
				if (priorityVolume != null)
				{
					this.previousProfile = this.currentProfile;
					this.currentProfile = priorityVolume.GetProfile();
					this.activeVolume = priorityVolume;
					if (priorityVolume.Preservation == VolumeBase.VolumePreservation.DiscardWhenLeaving || priorityVolume.Preservation == VolumeBase.VolumePreservation.Preserve)
					{
						this.InterpolateToProfile(priorityVolume.GetProfile(), priorityVolume.Duration, priorityVolume.LerpCurve);
						return;
					}
					this.FeatherToProfile(priorityVolume.GetProfile(), priorityVolume, priorityVolume.LerpCurve);
					return;
				}
				else if (this.activeVolume.Preservation != VolumeBase.VolumePreservation.Preserve && this.defaultProfile != null)
				{
					this.currentProfile = this.defaultProfile;
					this.previousProfile = null;
					this.activeVolume = null;
					this.InterpolateToProfile(this.currentProfile, 1f, AnimationCurve.EaseInOut(0f, 0f, 1f, 1f));
					return;
				}
			}
			else if (this.activeVolume.Preservation == VolumeBase.VolumePreservation.FeatheredVolume || this.activeVolume.Preservation == VolumeBase.VolumePreservation.FeatheredVolumePreserve)
			{
				VolumeBase priorityVolume2 = this.GetPriorityVolume();
				if (priorityVolume2 != null)
				{
					this.previousProfile = this.currentProfile;
					this.currentProfile = priorityVolume2.GetProfile();
					this.activeVolume = priorityVolume2;
					if (priorityVolume2.Preservation == VolumeBase.VolumePreservation.DiscardWhenLeaving || priorityVolume2.Preservation == VolumeBase.VolumePreservation.Preserve)
					{
						this.InterpolateToProfile(priorityVolume2.GetProfile(), priorityVolume2.Duration, priorityVolume2.LerpCurve);
						return;
					}
					this.FeatherToProfile(priorityVolume2.GetProfile(), priorityVolume2, priorityVolume2.LerpCurve);
					return;
				}
				else if (this.defaultProfile != null)
				{
					this.currentProfile = this.defaultProfile;
					this.previousProfile = null;
					this.activeVolume = null;
					this.SetProfileInstant(this.currentProfile);
				}
			}
		}
	}

	// Token: 0x06000327 RID: 807 RVA: 0x00015751 File Offset: 0x00013951
	protected IEnumerator LinearInterpolation(float duration, AnimationCurve curve)
	{
		float durationTimer = 0f;
		if (duration > 0f)
		{
			while (durationTimer <= duration)
			{
				durationTimer += Time.deltaTime;
				durationTimer = Mathf.Clamp(durationTimer, 0f, duration);
				float num = curve.Evaluate(durationTimer / duration);
				this.Interpolate(num);
				yield return null;
			}
		}
		else
		{
			this.Interpolate(1f);
		}
		this.LinearInterpolationComplete();
		yield break;
	}

	// Token: 0x06000328 RID: 808 RVA: 0x0001576E File Offset: 0x0001396E
	protected IEnumerator DistanceBasedInterpolation(VolumeBase volume, AnimationCurve curve)
	{
		while (volume != null)
		{
			Vector3 size = volume.BoxCollider.bounds.size;
			Vector3 vector = volume.transform.TransformPoint(volume.BoxCollider.center);
			Ray ray = new Ray(vector, (base.transform.position - vector).normalized);
			ray.origin = ray.GetPoint(100f);
			ray.direction = -ray.direction;
			Debug.DrawRay(ray.origin, ray.direction.normalized * 100f, Color.red);
			RaycastHit raycastHit;
			if (volume.BoxCollider.Raycast(ray, out raycastHit, 100f))
			{
				Vector3 point = raycastHit.point;
				Vector3 vector2 = volume.FeatheredBounds.ClosestPoint(base.transform.position);
				float num = Vector3.Distance(vector2, point);
				float num2 = Vector3.Distance(vector2, base.transform.position);
				float num3 = Mathf.InverseLerp(num, 0f, num2);
				Debug.DrawRay(vector2, Vector3.up, Color.cyan);
				Debug.DrawRay(point, Vector3.up, Color.yellow);
				this.Interpolate(curve.Evaluate(num3));
				if (volume.Preservation == VolumeBase.VolumePreservation.FeatheredVolumePreserve && num3 >= 1f)
				{
					this.activeVolume = null;
					if (this.interpolationRoutine != null)
					{
						base.StopCoroutine(this.interpolationRoutine);
					}
					this.interpolationRoutine = null;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000325 RID: 805
	[SerializeField]
	protected ProfileBase defaultProfile;

	// Token: 0x04000326 RID: 806
	[SerializeField]
	[HideInInspector]
	protected ProfileBase previousProfile;

	// Token: 0x04000327 RID: 807
	[SerializeField]
	[HideInInspector]
	protected ProfileBase currentProfile;

	// Token: 0x04000328 RID: 808
	[SerializeField]
	[HideInInspector]
	protected List<VolumeBase> enteredVolumes;

	// Token: 0x04000329 RID: 809
	[SerializeField]
	[HideInInspector]
	protected VolumeBase activeVolume;

	// Token: 0x0400032A RID: 810
	protected Coroutine interpolationRoutine;
}
