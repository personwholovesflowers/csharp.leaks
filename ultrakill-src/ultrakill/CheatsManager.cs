using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConsole;
using plog;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x020000B2 RID: 178
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class CheatsManager : MonoSingleton<CheatsManager>
{
	// Token: 0x1700005C RID: 92
	// (get) Token: 0x06000378 RID: 888 RVA: 0x00015E48 File Offset: 0x00014048
	public static bool KeepCheatsEnabled
	{
		get
		{
			return MonoSingleton<PrefsManager>.Instance.GetBool("cheat.ultrakill.keep-enabled", false) || (MapInfoBase.Instance && MapInfoBase.Instance.sandboxTools);
		}
	}

	// Token: 0x06000379 RID: 889 RVA: 0x00015E76 File Offset: 0x00014076
	public void ShowMenu()
	{
		this.cheatsManagerMenu.SetActive(true);
		GameStateManager.Instance.RegisterState(new GameState("cheats-menu", this.cheatsManagerMenu)
		{
			cursorLock = LockMode.Unlock,
			playerInputLock = LockMode.Lock,
			cameraInputLock = LockMode.Lock
		});
	}

	// Token: 0x0600037A RID: 890 RVA: 0x00015EB3 File Offset: 0x000140B3
	public void HideMenu()
	{
		this.cheatsManagerMenu.SetActive(false);
	}

	// Token: 0x0600037B RID: 891 RVA: 0x00015EC1 File Offset: 0x000140C1
	public bool IsMenuOpen()
	{
		return this.cheatsManagerMenu.activeSelf;
	}

	// Token: 0x0600037C RID: 892 RVA: 0x00015ED0 File Offset: 0x000140D0
	public void RegisterCheat(ICheat cheat, string category = null)
	{
		if (string.IsNullOrEmpty(category))
		{
			category = "misc";
		}
		category = category.ToUpper();
		if (this.GetStartCheatState(cheat))
		{
			this.SetCheatActive(cheat, true, false);
		}
		List<ICheat> list;
		if (this.allRegisteredCheats.TryGetValue(category, out list))
		{
			list.Add(cheat);
			return;
		}
		this.allRegisteredCheats.Add(category, new List<ICheat> { cheat });
	}

	// Token: 0x0600037D RID: 893 RVA: 0x00015F38 File Offset: 0x00014138
	public void RegisterCheats(ICheat[] cheats, string category = null)
	{
		if (string.IsNullOrEmpty(category))
		{
			category = "misc";
		}
		category = category.ToUpper();
		foreach (ICheat cheat in cheats)
		{
			if (this.GetStartCheatState(cheat))
			{
				this.SetCheatActive(cheat, true, false);
			}
		}
		List<ICheat> list;
		if (this.allRegisteredCheats.TryGetValue(category, out list))
		{
			list.AddRange(cheats);
			return;
		}
		this.allRegisteredCheats.Add(category, cheats.ToList<ICheat>());
	}

	// Token: 0x0600037E RID: 894 RVA: 0x00015FAC File Offset: 0x000141AC
	public void RegisterExternalCheat(ICheat cheat)
	{
		this.RegisterCheat(cheat, "external");
	}

	// Token: 0x0600037F RID: 895 RVA: 0x00015FBC File Offset: 0x000141BC
	public void ToggleCheat(ICheat targetCheat)
	{
		if (!this.menuItems.ContainsKey(targetCheat))
		{
			return;
		}
		MonoSingleton<CheatsController>.Instance.PlayToggleSound(!targetCheat.IsActive);
		this.SetCheatActive(targetCheat, !targetCheat.IsActive, true);
		this.UpdateCheatState(this.menuItems[targetCheat], targetCheat);
	}

	// Token: 0x06000380 RID: 896 RVA: 0x00016010 File Offset: 0x00014210
	public void RefreshCheatStates()
	{
		foreach (ICheat cheat in this.menuItems.Keys)
		{
			this.UpdateCheatState(this.menuItems[cheat], cheat);
		}
	}

	// Token: 0x06000381 RID: 897 RVA: 0x00016074 File Offset: 0x00014274
	public void SetCheatActive(ICheat targetCheat, bool isActive, bool saveState = true)
	{
		if (targetCheat.IsActive)
		{
			if (!isActive)
			{
				targetCheat.Disable();
				if (saveState)
				{
					this.SaveCheatState(targetCheat);
					return;
				}
			}
		}
		else if (isActive)
		{
			targetCheat.Enable(this);
			base.StartCoroutine(targetCheat.Coroutine(this));
			if (saveState)
			{
				this.SaveCheatState(targetCheat);
			}
		}
	}

	// Token: 0x06000382 RID: 898 RVA: 0x000160B4 File Offset: 0x000142B4
	public bool GetCheatState(string id)
	{
		ICheat cheat;
		return this.idToCheat != null && this.idToCheat.TryGetValue(id, out cheat) && this.idToCheat.ContainsKey(id) && cheat.IsActive;
	}

	// Token: 0x06000383 RID: 899 RVA: 0x000160F1 File Offset: 0x000142F1
	public void DisableCheat(string id)
	{
		if (!this.idToCheat.ContainsKey(id))
		{
			return;
		}
		this.DisableCheat(this.idToCheat[id]);
	}

	// Token: 0x06000384 RID: 900 RVA: 0x00016114 File Offset: 0x00014314
	public void DisableCheat(ICheat targetCheat)
	{
		if (!this.menuItems.ContainsKey(targetCheat))
		{
			return;
		}
		if (!targetCheat.IsActive)
		{
			return;
		}
		MonoSingleton<CheatsController>.Instance.PlayToggleSound(false);
		this.SetCheatActive(targetCheat, false, true);
		this.UpdateCheatState(this.menuItems[targetCheat], targetCheat);
	}

	// Token: 0x06000385 RID: 901 RVA: 0x00016160 File Offset: 0x00014360
	public T GetCheatInstance<T>() where T : ICheat
	{
		return this.allRegisteredCheats.Values.SelectMany((List<ICheat> cheats) => cheats).OfType<T>().FirstOrDefault<T>();
	}

	// Token: 0x06000386 RID: 902 RVA: 0x0001619C File Offset: 0x0001439C
	public void RebuildIcons()
	{
		this.spriteIcons = new Dictionary<string, Sprite>();
		if (MonoSingleton<IconManager>.Instance == null || MonoSingleton<IconManager>.Instance.CurrentIcons == null)
		{
			return;
		}
		foreach (CheatAssetObject.KeyIcon keyIcon in MonoSingleton<IconManager>.Instance.CurrentIcons.cheatIcons)
		{
			this.spriteIcons.Add(keyIcon.key, keyIcon.sprite);
		}
	}

	// Token: 0x06000387 RID: 903 RVA: 0x00016214 File Offset: 0x00014414
	private void Start()
	{
		this.RebuildIcons();
		this.RegisterCheat(new KeepEnabled(), "meta");
		if (MapInfoBase.Instance != null && MapInfoBase.Instance.sandboxTools)
		{
			if (Debug.isDebugBuild)
			{
				this.RegisterCheat(new ExperimentalArmRotation(), "sandbox");
			}
			this.RegisterCheat(new QuickSave(), "sandbox");
			this.RegisterCheat(new QuickLoad(), "sandbox");
			this.RegisterCheat(new ManageSaves(), "sandbox");
			this.RegisterCheat(new ClearMap(), "sandbox");
			if (MonoSingleton<SandboxNavmesh>.Instance)
			{
				this.RegisterCheat(new RebuildNavmesh(), "sandbox");
			}
			this.RegisterCheats(new ICheat[]
			{
				new ULTRAKILL.Cheats.Snapping(),
				new SpawnPhysics()
			}, "sandbox");
		}
		this.RegisterCheats(new ICheat[]
		{
			new SummonSandboxArm(),
			new TeleportMenu(),
			new FullBright(),
			new Invincibility()
		}, "general");
		this.RegisterCheats(new ICheat[]
		{
			new Noclip(),
			new Flight(),
			new InfiniteWallJumps()
		}, "movement");
		this.RegisterCheats(new ICheat[]
		{
			new NoWeaponCooldown(),
			new InfinitePowerUps()
		}, "weapons");
		this.RegisterCheats(new ICheat[]
		{
			new BlindEnemies(),
			new EnemiesHateEnemies(),
			new EnemyIgnorePlayer(),
			new DisableEnemySpawns(),
			new InvincibleEnemies(),
			new KillAllEnemies()
		}, "enemies");
		this.RegisterCheats(new ICheat[]
		{
			new HideWeapons(),
			new HideUI()
		}, "visual");
		if (GameProgressSaver.GetClashModeUnlocked())
		{
			this.RegisterCheat(new CrashMode(), "special");
		}
		if (GameProgressSaver.GetGhostDroneModeUnlocked())
		{
			this.RegisterCheat(new GhostDroneMode(), "special");
		}
		if (Debug.isDebugBuild)
		{
			this.RegisterCheats(new ICheat[]
			{
				new NonConvexJumpDebug(),
				new HideCheatsStatus(),
				new PlayerParentingDebug(),
				new StateDebug(),
				new GunControlDebug(),
				new SandboxArmDebug(),
				new EnemyIdentifierDebug(),
				new ForceBossBars(),
				new SpreadGasoline()
			}, "debug");
			this.RegisterCheat(new PauseTimedBombs(), "debug");
		}
		MonoSingleton<CheatBinds>.Instance.RestoreBinds(this.allRegisteredCheats);
		this.RebuildMenu();
	}

	// Token: 0x06000388 RID: 904 RVA: 0x0001647C File Offset: 0x0001467C
	public void CancelRebindIfNecessary()
	{
		if (MonoSingleton<CheatBinds>.Instance.isRebinding)
		{
			MonoSingleton<CheatBinds>.Instance.CancelRebind();
		}
	}

	// Token: 0x06000389 RID: 905 RVA: 0x00016494 File Offset: 0x00014694
	protected override void OnDestroy()
	{
		base.OnDestroy();
		foreach (KeyValuePair<string, List<ICheat>> keyValuePair in this.allRegisteredCheats)
		{
			foreach (ICheat cheat in keyValuePair.Value)
			{
				if (cheat.IsActive)
				{
					try
					{
						cheat.Disable();
					}
					catch (Exception ex)
					{
						CheatsManager.Log.Error(ex.ToString(), null, null, null);
					}
				}
			}
		}
		this.allRegisteredCheats.Clear();
	}

	// Token: 0x0600038A RID: 906 RVA: 0x00016564 File Offset: 0x00014764
	public void HandleCheatBind(string identifier)
	{
		if (!MonoSingleton<CheatsController>.Instance.cheatsEnabled)
		{
			return;
		}
		if (MonoSingleton<CheatBinds>.Instance.isRebinding)
		{
			return;
		}
		if (SandboxHud.SavesMenuOpen)
		{
			return;
		}
		if (Console.IsOpen)
		{
			return;
		}
		if (GameStateManager.Instance.IsStateActive("alter-menu"))
		{
			return;
		}
		if (GameStateManager.Instance.IsStateActive("agony-menu"))
		{
			return;
		}
		MonoSingleton<CheatsController>.Instance.PlayToggleSound(!this.idToCheat[identifier].IsActive);
		this.ToggleCheat(this.idToCheat[identifier]);
		this.UpdateCheatState(this.idToCheat[identifier]);
	}

	// Token: 0x0600038B RID: 907 RVA: 0x00016604 File Offset: 0x00014804
	private void OnGUI()
	{
		if (MonoSingleton<CheatsController>.Instance == null)
		{
			return;
		}
		if (!MonoSingleton<CheatsController>.Instance.cheatsEnabled)
		{
			return;
		}
		foreach (KeyValuePair<string, List<ICheat>> keyValuePair in this.allRegisteredCheats)
		{
			foreach (ICheat cheat in keyValuePair.Value)
			{
				if (cheat.IsActive)
				{
					ICheatGUI cheatGUI = cheat as ICheatGUI;
					if (cheatGUI != null)
					{
						cheatGUI.OnGUI();
					}
				}
			}
		}
	}

	// Token: 0x0600038C RID: 908 RVA: 0x000166C4 File Offset: 0x000148C4
	private bool GetStartCheatState(ICheat cheat)
	{
		if (cheat.Identifier == "ultrakill.spawner-arm" && MapInfoBase.Instance && MapInfoBase.Instance.sandboxTools)
		{
			return true;
		}
		if (cheat.PersistenceMode == StatePersistenceMode.NotPersistent)
		{
			return cheat.DefaultState;
		}
		if (!CheatsManager.KeepCheatsEnabled)
		{
			MonoSingleton<PrefsManager>.Instance.DeleteKey("cheat." + cheat.Identifier);
			return cheat.DefaultState;
		}
		return MonoSingleton<PrefsManager>.Instance.GetBool("cheat." + cheat.Identifier, cheat.DefaultState);
	}

	// Token: 0x0600038D RID: 909 RVA: 0x00016754 File Offset: 0x00014954
	private void SaveCheatState(ICheat cheat)
	{
		if (cheat.PersistenceMode == StatePersistenceMode.NotPersistent)
		{
			return;
		}
		MonoSingleton<PrefsManager>.Instance.SetBool("cheat." + cheat.Identifier, cheat.IsActive);
	}

	// Token: 0x0600038E RID: 910 RVA: 0x00016780 File Offset: 0x00014980
	public void RenderCheatsInfo()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (MonoSingleton<SandboxNavmesh>.Instance && MonoSingleton<SandboxNavmesh>.Instance.isDirty)
		{
			stringBuilder.AppendLine("<color=red>NAVMESH OUT OF DATE<size=12>\n(Rebuild navigation in cheats menu)</size>\n</color>");
		}
		if (this.GetCheatState("ultrakill.spawner-arm"))
		{
			stringBuilder.AppendLine("<color=#C2D7FF>Spawner Arm in slot 6\n</color>");
		}
		foreach (KeyValuePair<string, List<ICheat>> keyValuePair in this.allRegisteredCheats)
		{
			foreach (ICheat cheat in keyValuePair.Value)
			{
				if (cheat.IsActive)
				{
					string text = MonoSingleton<CheatBinds>.Instance.ResolveCheatKey(cheat.Identifier);
					if (!string.IsNullOrEmpty(text))
					{
						stringBuilder.Append("[<color=orange>" + text.ToUpper() + "</color>] ");
					}
					else
					{
						stringBuilder.Append("[ ] ");
					}
					stringBuilder.Append("<color=white>" + cheat.LongName + "</color>\n");
				}
			}
		}
		MonoSingleton<CheatsController>.Instance.cheatsInfo.text = stringBuilder.ToString();
	}

	// Token: 0x0600038F RID: 911 RVA: 0x000168D4 File Offset: 0x00014AD4
	public void UpdateCheatState(ICheat cheat)
	{
		if (!this.menuItems.ContainsKey(cheat))
		{
			return;
		}
		this.UpdateCheatState(this.menuItems[cheat], cheat);
	}

	// Token: 0x06000390 RID: 912 RVA: 0x000168F8 File Offset: 0x00014AF8
	private void UpdateCheatState(CheatMenuItem item, ICheat cheat)
	{
		item.longName.text = cheat.LongName;
		item.stateBackground.color = (cheat.IsActive ? this.enabledColor : this.disabledColor);
		item.stateText.text = (cheat.IsActive ? (cheat.ButtonEnabledOverride ?? "ENABLED") : (cheat.ButtonDisabledOverride ?? "DISABLED"));
		item.bindButtonBack.gameObject.SetActive(false);
		string text = MonoSingleton<CheatBinds>.Instance.ResolveCheatKey(cheat.Identifier);
		if (string.IsNullOrEmpty(text))
		{
			item.bindButtonText.text = "Press to Bind";
		}
		else
		{
			item.bindButtonText.text = text.ToUpper();
		}
		this.RenderCheatsInfo();
	}

	// Token: 0x06000391 RID: 913 RVA: 0x000169C0 File Offset: 0x00014BC0
	private void ResetMenu()
	{
		this.menuItems = new Dictionary<ICheat, CheatMenuItem>();
		this.idToCheat = new Dictionary<string, ICheat>();
		for (int i = 2; i < this.itemContainer.transform.childCount; i++)
		{
			Object.Destroy(this.itemContainer.transform.GetChild(i).gameObject);
		}
	}

	// Token: 0x06000392 RID: 914 RVA: 0x00016A1C File Offset: 0x00014C1C
	private void StartRebind(ICheat cheat)
	{
		if (MonoSingleton<CheatBinds>.Instance.isRebinding)
		{
			MonoSingleton<CheatBinds>.Instance.CancelRebind();
			return;
		}
		this.menuItems[cheat].bindButtonBack.gameObject.SetActive(true);
		this.menuItems[cheat].bindButtonText.text = "Press any key";
		MonoSingleton<CheatBinds>.Instance.SetupRebind(cheat);
	}

	// Token: 0x06000393 RID: 915 RVA: 0x00016A84 File Offset: 0x00014C84
	public void RebuildMenu()
	{
		this.ResetMenu();
		this.template.gameObject.SetActive(false);
		this.categoryTemplate.gameObject.SetActive(false);
		foreach (KeyValuePair<string, List<ICheat>> keyValuePair in this.allRegisteredCheats)
		{
			CheatMenuItem cheatMenuItem = Object.Instantiate<CheatMenuItem>(this.categoryTemplate, this.itemContainer.transform, false);
			cheatMenuItem.gameObject.SetActive(true);
			cheatMenuItem.longName.text = keyValuePair.Key;
			using (List<ICheat>.Enumerator enumerator2 = keyValuePair.Value.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					CheatsManager.<>c__DisplayClass39_0 CS$<>8__locals1 = new CheatsManager.<>c__DisplayClass39_0();
					CS$<>8__locals1.<>4__this = this;
					CS$<>8__locals1.cheat = enumerator2.Current;
					CheatMenuItem item = Object.Instantiate<CheatMenuItem>(this.template, this.itemContainer.transform, false);
					item.gameObject.SetActive(true);
					if (!string.IsNullOrEmpty(CS$<>8__locals1.cheat.Icon))
					{
						Sprite sprite;
						if (this.spriteIcons.TryGetValue(CS$<>8__locals1.cheat.Icon, out sprite))
						{
							item.iconTarget.sprite = sprite;
						}
						else if (MonoSingleton<IconManager>.Instance && MonoSingleton<IconManager>.Instance.CurrentIcons)
						{
							item.iconTarget.sprite = MonoSingleton<IconManager>.Instance.CurrentIcons.genericCheatIcon;
						}
					}
					item.stateButton.onClick.AddListener(delegate
					{
						CS$<>8__locals1.<>4__this.ToggleCheat(CS$<>8__locals1.cheat);
					});
					item.bindButton.onClick.AddListener(delegate
					{
						CS$<>8__locals1.<>4__this.StartRebind(CS$<>8__locals1.cheat);
					});
					item.resetBindButton.onClick.AddListener(delegate
					{
						MonoSingleton<CheatBinds>.Instance.ResetCheatBind(CS$<>8__locals1.cheat.Identifier);
						CS$<>8__locals1.<>4__this.UpdateCheatState(item, CS$<>8__locals1.cheat);
					});
					this.UpdateCheatState(item, CS$<>8__locals1.cheat);
					this.menuItems[CS$<>8__locals1.cheat] = item;
					this.idToCheat[CS$<>8__locals1.cheat.Identifier] = CS$<>8__locals1.cheat;
				}
			}
		}
	}

	// Token: 0x0400046C RID: 1132
	private static readonly global::plog.Logger Log = new global::plog.Logger("CheatsManager");

	// Token: 0x0400046D RID: 1133
	[SerializeField]
	private GameObject cheatsManagerMenu;

	// Token: 0x0400046E RID: 1134
	[Space]
	[SerializeField]
	private GameObject itemContainer;

	// Token: 0x0400046F RID: 1135
	[SerializeField]
	private CheatMenuItem template;

	// Token: 0x04000470 RID: 1136
	[Space]
	[SerializeField]
	private CheatMenuItem categoryTemplate;

	// Token: 0x04000471 RID: 1137
	[Space]
	[SerializeField]
	private Color enabledColor = Color.green;

	// Token: 0x04000472 RID: 1138
	[SerializeField]
	private Color disabledColor = Color.red;

	// Token: 0x04000473 RID: 1139
	private Dictionary<ICheat, CheatMenuItem> menuItems;

	// Token: 0x04000474 RID: 1140
	private Dictionary<string, ICheat> idToCheat;

	// Token: 0x04000475 RID: 1141
	private readonly Dictionary<string, List<ICheat>> allRegisteredCheats = new Dictionary<string, List<ICheat>>();

	// Token: 0x04000476 RID: 1142
	private Dictionary<string, Sprite> spriteIcons;
}
