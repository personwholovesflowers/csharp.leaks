using System;
using ScriptableObjects;
using UnityEngine;

// Token: 0x02000343 RID: 835
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class PlayerAnimations : MonoSingleton<PlayerAnimations>
{
	// Token: 0x06001338 RID: 4920 RVA: 0x0009AF39 File Offset: 0x00099139
	private void Start()
	{
		this.aud = base.GetComponent<AudioSource>();
	}

	// Token: 0x06001339 RID: 4921 RVA: 0x0009AF48 File Offset: 0x00099148
	private void Update()
	{
		if (MonoSingleton<NewMovement>.Instance.walking && (!MonoSingleton<NewMovement>.Instance.groundProperties || MonoSingleton<NewMovement>.Instance.groundProperties.friction > 0f))
		{
			this.footstepTimer = Mathf.MoveTowards(this.footstepTimer, 0f, Mathf.Min(MonoSingleton<NewMovement>.Instance.rb.velocity.magnitude, 15f) / 15f * Time.deltaTime * 3f);
		}
		if (this.footstepTimer <= 0f)
		{
			this.Footstep(0.25f, false, 0f);
		}
	}

	// Token: 0x0600133A RID: 4922 RVA: 0x0009AFF0 File Offset: 0x000991F0
	public void Footstep(float volume = 0.25f, bool force = false, float delay = 0f)
	{
		if (this.onGround || force)
		{
			this.footstepTimer = 1f;
			if (this.aud == null)
			{
				this.aud = base.GetComponent<AudioSource>();
			}
			if (!this.aud)
			{
				return;
			}
			this.aud.volume = volume;
			if (MonoSingleton<NewMovement>.Instance && MonoSingleton<NewMovement>.Instance.touchingWaters.Count > 0 && !MonoSingleton<UnderwaterController>.Instance.inWater)
			{
				AudioClip[] array;
				if (this.footstepSet != null && this.footstepSet.TryGetFootstepClips(SurfaceType.Wet, out array))
				{
					this.PlayRandomFootstepClip(array, delay);
					return;
				}
			}
			else if (!MonoSingleton<NewMovement>.Instance || !MonoSingleton<NewMovement>.Instance.groundProperties || (!MonoSingleton<NewMovement>.Instance.groundProperties.overrideFootsteps && !MonoSingleton<NewMovement>.Instance.groundProperties.overrideSurfaceType))
			{
				SceneHelper.HitSurfaceData hitSurfaceData;
				AudioClip[] array2;
				if (this.footstepSet != null && MonoSingleton<SceneHelper>.Instance.TryGetSurfaceData(base.transform.position, out hitSurfaceData) && this.footstepSet.TryGetFootstepClips(hitSurfaceData.surfaceType, out array2))
				{
					this.PlayRandomFootstepClip(array2, delay);
					return;
				}
				if (this.footsteps.Length != 0)
				{
					this.PlayRandomFootstepClip(this.footsteps, delay);
					return;
				}
			}
			else
			{
				CustomGroundProperties groundProperties = MonoSingleton<NewMovement>.Instance.groundProperties;
				AudioClip[] array3;
				if (this.footstepSet != null && groundProperties.overrideSurfaceType && this.footstepSet.TryGetFootstepClips(groundProperties.surfaceType, out array3))
				{
					this.PlayRandomFootstepClip(array3, delay);
					return;
				}
				if (groundProperties.newFootstepSound != null)
				{
					this.PlayFootstepClip(groundProperties.newFootstepSound, delay);
				}
			}
		}
	}

	// Token: 0x0600133B RID: 4923 RVA: 0x0009B198 File Offset: 0x00099398
	public void WallJump(Vector3 position)
	{
		if (this.aud == null)
		{
			this.aud = base.GetComponent<AudioSource>();
		}
		if (!this.aud)
		{
			return;
		}
		this.aud.volume = 0.5f;
		SceneHelper.HitSurfaceData hitSurfaceData;
		AudioClip[] array;
		if (this.footstepSet != null && MonoSingleton<SceneHelper>.Instance.TryGetSurfaceData(base.transform.position, position - base.transform.position, Vector3.Distance(base.transform.position, position) + 1f, out hitSurfaceData) && this.footstepSet.TryGetFootstepClips(hitSurfaceData.surfaceType, out array))
		{
			this.PlayRandomFootstepClip(array, 0f);
			return;
		}
		if (this.footsteps.Length != 0)
		{
			this.PlayRandomFootstepClip(this.footsteps, 0f);
		}
	}

	// Token: 0x0600133C RID: 4924 RVA: 0x0009B268 File Offset: 0x00099468
	public void WallJump(CustomGroundProperties cgp)
	{
		if (this.aud == null)
		{
			this.aud = base.GetComponent<AudioSource>();
		}
		if (!this.aud)
		{
			return;
		}
		this.aud.volume = 0.5f;
		if (cgp.overrideFootsteps)
		{
			this.PlayFootstepClip(cgp.newFootstepSound, 0f);
			return;
		}
		AudioClip[] array;
		this.footstepSet.TryGetFootstepClips(cgp.surfaceType, out array);
		this.PlayRandomFootstepClip(array, 0f);
	}

	// Token: 0x0600133D RID: 4925 RVA: 0x0009B2E8 File Offset: 0x000994E8
	private void PlayRandomFootstepClip(AudioClip[] clips, float delay = 0f)
	{
		if (clips == null || clips.Length == 0)
		{
			return;
		}
		int num = Random.Range(0, clips.Length);
		if (clips.Length > 1 && num == this.lastFootstep)
		{
			num = (num + 1) % clips.Length;
		}
		this.lastFootstep = num;
		this.PlayFootstepClip(clips[num], delay);
	}

	// Token: 0x0600133E RID: 4926 RVA: 0x0009B330 File Offset: 0x00099530
	private void PlayFootstepClip(AudioClip clip, float delay = 0f)
	{
		if (clip == null)
		{
			return;
		}
		this.aud.clip = clip;
		this.aud.pitch = Random.Range(0.9f, 1.1f);
		if (delay == 0f)
		{
			this.aud.Play();
			return;
		}
		this.aud.PlayDelayed(delay);
	}

	// Token: 0x04001A92 RID: 6802
	[SerializeField]
	private FootstepSet footstepSet;

	// Token: 0x04001A93 RID: 6803
	public bool onGround;

	// Token: 0x04001A94 RID: 6804
	public AudioClip[] footsteps;

	// Token: 0x04001A95 RID: 6805
	private AudioSource aud;

	// Token: 0x04001A96 RID: 6806
	private float footstepTimer = 1f;

	// Token: 0x04001A97 RID: 6807
	private int lastFootstep = -1;
}
