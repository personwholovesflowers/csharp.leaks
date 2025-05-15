using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200004E RID: 78
public class CustomContentGui : MonoBehaviour
{
	// Token: 0x17000051 RID: 81
	// (get) Token: 0x0600018D RID: 397 RVA: 0x00008115 File Offset: 0x00006315
	// (set) Token: 0x0600018E RID: 398 RVA: 0x0000811C File Offset: 0x0000631C
	public static bool wasAgonyOpen { get; private set; }

	// Token: 0x0600018F RID: 399 RVA: 0x00008124 File Offset: 0x00006324
	public void ShowLocalMaps()
	{
		CustomContentGui.lastTabWorkshop = false;
		base.gameObject.SetActive(true);
		this.typeSelectionMenu.SetActive(false);
		this.RefreshCustomMaps();
	}

	// Token: 0x06000190 RID: 400 RVA: 0x0000814A File Offset: 0x0000634A
	public void ShowWorkshopMaps()
	{
		CustomContentGui.lastTabWorkshop = true;
		base.gameObject.SetActive(true);
		this.typeSelectionMenu.SetActive(false);
		this.currentPage = 1;
		this.RefreshWorkshopItems(this.currentPage, false);
	}

	// Token: 0x06000191 RID: 401 RVA: 0x0000817E File Offset: 0x0000637E
	public void ReturnToTypeSelection()
	{
		base.gameObject.SetActive(false);
		this.typeSelectionMenu.SetActive(true);
	}

	// Token: 0x06000192 RID: 402 RVA: 0x00008198 File Offset: 0x00006398
	private void ResetItems()
	{
		for (int i = 2; i < this.grid.transform.childCount; i++)
		{
			if (!(this.pagination == this.grid.transform.GetChild(i).gameObject))
			{
				Object.Destroy(this.grid.transform.GetChild(i).gameObject);
			}
		}
		this.itemTemplate.gameObject.SetActive(false);
		this.categoryTemplate.gameObject.SetActive(false);
		this.gridTemplate.gameObject.SetActive(false);
	}

	// Token: 0x06000193 RID: 403 RVA: 0x00008231 File Offset: 0x00006431
	public void DismissBlockers()
	{
		this.loadingFailed.SetActive(false);
	}

	// Token: 0x06000194 RID: 404 RVA: 0x0000823F File Offset: 0x0000643F
	public void ShowInExplorer()
	{
		Application.OpenURL("file://" + GameProgressSaver.customMapsPath);
	}

	// Token: 0x06000195 RID: 405 RVA: 0x00008255 File Offset: 0x00006455
	public void SetLocalSortMode(int option)
	{
		CustomContentGui.currentLocalSortMode = (LocalSortMode)option;
		MonoSingleton<PrefsManager>.Instance.SetInt("agonyLocalSortMode", option);
		this.RefreshCustomMaps();
	}

	// Token: 0x06000196 RID: 406 RVA: 0x00008273 File Offset: 0x00006473
	public void SetDifficulty(int dif)
	{
		MonoSingleton<PrefsManager>.Instance.SetInt("difficulty", dif);
	}

