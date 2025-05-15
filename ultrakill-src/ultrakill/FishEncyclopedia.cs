using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x020001DF RID: 479
public class FishEncyclopedia : MonoBehaviour
{
	// Token: 0x060009B6 RID: 2486 RVA: 0x000435C0 File Offset: 0x000417C0
	private void Start()
	{
		this.fishButtonTemplate.gameObject.SetActive(false);
		foreach (KeyValuePair<FishObject, bool> keyValuePair in MonoSingleton<FishManager>.Instance.recognizedFishes)
		{
			FishObject fish = keyValuePair.Key;
			bool value = keyValuePair.Value;
			if (!this.fishButtons.ContainsKey(fish))
			{
				FishMenuButton fishMenuButton = Object.Instantiate<FishMenuButton>(this.fishButtonTemplate, this.fishGrid, false);
				this.fishButtons.Add(fish, fishMenuButton);
				fishMenuButton.gameObject.SetActive(true);
				fishMenuButton.Populate(fish, !value);
				fishMenuButton.GetComponent<ControllerPointer>().OnPressed.RemoveAllListeners();
				fishMenuButton.GetComponent<ControllerPointer>().OnPressed.AddListener(delegate
				{
					this.SelectFish(fish);
				});
			}
		}
		FishManager instance = MonoSingleton<FishManager>.Instance;
		instance.onFishUnlocked = (Action<FishObject>)Delegate.Combine(instance.onFishUnlocked, new Action<FishObject>(this.OnFishUnlocked));
	}

	// Token: 0x060009B7 RID: 2487 RVA: 0x000436F8 File Offset: 0x000418F8
	private void OnFishUnlocked(FishObject obj)
	{
		if (this.fishButtons.ContainsKey(obj))
		{
			this.fishButtons[obj].Populate(obj, false);
		}
	}

	// Token: 0x060009B8 RID: 2488 RVA: 0x0004371C File Offset: 0x0004191C
	private void DisplayFish(FishObject fish)
	{
		foreach (object obj in this.fish3dRenderContainer.transform)
		{
			Object.Destroy(((Transform)obj).gameObject);
		}
		if (MonoSingleton<FishManager>.Instance.recognizedFishes[fish])
		{
			GameObject gameObject = fish.InstantiateDumb();
			gameObject.transform.SetParent(this.fish3dRenderContainer.transform);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			SandboxUtils.SetLayerDeep(gameObject.transform, LayerMask.NameToLayer("VirtualRender"));
		}
	}

	// Token: 0x060009B9 RID: 2489 RVA: 0x000437E0 File Offset: 0x000419E0
	public void SelectFish(FishObject fish)
	{
		this.fishName.text = (MonoSingleton<FishManager>.Instance.recognizedFishes[fish] ? fish.fishName : "???");
		this.fishDescription.text = fish.description;
		this.fishPicker.SetActive(false);
		this.fishInfoContainer.SetActive(true);
		this.DisplayFish(fish);
	}

	// Token: 0x060009BA RID: 2490 RVA: 0x00043847 File Offset: 0x00041A47
	public void HideFishInfo()
	{
		this.fishPicker.SetActive(true);
		this.fishInfoContainer.SetActive(false);
	}

	// Token: 0x04000CA1 RID: 3233
	[SerializeField]
	private GameObject fishPicker;

	// Token: 0x04000CA2 RID: 3234
	[SerializeField]
	private GameObject fishInfoContainer;

	// Token: 0x04000CA3 RID: 3235
	[SerializeField]
	private TMP_Text fishName;

	// Token: 0x04000CA4 RID: 3236
	[SerializeField]
	private TMP_Text fishDescription;

	// Token: 0x04000CA5 RID: 3237
	[Space]
	[SerializeField]
	private Transform fishGrid;

	// Token: 0x04000CA6 RID: 3238
	[SerializeField]
	private FishMenuButton fishButtonTemplate;

	// Token: 0x04000CA7 RID: 3239
	[SerializeField]
	private GameObject fish3dRenderContainer;

	// Token: 0x04000CA8 RID: 3240
	[Space]
	[SerializeField]
	private FishDB[] fishDbs;

	// Token: 0x04000CA9 RID: 3241
	private Dictionary<FishObject, FishMenuButton> fishButtons = new Dictionary<FishObject, FishMenuButton>();
}
