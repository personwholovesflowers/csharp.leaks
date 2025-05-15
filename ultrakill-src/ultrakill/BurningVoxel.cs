using System;
using System.Collections;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x02000220 RID: 544
public class BurningVoxel : MonoBehaviour
{
	// Token: 0x06000BB1 RID: 2993 RVA: 0x00052364 File Offset: 0x00050564
	public void Initialize(VoxelProxy proxy)
	{
		this.proxy = proxy;
		this.hurtCooldownCollection = MonoSingleton<StainVoxelManager>.Instance.SharedHurtCooldownCollection;
		GameObject fire = MonoSingleton<FireObjectPool>.Instance.GetFire(true);
		fire.transform.SetParent(base.transform, false);
		fire.transform.localPosition = new Vector3(0f, 2.0625f, 0f);
		fire.transform.localRotation = Quaternion.identity;
		this.fireParticles = fire.transform;
		this.timeSinceInitialized = 0f;
		base.StartCoroutine(this.BurningCoroutine());
	}

	// Token: 0x06000BB2 RID: 2994 RVA: 0x00052400 File Offset: 0x00050600
	private void OnEnable()
	{
		if (this.proxy == null)
		{
			return;
		}
		if (this.timeSinceStartedExtinguishing == null && this.timeSinceInitialized > 6f)
		{
			this.timeSinceStartedExtinguishing = new TimeSince?(this.timeSinceInitialized - 6f);
		}
		if (this.hurtCooldownCollection == null)
		{
			this.hurtCooldownCollection = MonoSingleton<StainVoxelManager>.Instance.SharedHurtCooldownCollection;
		}
		base.StopAllCoroutines();
		base.StartCoroutine(this.BurningCoroutine());
	}

	// Token: 0x06000BB3 RID: 2995 RVA: 0x00052487 File Offset: 0x00050687
	public void Refuel()
	{
		base.StopAllCoroutines();
		this.timeSinceStartedExtinguishing = null;
		this.timeSinceInitialized = 0f;
		base.StartCoroutine(this.BurningCoroutine());
	}

	// Token: 0x06000BB4 RID: 2996 RVA: 0x000524B8 File Offset: 0x000506B8
	private void Remove()
	{
		base.StopAllCoroutines();
		this.ReturnFire();
		MonoSingleton<StainVoxelManager>.Instance.DoneBurning(this.proxy);
	}

	// Token: 0x06000BB5 RID: 2997 RVA: 0x000524D6 File Offset: 0x000506D6
	private void ReturnFire()
	{
		if (this.fireParticles != null && MonoSingleton<FireObjectPool>.Instance != null)
		{
			MonoSingleton<FireObjectPool>.Instance.ReturnFire(this.fireParticles.gameObject, true);
		}
	}

	// Token: 0x06000BB6 RID: 2998 RVA: 0x00052509 File Offset: 0x00050709
	private IEnumerator BurningCoroutine()
	{
		if (this.timeSinceStartedExtinguishing == null)
		{
			if (this.fireZone == null)
			{
				this.fireZone = base.gameObject.AddComponent<FireZone>();
				this.fireZone.source = FlameSource.Napalm;
				this.fireZone.HurtCooldownCollection = this.hurtCooldownCollection;
				this.fireZone.playerDamage = 10;
				BoxCollider boxCollider = base.gameObject.AddComponent<BoxCollider>();
				boxCollider.isTrigger = true;
				Vector3 vector = new Vector3(2.75f, 5.5f, 2.75f);
				boxCollider.size = vector;
				boxCollider.center = new Vector3(0f, 0.5f, 0f);
			}
			this.SetSize(1f);
			yield return new WaitForSeconds(Mathf.Max(0f, 6f - this.timeSinceInitialized));
			while (NoWeaponCooldown.NoCooldown)
			{
				yield return null;
			}
			this.timeSinceStartedExtinguishing = new TimeSince?(0f);
		}
		if (this.timeSinceStartedExtinguishing == null)
		{
			throw new Exception("timeSinceStartedExtinguishing is null. It shouldn't be.");
		}
		for (;;)
		{
			TimeSince? timeSince = this.timeSinceStartedExtinguishing;
			float? num = ((timeSince != null) ? new float?(timeSince.GetValueOrDefault()) : null);
			float num2 = 2f;
			if (!((num.GetValueOrDefault() < num2) & (num != null)))
			{
				break;
			}
			this.SetSize(1f - this.timeSinceStartedExtinguishing.Value / 2f);
			if (this.fireZone.canHurtPlayer)
			{
				timeSince = this.timeSinceStartedExtinguishing;
				num = ((timeSince != null) ? new float?(timeSince.GetValueOrDefault() / 2f) : null);
				num2 = 0.5f;
				if ((num.GetValueOrDefault() > num2) & (num != null))
				{
					this.fireZone.canHurtPlayer = false;
				}
			}
			if (this.fireZone != null)
			{
				timeSince = this.timeSinceStartedExtinguishing;
				num = ((timeSince != null) ? new float?(timeSince.GetValueOrDefault() / 2f) : null);
				num2 = 0.85f;
				if ((num.GetValueOrDefault() > num2) & (num != null))
				{
					Object.Destroy(this.fireZone);
				}
			}
			yield return null;
		}
		this.Remove();
		yield break;
	}

	// Token: 0x06000BB7 RID: 2999 RVA: 0x00052518 File Offset: 0x00050718
	private void SetSize(float size)
	{
		this.fireParticles.localScale = Vector3.one * 4f * size;
		this.proxy.SetStainSize(size);
	}

	// Token: 0x04000F4D RID: 3917
	private const float BurnTime = 6f;

	// Token: 0x04000F4E RID: 3918
	private const float ExtinguishTime = 2f;

	// Token: 0x04000F4F RID: 3919
	private const float KeepPlayerDamageForFraction = 0.5f;

	// Token: 0x04000F50 RID: 3920
	private const float KeepDamageForFraction = 0.85f;

	// Token: 0x04000F51 RID: 3921
	[SerializeField]
	[HideInInspector]
	private VoxelProxy proxy;

	// Token: 0x04000F52 RID: 3922
	private HurtCooldownCollection hurtCooldownCollection;

	// Token: 0x04000F53 RID: 3923
	[SerializeField]
	[HideInInspector]
	private FireZone fireZone;

	// Token: 0x04000F54 RID: 3924
	[SerializeField]
	[HideInInspector]
	private Transform fireParticles;

	// Token: 0x04000F55 RID: 3925
	[SerializeField]
	[HideInInspector]
	private TimeSince timeSinceInitialized;

	// Token: 0x04000F56 RID: 3926
	private TimeSince? timeSinceStartedExtinguishing;
}
