using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003BF RID: 959
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class SaveLoadFailMessage : MonoSingleton<SaveLoadFailMessage>
{
	// Token: 0x060015D4 RID: 5588 RVA: 0x000B1481 File Offset: 0x000AF681
	public static void DisplayMergeConsent(SaveSlotMenu.SlotData rootSlot, SaveSlotMenu.SlotData slotOneData)
	{
		if (!MonoSingleton<SaveLoadFailMessage>.Instance)
		{
			SaveLoadFailMessage.queuedRootSlot = rootSlot;
			SaveLoadFailMessage.queuedSlotOneData = slotOneData;
			SaveLoadFailMessage.consentQueued = true;
			return;
		}
		MonoSingleton<SaveLoadFailMessage>.Instance.DisplayMergeConsentInstance(rootSlot, slotOneData);
	}

	// Token: 0x060015D5 RID: 5589 RVA: 0x000B14B0 File Offset: 0x000AF6B0
	private void DisplayMergeConsentInstance(SaveSlotMenu.SlotData rootSlot, SaveSlotMenu.SlotData slotOneData)
	{
		this.rootMergeOptionBtnText.text = string.Format("<b>Saves{0}</b>\n{1}", Path.DirectorySeparatorChar, rootSlot);
		this.slotOneMergeOptionBtnText.text = string.Format("<b>Saves{0}Slot1{1}</b>\n{2}", Path.DirectorySeparatorChar, Path.DirectorySeparatorChar, slotOneData);
		this.saveMergeConsentPanel.SetActive(true);
	}

	// Token: 0x060015D6 RID: 5590 RVA: 0x000B1513 File Offset: 0x000AF713
	private new void OnEnable()
	{
		if (SaveLoadFailMessage.consentQueued)
		{
			SaveLoadFailMessage.consentQueued = false;
			this.DisplayMergeConsentInstance(SaveLoadFailMessage.queuedRootSlot, SaveLoadFailMessage.queuedSlotOneData);
		}
	}

	// Token: 0x060015D7 RID: 5591 RVA: 0x000B1532 File Offset: 0x000AF732
	public void ConfirmMergeRoot()
	{
		GameProgressSaver.MergeRootWithSlotOne(true);
		SceneHelper.LoadScene("Main Menu", false);
	}

	// Token: 0x060015D8 RID: 5592 RVA: 0x000B1545 File Offset: 0x000AF745
	public void ConfirmMergeFirstSlot()
	{
		GameProgressSaver.MergeRootWithSlotOne(false);
		SceneHelper.LoadScene("Main Menu", false);
	}

	// Token: 0x060015D9 RID: 5593 RVA: 0x00094CC4 File Offset: 0x00092EC4
	public void QuitGame()
	{
		if (SceneHelper.IsPlayingCustom)
		{
			return;
		}
		Application.Quit();
	}

	// Token: 0x060015DA RID: 5594 RVA: 0x000B1558 File Offset: 0x000AF758
	public void ShowError(SaveLoadFailMessage.SaveLoadError error, string errorCode, Action saveRedo)
	{
		this.currentError = error;
		if (error != SaveLoadFailMessage.SaveLoadError.Generic)
		{
			if (error == SaveLoadFailMessage.SaveLoadError.TempValidation)
			{
				this.potentialSaveRedo = saveRedo;
				this.errorTempValidation.SetActive(true);
				if (!string.IsNullOrEmpty(errorCode))
				{
					this.tempErrorCode.text = errorCode;
					this.tempErrorCode.gameObject.SetActive(true);
				}
			}
		}
		else
		{
			this.errorGeneric.SetActive(true);
		}
		this.beenActivated = true;
	}

	// Token: 0x060015DB RID: 5595 RVA: 0x000B15C4 File Offset: 0x000AF7C4
	private void HideAll()
	{
		foreach (object obj in base.transform)
		{
			((Transform)obj).gameObject.SetActive(false);
		}
		this.beenActivated = false;
	}

	// Token: 0x060015DC RID: 5596 RVA: 0x000B1628 File Offset: 0x000AF828
	private void Update()
	{
		if (this.beenActivated)
		{
			SaveLoadFailMessage.SaveLoadError saveLoadError = this.currentError;
			if (saveLoadError != SaveLoadFailMessage.SaveLoadError.Generic)
			{
				if (saveLoadError != SaveLoadFailMessage.SaveLoadError.TempValidation)
				{
					return;
				}
				if (Input.GetKeyDown(KeyCode.Y))
				{
					if (this.potentialSaveRedo == null)
					{
						return;
					}
					this.HideAll();
					this.potentialSaveRedo();
				}
				if (Input.GetKeyDown(KeyCode.N))
				{
					this.HideAll();
					MonoSingleton<ProgressChecker>.Instance.DisableSaving();
				}
			}
			else
			{
				if (Input.GetKeyDown(KeyCode.Y))
				{
					this.HideAll();
				}
				if (Input.GetKeyDown(KeyCode.N))
				{
					Application.Quit();
					return;
				}
			}
		}
	}

	// Token: 0x04001E10 RID: 7696
	[SerializeField]
	private GameObject saveMergeConsentPanel;

	// Token: 0x04001E11 RID: 7697
	[SerializeField]
	private Text rootMergeOptionBtnText;

	// Token: 0x04001E12 RID: 7698
	[SerializeField]
	private Text slotOneMergeOptionBtnText;

	// Token: 0x04001E13 RID: 7699
	[SerializeField]
	private GameObject errorGeneric;

	// Token: 0x04001E14 RID: 7700
	[SerializeField]
	private GameObject errorTempValidation;

	// Token: 0x04001E15 RID: 7701
	[SerializeField]
	private Text tempErrorCode;

	// Token: 0x04001E16 RID: 7702
	private SaveLoadFailMessage.SaveLoadError currentError;

	// Token: 0x04001E17 RID: 7703
	private bool beenActivated;

	// Token: 0x04001E18 RID: 7704
	private Action potentialSaveRedo;

	// Token: 0x04001E19 RID: 7705
	private static bool consentQueued;

	// Token: 0x04001E1A RID: 7706
	private static SaveSlotMenu.SlotData queuedRootSlot;

	// Token: 0x04001E1B RID: 7707
	private static SaveSlotMenu.SlotData queuedSlotOneData;

	// Token: 0x020003C0 RID: 960
	public enum SaveLoadError
	{
		// Token: 0x04001E1D RID: 7709
		Generic,
		// Token: 0x04001E1E RID: 7710
		TempValidation
	}
}
