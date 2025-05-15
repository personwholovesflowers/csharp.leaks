using System;
using SettingsMenu.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SettingsMenu.Components
{
	// Token: 0x0200053D RID: 1341
	public class SettingsSlider : SettingsBuilderBase
	{
		// Token: 0x06001E48 RID: 7752 RVA: 0x000FACE0 File Offset: 0x000F8EE0
		private void Awake()
		{
			this.containerButton.onClick.AddListener(new UnityAction(this.OnContainerButtonClicked));
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x000FACFE File Offset: 0x000F8EFE
		private void OnContainerButtonClicked()
		{
			EventSystem.current.SetSelectedGameObject(this.slider.gameObject);
		}

		// Token: 0x06001E4A RID: 7754 RVA: 0x000FAD18 File Offset: 0x000F8F18
		public override void ConfigureFrom(SettingsItemBuilder itemBuilder, SettingsPageBuilder pageBuilder)
		{
			if (this.slider == null)
			{
				return;
			}
			SettingsItem asset = itemBuilder.asset;
			if (asset.sliderConfig != null)
			{
				this.slider.minValue = asset.sliderConfig.minValue;
				this.slider.maxValue = asset.sliderConfig.maxValue;
				this.slider.wholeNumbers = asset.sliderConfig.wholeNumbers;
				this.sliderValueToText.ConfigureFrom(asset.sliderConfig.textConfig);
			}
			if (asset.preferenceKey.IsValid())
			{
				float num = asset.preferenceKey.GetFloatValue(0f) * asset.valueDisplayMultiplayer;
				this.slider.SetValueWithoutNotify(num);
			}
			this.slider.onValueChanged.AddListener(new UnityAction<float>(itemBuilder.ValueChanged<float>));
		}

		// Token: 0x06001E4B RID: 7755 RVA: 0x000FADE8 File Offset: 0x000F8FE8
		public void SelectInnerSlider()
		{
			SettingsMenu.SetSelected(this.slider);
		}

		// Token: 0x06001E4C RID: 7756 RVA: 0x000FADF5 File Offset: 0x000F8FF5
		public override void SetSelected()
		{
			SettingsMenu.SetSelected(this.containerButton);
		}

		// Token: 0x06001E4D RID: 7757 RVA: 0x000FAE02 File Offset: 0x000F9002
		public override void AttachRestoreDefaultButton(SettingsRestoreDefaultButton restoreDefaultButton)
		{
			restoreDefaultButton.slider = this.slider;
			restoreDefaultButton.integerSlider = this.slider.wholeNumbers;
		}

		// Token: 0x04002ADE RID: 10974
		[SerializeField]
		private Button containerButton;

		// Token: 0x04002ADF RID: 10975
		[SerializeField]
		private Slider slider;

		// Token: 0x04002AE0 RID: 10976
		[SerializeField]
		private SliderValueToText sliderValueToText;
	}
}
