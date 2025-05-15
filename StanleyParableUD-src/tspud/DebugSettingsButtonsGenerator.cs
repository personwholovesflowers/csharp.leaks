using System;
using System.Collections.Generic;
using StanleyUI;
using TMPro;
using UnityEngine;

// Token: 0x02000110 RID: 272
public class DebugSettingsButtonsGenerator : MonoBehaviour
{
	// Token: 0x06000692 RID: 1682 RVA: 0x00005444 File Offset: 0x00003644
	private void Start()
	{
	}

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x06000693 RID: 1683 RVA: 0x00023676 File Offset: 0x00021876
	private IEnumerable<Configurable> ConfigurableList
	{
		get
		{
			return this.configurablesResettor.allConfigurables;
		}
	}

	// Token: 0x06000694 RID: 1684 RVA: 0x00023683 File Offset: 0x00021883
	public static void DestroyEditorSafe(GameObject go)
	{
		if (Application.isPlaying)
		{
			Object.Destroy(go);
			return;
		}
		Object.DestroyImmediate(go);
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x00023699 File Offset: 0x00021899
	public static GameObject InstantiateEditorSafe(GameObject prefab)
	{
		return Object.Instantiate<GameObject>(prefab);
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x000236A4 File Offset: 0x000218A4
	[ContextMenu("DestroyDebugButtonInstances")]
	public void DestroyDebugButtonInstances()
	{
		for (int i = 0; i < this.debugButtonInstances.Count; i++)
		{
			DebugSettingsButtonsGenerator.DestroyEditorSafe(this.debugButtonInstances[i]);
		}
		this.debugButtonInstances.Clear();
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x000236E4 File Offset: 0x000218E4
	[ContextMenu("CreateDebugButtonInstances")]
	public void CreateDebugButtonInstances()
	{
		this.DestroyDebugButtonInstances();
		foreach (Configurable configurable in this.ConfigurableList)
		{
			GameObject gameObject;
			if (configurable is IntConfigurable)
			{
				gameObject = DebugSettingsButtonsGenerator.InstantiateEditorSafe(this.debugIntSliderPrefab);
			}
			else if (configurable is BooleanConfigurable)
			{
				gameObject = DebugSettingsButtonsGenerator.InstantiateEditorSafe(this.debugTogglePrefab);
			}
			else
			{
				if (!(configurable is StringConfigurable))
				{
					continue;
				}
				gameObject = DebugSettingsButtonsGenerator.InstantiateEditorSafe(this.debugStringPrefab);
			}
			gameObject.transform.SetParent(base.transform.parent);
			gameObject.transform.SetSiblingIndex(gameObject.transform.parent.childCount - 1);
			GameObject gameObject2 = gameObject;
			gameObject2.name = gameObject2.name + " (" + configurable.name + ")";
			gameObject.transform.localScale = Vector3.one;
			Array.Find<TextMeshProUGUI>(gameObject.GetComponentsInChildren<TextMeshProUGUI>(), (TextMeshProUGUI x) => x.name == "Label").text = configurable.name.Replace("CONFIGURABLE_", "");
			StanleyMenuUIEntityUtility component = gameObject.GetComponent<StanleyMenuUIEntityUtility>();
			if (component != null)
			{
				component.targetConfigurable = configurable;
			}
			if (configurable is IntConfigurable)
			{
				gameObject.GetComponent<StanleyMenuSlider>().minValue = (float)(configurable as IntConfigurable).MinValue;
				gameObject.GetComponent<StanleyMenuSlider>().maxValue = (float)(configurable as IntConfigurable).MaxValue;
			}
			this.debugButtonInstances.Add(gameObject);
		}
	}

	// Token: 0x040006E9 RID: 1769
	public ResetableConfigurablesList configurablesResettor;

	// Token: 0x040006EA RID: 1770
	public Configurable[] testConfigs;

	// Token: 0x040006EB RID: 1771
	public List<GameObject> debugButtonInstances;

	// Token: 0x040006EC RID: 1772
	public GameObject debugTogglePrefab;

	// Token: 0x040006ED RID: 1773
	public GameObject debugIntSliderPrefab;

	// Token: 0x040006EE RID: 1774
	public GameObject debugStringPrefab;
}
