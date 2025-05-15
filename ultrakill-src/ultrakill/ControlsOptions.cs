using System;
using System.Collections.Generic;
using System.Linq;
using SettingsMenu.Components;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Token: 0x020000DF RID: 223
public class ControlsOptions : MonoBehaviour
{
	// Token: 0x0600045C RID: 1116 RVA: 0x0001E5E9 File Offset: 0x0001C7E9
	public void ShowModal()
	{
		this.modalBackground.SetActive(true);
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x0001E5F7 File Offset: 0x0001C7F7
	public void HideModal()
	{
		this.modalBackground.SetActive(false);
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x0001E608 File Offset: 0x0001C808
	private void Awake()
	{
		this.inman = MonoSingleton<InputManager>.Instance;
		this.opm = MonoSingleton<OptionsManager>.Instance;
		this.idConfigDict = this.actionConfig.ToDictionary((ActionDisplayConfig config) => config.actionRef.action.id);
	}

	// Token: 0x0600045F RID: 1119 RVA: 0x0001E65C File Offset: 0x0001C85C
	private void OnEnable()
	{
		this.Rebuild(MonoSingleton<InputManager>.Instance.InputSource.Actions.KeyboardMouseScheme);
		InputManager instance = MonoSingleton<InputManager>.Instance;
		instance.actionModified = (Action<InputAction>)Delegate.Combine(instance.actionModified, new Action<InputAction>(this.OnActionChanged));
	}

	// Token: 0x06000460 RID: 1120 RVA: 0x0001E6AC File Offset: 0x0001C8AC
	private void OnDisable()
	{
		if (this.currentKey != null)
		{
			if (this.opm == null)
			{
				this.opm = MonoSingleton<OptionsManager>.Instance;
			}
			this.currentKey.GetComponent<Image>().color = this.normalColor;
			this.currentKey = null;
			if (this.opm)
			{
				this.opm.dontUnpause = false;
			}
		}
		if (MonoSingleton<InputManager>.Instance)
		{
			InputManager instance = MonoSingleton<InputManager>.Instance;
			instance.actionModified = (Action<InputAction>)Delegate.Remove(instance.actionModified, new Action<InputAction>(this.OnActionChanged));
		}
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x0001E748 File Offset: 0x0001C948
	public void OnActionChanged(InputAction action)
	{
		this.Rebuild(MonoSingleton<InputManager>.Instance.InputSource.Actions.KeyboardMouseScheme);
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x0001E764 File Offset: 0x0001C964
	public void ResetToDefault()
	{
		this.inman.ResetToDefault();
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x0001E774 File Offset: 0x0001C974
	private void Rebuild(InputControlScheme controlScheme)
	{
		MonoSingleton<InputManager>.Instance.InputSource.ValidateBindings(MonoSingleton<InputManager>.Instance.InputSource.Actions.KeyboardMouseScheme);
		foreach (GameObject gameObject in this.rebindUIObjects)
		{
			Object.Destroy(gameObject);
		}
		this.rebindUIObjects.Clear();
		InputActionMap[] array = new InputActionMap[]
		{
			this.inman.InputSource.Actions.Movement,
			this.inman.InputSource.Actions.Weapon,
			this.inman.InputSource.Actions.Fist,
			this.inman.InputSource.Actions.HUD
		};
		Debug.Log("Rebuilding");
		Selectable selectable = this.settingsPageBuilder.GetLastSelectable();
		Selectable firstSelectable = this.settingsPageBuilder.GetFirstSelectable();
		foreach (InputActionMap inputActionMap in array)
		{
			GameObject gameObject2 = Object.Instantiate<GameObject>(this.sectionTemplate, this.actionParent);
			gameObject2.GetComponent<TextMeshProUGUI>().text = "-- " + inputActionMap.name.ToUpper() + " --";
			gameObject2.SetActive(true);
			this.rebindUIObjects.Add(gameObject2);
			foreach (InputAction inputAction in inputActionMap)
			{
				if (!(inputAction.expectedControlType != "Button") || !(inputAction.expectedControlType != "Vector2"))
				{
					Debug.Log("Building " + inputAction.name);
					if (inputAction != this.inman.InputSource.Look.Action && inputAction != this.inman.InputSource.WheelLook.Action)
					{
						bool flag = true;
						ActionDisplayConfig actionDisplayConfig;
						if (this.idConfigDict.TryGetValue(inputAction.id, out actionDisplayConfig))
						{
							if (actionDisplayConfig.hidden)
							{
								continue;
							}
							if (!string.IsNullOrEmpty(actionDisplayConfig.requiredWeapon) && GameProgressSaver.CheckGear(actionDisplayConfig.requiredWeapon) == 0)
							{
								flag = false;
							}
						}
						GameObject gameObject3 = Object.Instantiate<GameObject>(this.actionTemplate, this.actionParent);
						ControlsOptionsKey component = gameObject3.GetComponent<ControlsOptionsKey>();
						if (selectable != null)
						{
							Navigation navigation = selectable.navigation;
							navigation.mode = Navigation.Mode.Explicit;
							navigation.selectOnDown = component.selectable;
							selectable.navigation = navigation;
							Navigation navigation2 = component.selectable.navigation;
							navigation2.mode = Navigation.Mode.Explicit;
							navigation2.selectOnUp = selectable;
							component.selectable.navigation = navigation2;
						}
						component.actionText.text = (flag ? inputAction.name.ToUpper() : "???");
						component.RebuildBindings(inputAction, controlScheme);
						component.selectable.gameObject.AddComponent<ControllerDisallowedSelection>().fallbackSelectable = firstSelectable;
						Debug.Log("Rebuilt", gameObject3);
						this.rebindUIObjects.Add(gameObject3);
						gameObject3.SetActive(true);
						selectable = component.selectable;
					}
				}
			}
		}
		if (this.tooltipManager != null)
		{
			TooltipOnHover[] componentsInChildren = this.actionParent.GetComponentsInChildren<TooltipOnHover>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].tooltipManager = this.tooltipManager;
			}
		}
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x0001EB30 File Offset: 0x0001CD30
	private void LateUpdate()
	{
		if (this.canUnpause)
		{
			if (this.opm == null)
			{
				this.opm = MonoSingleton<OptionsManager>.Instance;
			}
			this.canUnpause = false;
			this.opm.dontUnpause = false;
		}
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x0001EB68 File Offset: 0x0001CD68
	public void ScrollOn(bool stuff)
	{
		if (this.inman == null)
		{
			this.inman = MonoSingleton<InputManager>.Instance;
		}
		if (stuff)
		{
			MonoSingleton<PrefsManager>.Instance.SetBool("scrollEnabled", true);
			this.inman.ScrOn = true;
			return;
		}
		MonoSingleton<PrefsManager>.Instance.SetBool("scrollEnabled", false);
		this.inman.ScrOn = false;
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x0001EBCC File Offset: 0x0001CDCC
	public void ScrollVariations(int stuff)
	{
		if (this.inman == null)
		{
			this.inman = MonoSingleton<InputManager>.Instance;
		}
		if (stuff == 0)
		{
			MonoSingleton<PrefsManager>.Instance.SetBool("scrollWeapons", true);
			MonoSingleton<PrefsManager>.Instance.SetBool("scrollVariations", false);
			this.inman.ScrWep = true;
			this.inman.ScrVar = false;
			return;
		}
		if (stuff == 1)
		{
			MonoSingleton<PrefsManager>.Instance.SetBool("scrollWeapons", false);
			MonoSingleton<PrefsManager>.Instance.SetBool("scrollVariations", true);
			this.inman.ScrWep = false;
			this.inman.ScrVar = true;
			return;
		}
		MonoSingleton<PrefsManager>.Instance.SetBool("scrollWeapons", true);
		MonoSingleton<PrefsManager>.Instance.SetBool("scrollVariations", true);
		this.inman.ScrWep = true;
		this.inman.ScrVar = true;
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x0001ECA4 File Offset: 0x0001CEA4
	public void ScrollReverse(bool stuff)
	{
		if (this.inman == null)
		{
			this.inman = MonoSingleton<InputManager>.Instance;
		}
		if (stuff)
		{
			MonoSingleton<PrefsManager>.Instance.SetBool("scrollReversed", true);
			this.inman.ScrRev = true;
			return;
		}
		MonoSingleton<PrefsManager>.Instance.SetBool("scrollReversed", false);
		this.inman.ScrRev = false;
	}

	// Token: 0x040005FC RID: 1532
	private InputManager inman;

	// Token: 0x040005FD RID: 1533
	[HideInInspector]
	public OptionsManager opm;

	// Token: 0x040005FE RID: 1534
	public List<ActionDisplayConfig> actionConfig;

	// Token: 0x040005FF RID: 1535
	private Dictionary<Guid, ActionDisplayConfig> idConfigDict;

	// Token: 0x04000600 RID: 1536
	public Transform actionParent;

	// Token: 0x04000601 RID: 1537
	public GameObject actionTemplate;

	// Token: 0x04000602 RID: 1538
	public GameObject sectionTemplate;

	// Token: 0x04000603 RID: 1539
	private GameObject currentKey;

	// Token: 0x04000604 RID: 1540
	public Color normalColor;

	// Token: 0x04000605 RID: 1541
	public Color pressedColor;

	// Token: 0x04000606 RID: 1542
	private bool canUnpause;

	// Token: 0x04000607 RID: 1543
	public SettingsPageBuilder settingsPageBuilder;

	// Token: 0x04000608 RID: 1544
	public TooltipManager tooltipManager;

	// Token: 0x04000609 RID: 1545
	private List<GameObject> rebindUIObjects = new List<GameObject>();

	// Token: 0x0400060A RID: 1546
	public GameObject modalBackground;
}
