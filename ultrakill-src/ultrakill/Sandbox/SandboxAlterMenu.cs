using System;
using System.Collections.Generic;
using System.Globalization;
using plog;
using Sandbox.Arm;
using TMPro;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Sandbox
{
	// Token: 0x02000561 RID: 1377
	[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
	public class SandboxAlterMenu : MonoSingleton<SandboxAlterMenu>
	{
		// Token: 0x06001EE3 RID: 7907 RVA: 0x000FD6BC File Offset: 0x000FB8BC
		public Vector3 SafeSize(Vector3 originalSize)
		{
			float num = 0.00390625f;
			float num2 = 128f;
			float num3 = Mathf.Clamp(originalSize.x, num, num2);
			float num4 = Mathf.Clamp(originalSize.y, num, num2);
			float num5 = Mathf.Clamp(originalSize.z, num, num2);
			return new Vector3(num3, num4, num5);
		}

		// Token: 0x06001EE4 RID: 7908 RVA: 0x000FD708 File Offset: 0x000FB908
		protected override void Awake()
		{
			base.Awake();
			this.sizeFieldX.onValueChanged.AddListener(new UnityAction<string>(this.SetSizeX));
			this.sizeFieldY.onValueChanged.AddListener(new UnityAction<string>(this.SetSizeY));
			this.sizeFieldZ.onValueChanged.AddListener(new UnityAction<string>(this.SetSizeZ));
			this.sizeField.onValueChanged.AddListener(new UnityAction<string>(this.SetSize));
			this.sizeFieldX.onEndEdit.AddListener(delegate(string _)
			{
				this.UpdateSizeValues();
			});
			this.sizeFieldY.onEndEdit.AddListener(delegate(string _)
			{
				this.UpdateSizeValues();
			});
			this.sizeFieldZ.onEndEdit.AddListener(delegate(string _)
			{
				this.UpdateSizeValues();
			});
			this.sizeField.onEndEdit.AddListener(delegate(string _)
			{
				this.UpdateSizeValues();
			});
		}

		// Token: 0x06001EE5 RID: 7909 RVA: 0x000FD7FC File Offset: 0x000FB9FC
		private void SetSizeX(string value)
		{
			if (this.editedObject == null)
			{
				return;
			}
			Vector3 normalizedSize = this.editedObject.normalizedSize;
			float num;
			if (!float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out num))
			{
				return;
			}
			this.editedObject.SetSize(this.SafeSize(new Vector3(num, normalizedSize.y, normalizedSize.z)));
		}

		// Token: 0x06001EE6 RID: 7910 RVA: 0x000FD85C File Offset: 0x000FBA5C
		private void SetSizeY(string value)
		{
			if (this.editedObject == null)
			{
				return;
			}
			Vector3 normalizedSize = this.editedObject.normalizedSize;
			Debug.Log(value);
			float num;
			if (!float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out num))
			{
				return;
			}
			this.editedObject.SetSize(this.SafeSize(new Vector3(normalizedSize.x, num, normalizedSize.z)));
		}

		// Token: 0x06001EE7 RID: 7911 RVA: 0x000FD8C4 File Offset: 0x000FBAC4
		private void SetSizeZ(string value)
		{
			if (this.editedObject == null)
			{
				return;
			}
			Vector3 normalizedSize = this.editedObject.normalizedSize;
			float num;
			if (!float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out num))
			{
				return;
			}
			this.editedObject.SetSize(this.SafeSize(new Vector3(normalizedSize.x, normalizedSize.y, num)));
		}

		// Token: 0x06001EE8 RID: 7912 RVA: 0x000FD924 File Offset: 0x000FBB24
		private void SetSize(string value)
		{
			if (this.editedObject == null)
			{
				return;
			}
			float num;
			if (!float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out num))
			{
				return;
			}
			this.editedObject.SetSizeUniform(num);
		}

		// Token: 0x06001EE9 RID: 7913 RVA: 0x000FD964 File Offset: 0x000FBB64
		public void SetJumpPadPower(float value)
		{
			if (this.editedObject == null)
			{
				return;
			}
			JumpPad componentInChildren = this.editedObject.GetComponentInChildren<JumpPad>();
			if (componentInChildren == null)
			{
				return;
			}
			componentInChildren.force = value;
		}

		// Token: 0x06001EEA RID: 7914 RVA: 0x000FD99D File Offset: 0x000FBB9D
		public void SetFrozen(bool frozen)
		{
			if (!this.editedObject)
			{
				return;
			}
			this.editedObject.frozen = frozen;
		}

		// Token: 0x06001EEB RID: 7915 RVA: 0x000FD9B9 File Offset: 0x000FBBB9
		public void SetDisallowManipulation(bool disallow)
		{
			if (!this.editedObject)
			{
				return;
			}
			this.editedObject.disallowManipulation = disallow;
		}

		// Token: 0x06001EEC RID: 7916 RVA: 0x000FD9D5 File Offset: 0x000FBBD5
		public void SetDisallowFreezing(bool disallow)
		{
			if (!this.editedObject)
			{
				return;
			}
			this.editedObject.disallowFreezing = disallow;
		}

		// Token: 0x06001EED RID: 7917 RVA: 0x000FD9F1 File Offset: 0x000FBBF1
		public void SetRadianceTierSlider(float value)
		{
			this.SetRadianceTier(value / 2f);
		}

		// Token: 0x06001EEE RID: 7918 RVA: 0x000FDA00 File Offset: 0x000FBC00
		public void SetRadianceTier(float value)
		{
			SandboxEnemy sandboxEnemy = this.editedObject as SandboxEnemy;
			if (sandboxEnemy == null)
			{
				return;
			}
			sandboxEnemy.radiance.tier = value;
			sandboxEnemy.UpdateRadiance();
		}

		// Token: 0x06001EEF RID: 7919 RVA: 0x000FDA2F File Offset: 0x000FBC2F
		public void SetHealthBuffSlider(float value)
		{
			this.SetHealthBuff(value / 2f);
		}

		// Token: 0x06001EF0 RID: 7920 RVA: 0x000FDA40 File Offset: 0x000FBC40
		public void SetHealthBuff(float value)
		{
			SandboxEnemy sandboxEnemy = this.editedObject as SandboxEnemy;
			if (sandboxEnemy == null)
			{
				return;
			}
			sandboxEnemy.radiance.healthBuff = value;
			sandboxEnemy.UpdateRadiance();
		}

		// Token: 0x06001EF1 RID: 7921 RVA: 0x000FDA6F File Offset: 0x000FBC6F
		public void SetDamageBuffSlider(float value)
		{
			this.SetDamageBuff(value / 2f);
		}

		// Token: 0x06001EF2 RID: 7922 RVA: 0x000FDA80 File Offset: 0x000FBC80
		public void SetDamageBuff(float value)
		{
			SandboxEnemy sandboxEnemy = this.editedObject as SandboxEnemy;
			if (sandboxEnemy == null)
			{
				return;
			}
			Debug.Log("Setting Damage Buff: " + value.ToString());
			sandboxEnemy.radiance.damageBuff = value;
			sandboxEnemy.UpdateRadiance();
		}

		// Token: 0x06001EF3 RID: 7923 RVA: 0x000FDAC5 File Offset: 0x000FBCC5
		public void SetSpeedBuffSlider(float value)
		{
			this.SetSpeedBuff(value / 2f);
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x000FDAD4 File Offset: 0x000FBCD4
		public void SetSpeedBuff(float value)
		{
			SandboxEnemy sandboxEnemy = this.editedObject as SandboxEnemy;
			if (sandboxEnemy == null)
			{
				return;
			}
			sandboxEnemy.radiance.speedBuff = value;
			sandboxEnemy.UpdateRadiance();
		}

		// Token: 0x06001EF5 RID: 7925 RVA: 0x000FDB04 File Offset: 0x000FBD04
		public void ShowRadianceOptions(bool value)
		{
			this.radianceSettings.SetActive(value);
			SandboxEnemy sandboxEnemy = this.editedObject as SandboxEnemy;
			if (sandboxEnemy != null)
			{
				if (sandboxEnemy.radiance == null)
				{
					sandboxEnemy.radiance = new EnemyRadianceConfig(sandboxEnemy.enemyId);
				}
				sandboxEnemy.radiance.enabled = value;
				if (value)
				{
					Debug.Log("Loading Damage Buff: " + sandboxEnemy.radiance.damageBuff.ToString());
					this.radianceEnabled.SetIsOnWithoutNotify(true);
					this.radianceTier.SetValueWithoutNotify(sandboxEnemy.radiance.tier * 2f);
					this.radianceDamage.SetValueWithoutNotify(sandboxEnemy.radiance.damageBuff * 2f);
					this.radianceHealth.SetValueWithoutNotify(sandboxEnemy.radiance.healthBuff * 2f);
					this.radianceSpeed.SetValueWithoutNotify(sandboxEnemy.radiance.speedBuff * 2f);
				}
				else
				{
					this.radianceEnabled.SetIsOnWithoutNotify(false);
				}
				sandboxEnemy.UpdateRadiance();
			}
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x000FDC0C File Offset: 0x000FBE0C
		public void ShowUniformSizeMenu(bool value)
		{
			this.uniformSize.SetIsOnWithoutNotify(value);
			this.uniformContainer.SetActive(value);
			this.splitContainer.SetActive(!value);
			this.sizeFieldX.interactable = !value;
			this.sizeFieldY.interactable = !value;
			this.sizeFieldZ.interactable = !value;
			this.sizeField.interactable = value;
			if (value)
			{
				this.editedObject.SetSizeUniform(this.editedObject.normalizedSize.x);
			}
			this.UpdateSizeValues();
		}

		// Token: 0x06001EF7 RID: 7927 RVA: 0x000FDCA0 File Offset: 0x000FBEA0
		public void DefaultSize()
		{
			if (this.editedObject == null)
			{
				return;
			}
			this.editedObject.transform.localScale = this.editedObject.defaultSize;
			this.UpdateSizeValues();
			Object.Instantiate<GameObject>(this.scaleResetSound, base.transform.position, Quaternion.identity);
		}

		// Token: 0x06001EF8 RID: 7928 RVA: 0x000FDCFC File Offset: 0x000FBEFC
		public void MultiplySize(float value)
		{
			if (this.editedObject == null)
			{
				return;
			}
			Vector3 vector = this.editedObject.transform.localScale;
			vector *= value;
			vector = this.SafeSize(vector);
			this.editedObject.transform.localScale = vector;
			this.ShowUniformSizeMenu(this.uniformContainer.activeSelf);
			this.UpdateSizeValues();
			Object.Instantiate<GameObject>((value > 1f) ? this.scaleUpSound : this.scaleDownSound, this.editedObject.transform.position, Quaternion.identity);
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x000FDD94 File Offset: 0x000FBF94
		public void UpdateSizeValues()
		{
			Vector3 localScale = this.editedObject.transform.localScale;
			if (this.uniformContainer.activeSelf)
			{
				this.sizeField.SetTextWithoutNotify((localScale.x / this.editedObject.defaultSize.x).ToString(CultureInfo.InvariantCulture));
				return;
			}
			this.sizeFieldX.SetTextWithoutNotify((localScale.x / this.editedObject.defaultSize.x).ToString(CultureInfo.InvariantCulture));
			this.sizeFieldY.SetTextWithoutNotify((localScale.y / this.editedObject.defaultSize.y).ToString(CultureInfo.InvariantCulture));
			this.sizeFieldZ.SetTextWithoutNotify((localScale.z / this.editedObject.defaultSize.z).ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x06001EFA RID: 7930 RVA: 0x000FDE7C File Offset: 0x000FC07C
		public void Show(SandboxSpawnableInstance prop, AlterMode instance)
		{
			if (SandboxArmDebug.DebugActive)
			{
				SandboxAlterMenu.Log.Info("Showing Sandbox Alter Menu for " + prop.name, null, null, null);
			}
			prop.Pause(false);
			this.shadow.SetActive(true);
			this.elementManager.Reset();
			this.menu.SetActive(true);
			this.frozenCheckbox.SetIsOnWithoutNotify(prop.frozen);
			this.disallowManipulationCheckbox.SetIsOnWithoutNotify(prop.disallowManipulation);
			this.disallowFreezingCheckbox.SetIsOnWithoutNotify(prop.disallowFreezing);
			this.nameText.text = prop.name;
			this.editedObject = prop;
			this.alterInstance = instance;
			GameStateManager.Instance.RegisterState(new GameState("alter-menu", this.menu)
			{
				cursorLock = LockMode.Unlock,
				cameraInputLock = LockMode.Lock,
				playerInputLock = LockMode.Unlock
			});
			MonoSingleton<CameraController>.Instance.activated = false;
			MonoSingleton<GunControl>.Instance.activated = false;
			bool flag = !(prop is BrushBlock);
			this.sizeContainer.SetActive(flag);
			if (flag)
			{
				this.ShowUniformSizeMenu(prop.uniformSize);
			}
			SandboxEnemy sandboxEnemy = prop as SandboxEnemy;
			if (sandboxEnemy != null)
			{
				this.ShowRadianceOptions(sandboxEnemy.radiance.enabled);
				this.enemyOptionsContainer.SetActive(true);
			}
			else
			{
				this.enemyOptionsContainer.SetActive(false);
				this.radianceSettings.SetActive(false);
			}
			IAlter[] componentsInChildren = prop.GetComponentsInChildren<IAlter>();
			List<string> list = new List<string>();
			IAlter[] array = componentsInChildren;
			int i = 0;
			while (i < array.Length)
			{
				IAlter alter = array[i];
				if (alter.alterKey == null)
				{
					goto IL_0191;
				}
				if (!list.Contains(alter.alterKey))
				{
					list.Add(alter.alterKey);
					goto IL_0191;
				}
				IL_03FF:
				i++;
				continue;
				IL_0191:
				int num = 0;
				IAlterOptions<bool> alterOptions = alter as IAlterOptions<bool>;
				if (alterOptions != null && alterOptions.options != null)
				{
					num += alterOptions.options.Length;
				}
				IAlterOptions<float> alterOptions2 = alter as IAlterOptions<float>;
				if (alterOptions2 != null && alterOptions2.options != null)
				{
					num += alterOptions2.options.Length;
				}
				IAlterOptions<Vector3> alterOptions3 = alter as IAlterOptions<Vector3>;
				if (alterOptions3 != null && alterOptions3.options != null)
				{
					num += alterOptions3.options.Length;
				}
				IAlterOptions<int> alterOptions4 = alter as IAlterOptions<int>;
				if (alterOptions4 != null && alterOptions4.options != null)
				{
					num += alterOptions4.options.Length;
				}
				if (num == 0)
				{
					goto IL_03FF;
				}
				AlterMenuElements alterMenuElements = this.elementManager;
				string text;
				if ((text = alter.alterCategoryName) == null)
				{
					text = alter.alterKey ?? string.Empty;
				}
				alterMenuElements.CreateTitle(text);
				IAlterOptions<bool> alterOptions5 = alter as IAlterOptions<bool>;
				if (alterOptions5 != null)
				{
					if (alterOptions5.options == null)
					{
						goto IL_03FF;
					}
					foreach (AlterOption<bool> alterOption in alterOptions5.options)
					{
						this.elementManager.CreateBoolRow(alterOption.name, alterOption.value, alterOption.callback, alterOption.tooltip);
					}
				}
				IAlterOptions<float> alterOptions6 = alter as IAlterOptions<float>;
				if (alterOptions6 != null)
				{
					if (alterOptions6.options == null)
					{
						goto IL_03FF;
					}
					foreach (AlterOption<float> alterOption2 in alterOptions6.options)
					{
						this.elementManager.CreateFloatRow(alterOption2.name, alterOption2.value, alterOption2.callback, alterOption2.constraints, alterOption2.tooltip);
					}
				}
				IAlterOptions<Vector3> alterOptions7 = alter as IAlterOptions<Vector3>;
				if (alterOptions7 != null)
				{
					if (alterOptions7.options == null)
					{
						goto IL_03FF;
					}
					foreach (AlterOption<Vector3> alterOption3 in alterOptions7.options)
					{
						this.elementManager.CreateVector3Row(alterOption3.name, alterOption3.value, alterOption3.callback, alterOption3.tooltip);
					}
				}
				IAlterOptions<int> alterOptions8 = alter as IAlterOptions<int>;
				if (alterOptions8 != null && alterOptions8.options != null)
				{
					foreach (AlterOption<int> alterOption4 in alterOptions8.options)
					{
						Type type = alterOption4.type;
						if (!(type == null) && type.IsEnum)
						{
							this.elementManager.CreateEnumRow(alterOption4.name, alterOption4.value, alterOption4.callback, type, alterOption4.tooltip);
						}
					}
					goto IL_03FF;
				}
				goto IL_03FF;
			}
		}

		// Token: 0x06001EFB RID: 7931 RVA: 0x000FE298 File Offset: 0x000FC498
		public void Close()
		{
			if (SandboxArmDebug.DebugActive)
			{
				SandboxAlterMenu.Log.Info("Closing Alter Menu", null, null, null);
			}
			this.shadow.SetActive(false);
			this.menu.SetActive(false);
			this.editedObject = null;
			AlterMode alterMode = this.alterInstance;
			if (alterMode != null)
			{
				alterMode.EndSession();
			}
			MonoSingleton<CameraController>.Instance.activated = true;
			MonoSingleton<GunControl>.Instance.activated = true;
		}

		// Token: 0x06001EFC RID: 7932 RVA: 0x000FE304 File Offset: 0x000FC504
		private void Update()
		{
			if (this.editedObject == null && this.menu.activeSelf)
			{
				this.Close();
				MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage("<color=red>Altered object was destroyed.</color>", "", "", 0, false, false, true);
			}
			if (this.menu.activeSelf || !this.shadow.activeSelf)
			{
				return;
			}
			this.alterInstance.EndSession();
			this.shadow.SetActive(false);
			this.editedObject = null;
		}

		// Token: 0x04002B55 RID: 11093
		private static readonly global::plog.Logger Log = new global::plog.Logger("SandboxAlterMenu");

		// Token: 0x04002B56 RID: 11094
		[SerializeField]
		private GameObject shadow;

		// Token: 0x04002B57 RID: 11095
		[SerializeField]
		private GameObject menu;

		// Token: 0x04002B58 RID: 11096
		[Space]
		[SerializeField]
		private TMP_Text nameText;

		// Token: 0x04002B59 RID: 11097
		[Space]
		[SerializeField]
		private Toggle uniformSize;

		// Token: 0x04002B5A RID: 11098
		[SerializeField]
		private InputField sizeField;

		// Token: 0x04002B5B RID: 11099
		[SerializeField]
		private InputField sizeFieldX;

		// Token: 0x04002B5C RID: 11100
		[SerializeField]
		private InputField sizeFieldY;

		// Token: 0x04002B5D RID: 11101
		[SerializeField]
		private InputField sizeFieldZ;

		// Token: 0x04002B5E RID: 11102
		[Space]
		[SerializeField]
		private Toggle radianceEnabled;

		// Token: 0x04002B5F RID: 11103
		[SerializeField]
		private Slider radianceTier;

		// Token: 0x04002B60 RID: 11104
		[SerializeField]
		private Slider radianceHealth;

		// Token: 0x04002B61 RID: 11105
		[SerializeField]
		private Slider radianceDamage;

		// Token: 0x04002B62 RID: 11106
		[SerializeField]
		private Slider radianceSpeed;

		// Token: 0x04002B63 RID: 11107
		[Space]
		[SerializeField]
		private GameObject sizeContainer;

		// Token: 0x04002B64 RID: 11108
		[SerializeField]
		private GameObject uniformContainer;

		// Token: 0x04002B65 RID: 11109
		[SerializeField]
		private Toggle frozenCheckbox;

		// Token: 0x04002B66 RID: 11110
		[SerializeField]
		private Toggle disallowManipulationCheckbox;

		// Token: 0x04002B67 RID: 11111
		[SerializeField]
		private Toggle disallowFreezingCheckbox;

		// Token: 0x04002B68 RID: 11112
		[SerializeField]
		private GameObject splitContainer;

		// Token: 0x04002B69 RID: 11113
		[SerializeField]
		private GameObject enemyOptionsContainer;

		// Token: 0x04002B6A RID: 11114
		[SerializeField]
		private GameObject radianceSettings;

		// Token: 0x04002B6B RID: 11115
		[Space]
		[SerializeField]
		private GameObject scaleUpSound;

		// Token: 0x04002B6C RID: 11116
		[SerializeField]
		private GameObject scaleDownSound;

		// Token: 0x04002B6D RID: 11117
		[SerializeField]
		private GameObject scaleResetSound;

		// Token: 0x04002B6E RID: 11118
		[Space]
		[SerializeField]
		private AlterMenuElements elementManager;

		// Token: 0x04002B6F RID: 11119
		public SandboxSpawnableInstance editedObject;

		// Token: 0x04002B70 RID: 11120
		public AlterMode alterInstance;
	}
}
