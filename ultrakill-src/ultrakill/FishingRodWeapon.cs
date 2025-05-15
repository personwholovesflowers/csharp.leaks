using System;
using UnityEngine;

// Token: 0x020001E8 RID: 488
public class FishingRodWeapon : MonoBehaviour
{
	// Token: 0x170000ED RID: 237
	// (get) Token: 0x060009ED RID: 2541 RVA: 0x000446B5 File Offset: 0x000428B5
	private float bottomBound
	{
		get
		{
			return this.fishDesirePosition + this.fishTolerance / 2f;
		}
	}

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x060009EE RID: 2542 RVA: 0x000446CA File Offset: 0x000428CA
	private float topBound
	{
		get
		{
			return this.fishDesirePosition - this.fishTolerance / 2f;
		}
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x060009EF RID: 2543 RVA: 0x000446DF File Offset: 0x000428DF
	private bool struggleSatisfied
	{
		get
		{
			return this.playerProvidedPosition < this.bottomBound && this.playerProvidedPosition > this.topBound;
		}
	}

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x060009F0 RID: 2544 RVA: 0x00044700 File Offset: 0x00042900
	private Vector3 approximateTargetPosition
	{
		get
		{
			return MonoSingleton<NewMovement>.Instance.transform.position + (MonoSingleton<NewMovement>.Instance.transform.forward * 10f * FishingRodWeapon.minDistanceMulti + MonoSingleton<NewMovement>.Instance.transform.forward * 35f * this.selectedPower) * FishingRodWeapon.suggestedDistanceMulti - Vector3.up * 1.9f;
		}
	}

	// Token: 0x060009F1 RID: 2545 RVA: 0x0004478C File Offset: 0x0004298C
	public void ThrowBaitEvent()
	{
		if (this.spawnedBaitCon == null)
		{
			this.spawnedBaitCon = Object.Instantiate<FishBait>(this.baitPrefab, this.rodTip.position, Quaternion.identity, this.rodTip);
			this.spawnedBaitCon.landed = false;
			this.spawnedBaitCon.ThrowStart(this.targetingCircle.transform.position, this.rodTip, this);
		}
	}

	// Token: 0x060009F2 RID: 2546 RVA: 0x000447FC File Offset: 0x000429FC
	private void Awake()
	{
		FishingRodWeapon.suggestedDistanceMulti = 1f;
		this.timeSinceAction = 0f;
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x00044818 File Offset: 0x00042A18
	private void OnEnable()
	{
		this.ResetFishing();
		MonoSingleton<FishingHUD>.Instance.ShowHUD();
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x0004482C File Offset: 0x00042A2C
	public static GameObject CreateFishPickup(ItemIdentifier template, FishObject fish, bool grab, bool unlock = true)
	{
		if (unlock)
		{
			MonoSingleton<FishManager>.Instance.UnlockFish(fish);
		}
		if (grab)
		{
			if (MonoSingleton<FistControl>.Instance.heldObject)
			{
				Object.Destroy(MonoSingleton<FistControl>.Instance.heldObject.gameObject);
			}
			MonoSingleton<FistControl>.Instance.currentPunch.ResetHeldState();
		}
		ItemIdentifier itemIdentifier;
		if (fish.customPickup != null)
		{
			itemIdentifier = Object.Instantiate<ItemIdentifier>(fish.customPickup);
			if (!itemIdentifier.GetComponent<FishObjectReference>())
			{
				itemIdentifier.gameObject.AddComponent<FishObjectReference>().fishObject = fish;
			}
		}
		else
		{
			itemIdentifier = Object.Instantiate<ItemIdentifier>(template);
			itemIdentifier.gameObject.AddComponent<FishObjectReference>().fishObject = fish;
			Transform transform = itemIdentifier.transform.GetChild(0).transform;
			Vector3 localPosition = transform.localPosition;
			Quaternion localRotation = transform.localRotation;
			Vector3 localScale = transform.localScale;
			Object.Destroy(transform.gameObject);
			GameObject gameObject = fish.InstantiateDumb();
			gameObject.transform.SetParent(itemIdentifier.transform);
			gameObject.transform.localPosition = localPosition;
			gameObject.transform.localRotation = localRotation;
			gameObject.transform.localScale = localScale;
		}
		if (grab)
		{
			MonoSingleton<FistControl>.Instance.currentPunch.ForceHold(itemIdentifier);
		}
		return itemIdentifier.gameObject;
	}

	// Token: 0x060009F5 RID: 2549 RVA: 0x0004495C File Offset: 0x00042B5C
	public void FishCaughtAndGrabbed()
	{
		this.animator.SetTrigger(FishingRodWeapon.Idle);
		MonoSingleton<FishingHUD>.Instance.ShowFishCaught(true, this.hookedFishe.fish);
		FishingRodWeapon.CreateFishPickup(this.fishPickupTemplate, this.hookedFishe.fish, true, true);
		this.ResetFishing();
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x000449B0 File Offset: 0x00042BB0
	private void ResetFishing()
	{
		this.state = FishingRodState.ReadyToThrow;
		if (this.spawnedBaitCon)
		{
			this.spawnedBaitCon.Dispose();
			Object.Destroy(this.spawnedBaitCon.gameObject);
		}
		if (this.targetingCircle)
		{
			Object.Destroy(this.targetingCircle.gameObject);
		}
		this.selectedPower = 0f;
		this.climaxed = false;
		this.baitThrown = false;
		this.animator.ResetTrigger(FishingRodWeapon.Idle);
		this.animator.ResetTrigger(FishingRodWeapon.Throw);
		this.fishHooked = false;
		this.timeSinceAction = 0f;
		this.noFishErrorDisplayed = false;
		MonoSingleton<FishingHUD>.Instance.SetFishHooked(false);
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x00004AE3 File Offset: 0x00002CE3
	private void OnGUI()
	{
	}

	// Token: 0x060009F8 RID: 2552 RVA: 0x00044A6C File Offset: 0x00042C6C
	private void Update()
	{
		if (GameStateManager.Instance.PlayerInputLocked)
		{
			return;
		}
		if (MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo())
		{
			return;
		}
		if (MonoSingleton<FishingHUD>.Instance.timeSinceFishCaught >= 1f && (MonoSingleton<InputManager>.Instance.InputSource.Punch.WasPerformedThisFrame || MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame))
		{
			MonoSingleton<FishingHUD>.Instance.ShowFishCaught(false, null);
		}
		MonoSingleton<FishingHUD>.Instance.SetState(this.state);
		switch (this.state)
		{
		case FishingRodState.ReadyToThrow:
			if (MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame && this.timeSinceAction > 0.1f)
			{
				MonoSingleton<FishingHUD>.Instance.SetPowerMeter(0f, false);
				this.selectedPower = 0f;
				this.climaxed = false;
				this.fishHooked = false;
				this.baitThrown = false;
				this.state = FishingRodState.SelectingPower;
				this.targetingCircle = Object.Instantiate<FishingRodTarget>(this.targetPrefab, this.approximateTargetPosition, Quaternion.identity);
				this.timeSinceAction = 0f;
				return;
			}
			break;
		case FishingRodState.SelectingPower:
		{
			this.selectedPower += (Time.deltaTime * 0.4f + this.selectedPower * 0.01f) * (this.climaxed ? (-0.5f) : 1f);
			if (this.selectedPower > 1f)
			{
				this.selectedPower = 1f;
				this.climaxed = true;
			}
			if (this.selectedPower < 0.1f)
			{
				this.climaxed = false;
			}
			Vector3 vector = this.approximateTargetPosition;
			RaycastHit raycastHit;
			bool flag;
			if (Physics.Raycast(vector + Vector3.up * 3f, Vector3.down, out raycastHit, 30f))
			{
				vector = raycastHit.point;
				if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Water"))
				{
					Water water;
					if (raycastHit.collider.TryGetComponent<Water>(out water) && water.fishDB)
					{
						this.currentFishPool = water.fishDB;
						this.currentWater = water;
						flag = true;
						if (water.overrideFishingPoint)
						{
							vector = water.overrideFishingPoint.position;
						}
					}
					else
					{
						this.currentFishPool = null;
						this.currentWater = null;
						flag = false;
					}
				}
				else
				{
					this.currentFishPool = null;
					this.currentWater = null;
					flag = false;
				}
			}
			else
			{
				this.currentFishPool = null;
				this.currentWater = null;
				flag = false;
			}
			MonoSingleton<FishingHUD>.Instance.SetPowerMeter(this.selectedPower, flag);
			if (flag)
			{
				this.targetingCircle.transform.position = vector + Vector3.up * 0.5f;
				this.targetingCircle.SetState(true, Vector3.Distance(raycastHit.point, MonoSingleton<NewMovement>.Instance.transform.position));
				this.targetingCircle.waterNameText.text = this.currentFishPool.fullName;
				this.targetingCircle.waterNameText.color = this.currentFishPool.symbolColor;
			}
			else
			{
				this.targetingCircle.transform.position = vector + Vector3.up * 0.5f;
				this.targetingCircle.SetState(false, Vector3.Distance(raycastHit.point, MonoSingleton<NewMovement>.Instance.transform.position));
				this.targetingCircle.waterNameText.text = "";
			}
			this.targetingCircle.transform.forward = MonoSingleton<NewMovement>.Instance.transform.forward;
			if (MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasCanceledThisFrame && this.timeSinceAction > 0.1f)
			{
				if (flag)
				{
					this.targetingCircle.GetComponent<Animator>().SetTrigger(FishingRodWeapon.Set);
					this.animator.ResetTrigger(FishingRodWeapon.Throw);
					this.state = FishingRodState.Throwing;
					this.timeSinceAction = 0f;
					return;
				}
				this.ResetFishing();
				return;
			}
			break;
		}
		case FishingRodState.Throwing:
			this.targetingCircle.transform.forward = MonoSingleton<NewMovement>.Instance.transform.forward;
			this.fishHooked = false;
			if (!this.baitThrown)
			{
				this.baitThrown = true;
				this.animator.SetTrigger(FishingRodWeapon.Throw);
			}
			if (this.spawnedBaitCon && this.spawnedBaitCon.landed)
			{
				this.state = FishingRodState.WaitingForFish;
				this.timeSinceBaitInWater = 0f;
				this.distanceAfterThrow = Vector3.Distance(MonoSingleton<NewMovement>.Instance.transform.position, this.spawnedBaitCon.baitPoint.position);
				Object.Destroy(this.targetingCircle.gameObject);
				return;
			}
			break;
		case FishingRodState.WaitingForFish:
			this.baitThrown = false;
			if (Vector3.Distance(MonoSingleton<NewMovement>.Instance.transform.position, this.spawnedBaitCon.baitPoint.position) > this.distanceAfterThrow + 30f)
			{
				Object.Destroy(this.spawnedBaitCon.gameObject);
				MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage("Fishing interrupted", "", "", 0, false, false, true);
				this.ResetFishing();
				return;
			}
			if (!this.fishHooked && Random.value < 0.002f + this.timeSinceBaitInWater * 0.01f)
			{
				this.hookedFishe = this.currentFishPool.GetRandomFish(this.currentWater.attractFish);
				if (this.hookedFishe == null)
				{
					if (!this.noFishErrorDisplayed)
					{
						this.noFishErrorDisplayed = true;
						MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage("Nothing seems to be biting here...", "", "", 0, false, false, true);
						return;
					}
					break;
				}
				else
				{
					this.currentWater.attractFish = null;
					this.fishHooked = true;
					MonoSingleton<FishingHUD>.Instance.SetFishHooked(true);
					this.spawnedBaitCon.FishHooked();
				}
			}
			if (MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed || MonoSingleton<InputManager>.Instance.InputSource.Fire2.IsPressed)
			{
				this.animator.SetTrigger(FishingRodWeapon.Pull);
				if (this.fishHooked)
				{
					MonoSingleton<FishingHUD>.Instance.SetFishHooked(false);
					this.state = FishingRodState.FishStruggle;
					this.spawnedBaitCon.CatchFish(this.hookedFishe.fish);
					return;
				}
				Object.Destroy(this.spawnedBaitCon.gameObject);
				this.animator.SetTrigger(FishingRodWeapon.Idle);
				this.animator.ResetTrigger(FishingRodWeapon.Throw);
				this.animator.Play(FishingRodWeapon.Idle);
				this.ResetFishing();
				return;
			}
			break;
		case FishingRodState.FishStruggle:
			this.fishDesirePosition = Mathf.PerlinNoise(Time.time * 0.3f, 0f);
			this.fishTolerance = 0.1f + 0.4f * Mathf.PerlinNoise(Time.time * 0.4f, 0f);
			if (MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed)
			{
				this.playerPositionVelocity += 1.9f * Time.deltaTime;
				this.animator.SetTrigger(FishingRodWeapon.Pull);
			}
			else if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.IsPressed)
			{
				this.playerPositionVelocity -= 1.9f * Time.deltaTime;
				this.animator.SetTrigger(FishingRodWeapon.Pull);
			}
			else
			{
				this.playerPositionVelocity *= 1f - 2f * Time.deltaTime;
			}
			this.playerProvidedPosition += this.playerPositionVelocity * Time.deltaTime;
			if (this.playerProvidedPosition > 1f)
			{
				this.playerProvidedPosition = 1f;
				this.playerPositionVelocity = -this.playerPositionVelocity;
			}
			if (this.playerProvidedPosition < 0f)
			{
				this.playerProvidedPosition = 0f;
				this.playerPositionVelocity = -this.playerPositionVelocity;
			}
			MonoSingleton<FishingHUD>.Instance.SetPlayerStrugglePosition(this.playerProvidedPosition);
			MonoSingleton<FishingHUD>.Instance.SetStruggleSatisfied(this.struggleSatisfied);
			MonoSingleton<FishingHUD>.Instance.SetFishDesire(Mathf.Clamp01(this.topBound), Mathf.Clamp01(this.bottomBound));
			this.spawnedBaitCon.allowedToProgress = this.struggleSatisfied;
			MonoSingleton<FishingHUD>.Instance.SetStruggleProgress(this.spawnedBaitCon.flyProgress, this.hookedFishe.fish.blockedIcon, this.hookedFishe.fish.icon);
			break;
		default:
			return;
		}
	}

	// Token: 0x04000CE6 RID: 3302
	[SerializeField]
	private Animator animator;

	// Token: 0x04000CE7 RID: 3303
	[SerializeField]
	private FishingRodTarget targetPrefab;

	// Token: 0x04000CE8 RID: 3304
	[SerializeField]
	private FishBait baitPrefab;

	// Token: 0x04000CE9 RID: 3305
	[SerializeField]
	private Transform rodTip;

	// Token: 0x04000CEA RID: 3306
	[SerializeField]
	private ItemIdentifier fishPickupTemplate;

	// Token: 0x04000CEB RID: 3307
	public AudioSource pullSound;

	// Token: 0x04000CEC RID: 3308
	private FishingRodTarget targetingCircle;

	// Token: 0x04000CED RID: 3309
	private FishBait spawnedBaitCon;

	// Token: 0x04000CEE RID: 3310
	private FishingRodState state;

	// Token: 0x04000CEF RID: 3311
	private float selectedPower;

	// Token: 0x04000CF0 RID: 3312
	private bool climaxed;

	// Token: 0x04000CF1 RID: 3313
	private static readonly int Set = Animator.StringToHash("Set");

	// Token: 0x04000CF2 RID: 3314
	private static readonly int Throw = Animator.StringToHash("Throw");

	// Token: 0x04000CF3 RID: 3315
	private bool baitThrown;

	// Token: 0x04000CF4 RID: 3316
	private float distanceAfterThrow;

	// Token: 0x04000CF5 RID: 3317
	private bool fishHooked;

	// Token: 0x04000CF6 RID: 3318
	private FishDB currentFishPool;

	// Token: 0x04000CF7 RID: 3319
	private Water currentWater;

	// Token: 0x04000CF8 RID: 3320
	private FishDescriptor hookedFishe;

	// Token: 0x04000CF9 RID: 3321
	private static readonly int Pull = Animator.StringToHash("Pull");

	// Token: 0x04000CFA RID: 3322
	private static readonly int Idle = Animator.StringToHash("Idle");

	// Token: 0x04000CFB RID: 3323
	private float fishTolerance = 0.5f;

	// Token: 0x04000CFC RID: 3324
	private float fishDesirePosition = 0.25f;

	// Token: 0x04000CFD RID: 3325
	private float playerProvidedPosition;

	// Token: 0x04000CFE RID: 3326
	private float playerPositionVelocity;

	// Token: 0x04000CFF RID: 3327
	private TimeSince timeSinceBaitInWater;

	// Token: 0x04000D00 RID: 3328
	private TimeSince timeSinceAction;

	// Token: 0x04000D01 RID: 3329
	private bool noFishErrorDisplayed;

	// Token: 0x04000D02 RID: 3330
	public static float suggestedDistanceMulti = 1f;

	// Token: 0x04000D03 RID: 3331
	public static float minDistanceMulti = 1f;
}
