using System;
using System.Collections.Generic;
using ULTRAKILL.Cheats;
using ULTRAKILL.Cheats.UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000427 RID: 1063
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class SpawnMenu : MonoSingleton<SpawnMenu>
{
	// Token: 0x060017F3 RID: 6131 RVA: 0x000C3420 File Offset: 0x000C1620
	protected override void Awake()
	{
		base.Awake();
		this.RebuildIcons();
		this.RebuildMenu();
		if (MonoSingleton<UnlockablesData>.Instance != null)
		{
			UnlockablesData instance = MonoSingleton<UnlockablesData>.Instance;
			instance.unlockableFound = (UnityAction)Delegate.Combine(instance.unlockableFound, new UnityAction(this.RebuildMenu));
		}
	}

	// Token: 0x060017F4 RID: 6132 RVA: 0x000C3472 File Offset: 0x000C1672
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (MonoSingleton<UnlockablesData>.Instance != null)
		{
			UnlockablesData instance = MonoSingleton<UnlockablesData>.Instance;
			instance.unlockableFound = (UnityAction)Delegate.Remove(instance.unlockableFound, new UnityAction(this.RebuildMenu));
		}
	}

	// Token: 0x060017F5 RID: 6133 RVA: 0x000C34B0 File Offset: 0x000C16B0
	public void RebuildIcons()
	{
		this.spriteIcons = new Dictionary<string, Sprite>();
		foreach (CheatAssetObject.KeyIcon keyIcon in MonoSingleton<IconManager>.Instance.CurrentIcons.sandboxMenuIcons)
		{
			this.spriteIcons.Add(keyIcon.key, keyIcon.sprite);
		}
	}

	// Token: 0x060017F6 RID: 6134 RVA: 0x000C3508 File Offset: 0x000C1708
	public void ResetMenu()
	{
		if (this.sectionReference == null || this.sectionReference.transform.parent == null)
		{
			return;
		}
		for (int i = 1; i < this.sectionReference.transform.parent.childCount; i++)
		{
			Object.Destroy(this.sectionReference.transform.parent.GetChild(i).gameObject);
		}
	}

	// Token: 0x060017F7 RID: 6135 RVA: 0x000C357C File Offset: 0x000C177C
	public void RebuildMenu()
	{
		this.ResetMenu();
		this.CreateButtons(this.objects.sandboxTools, "SANDBOX TOOLS :^)");
		if (MapInfoBase.Instance.sandboxTools)
		{
			this.CreateButtons(this.objects.sandboxObjects, "SANDBOX");
			this.CreateButtons(this.objects.specialSandbox, "SPECIAL");
		}
		this.CreateButtons(this.objects.enemies, "ENEMIES");
		this.CreateButtons(this.objects.objects, "ITEMS");
		this.CreateButtons(this.objects.unlockables, "UNLOCKABLES");
		this.sectionReference.gameObject.SetActive(false);
	}

	// Token: 0x060017F8 RID: 6136 RVA: 0x000C3630 File Offset: 0x000C1830
	private Sprite ResolveSprite(SpawnableObject target)
	{
		if (!string.IsNullOrEmpty(target.iconKey) && this.spriteIcons.ContainsKey(target.iconKey))
		{
			return this.spriteIcons[target.iconKey];
		}
		if (target.gridIcon != null)
		{
			return target.gridIcon;
		}
		return MonoSingleton<IconManager>.Instance.CurrentIcons.genericSandboxToolIcon;
	}

	// Token: 0x060017F9 RID: 6137 RVA: 0x000C3694 File Offset: 0x000C1894
	private void CreateButtons(SpawnableObject[] list, string sectionName)
	{
		SpawnMenuSectionReference spawnMenuSectionReference = Object.Instantiate<SpawnMenuSectionReference>(this.sectionReference, this.sectionReference.transform.parent);
		spawnMenuSectionReference.gameObject.SetActive(true);
		spawnMenuSectionReference.sectionName.text = sectionName;
		for (int i = 0; i < list.Length; i++)
		{
			if (!(list[i] == null) && (!list[i].sandboxOnly || MapInfoBase.Instance.sandboxTools))
			{
				bool flag = list != this.objects.enemies || MonoSingleton<BestiaryData>.Instance.GetEnemy(list[i].enemyType) >= 1;
				if (list[i].spawnableObjectType == SpawnableObject.SpawnableObjectDataType.Unlockable)
				{
					UnlockableType unlockableType = list[i].unlockableType;
					if (!MonoSingleton<UnlockablesData>.Instance.IsUnlocked(unlockableType))
					{
						flag = false;
					}
				}
				if (OverwriteUnlocks.Enabled)
				{
					flag = true;
				}
				if (flag)
				{
					spawnMenuSectionReference.buttonBackgroundImage.color = list[i].backgroundColor;
					spawnMenuSectionReference.buttonForegroundImage.sprite = this.ResolveSprite(list[i]);
				}
				else
				{
					spawnMenuSectionReference.buttonBackgroundImage.color = Color.gray;
					spawnMenuSectionReference.buttonForegroundImage.sprite = this.lockedIcon;
				}
				Button button = Object.Instantiate<Button>(spawnMenuSectionReference.button, spawnMenuSectionReference.grid.transform, false);
				SpawnableObject spawnableObj = list[i];
				if (flag)
				{
					button.onClick.AddListener(delegate
					{
						this.SelectObject(spawnableObj);
					});
				}
			}
		}
		spawnMenuSectionReference.button.gameObject.SetActive(false);
	}

	// Token: 0x060017FA RID: 6138 RVA: 0x000C3810 File Offset: 0x000C1A10
	private void Update()
	{
		if (MonoSingleton<InputManager>.Instance.InputSource.Pause.WasPerformedThisFrame)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x060017FB RID: 6139 RVA: 0x000C3834 File Offset: 0x000C1A34
	private void OnDisable()
	{
		if (!base.gameObject.scene.isLoaded)
		{
			return;
		}
		MonoSingleton<OptionsManager>.Instance.UnFreeze();
	}

	// Token: 0x060017FC RID: 6140 RVA: 0x000C3861 File Offset: 0x000C1A61
	protected override void OnEnable()
	{
		base.OnEnable();
		GameStateManager.Instance.RegisterState(new GameState("sandbox-spawn-menu", base.gameObject)
		{
			cursorLock = LockMode.Unlock,
			playerInputLock = LockMode.Lock,
			cameraInputLock = LockMode.Lock
		});
	}

	// Token: 0x060017FD RID: 6141 RVA: 0x000C3898 File Offset: 0x000C1A98
	private void SelectObject(SpawnableObject obj)
	{
		this.armManager.SelectArm(obj);
	}

	// Token: 0x0400219B RID: 8603
	[SerializeField]
	private SpawnMenuSectionReference sectionReference;

	// Token: 0x0400219C RID: 8604
	[SerializeField]
	private SpawnableObjectsDatabase objects;

	// Token: 0x0400219D RID: 8605
	[HideInInspector]
	public SummonSandboxArm armManager;

	// Token: 0x0400219E RID: 8606
	[SerializeField]
	private Sprite lockedIcon;

	// Token: 0x0400219F RID: 8607
	private Dictionary<string, Sprite> spriteIcons;
}
