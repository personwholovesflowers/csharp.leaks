using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace StanleyUI
{
	// Token: 0x020001FC RID: 508
	public class StanleyMenuUIEntityUtility : MonoBehaviour
	{
		// Token: 0x06000BBB RID: 3003 RVA: 0x0003590F File Offset: 0x00033B0F
		private bool IsBoolType()
		{
			return this.type == StanleyMenuUIEntityUtility.UIType.Toggle;
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x0003591A File Offset: 0x00033B1A
		private bool IsIntType()
		{
			return this.type == StanleyMenuUIEntityUtility.UIType.Selector_IntBased;
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x00035925 File Offset: 0x00033B25
		private bool IsFloatType()
		{
			return this.type == StanleyMenuUIEntityUtility.UIType.Slider;
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x00035930 File Offset: 0x00033B30
		private bool IsStringType()
		{
			return this.type == StanleyMenuUIEntityUtility.UIType.Selector_StringBased;
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x0003593C File Offset: 0x00033B3C
		public void RunSettingCodeProxy_bool(bool on)
		{
			ISettingsBoolListener[] components = base.GetComponents<ISettingsBoolListener>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].SetValue(on);
			}
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x00035968 File Offset: 0x00033B68
		public void RunSettingCodeProxy_float(float val)
		{
			ISettingsFloatListener[] components = base.GetComponents<ISettingsFloatListener>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].SetValue(val);
			}
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x00035994 File Offset: 0x00033B94
		public void RunSettingCodeProxy_int(int val)
		{
			ISettingsIntListener[] components = base.GetComponents<ISettingsIntListener>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].SetValue(val);
			}
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x000359C0 File Offset: 0x00033BC0
		public void RunSettingCodeProxy_string(string val)
		{
			ISettingsStringListener[] components = base.GetComponents<ISettingsStringListener>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].SetValue(val);
			}
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x000359EC File Offset: 0x00033BEC
		public void RunSettingCodeFromConfigurable()
		{
			if (this.targetConfigurable == null)
			{
				return;
			}
			switch (this.type)
			{
			case StanleyMenuUIEntityUtility.UIType.Slider:
				this.RunSettingCodeProxy_float(this.targetConfigurable.GetFloatValue());
				return;
			case StanleyMenuUIEntityUtility.UIType.Selector:
			case StanleyMenuUIEntityUtility.UIType.SubHeading:
				break;
			case StanleyMenuUIEntityUtility.UIType.Toggle:
				this.RunSettingCodeProxy_bool(this.targetConfigurable.GetBooleanValue());
				return;
			case StanleyMenuUIEntityUtility.UIType.Selector_IntBased:
				this.RunSettingCodeProxy_int(this.targetConfigurable.GetIntValue());
				return;
			case StanleyMenuUIEntityUtility.UIType.Selector_StringBased:
				this.RunSettingCodeProxy_string(this.targetConfigurable.GetStringValue());
				break;
			default:
				return;
			}
		}

		// Token: 0x04000B4A RID: 2890
		[Header("Hints")]
		[InspectorButton("FindType", "Find Type")]
		public StanleyMenuUIEntityUtility.UIType type;

		// Token: 0x04000B4B RID: 2891
		public string title;

		// Token: 0x04000B4C RID: 2892
		[InspectorButton("TryConfigure", "Try Configure Using Hints")]
		public Configurable targetConfigurable;

		// Token: 0x04000B4D RID: 2893
		[Header("Setter Code Overrides (can be null)")]
		[EnabledIf("IsBoolType")]
		public UnityEvent<bool> settingCodeOverride_bool;

		// Token: 0x04000B4E RID: 2894
		[EnabledIf("IsIntType")]
		public UnityEvent<int> settingCodeOverride_int;

		// Token: 0x04000B4F RID: 2895
		[EnabledIf("IsStringType")]
		public UnityEvent<string> settingCodeOverride_string;

		// Token: 0x04000B50 RID: 2896
		[Header("Set in prefab")]
		public TextMeshProUGUI label;

		// Token: 0x04000B51 RID: 2897
		[Header("Results:")]
		public bool hasConfigurableEvent;

		// Token: 0x04000B52 RID: 2898
		public bool hasConfigurableEventConfigurableSet;

		// Token: 0x04000B53 RID: 2899
		public bool hasConfigurableEventSelfInvoke;

		// Token: 0x04000B54 RID: 2900
		public bool hasConfigurableEventEventsSetUpCorrectly;

		// Token: 0x04000B55 RID: 2901
		public bool hasConfigurator;

		// Token: 0x04000B56 RID: 2902
		public bool hasConfiguratorConfigurableSet;

		// Token: 0x04000B57 RID: 2903
		public string labelText;

		// Token: 0x04000B58 RID: 2904
		public bool hasCustomLabel;

		// Token: 0x04000B59 RID: 2905
		public bool hasISettingsListener;

		// Token: 0x04000B5A RID: 2906
		[InspectorButton("CheckConfigurable", "Run Checks")]
		public bool noListenerButAllGood;

		// Token: 0x04000B5B RID: 2907
		[InspectorButton("CorrectConfigurable", "Run Corrections")]
		public bool allGood;

		// Token: 0x0200041D RID: 1053
		public enum UIType
		{
			// Token: 0x0400155D RID: 5469
			Unknown,
			// Token: 0x0400155E RID: 5470
			Slider,
			// Token: 0x0400155F RID: 5471
			Selector,
			// Token: 0x04001560 RID: 5472
			Toggle,
			// Token: 0x04001561 RID: 5473
			SubHeading,
			// Token: 0x04001562 RID: 5474
			Selector_IntBased,
			// Token: 0x04001563 RID: 5475
			Selector_StringBased
		}
	}
}