	// Token: 0x06000197 RID: 407 RVA: 0x00008288 File Offset: 0x00006488
	public void SetWorkshopTab(int tab)
	{
		Button[] array = this.workshopTabButtons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].interactable = true;
		}
		MonoSingleton<PrefsManager>.Instance.SetInt("agonyWorkshopTab", tab);
		CustomContentGui.currentWorkshopTab = (WorkshopTab)tab;
		this.workshopTabButtons[(int)CustomContentGui.currentWorkshopTab].interactable = false;
		List<WorkshopTab> list = new List<WorkshopTab>
		{
			WorkshopTab.Favorite,
			WorkshopTab.Subscribed,
			WorkshopTab.YourUploads
		};
		this.workshopSearch.interactable = !list.Contains(CustomContentGui.currentWorkshopTab);
		if (!this.workshopSearch.interactable)
		{
			this.workshopSearch.text = string.Empty;
		}
		this.RefreshWorkshopItems(1, false);
	}

	// Token: 0x06000198 RID: 408 RVA: 0x00008334 File Offset: 0x00006534
	public async void RefreshWorkshopItems(int page = 1, bool lockScroll = false)
	{
	}

	// Token: 0x06000199 RID: 409 RVA: 0x00008363 File Offset: 0x00006563
	public void LoadMore()
	{
		this.currentPage++;
		this.RefreshWorkshopItems(this.currentPage, true);
	}

	// Token: 0x0600019A RID: 410 RVA: 0x00008380 File Offset: 0x00006580
	public async void RefreshCustomMaps()
	{
	}

	// Token: 0x0600019B RID: 411 RVA: 0x000083AF File Offset: 0x000065AF
	private void Update()
	{
		if (this.localListUpdatePending)
		{
			this.localListUpdatePending = false;
			Debug.Log("Refreshing local maps");
			this.RefreshCustomMaps();
		}
	}

	// Token: 0x0400016D RID: 365
	[SerializeField]
	private GameObject typeSelectionMenu;

	// Token: 0x0400016E RID: 366
	[Space]
	[SerializeField]
	private GameObject grid;

	// Token: 0x0400016F RID: 367
	[SerializeField]
	private GameObject gridLoadingBlocker;

	// Token: 0x04000170 RID: 368
	[SerializeField]
	private CustomLevelPanel itemTemplate;

	// Token: 0x04000171 RID: 369
	[SerializeField]
	private CustomContentCategory categoryTemplate;

	// Token: 0x04000172 RID: 370
	[SerializeField]
	private CustomContentGrid gridTemplate;

	// Token: 0x04000173 RID: 371
	[SerializeField]
	private GameObject pagination;

	// Token: 0x04000174 RID: 372
	[Space]
	[SerializeField]
	private ScrollRect scrollRect;

	// Token: 0x04000175 RID: 373
	[SerializeField]
	private ForceLayoutRebuilds forceLayoutRebuilds;

	// Token: 0x04000176 RID: 374
	[SerializeField]
	private GameObject workshopError;

	// Token: 0x04000177 RID: 375
	[SerializeField]
	private GameObject fetchingPanel;

	// Token: 0x04000178 RID: 376
	[SerializeField]
	private GameObject loadingFailed;

	// Token: 0x04000179 RID: 377
	[SerializeField]
	private InputField workshopSearch;

	// Token: 0x0400017A RID: 378
	[SerializeField]
	private Dropdown difficultyDropdown;

	// Token: 0x0400017B RID: 379
	[SerializeField]
	private GameObject workshopButtons;

	// Token: 0x0400017C RID: 380
	[SerializeField]
	private Button[] workshopTabButtons;

	// Token: 0x0400017D RID: 381
	[SerializeField]
	private GameObject localButtons;

	// Token: 0x0400017E RID: 382
	[SerializeField]
	private Dropdown localSortModeDropdown;

	// Token: 0x0400017F RID: 383
	[Space]
	[SerializeField]
	public CampaignViewScreen campaignView;

	// Token: 0x04000180 RID: 384
	private Action afterLegacyAgonyInterrupt;

	// Token: 0x04000181 RID: 385
	private UnscaledTimeSince timeSinceStart;

	// Token: 0x04000182 RID: 386
	private FileSystemWatcher watcher;

	// Token: 0x04000183 RID: 387
	private bool localListUpdatePending;

	// Token: 0x04000184 RID: 388
	private int currentPage = 1;

	// Token: 0x04000185 RID: 389
	private static LocalSortMode currentLocalSortMode = LocalSortMode.Name;

	// Token: 0x04000186 RID: 390
	private static WorkshopTab currentWorkshopTab = WorkshopTab.Trending;

	// Token: 0x04000187 RID: 391
	private static bool lastTabWorkshop;

	// Token: 0x04000189 RID: 393
	public static CustomCampaign lastCustomCampaign;
}
