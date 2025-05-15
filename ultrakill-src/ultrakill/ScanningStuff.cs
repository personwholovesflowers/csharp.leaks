using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003C8 RID: 968
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class ScanningStuff : MonoSingleton<ScanningStuff>
{
	// Token: 0x17000191 RID: 401
	// (get) Token: 0x06001602 RID: 5634 RVA: 0x000B22D7 File Offset: 0x000B04D7
	public bool IsReading
	{
		get
		{
			return this.readingPanel.activeInHierarchy;
		}
	}

	// Token: 0x06001603 RID: 5635 RVA: 0x000B22E4 File Offset: 0x000B04E4
	public void ReleaseScroll(int instanceId)
	{
		if (this.bookScrollStates.ContainsKey(instanceId))
		{
			this.bookScrollStates.Remove(instanceId);
		}
	}

	// Token: 0x06001604 RID: 5636 RVA: 0x000B2304 File Offset: 0x000B0504
	public void ScanBook(string text, bool noScan, int instanceId)
	{
		this.oldWeaponState = !MonoSingleton<GunControl>.Instance.noWeapons;
		MonoSingleton<GunControl>.Instance.NoWeapon();
		this.readingText.text = text;
		if (this.bookScrollStates.ContainsKey(instanceId))
		{
			this.scrollRect.verticalNormalizedPosition = this.bookScrollStates[instanceId];
		}
		else
		{
			this.scrollRect.verticalNormalizedPosition = 1f;
		}
		this.currentBookId = instanceId;
		if (noScan || (this.scannedBooks.ContainsKey(instanceId) && this.scannedBooks[instanceId]))
		{
			this.scanningPanel.SetActive(false);
			this.readingPanel.SetActive(true);
			this.scanning = false;
			return;
		}
		this.scanningPanel.SetActive(true);
		this.readingPanel.SetActive(false);
		this.scanning = true;
		this.loading = 0f;
		this.meter.fillAmount = 0f;
	}

	// Token: 0x06001605 RID: 5637 RVA: 0x000B23F4 File Offset: 0x000B05F4
	public void ResetState()
	{
		if (this.bookScrollStates.ContainsKey(this.currentBookId))
		{
			this.bookScrollStates[this.currentBookId] = this.scrollRect.verticalNormalizedPosition;
		}
		else
		{
			this.bookScrollStates.Add(this.currentBookId, this.scrollRect.verticalNormalizedPosition);
		}
		this.scanning = false;
		this.loading = 0f;
		this.meter.fillAmount = 0f;
		this.scanningPanel.SetActive(false);
		this.readingPanel.SetActive(false);
		this.currentBookId = -1;
		this.scrollRect.verticalNormalizedPosition = 1f;
		if (this.oldWeaponState)
		{
			MonoSingleton<GunControl>.Instance.YesWeapon();
			return;
		}
		MonoSingleton<GunControl>.Instance.NoWeapon();
	}

	// Token: 0x06001606 RID: 5638 RVA: 0x000B24BC File Offset: 0x000B06BC
	private void Update()
	{
		if (this.scanning)
		{
			this.loading = Mathf.MoveTowards(this.loading, 1f, Time.deltaTime / 2f);
			this.meter.fillAmount = this.loading;
			if (this.loading == 1f)
			{
				this.scanning = false;
				this.scanningPanel.SetActive(false);
				this.readingPanel.SetActive(true);
				this.scannedBooks.Add(this.currentBookId, true);
			}
		}
	}

	// Token: 0x04001E4A RID: 7754
	[SerializeField]
	private GameObject scanningPanel;

	// Token: 0x04001E4B RID: 7755
	[SerializeField]
	private GameObject readingPanel;

	// Token: 0x04001E4C RID: 7756
	[SerializeField]
	private TMP_Text readingText;

	// Token: 0x04001E4D RID: 7757
	[SerializeField]
	private ScrollRect scrollRect;

	// Token: 0x04001E4E RID: 7758
	public Image meter;

	// Token: 0x04001E4F RID: 7759
	private float loading;

	// Token: 0x04001E50 RID: 7760
	private bool scanning;

	// Token: 0x04001E51 RID: 7761
	private Dictionary<int, bool> scannedBooks = new Dictionary<int, bool>();

	// Token: 0x04001E52 RID: 7762
	private Dictionary<int, float> bookScrollStates = new Dictionary<int, float>();

	// Token: 0x04001E53 RID: 7763
	private int currentBookId;

	// Token: 0x04001E54 RID: 7764
	public bool oldWeaponState;
}
