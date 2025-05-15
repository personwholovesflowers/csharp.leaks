using System;
using System.Collections;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200008F RID: 143
public class ApplyResolutionButton : MonoBehaviour
{
	// Token: 0x06000368 RID: 872 RVA: 0x00016C27 File Offset: 0x00014E27
	private void Awake()
	{
		this.textLocalization = this.text.GetComponent<Localize>();
		this.textLocalization2 = this.text2.GetComponent<Localize>();
	}

	// Token: 0x06000369 RID: 873 RVA: 0x00016C4B File Offset: 0x00014E4B
	private void OnEnable()
	{
		this.ConfirmResolution();
	}

	// Token: 0x0600036A RID: 874 RVA: 0x00016C53 File Offset: 0x00014E53
	private void OnDisable()
	{
		base.StopAllCoroutines();
		this.inConfirmStage = false;
	}

	// Token: 0x0600036B RID: 875 RVA: 0x00016C62 File Offset: 0x00014E62
	public void PressedButton()
	{
		if (this.inConfirmStage)
		{
			this.ConfirmResolution();
			return;
		}
		this.ApplySelectedResolutionAndFullScreenMode();
	}

	// Token: 0x0600036C RID: 876 RVA: 0x00016C7C File Offset: 0x00014E7C
	private void ApplySelectedResolutionAndFullScreenMode()
	{
		FullScreenMode selectedFullScreenMode = ResolutionController.Instance.SelectedFullScreenMode;
		FullScreenMode currentFullScreenMode = ResolutionController.Instance.CurrentFullScreenMode;
		int selectedResolutionIndex = ResolutionController.Instance.SelectedResolutionIndex;
		int currentResolutionIndex = ResolutionController.Instance.CurrentResolutionIndex;
		ResolutionController.Instance.ApplyResolutionAtIndex(selectedResolutionIndex, selectedFullScreenMode);
		this.oldResolutionIndexDEBUG = currentResolutionIndex;
		this.oldFullscreenModeDEBUG = currentFullScreenMode;
		base.StartCoroutine(this.StartCountdown(currentResolutionIndex, currentFullScreenMode));
	}

	// Token: 0x0600036D RID: 877 RVA: 0x00016CDE File Offset: 0x00014EDE
	private IEnumerator StartCountdown(int oldResolutionIndex, FullScreenMode oldFullscreenModeIndex)
	{
		this.textLocalization.enabled = false;
		this.textLocalization2.enabled = false;
		this.inConfirmStage = true;
		int num;
		for (int i = 15; i >= 0; i = num - 1)
		{
			this.text.text = LocalizationManager.GetTranslation(this.keepChangesLocalizationTerm, true, 0, true, false, null, null).Replace("%!C!%", string.Format("{0}", i));
			this.text2.text = this.text.text;
			base.StartCoroutine(this.RefreshCanvasesAndText());
			yield return new WaitForSecondsRealtime(1f);
			num = i;
		}
		this.inConfirmStage = false;
		this.RevertResolutionAndFSMode(oldResolutionIndex, oldFullscreenModeIndex);
		yield break;
	}

	// Token: 0x0600036E RID: 878 RVA: 0x00016CFB File Offset: 0x00014EFB
	private void RevertResolutionAndFSMode(int oldResolutionIndex, FullScreenMode oldFullscreenModeIndex)
	{
		ResolutionController.Instance.ApplyResolutionAtIndex(oldResolutionIndex, oldFullscreenModeIndex);
		this.ResetLabel();
	}

	// Token: 0x0600036F RID: 879 RVA: 0x00016D0F File Offset: 0x00014F0F
	private void ConfirmResolution()
	{
		base.StopAllCoroutines();
		this.inConfirmStage = false;
		this.ResetLabel();
	}

	// Token: 0x06000370 RID: 880 RVA: 0x00016D24 File Offset: 0x00014F24
	private void ResetLabel()
	{
		if (this.textLocalization != null)
		{
			this.textLocalization.enabled = true;
			this.textLocalization.OnLocalize(true);
			this.textLocalization2.enabled = true;
			this.textLocalization2.OnLocalize(true);
			base.StartCoroutine(this.RefreshCanvasesAndText());
		}
	}

	// Token: 0x06000371 RID: 881 RVA: 0x00016D7C File Offset: 0x00014F7C
	private IEnumerator RefreshCanvasesAndText()
	{
		Canvas.ForceUpdateCanvases();
		this.text.GetComponent<ContentSizeFitter>().enabled = false;
		this.text.GetComponent<ContentSizeFitter>().enabled = true;
		this.text2.GetComponent<ContentSizeFitter>().enabled = false;
		this.text2.GetComponent<ContentSizeFitter>().enabled = true;
		yield return null;
		Canvas.ForceUpdateCanvases();
		this.text.GetComponent<ContentSizeFitter>().enabled = false;
		this.text.GetComponent<ContentSizeFitter>().enabled = true;
		this.text2.GetComponent<ContentSizeFitter>().enabled = false;
		this.text2.GetComponent<ContentSizeFitter>().enabled = true;
		yield break;
	}

	// Token: 0x0400035D RID: 861
	[SerializeField]
	private ResolutionConfigurator resolutionConfigurator;

	// Token: 0x0400035E RID: 862
	[SerializeField]
	private FullscreenModeConfigurator fullscreenConfigurator;

	// Token: 0x0400035F RID: 863
	[SerializeField]
	private TextMeshProUGUI text;

	// Token: 0x04000360 RID: 864
	[SerializeField]
	private TextMeshProUGUI text2;

	// Token: 0x04000361 RID: 865
	private Localize textLocalization;

	// Token: 0x04000362 RID: 866
	private Localize textLocalization2;

	// Token: 0x04000363 RID: 867
	[SerializeField]
	private string keepChangesLocalizationTerm = "Menu_KeepResolution";

	// Token: 0x04000364 RID: 868
	private int oldResolutionIndexDEBUG;

	// Token: 0x04000365 RID: 869
	private FullScreenMode oldFullscreenModeDEBUG;

	// Token: 0x04000366 RID: 870
	private bool inConfirmStage;
}
