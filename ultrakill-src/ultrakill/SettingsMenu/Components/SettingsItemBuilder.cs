using System;
using SettingsMenu.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SettingsMenu.Components
{
	// Token: 0x02000542 RID: 1346
	public class SettingsItemBuilder : MonoBehaviour, ISettingsGroupUser
	{
		// Token: 0x06001E5D RID: 7773 RVA: 0x000FB009 File Offset: 0x000F9209
		private void Awake()
		{
			this.rectTransform = base.GetComponent<RectTransform>();
			this.image = base.GetComponent<Image>();
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x000FB024 File Offset: 0x000F9224
		public void ConfigureFrom(SettingsItem item, SettingsCategory category, SettingsPageBuilder pageBuilder)
		{
			if (item == null)
			{
				return;
			}
			this.asset = item;
			SettingsMenuAssets assets = pageBuilder.assets;
			if (this.label == null)
			{
				return;
			}
			this.label.text = item.GetLabel(true);
			SettingsRestoreDefaultButton settingsRestoreDefaultButton = null;
			if (item.itemType != SettingsItemType.Button && !item.noResetButton)
			{
				settingsRestoreDefaultButton = Object.Instantiate<SettingsRestoreDefaultButton>((item.style == SettingsItemStyle.Thin) ? assets.smallResetButtonPrefab : assets.resetButtonPrefab, base.transform);
				float valueDisplayMultiplayer = item.valueDisplayMultiplayer;
				settingsRestoreDefaultButton.valueToPrefMultiplier = ((valueDisplayMultiplayer == 0f) ? 1f : (1f / valueDisplayMultiplayer));
			}
			SettingsBuilderBase settingsBuilderBase = Object.Instantiate<SettingsBuilderBase>(assets.GetBuilderFor(item.itemType), base.transform);
			settingsBuilderBase.ConfigureFrom(this, pageBuilder);
			pageBuilder.AddBuilderInstance(settingsBuilderBase, item);
			Selectable selectable;
			if (settingsBuilderBase.TryGetComponent<Selectable>(out selectable))
			{
				pageBuilder.AddSelectableRow(selectable);
				if (pageBuilder.navigationButtonSelectable != null)
				{
					selectable.gameObject.AddComponent<BackSelectOverride>().Selectable = pageBuilder.navigationButtonSelectable;
				}
				if (settingsRestoreDefaultButton != null)
				{
					settingsRestoreDefaultButton.SetNavigation(selectable);
				}
			}
			if (settingsRestoreDefaultButton != null)
			{
				settingsBuilderBase.AttachRestoreDefaultButton(settingsRestoreDefaultButton);
			}
			if (item.preferenceKey.IsValid() && settingsRestoreDefaultButton != null)
			{
				settingsRestoreDefaultButton.settingKey = item.preferenceKey.key;
			}
			if (this.sideNote != null)
			{
				if (string.IsNullOrEmpty(item.sideNote))
				{
					Object.Destroy(this.sideNote.gameObject);
				}
				else
				{
					this.sideNote.text = item.sideNote;
				}
			}
			SettingsGroup settingsGroup = item.group ?? category.group;
			if (settingsGroup != null)
			{
				SettingsGroupTogglingMode togglingMode = settingsGroup.togglingMode;
				if (togglingMode != SettingsGroupTogglingMode.GrayedOut)
				{
					if (togglingMode == SettingsGroupTogglingMode.Hidden)
					{
						Object.Destroy(this.blocker);
					}
				}
				else
				{
					this.blocker.SetActive(true);
					this.blocker.transform.SetAsLastSibling();
					this.canvasGroup = base.gameObject.AddComponent<CanvasGroup>();
				}
				pageBuilder.AddToGroup(settingsGroup, this);
			}
			else
			{
				Object.Destroy(this.blocker);
			}
			this.ApplyStyle(item.style);
		}

		// Token: 0x06001E5F RID: 7775 RVA: 0x000FB234 File Offset: 0x000F9434
		private void ApplyStyle(SettingsItemStyle style)
		{
			if (style == SettingsItemStyle.Thin)
			{
				this.rectTransform.sizeDelta = new Vector2(this.rectTransform.sizeDelta.x, this.rectTransform.sizeDelta.y * 0.65f);
				Color color = this.image.color;
				color.a *= 0.65f;
				this.image.color = color;
			}
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x000FB2A4 File Offset: 0x000F94A4
		private void ResizeBuilderToFitSideNote(RectTransform builderRectTransform)
		{
			if (this.sideNote == null || builderRectTransform == null)
			{
				return;
			}
			float width = builderRectTransform.rect.width;
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.sideNote.rectTransform);
			float width2 = this.sideNote.rectTransform.rect.width;
			Vector2 sizeDelta = builderRectTransform.sizeDelta;
			sizeDelta.x = width - width2;
			builderRectTransform.sizeDelta = sizeDelta;
			Vector2 anchoredPosition = builderRectTransform.anchoredPosition;
			anchoredPosition.x -= width2;
			builderRectTransform.anchoredPosition = anchoredPosition;
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x000FB338 File Offset: 0x000F9538
		public void ValueChanged<T>(T value)
		{
			if (this.asset == null)
			{
				return;
			}
			switch (this.asset.valueType)
			{
			case ValueType.Int:
			{
				int num = Convert.ToInt32(value);
				if (value is float && this.asset.preferenceKey.IsValid())
				{
					this.asset.preferenceKey.SetValue<int>(num);
					return;
				}
				break;
			}
			case ValueType.Float:
				value = (T)((object)Convert.ChangeType(Convert.ToSingle(value) / this.asset.valueDisplayMultiplayer, typeof(T)));
				break;
			case ValueType.BoolCombination:
			{
				int num2 = Convert.ToInt32(value);
				if (this.asset.combinationOptions.Count <= num2)
				{
					throw new Exception("Dropdown value out of range");
				}
				foreach (DropdownCombinationRestoreDefaultButton.BooleanPrefOption booleanPrefOption in this.asset.combinationOptions[num2].subOptions)
				{
					Debug.Log(booleanPrefOption.preferenceKey.key + " " + booleanPrefOption.expectedValue.ToString());
					booleanPrefOption.preferenceKey.SetValue<bool>(booleanPrefOption.expectedValue);
				}
				return;
			}
			}
			if (this.asset.preferenceKey.IsValid())
			{
				this.asset.preferenceKey.SetValue<T>(value);
			}
		}

		// Token: 0x06001E62 RID: 7778 RVA: 0x000FB4D0 File Offset: 0x000F96D0
		public void UpdateGroupStatus(bool groupEnabled, SettingsGroupTogglingMode togglingMode)
		{
			if (togglingMode != SettingsGroupTogglingMode.GrayedOut)
			{
				if (togglingMode != SettingsGroupTogglingMode.Hidden)
				{
					return;
				}
				base.gameObject.SetActive(groupEnabled);
			}
			else
			{
				if (this.blocker != null)
				{
					this.blocker.SetActive(!groupEnabled);
				}
				if (this.canvasGroup != null)
				{
					this.canvasGroup.interactable = groupEnabled;
					return;
				}
			}
		}

		// Token: 0x04002AE8 RID: 10984
		[NonSerialized]
		public SettingsItem asset;

		// Token: 0x04002AE9 RID: 10985
		[SerializeField]
		private TMP_Text label;

		// Token: 0x04002AEA RID: 10986
		[SerializeField]
		private TMP_Text sideNote;

		// Token: 0x04002AEB RID: 10987
		[SerializeField]
		private GameObject blocker;

		// Token: 0x04002AEC RID: 10988
		private RectTransform rectTransform;

		// Token: 0x04002AED RID: 10989
		private Image image;

		// Token: 0x04002AEE RID: 10990
		private CanvasGroup canvasGroup;
	}
}
