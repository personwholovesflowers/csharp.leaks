using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003A4 RID: 932
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class SandboxHud : MonoSingleton<SandboxHud>
{
	// Token: 0x17000189 RID: 393
	// (get) Token: 0x06001562 RID: 5474 RVA: 0x000AE3EA File Offset: 0x000AC5EA
	public static bool SavesMenuOpen
	{
		get
		{
			return MonoSingleton<SandboxHud>.Instance && MonoSingleton<SandboxHud>.Instance.sandboxSavesWindow.activeSelf;
		}
	}

	// Token: 0x06001563 RID: 5475 RVA: 0x000AE409 File Offset: 0x000AC609
	private void Start()
	{
		this.navmeshNoticeGroup.alpha = 0f;
		this.UpdateNewSaveInput();
	}

	// Token: 0x06001564 RID: 5476 RVA: 0x000AE424 File Offset: 0x000AC624
	private void ResetSavesMenu()
	{
		this.sandboxSaveTemplate.gameObject.SetActive(false);
		for (int i = 1; i < this.savesContainer.childCount; i++)
		{
			Object.Destroy(this.savesContainer.GetChild(i).gameObject);
		}
	}

	// Token: 0x06001565 RID: 5477 RVA: 0x000AE470 File Offset: 0x000AC670
	private void BuildSavesMenu()
	{
		this.ResetSavesMenu();
		string[] array = MonoSingleton<SandboxSaver>.Instance.ListSaves();
		for (int i = 0; i < array.Length; i++)
		{
			string save = array[i];
			SandboxSaveItem sandboxSaveItem = Object.Instantiate<SandboxSaveItem>(this.sandboxSaveTemplate);
			sandboxSaveItem.transform.SetParent(this.savesContainer, false);
			sandboxSaveItem.gameObject.SetActive(true);
			sandboxSaveItem.saveName.text = save + "<color=#7A7A7A>.pitr</color>";
			sandboxSaveItem.deleteButton.onClick.AddListener(delegate
			{
				MonoSingleton<SandboxSaver>.Instance.Delete(save);
				this.BuildSavesMenu();
			});
			sandboxSaveItem.loadButton.onClick.AddListener(delegate
			{
				MonoSingleton<SandboxSaver>.Instance.Load(save);
				this.HideSavesMenu();
				MonoSingleton<CheatsManager>.Instance.HideMenu();
				if (MonoSingleton<OptionsManager>.Instance.paused)
				{
					MonoSingleton<OptionsManager>.Instance.UnPause();
				}
			});
			sandboxSaveItem.saveButton.onClick.AddListener(delegate
			{
				MonoSingleton<SandboxSaver>.Instance.Save(save);
				this.BuildSavesMenu();
			});
		}
	}

	// Token: 0x06001566 RID: 5478 RVA: 0x000AE54F File Offset: 0x000AC74F
	public void OpenDirectory()
	{
		Application.OpenURL("file://" + SandboxSaver.SavePath);
	}

	// Token: 0x06001567 RID: 5479 RVA: 0x000AE568 File Offset: 0x000AC768
	public void UpdateNewSaveInput()
	{
		if (string.IsNullOrEmpty(this.newSaveName.text))
		{
			this.newSaveButton.interactable = false;
			return;
		}
		this.newSaveButton.interactable = !Path.GetInvalidFileNameChars().Any(new Func<char, bool>(this.newSaveName.text.Contains));
	}

	// Token: 0x06001568 RID: 5480 RVA: 0x000AE5C2 File Offset: 0x000AC7C2
	public void NewSave()
	{
		MonoSingleton<SandboxSaver>.Instance.Save(this.newSaveName.text);
		this.newSaveName.text = string.Empty;
		this.UpdateNewSaveInput();
		this.BuildSavesMenu();
	}

	// Token: 0x06001569 RID: 5481 RVA: 0x000AE5F5 File Offset: 0x000AC7F5
	public void HideSavesMenu()
	{
		this.sandboxSavesWindow.SetActive(false);
	}

	// Token: 0x0600156A RID: 5482 RVA: 0x000AE603 File Offset: 0x000AC803
	public void ShowSavesMenu()
	{
		this.BuildSavesMenu();
		this.UpdateNewSaveInput();
		this.sandboxSavesWindow.SetActive(true);
	}

	// Token: 0x0600156B RID: 5483 RVA: 0x000AE61D File Offset: 0x000AC81D
	public void NavmeshDirty()
	{
		this.navmeshNoticeGroup.alpha = 1f;
		base.StopAllCoroutines();
		base.CancelInvoke("NavmeshStartFadeOut");
		base.Invoke("NavmeshStartFadeOut", 3f);
	}

	// Token: 0x0600156C RID: 5484 RVA: 0x000AE650 File Offset: 0x000AC850
	private void NavmeshStartFadeOut()
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.FadeOutNotice());
	}

	// Token: 0x0600156D RID: 5485 RVA: 0x000AE665 File Offset: 0x000AC865
	private IEnumerator FadeOutNotice()
	{
		this.navmeshNoticeGroup.alpha = 1f;
		yield return null;
		while (this.navmeshNoticeGroup.alpha > 0f)
		{
			this.navmeshNoticeGroup.alpha -= Time.deltaTime;
			yield return null;
		}
		this.HideNavmeshNotice();
		yield break;
	}

	// Token: 0x0600156E RID: 5486 RVA: 0x000AE674 File Offset: 0x000AC874
	public void HideNavmeshNotice()
	{
		this.navmeshNoticeGroup.alpha = 0f;
	}

	// Token: 0x04001DAB RID: 7595
	[Header("Nav")]
	[SerializeField]
	private CanvasGroup navmeshNoticeGroup;

	// Token: 0x04001DAC RID: 7596
	[Space]
	[SerializeField]
	private GameObject sandboxSavesWindow;

	// Token: 0x04001DAD RID: 7597
	[SerializeField]
	private SandboxSaveItem sandboxSaveTemplate;

	// Token: 0x04001DAE RID: 7598
	[SerializeField]
	private Transform savesContainer;

	// Token: 0x04001DAF RID: 7599
	[Space]
	[SerializeField]
	private InputField newSaveName;

	// Token: 0x04001DB0 RID: 7600
	[SerializeField]
	private Button newSaveButton;
}
