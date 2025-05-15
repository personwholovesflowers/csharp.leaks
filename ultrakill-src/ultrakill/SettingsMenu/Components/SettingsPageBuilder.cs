using System;
using System.Collections.Generic;
using SettingsMenu.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SettingsMenu.Components
{
	// Token: 0x02000545 RID: 1349
	[DefaultExecutionOrder(-100)]
	public class SettingsPageBuilder : MonoBehaviour
	{
		// Token: 0x06001E6F RID: 7791 RVA: 0x000FB6AC File Offset: 0x000F98AC
		private void Awake()
		{
			this.gamepadObjectSelector = base.GetComponent<GamepadObjectSelector>();
			if (this.page == null)
			{
				return;
			}
			this.BuildPage(this.page);
		}

		// Token: 0x06001E70 RID: 7792 RVA: 0x000FB6D5 File Offset: 0x000F98D5
		private void Start()
		{
			PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
		}

		// Token: 0x06001E71 RID: 7793 RVA: 0x000FB6F7 File Offset: 0x000F98F7
		private void OnDestroy()
		{
			PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x000FB71C File Offset: 0x000F991C
		private void OnPrefChanged(string key, object value)
		{
			foreach (KeyValuePair<SettingsGroup, List<ISettingsGroupUser>> keyValuePair in this.groups)
			{
				if (keyValuePair.Key.preferenceKey.key == key)
				{
					bool enabled = keyValuePair.Key.GetEnabled();
					this.UpdateGroupUsers(keyValuePair.Key, enabled);
				}
			}
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x000FB79C File Offset: 0x000F999C
		private void OnValidate()
		{
			if (this.page == null)
			{
				return;
			}
			if (this.buttonEvents == null)
			{
				this.buttonEvents = new List<SettingsButtonEvent>();
			}
			List<SettingsItem> buttonItems = new List<SettingsItem>();
			SettingsCategory[] categories = this.page.categories;
			for (int i = 0; i < categories.Length; i++)
			{
				foreach (SettingsItem settingsItem in categories[i].items)
				{
					if (settingsItem.itemType == SettingsItemType.Button)
					{
						buttonItems.Add(settingsItem);
					}
				}
			}
			this.buttonEvents.RemoveAll((SettingsButtonEvent x) => !buttonItems.Contains(x.buttonItem));
			using (List<SettingsItem>.Enumerator enumerator = buttonItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SettingsItem item = enumerator.Current;
					if (this.buttonEvents.Find((SettingsButtonEvent x) => x.buttonItem == item).buttonItem == null)
					{
						this.buttonEvents.Add(new SettingsButtonEvent
						{
							buttonItem = item,
							onClickEvent = new UnityEvent()
						});
					}
				}
			}
		}

		// Token: 0x06001E74 RID: 7796 RVA: 0x000FB904 File Offset: 0x000F9B04
		private void BuildPage(SettingsPage settingsPage)
		{
			if (this.targetContainer == null)
			{
				return;
			}
			this.createdInstances = new Dictionary<SettingsItem, SettingsBuilderBase>();
			this.groups = new Dictionary<SettingsGroup, List<ISettingsGroupUser>>();
			this.selectableRows = new List<Selectable>();
			foreach (object obj in this.targetContainer)
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
			foreach (SettingsCategory settingsCategory in settingsPage.categories)
			{
				SettingsCategoryBuilder settingsCategoryBuilder = Object.Instantiate<SettingsCategoryBuilder>(this.assets.categoryTitlePrefab, this.targetContainer);
				string label = settingsCategory.GetLabel(false);
				settingsCategoryBuilder.gameObject.name = label;
				settingsCategoryBuilder.ConfigureFrom(settingsCategory, this);
				foreach (SettingsItem settingsItem in settingsCategory.items)
				{
					if (settingsItem.platformRequirements == null || settingsItem.platformRequirements.Check())
					{
						SettingsItemBuilder settingsItemBuilder = Object.Instantiate<SettingsItemBuilder>(this.assets.itemPrefab, this.targetContainer);
						settingsItemBuilder.ConfigureFrom(settingsItem, settingsCategory, this);
						settingsItemBuilder.name = settingsItem.GetLabel(false);
					}
				}
			}
			foreach (KeyValuePair<SettingsGroup, List<ISettingsGroupUser>> keyValuePair in this.groups)
			{
				bool enabled = keyValuePair.Key.GetEnabled();
				foreach (ISettingsGroupUser settingsGroupUser in keyValuePair.Value)
				{
					settingsGroupUser.UpdateGroupStatus(enabled, keyValuePair.Key.togglingMode);
				}
			}
			this.RefreshSelectableNavigation();
			this.pageBuilt = true;
			if (this.selectAfterBuild)
			{
				this.SetSelected();
				this.selectAfterBuild = false;
			}
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x000FBB24 File Offset: 0x000F9D24
		public void SetSelected()
		{
			if (!this.pageBuilt)
			{
				this.selectAfterBuild = true;
				return;
			}
			if (!this.gamepadObjectSelector)
			{
				return;
			}
			this.gamepadObjectSelector.Activate();
			this.gamepadObjectSelector.SetTop();
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x000FBB5C File Offset: 0x000F9D5C
		public void RefreshSelectableNavigation()
		{
			List<Selectable> list = new List<Selectable>(this.selectableRows);
			list.InsertRange(0, this.customTopSelectables);
			list.AddRange(this.customBottomSelectables);
			if (list.Count == 0)
			{
				return;
			}
			Selectable selectable = null;
			Selectable selectable2 = null;
			foreach (Selectable selectable3 in list)
			{
				if (!(selectable3 == null) && selectable3.gameObject.activeInHierarchy && selectable3.IsInteractable())
				{
					if (selectable == null)
					{
						selectable = selectable3;
					}
					if (selectable2 != null)
					{
						Navigation navigation = selectable2.navigation;
						navigation.selectOnDown = selectable3;
						selectable2.navigation = navigation;
						navigation.mode = Navigation.Mode.Explicit;
						Navigation navigation2 = selectable3.navigation;
						navigation2.mode = Navigation.Mode.Explicit;
						navigation2.selectOnUp = selectable2;
						selectable3.navigation = navigation2;
					}
					selectable2 = selectable3;
				}
			}
			if (selectable != null && selectable2 != null)
			{
				Navigation navigation3 = selectable.navigation;
				navigation3.selectOnUp = selectable2;
				selectable.navigation = navigation3;
				Navigation navigation4 = selectable2.navigation;
				navigation4.selectOnDown = selectable;
				selectable2.navigation = navigation4;
			}
			if (this.gamepadObjectSelector != null && selectable != null)
			{
				this.gamepadObjectSelector.SetMainTarget(selectable);
			}
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x000FBCC0 File Offset: 0x000F9EC0
		public void AddBuilderInstance(SettingsBuilderBase builder, SettingsItem item)
		{
			if (this.createdInstances == null)
			{
				this.createdInstances = new Dictionary<SettingsItem, SettingsBuilderBase>();
			}
			this.createdInstances[item] = builder;
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x000FBCE4 File Offset: 0x000F9EE4
		public void AddToGroup(SettingsGroup group, ISettingsGroupUser builder)
		{
			if (this.groups == null)
			{
				this.groups = new Dictionary<SettingsGroup, List<ISettingsGroupUser>>();
			}
			if (!this.groups.ContainsKey(group))
			{
				this.groups[group] = new List<ISettingsGroupUser>();
			}
			this.groups[group].Add(builder);
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x000FBD35 File Offset: 0x000F9F35
		public void AddSelectableRow(Selectable selectable)
		{
			if (this.selectableRows == null)
			{
				this.selectableRows = new List<Selectable>();
			}
			this.selectableRows.Add(selectable);
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x000FBD58 File Offset: 0x000F9F58
		public Selectable GetFirstSelectable()
		{
			if (this.selectableRows == null)
			{
				return null;
			}
			foreach (Selectable selectable in this.selectableRows)
			{
				if (!(selectable == null) && selectable.gameObject.activeInHierarchy && selectable.IsInteractable())
				{
					return selectable;
				}
			}
			return null;
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x000FBDD4 File Offset: 0x000F9FD4
		public Selectable GetLastSelectable()
		{
			if (this.selectableRows == null)
			{
				return null;
			}
			for (int i = this.selectableRows.Count - 1; i >= 0; i--)
			{
				Selectable selectable = this.selectableRows[i];
				if (!(selectable == null) && selectable.gameObject.activeInHierarchy && selectable.IsInteractable())
				{
					return selectable;
				}
			}
			return null;
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x000FBE31 File Offset: 0x000FA031
		public void ConfirmGroupEnabled(SettingsGroup group)
		{
			this.SetGroupEnabled(group, true, true);
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x000FBE3C File Offset: 0x000FA03C
		public void SetGroupEnabled(SettingsGroup group, bool groupEnabled, bool noInterrupts = false)
		{
			List<SettingsGroupInterrupt> list = this.groupInterrupts;
			if (list != null && list.Count > 0 && groupEnabled && !noInterrupts)
			{
				foreach (SettingsGroupInterrupt settingsGroupInterrupt in this.groupInterrupts)
				{
					if (!(settingsGroupInterrupt.group != group))
					{
						settingsGroupInterrupt.onEnableEvent.Invoke();
						if (settingsGroupInterrupt.suppressDefaultEnable)
						{
							return;
						}
					}
				}
			}
			group.SetEnabledBool(groupEnabled);
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x000FBED0 File Offset: 0x000FA0D0
		private void UpdateGroupUsers(SettingsGroup group, bool groupEnabled)
		{
			if (this.groups == null)
			{
				return;
			}
			List<ISettingsGroupUser> list;
			if (!this.groups.TryGetValue(group, out list))
			{
				return;
			}
			foreach (ISettingsGroupUser settingsGroupUser in list)
			{
				settingsGroupUser.UpdateGroupStatus(groupEnabled, group.togglingMode);
			}
			this.RefreshSelectableNavigation();
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x000FBF44 File Offset: 0x000FA144
		public bool TryGetItemBuilderInstance<T>(SettingsItem item, out T builder) where T : SettingsBuilderBase
		{
			builder = default(T);
			if (this.createdInstances == null)
			{
				return false;
			}
			SettingsBuilderBase settingsBuilderBase;
			if (this.createdInstances.TryGetValue(item, out settingsBuilderBase))
			{
				builder = settingsBuilderBase as T;
				return builder != null;
			}
			return false;
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x000FBF98 File Offset: 0x000FA198
		public void SetSelectedItem(SettingsItem item)
		{
			if (this.createdInstances == null)
			{
				return;
			}
			SettingsBuilderBase settingsBuilderBase;
			if (!this.createdInstances.TryGetValue(item, out settingsBuilderBase))
			{
				return;
			}
			settingsBuilderBase.SetSelected();
		}

		// Token: 0x04002AF6 RID: 10998
		[SerializeField]
		public SettingsMenuAssets assets;

		// Token: 0x04002AF7 RID: 10999
		[Space]
		[SerializeField]
		private SettingsPage page;

		// Token: 0x04002AF8 RID: 11000
		[SerializeField]
		private Transform targetContainer;

		// Token: 0x04002AF9 RID: 11001
		public Selectable navigationButtonSelectable;

		// Token: 0x04002AFA RID: 11002
		public Selectable[] customTopSelectables;

		// Token: 0x04002AFB RID: 11003
		public Selectable[] customBottomSelectables;

		// Token: 0x04002AFC RID: 11004
		[Space]
		[FormerlySerializedAs("buttonCallbacks")]
		public List<SettingsButtonEvent> buttonEvents;

		// Token: 0x04002AFD RID: 11005
		public List<SettingsGroupInterrupt> groupInterrupts;

		// Token: 0x04002AFE RID: 11006
		private bool pageBuilt;

		// Token: 0x04002AFF RID: 11007
		private bool selectAfterBuild;

		// Token: 0x04002B00 RID: 11008
		private GamepadObjectSelector gamepadObjectSelector;

		// Token: 0x04002B01 RID: 11009
		private Dictionary<SettingsItem, SettingsBuilderBase> createdInstances;

		// Token: 0x04002B02 RID: 11010
		private Dictionary<SettingsGroup, List<ISettingsGroupUser>> groups;

		// Token: 0x04002B03 RID: 11011
		private List<Selectable> selectableRows;
	}
}
