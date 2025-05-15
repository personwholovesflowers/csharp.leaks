using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000199 RID: 409
public class ConfigurableCountTest : MonoBehaviour
{
	// Token: 0x06000953 RID: 2387 RVA: 0x0002C060 File Offset: 0x0002A260
	public void TestConfigurableWeightSum()
	{
		int num = 0;
		foreach (ConfigurableCountTest.ConfigurableIntergerPair configurableIntergerPair in this.weightedConfigurables)
		{
			bool booleanValue = configurableIntergerPair.configurable.GetBooleanValue();
			configurableIntergerPair.LastWeightedValue = booleanValue;
			if (booleanValue)
			{
				num += configurableIntergerPair.weighting;
			}
		}
		this.DEBUG_LastTestedWeight = num;
		int num2 = (this.hasPlayedTSPBeforeConfigurable.GetBooleanValue() ? this.thresholdIfPlayedTSPBefore : this.thresholdIfNotPlayedTSPBefore);
		if (num >= num2)
		{
			if (this.OnSumAboveOrEqualThreshold != null)
			{
				this.OnSumAboveOrEqualThreshold.Invoke();
				return;
			}
			if (this.OnSumBelowThreshold != null)
			{
				this.OnSumBelowThreshold.Invoke();
			}
		}
	}

	// Token: 0x06000954 RID: 2388 RVA: 0x0002C120 File Offset: 0x0002A320
	private void OnValidate()
	{
		while (this.configurableToAdd != null && this.configurableToAdd.Count > 0)
		{
			this.weightedConfigurables.Add(new ConfigurableCountTest.ConfigurableIntergerPair
			{
				configurable = this.configurableToAdd[0]
			});
			this.configurableToAdd.RemoveAt(0);
		}
	}

	// Token: 0x0400092C RID: 2348
	[SerializeField]
	private List<ConfigurableCountTest.ConfigurableIntergerPair> weightedConfigurables = new List<ConfigurableCountTest.ConfigurableIntergerPair>();

	// Token: 0x0400092D RID: 2349
	[InspectorButton("TestConfigurableWeightSum", "Perform manual Test")]
	[SerializeField]
	private Configurable hasPlayedTSPBeforeConfigurable;

	// Token: 0x0400092E RID: 2350
	[SerializeField]
	private int thresholdIfPlayedTSPBefore = 10;

	// Token: 0x0400092F RID: 2351
	[SerializeField]
	private int thresholdIfNotPlayedTSPBefore = 8;

	// Token: 0x04000930 RID: 2352
	[SerializeField]
	private UnityEvent OnSumAboveOrEqualThreshold;

	// Token: 0x04000931 RID: 2353
	[SerializeField]
	private UnityEvent OnSumBelowThreshold;

	// Token: 0x04000932 RID: 2354
	[Header("Easy Add Multiple Configurables")]
	[SerializeField]
	private List<Configurable> configurableToAdd = new List<Configurable>();

	// Token: 0x04000933 RID: 2355
	[Header("DEBUG DO NOT EDIT")]
	public int DEBUG_LastTestedWeight;

	// Token: 0x020003EC RID: 1004
	[Serializable]
	private class ConfigurableIntergerPair
	{
		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x060017A1 RID: 6049 RVA: 0x00079A74 File Offset: 0x00077C74
		// (set) Token: 0x060017A2 RID: 6050 RVA: 0x00079A7C File Offset: 0x00077C7C
		public bool LastWeightedValue { get; set; }

		// Token: 0x04001470 RID: 5232
		public Configurable configurable;

		// Token: 0x04001471 RID: 5233
		public int weighting = 1;
	}
}
