using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200012C RID: 300
public class TSP3BackgroundManager : MonoBehaviour
{
	// Token: 0x06000711 RID: 1809 RVA: 0x00025010 File Offset: 0x00023210
	private void Awake()
	{
		if (TSP3BackgroundManager.playthroughSeed == -1)
		{
			TSP3BackgroundManager.playthroughSeed = Random.Range(0, 100000);
		}
		IntConfigurable intConfigurable = this.sequelNumber;
		intConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(intConfigurable.OnValueChanged, new Action<LiveData>(this.SetupBackgroundElementsLD));
		GameMaster.OnPrepareLoadingLevel += this.SetupBackgroundElements;
		AssetBundleControl.OnSceneReady = (Action)Delegate.Combine(AssetBundleControl.OnSceneReady, new Action(this.SetupBackgroundElements));
		foreach (Transform transform in new Transform[] { this.uiObjectHolder, this.objectHolder })
		{
			for (int j = 0; j < transform.childCount; j++)
			{
				Object.Destroy(transform.GetChild(j).gameObject);
			}
		}
	}

	// Token: 0x06000712 RID: 1810 RVA: 0x000250DC File Offset: 0x000232DC
	private void OnDestroy()
	{
		IntConfigurable intConfigurable = this.sequelNumber;
		intConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(intConfigurable.OnValueChanged, new Action<LiveData>(this.SetupBackgroundElementsLD));
		GameMaster.OnPrepareLoadingLevel -= this.SetupBackgroundElements;
		AssetBundleControl.OnSceneReady = (Action)Delegate.Remove(AssetBundleControl.OnSceneReady, new Action(this.SetupBackgroundElements));
		this.RemoveAllInstantiatedObjects();
	}

	// Token: 0x06000713 RID: 1811 RVA: 0x00025147 File Offset: 0x00023347
	private void Start()
	{
		this.SetupBackgroundElements();
	}

	// Token: 0x06000714 RID: 1812 RVA: 0x00025147 File Offset: 0x00023347
	private void SetupBackgroundElementsLD(LiveData ld)
	{
		this.SetupBackgroundElements();
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x00025150 File Offset: 0x00023350
	private void SetupBackgroundElements()
	{
		int intValue = this.sequelNumber.GetIntValue();
		int num = intValue * intValue * TSP3BackgroundManager.playthroughSeed;
		TSP3BackgroundRandomSet tsp3BackgroundRandomSet;
		if (intValue < this.presetBackgrounds.Count)
		{
			tsp3BackgroundRandomSet = this.presetBackgrounds[intValue];
		}
		else if (intValue < 100)
		{
			tsp3BackgroundRandomSet = this.afterPresets;
		}
		else
		{
			tsp3BackgroundRandomSet = ((this.after100Presets != null) ? this.after100Presets : this.afterPresets);
		}
		if (tsp3BackgroundRandomSet != null)
		{
			this.SetupTSP3Menu(tsp3BackgroundRandomSet, num);
			return;
		}
		Debug.LogError("Could not find TSP3+ background setup data");
	}

	// Token: 0x06000716 RID: 1814 RVA: 0x000251DC File Offset: 0x000233DC
	private void RemoveAllInstantiatedObjects()
	{
		foreach (GameObject gameObject in this.instantiatedGameObjects)
		{
			Object.Destroy(gameObject);
		}
		this.instantiatedGameObjects.Clear();
	}

	// Token: 0x06000717 RID: 1815 RVA: 0x00025238 File Offset: 0x00023438
	private void SetupTSP3Menu(TSP3BackgroundRandomSet setupData, int seed)
	{
		this.amplifyColor.LutTexture = setupData.GetLUTImage(seed);
		this.backgroundImage.texture = setupData.GetBackgroundImage(seed);
		this.RemoveAllInstantiatedObjects();
		foreach (GameObject gameObject in setupData.GetAllToInstantiate(seed))
		{
			if (!(gameObject == null))
			{
				GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject);
				GameObject gameObject3 = gameObject2;
				gameObject3.name += string.Format(" {0} {1}", Time.frameCount, seed);
				this.instantiatedGameObjects.Add(gameObject2);
				RectTransform component = gameObject2.GetComponent<RectTransform>();
				if (component != null)
				{
					gameObject2.transform.parent = this.uiObjectHolder;
					gameObject2.transform.localPosition = Vector3.zero;
					gameObject2.transform.localScale = Vector3.one;
					gameObject2.transform.localRotation = Quaternion.identity;
					component.anchorMin = Vector3.zero;
					component.anchorMax = Vector3.one;
					component.anchoredPosition = Vector3.zero;
					component.sizeDelta = Vector3.zero;
					component.pivot = Vector3.one * 0.5f;
				}
				else
				{
					gameObject2.transform.parent = this.objectHolder;
					gameObject2.transform.localPosition = Vector3.zero;
					gameObject2.transform.localScale = Vector3.one;
					gameObject2.transform.localRotation = Quaternion.identity;
				}
			}
		}
	}

	// Token: 0x0400074B RID: 1867
	[Header("LEAVE THE FIRST 3 ELEMENTS AS NULL")]
	public List<TSP3BackgroundRandomSet> presetBackgrounds;

	// Token: 0x0400074C RID: 1868
	public TSP3BackgroundRandomSet afterPresets;

	// Token: 0x0400074D RID: 1869
	public TSP3BackgroundRandomSet after100Presets;

	// Token: 0x0400074E RID: 1870
	[Header("Data")]
	public IntConfigurable sequelNumber;

	// Token: 0x0400074F RID: 1871
	public RawImage backgroundImage;

	// Token: 0x04000750 RID: 1872
	public AmplifyColorEffect amplifyColor;

	// Token: 0x04000751 RID: 1873
	public Transform uiObjectHolder;

	// Token: 0x04000752 RID: 1874
	[InspectorButton("SetupBackgroundElements", null)]
	public Transform objectHolder;

	// Token: 0x04000753 RID: 1875
	private List<GameObject> instantiatedGameObjects = new List<GameObject>();

	// Token: 0x04000754 RID: 1876
	private static int playthroughSeed = -1;
}
