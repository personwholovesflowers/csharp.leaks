using System;
using UnityEngine;

// Token: 0x020001D7 RID: 471
public class FishBait : MonoBehaviour
{
	// Token: 0x06000998 RID: 2456 RVA: 0x00042938 File Offset: 0x00040B38
	private void Update()
	{
		if (this.landed && !this.returnToRod)
		{
			this.UpdateLineRenderer();
			return;
		}
		if (this.returnToRod)
		{
			if (this.allowedToProgress || this.outOfWater)
			{
				this.fishPullVelocity += 0.3f * Time.deltaTime;
			}
			else
			{
				this.fishPullVelocity *= 1f - 0.4f * Time.deltaTime;
			}
			if (this.fishPullVelocity > 1f)
			{
				this.fishPullVelocity = 1f;
			}
			if (this.fishPullVelocity < 0f)
			{
				this.fishPullVelocity = 0f;
			}
			this.flyProgress += Time.deltaTime * 0.9f * this.fishPullVelocity;
			if (this.fishPullVelocity > 0.1f)
			{
				if (!this.sourceWeapon.pullSound.isPlaying)
				{
					this.sourceWeapon.pullSound.Play();
				}
				this.sourceWeapon.pullSound.pitch = Mathf.Abs(0.7f + this.fishPullVelocity * 2f);
			}
			else
			{
				this.sourceWeapon.pullSound.Stop();
			}
			this.ReturnAnim();
		}
		else
		{
			this.flyProgress += Time.deltaTime;
			this.ThrowAnim();
		}
		this.UpdateLineRenderer();
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x00042A8C File Offset: 0x00040C8C
	private void ThrowAnim()
	{
		if (this.flyProgress >= 1f)
		{
			this.flyProgress = 1f;
			Object.Instantiate<GameObject>(this.splashPrefab, this.baitPoint.position + Vector3.down * 0.3f, Quaternion.Euler(-90f, 0f, 0f));
			this.landed = true;
		}
		float num = Mathf.Sin(this.flyProgress * 3.1415927f) * 20f;
		this.baitPoint.position = Vector3.Lerp(this.baitPoint.position, this.landTarget, this.flyProgress);
		this.baitPoint.position = new Vector3(this.baitPoint.position.x, this.baitPoint.position.y + num, this.baitPoint.position.z);
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x00042B78 File Offset: 0x00040D78
	private void ReturnAnim()
	{
		if (this.flyProgress >= 1f)
		{
			this.flyProgress = 1f;
			this.sourceWeapon.FishCaughtAndGrabbed();
			this.sourceWeapon.pullSound.Stop();
			Object.Destroy(this.spawnedFish.gameObject);
			MonoSingleton<LeaderboardController>.Instance.SubmitFishSize(SteamController.FishSizeMulti);
			return;
		}
		Vector3 vector = this.initialParent.position - this.baitPoint.position;
		this.spawnedFish.rotation = Quaternion.LookRotation(vector);
		float num = ((this.overrideLastMile > 0f) ? this.overrideLastMile : 0.95f);
		float num2 = (this.flyProgress - num) / (1f - num);
		float num3 = this.landTarget.y - 1f;
		this.baitPoint.position = Vector3.Lerp(this.landTarget, this.initialParent.position, this.flyProgress);
		RaycastHit raycastHit;
		bool flag = Physics.Raycast(this.baitPoint.position + Vector3.up * 2f, Vector3.down, out raycastHit, 10f, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies));
		this.baitPoint.position = new Vector3(this.baitPoint.position.x, Mathf.Max(Mathf.Lerp(num3, this.initialParent.position.y, num2), flag ? raycastHit.point.y : float.NegativeInfinity), this.baitPoint.position.z);
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x00042D0C File Offset: 0x00040F0C
	private void UpdateLineRenderer()
	{
		Camera cam = MonoSingleton<CameraController>.Instance.cam;
		Vector3 vector = MonoSingleton<PostProcessV2_Handler>.Instance.hudCam.WorldToScreenPoint(base.transform.position);
		vector = cam.ScreenToWorldPoint(vector);
		vector = this.lineRenderer.transform.InverseTransformPoint(vector);
		this.lineRenderer.SetPosition(0, vector);
		Vector3 position = this.baitPoint.position;
		Vector3 vector2 = this.lineRenderer.transform.InverseTransformPoint(position);
		this.lineRenderer.SetPosition(1, vector2);
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x00042D8F File Offset: 0x00040F8F
	public void ThrowStart(Vector3 targetWorldPosition, Transform inPar, FishingRodWeapon srcWpn)
	{
		this.flyProgress = 0f;
		this.landTarget = targetWorldPosition;
		this.initialParent = inPar;
		this.sourceWeapon = srcWpn;
		this.baitPoint.SetParent(null);
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x00042DBD File Offset: 0x00040FBD
	public void FishHooked()
	{
		this.fishHookedSpawned = Object.Instantiate<GameObject>(this.fishHookedPrefab, this.baitPoint.position + Vector3.up * 3f, Quaternion.identity);
	}

	// Token: 0x0600099E RID: 2462 RVA: 0x00042DF4 File Offset: 0x00040FF4
	public void Dispose()
	{
		Object.Destroy(this.baitPoint.gameObject);
		if (this.fishHookedSpawned)
		{
			Object.Destroy(this.fishHookedSpawned);
		}
	}

	// Token: 0x0600099F RID: 2463 RVA: 0x00042E20 File Offset: 0x00041020
	public void CatchFish(FishObject fish)
	{
		if (this.returnToRod)
		{
			return;
		}
		this.returnToRod = true;
		Object.Destroy(this.fishHookedSpawned);
		this.flyProgress = 0f;
		this.spawnedFish = fish.InstantiateWorld(this.baitPoint.position).transform;
		this.spawnedFish.SetParent(this.baitPoint);
	}

	// Token: 0x060009A0 RID: 2464 RVA: 0x00042E80 File Offset: 0x00041080
	public void OutOfWater()
	{
		if (!this.returnToRod || this.outOfWater)
		{
			return;
		}
		this.outOfWater = true;
		this.overrideLastMile = this.flyProgress;
		MonoSingleton<FishingHUD>.Instance.ShowOutOfWater();
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x00042EB0 File Offset: 0x000410B0
	public void OnTriggerExit(Collider other)
	{
		if (!this.returnToRod)
		{
			return;
		}
		if (other.gameObject.layer == 4)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(this.baitPoint.position, Vector3.down, out raycastHit, 6f) && raycastHit.collider.gameObject.layer == 4)
			{
				Debug.Log("We're above water, ignore trigger exit");
				return;
			}
			Debug.Log("out of water since trigger exit");
			this.OutOfWater();
		}
	}

	// Token: 0x060009A2 RID: 2466 RVA: 0x00042F24 File Offset: 0x00041124
	public void OnCollisionEnter(Collision collision)
	{
		if (!this.returnToRod)
		{
			return;
		}
		LayerMask layerMask = LayerMaskDefaults.Get(LMD.Environment);
		if (layerMask == (layerMask | (1 << collision.gameObject.layer)))
		{
			Debug.LogError("touched env!!!");
			this.OutOfWater();
		}
	}

	// Token: 0x04000C7E RID: 3198
	public Transform baitPoint;

	// Token: 0x04000C7F RID: 3199
	[SerializeField]
	private LineRenderer lineRenderer;

	// Token: 0x04000C80 RID: 3200
	[SerializeField]
	private GameObject splashPrefab;

	// Token: 0x04000C81 RID: 3201
	[SerializeField]
	private GameObject fishHookedPrefab;

	// Token: 0x04000C82 RID: 3202
	private GameObject fishHookedSpawned;

	// Token: 0x04000C83 RID: 3203
	private Transform initialParent;

	// Token: 0x04000C84 RID: 3204
	private Vector3 landTarget;

	// Token: 0x04000C85 RID: 3205
	private FishingRodWeapon sourceWeapon;

	// Token: 0x04000C86 RID: 3206
	public bool landed = true;

	// Token: 0x04000C87 RID: 3207
	public float flyProgress;

	// Token: 0x04000C88 RID: 3208
	private float fishPullVelocity;

	// Token: 0x04000C89 RID: 3209
	private float overrideLastMile = -1f;

	// Token: 0x04000C8A RID: 3210
	public bool allowedToProgress;

	// Token: 0x04000C8B RID: 3211
	private bool returnToRod;

	// Token: 0x04000C8C RID: 3212
	private bool outOfWater;

	// Token: 0x04000C8D RID: 3213
	private Transform spawnedFish;
}
