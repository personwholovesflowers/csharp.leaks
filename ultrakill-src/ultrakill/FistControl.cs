using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x020001F2 RID: 498
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class FistControl : MonoSingleton<FistControl>
{
	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x06000A13 RID: 2579 RVA: 0x000459EC File Offset: 0x00043BEC
	// (set) Token: 0x06000A14 RID: 2580 RVA: 0x00045A05 File Offset: 0x00043C05
	public bool activated
	{
		get
		{
			return this._activated && !MonoSingleton<OptionsManager>.Instance.paused;
		}
		set
		{
			this._activated = value;
		}
	}

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x06000A15 RID: 2581 RVA: 0x00045A0E File Offset: 0x00043C0E
	public GameObject currentArmObject
	{
		get
		{
			return this.spawnedArms[this.currentOrderNum];
		}
	}

	// Token: 0x06000A16 RID: 2582 RVA: 0x00045A21 File Offset: 0x00043C21
	private void Start()
	{
		this.inman = MonoSingleton<InputManager>.Instance;
		this.aud = base.GetComponent<AudioSource>();
		this.currentVarNum = PlayerPrefs.GetInt("CurArm", 0);
		this.ResetFists();
		this.fistCooldown = 0f;
	}

	// Token: 0x06000A17 RID: 2583 RVA: 0x00045A5C File Offset: 0x00043C5C
	private void Update()
	{
		if (this.fistCooldown > -1f)
		{
			this.fistCooldown = Mathf.MoveTowards(this.fistCooldown, 0f, Time.deltaTime * 2f);
		}
		float punchStamina = MonoSingleton<WeaponCharges>.Instance.punchStamina;
		if (!MonoSingleton<OptionsManager>.Instance.paused && this.fistCooldown <= 0f && punchStamina >= 1f)
		{
			if (this.inman.InputSource.Actions.Fist.PunchFeedbacker.WasPerformedThisFrame() && (this.currentVarNum == 0 || this.ForceArm(0, false)))
			{
				this.currentPunch.heldAction = MonoSingleton<InputManager>.Instance.InputSource.Actions.Fist.PunchFeedbacker;
				this.currentPunch.PunchStart();
			}
			if (this.inman.InputSource.Actions.Fist.PunchKnuckleblaster.WasPerformedThisFrame() && (this.currentVarNum == 1 || this.ForceArm(1, false)))
			{
				this.currentPunch.heldAction = MonoSingleton<InputManager>.Instance.InputSource.Actions.Fist.PunchKnuckleblaster;
				this.currentPunch.PunchStart();
			}
		}
		if (!MonoSingleton<OptionsManager>.Instance || MonoSingleton<OptionsManager>.Instance.mainMenu || MonoSingleton<OptionsManager>.Instance.inIntro || MonoSingleton<OptionsManager>.Instance.paused || (MonoSingleton<ScanningStuff>.Instance && MonoSingleton<ScanningStuff>.Instance.IsReading) || GameStateManager.Instance.PlayerInputLocked)
		{
			return;
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.IsPressed && this.shopping)
		{
			this.zooming = true;
			MonoSingleton<CameraController>.Instance.Zoom(MonoSingleton<CameraController>.Instance.defaultFov / 2f);
		}
		else if (this.zooming)
		{
			this.zooming = false;
			MonoSingleton<CameraController>.Instance.StopZoom();
		}
		if (this.spawnedArms.Count > 1 && !this.shopping && (MonoSingleton<SpawnMenu>.Instance == null || !MonoSingleton<SpawnMenu>.Instance.gameObject.activeInHierarchy))
		{
			if (MonoSingleton<InputManager>.Instance.InputSource.ChangeFist.WasPerformedThisFrame)
			{
				this.ScrollArm();
			}
		}
		else if (this.spawnedArms.Count > 0 && this.currentPunch == null)
		{
			this.ArmChange(0);
		}
		if (this.spawnedArms.Count == 0 && MonoSingleton<InputManager>.Instance.InputSource.Punch.WasPerformedThisFrame && (this.forcedLoadout == null || this.forcedLoadout.arm.blueVariant != VariantOption.ForceOff || this.forcedLoadout.arm.redVariant != VariantOption.ForceOff))
		{
			MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage("<color=red>CAN'T PUNCH IF YOU HAVE NO ARM EQUIPPED, DUMBASS</color>\nArms can be re-equipped at the shop", "", "", 0, false, false, true);
		}
	}

	// Token: 0x06000A18 RID: 2584 RVA: 0x00045D2F File Offset: 0x00043F2F
	public void ScrollArm()
	{
		if (this.currentOrderNum < this.spawnedArms.Count - 1)
		{
			this.ArmChange(this.currentOrderNum + 1);
		}
		else
		{
			this.ArmChange(0);
		}
		this.aud.Play();
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x00045D68 File Offset: 0x00043F68
	public void RefreshArm()
	{
		this.ArmChange(this.currentOrderNum);
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x00045D78 File Offset: 0x00043F78
	public bool ForceArm(int varNum, bool animation = false)
	{
		bool flag = false;
		if (this.spawnedArmNums.Contains(varNum))
		{
			this.ArmChange(this.spawnedArmNums.IndexOf(varNum));
			flag = true;
		}
		if (animation)
		{
			Punch punch;
			if (varNum == 2)
			{
				MonoSingleton<HookArm>.Instance.Inspect();
			}
			else if (this.spawnedArms[this.currentOrderNum].TryGetComponent<Punch>(out punch))
			{
				punch.EquipAnimation();
			}
			this.aud.Play();
		}
		return flag;
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x00045DE8 File Offset: 0x00043FE8
	public void ArmChange(int orderNum)
	{
		if (orderNum < this.spawnedArms.Count)
		{
			if (this.currentOrderNum < this.spawnedArms.Count)
			{
				this.spawnedArms[this.currentOrderNum].SetActive(false);
			}
			this.spawnedArms[orderNum].SetActive(true);
			this.currentOrderNum = orderNum;
			this.currentVarNum = this.spawnedArmNums[orderNum];
			PlayerPrefs.SetInt("CurArm", this.currentVarNum);
			this.UpdateFistIcon();
			this.currentPunch = this.currentArmObject.GetComponent<Punch>();
			if (MonoSingleton<FistControl>.Instance.fistCooldown > 0.1f)
			{
				MonoSingleton<FistControl>.Instance.fistCooldown = 0.1f;
			}
		}
	}

	// Token: 0x14000007 RID: 7
	// (add) Token: 0x06000A1C RID: 2588 RVA: 0x00045EA4 File Offset: 0x000440A4
	// (remove) Token: 0x06000A1D RID: 2589 RVA: 0x00045EDC File Offset: 0x000440DC
	public event Action<int> FistIconUpdated;

	// Token: 0x06000A1E RID: 2590 RVA: 0x00045F11 File Offset: 0x00044111
	public void UpdateFistIcon()
	{
		Action<int> fistIconUpdated = this.FistIconUpdated;
		if (fistIconUpdated == null)
		{
			return;
		}
		fistIconUpdated(this.currentVarNum);
	}

	// Token: 0x06000A1F RID: 2591 RVA: 0x00045F29 File Offset: 0x00044129
	public void NoFist()
	{
		if (this.spawnedArms.Count > 0)
		{
			this.spawnedArms[this.currentOrderNum].SetActive(false);
		}
		this.activated = false;
	}

	// Token: 0x06000A20 RID: 2592 RVA: 0x00045F57 File Offset: 0x00044157
	public void YesFist()
	{
		if (this.spawnedArms.Count > 0)
		{
			this.spawnedArms[this.currentOrderNum].SetActive(true);
		}
		this.activated = true;
		this.UpdateFistIcon();
	}

	// Token: 0x06000A21 RID: 2593 RVA: 0x00045F8C File Offset: 0x0004418C
	public void ResetFists()
	{
		if (this.spawnedArms.Count > 0)
		{
			for (int i = 0; i < this.spawnedArms.Count; i++)
			{
				Object.Destroy(this.spawnedArms[i]);
			}
			this.spawnedArms.Clear();
			this.spawnedArmNums.Clear();
		}
		MonoSingleton<HookArm>.Instance.equipped = false;
		if ((MonoSingleton<PrefsManager>.Instance.GetInt("weapon.arm0", 1) == 1 && (this.forcedLoadout == null || this.forcedLoadout.arm.blueVariant == VariantOption.IfEquipped)) || (this.forcedLoadout != null && this.forcedLoadout.arm.blueVariant == VariantOption.ForceOn))
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.blueArm.ToAsset(), base.transform);
			gameObject.SetActive(false);
			this.spawnedArms.Add(gameObject);
			this.spawnedArmNums.Add(0);
		}
		this.CheckFist("arm1");
		this.CheckFist("arm2");
		this.CheckFist("arm3");
		if (this.spawnedArms.Count < 1 || !MonoSingleton<PrefsManager>.Instance.GetBool("armIcons", false))
		{
			GameObject[] array = this.fistPanels;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].SetActive(false);
			}
		}
		else
		{
			GameObject[] array = this.fistPanels;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].SetActive(true);
			}
		}
		this.ForceArm(this.currentVarNum, false);
		this.UpdateFistIcon();
	}

	// Token: 0x06000A22 RID: 2594 RVA: 0x00046100 File Offset: 0x00044300
	private void CheckFist(string name)
	{
		if (this.forcedLoadout != null)
		{
			if (!(name == "arm1"))
			{
				if (name == "arm2")
				{
					if (this.forcedLoadout.arm.greenVariant == VariantOption.ForceOn)
					{
						MonoSingleton<HookArm>.Instance.equipped = true;
						return;
					}
					if (this.forcedLoadout.arm.greenVariant == VariantOption.ForceOff)
					{
						return;
					}
				}
			}
			else
			{
				if (this.forcedLoadout.arm.redVariant == VariantOption.ForceOn)
				{
					this.spawnedArmNums.Add(1);
					GameObject gameObject = Object.Instantiate<GameObject>(this.redArm.ToAsset(), base.transform);
					gameObject.SetActive(false);
					this.spawnedArms.Add(gameObject);
					return;
				}
				if (this.forcedLoadout.arm.redVariant == VariantOption.ForceOff)
				{
					return;
				}
			}
		}
		if (MonoSingleton<PrefsManager>.Instance.GetInt("weapon." + name, 1) == 1 && GameProgressSaver.CheckGear(name) == 1)
		{
			GameObject gameObject2 = null;
			if (!(name == "arm1"))
			{
				if (!(name == "arm2"))
				{
					if (name == "arm3")
					{
						gameObject2 = Object.Instantiate<GameObject>(this.goldArm.ToAsset(), base.transform);
						this.spawnedArmNums.Add(3);
					}
				}
				else
				{
					MonoSingleton<HookArm>.Instance.equipped = true;
				}
			}
			else
			{
				gameObject2 = Object.Instantiate<GameObject>(this.redArm.ToAsset(), base.transform);
				this.spawnedArmNums.Add(1);
			}
			if (gameObject2 != null)
			{
				this.spawnedArms.Add(gameObject2);
			}
		}
	}

	// Token: 0x06000A23 RID: 2595 RVA: 0x00046285 File Offset: 0x00044485
	public void ShopMode()
	{
		this.shopping = true;
		this.shopRequests++;
	}

	// Token: 0x06000A24 RID: 2596 RVA: 0x0004629C File Offset: 0x0004449C
	public void StopShop()
	{
		this.shopRequests--;
		if (this.shopRequests <= 0)
		{
			this.shopping = false;
		}
	}

	// Token: 0x06000A25 RID: 2597 RVA: 0x000462BC File Offset: 0x000444BC
	public void ResetHeldItemPosition()
	{
		if (this.heldObject.reverseTransformSettings)
		{
			this.heldObject.transform.localPosition = this.heldObject.putDownPosition;
			this.heldObject.transform.localScale = this.heldObject.putDownScale;
			this.heldObject.transform.localRotation = Quaternion.Euler(this.heldObject.putDownRotation);
		}
		else
		{
			this.heldObject.transform.localPosition = Vector3.zero;
			this.heldObject.transform.localScale = Vector3.one;
			this.heldObject.transform.localRotation = Quaternion.identity;
		}
		Transform[] componentsInChildren = this.heldObject.GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = 13;
		}
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x00046396 File Offset: 0x00044596
	public void TutorialCheckForArmThatCanPunch()
	{
		if (MonoSingleton<PrefsManager>.Instance.GetInt("weapon.arm0", 1) == 0 && MonoSingleton<PrefsManager>.Instance.GetInt("weapon.arm1", 1) == 0)
		{
			MonoSingleton<PrefsManager>.Instance.SetInt("weapon.arm0", 1);
			this.ResetFists();
		}
	}

	// Token: 0x04000D28 RID: 3368
	private InputManager inman;

	// Token: 0x04000D29 RID: 3369
	public ForcedLoadout forcedLoadout;

	// Token: 0x04000D2A RID: 3370
	public AssetReference blueArm;

	// Token: 0x04000D2B RID: 3371
	public AssetReference redArm;

	// Token: 0x04000D2C RID: 3372
	public AssetReference goldArm;

	// Token: 0x04000D2D RID: 3373
	private int currentOrderNum;

	// Token: 0x04000D2E RID: 3374
	private int currentVarNum;

	// Token: 0x04000D2F RID: 3375
	private List<GameObject> spawnedArms = new List<GameObject>();

	// Token: 0x04000D30 RID: 3376
	private List<int> spawnedArmNums = new List<int>();

	// Token: 0x04000D31 RID: 3377
	private AudioSource aud;

	// Token: 0x04000D32 RID: 3378
	public bool shopping;

	// Token: 0x04000D33 RID: 3379
	private int shopRequests;

	// Token: 0x04000D34 RID: 3380
	public GameObject[] fistPanels;

	// Token: 0x04000D35 RID: 3381
	public ItemIdentifier heldObject;

	// Token: 0x04000D36 RID: 3382
	public float fistCooldown;

	// Token: 0x04000D37 RID: 3383
	public Color fistIconColor;

	// Token: 0x04000D38 RID: 3384
	private bool _activated = true;

	// Token: 0x04000D39 RID: 3385
	public Punch currentPunch;

	// Token: 0x04000D3A RID: 3386
	[HideInInspector]
	public int forceNoHold;

	// Token: 0x04000D3B RID: 3387
	private bool zooming;
}
