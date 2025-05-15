using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Token: 0x020003C1 RID: 961
public class SaveSlotMenu : MonoBehaviour
{
	// Token: 0x060015DE RID: 5598 RVA: 0x000B16B0 File Offset: 0x000AF8B0
	private void OnEnable()
	{
		if (this.slots == null)
		{
			List<SlotRowPanel> list = new List<SlotRowPanel>();
			SaveSlotMenu.SlotData[] array = GameProgressSaver.GetSlots();
			for (int i = 0; i < 5; i++)
			{
				SlotRowPanel newRow = Object.Instantiate<SlotRowPanel>(this.templateRow, this.templateRow.transform.parent);
				newRow.slotIndex = i;
				newRow.gameObject.SetActive(true);
				this.UpdateSlotState(newRow, array[i]);
				newRow.selectButton.onClick.AddListener(delegate
				{
					this.SelectSlot(newRow.slotIndex);
				});
				newRow.deleteButton.onClick.AddListener(delegate
				{
					this.ClearSlot(newRow.slotIndex);
				});
				list.Add(newRow);
			}
			Navigation navigation;
			for (int j = 0; j < 5; j++)
			{
				Selectable selectButton = list[j].selectButton;
				navigation = new Navigation
				{
					mode = Navigation.Mode.Explicit,
					selectOnUp = ((j > 0) ? list[j - 1].selectButton : null),
					selectOnDown = ((j + 1 < 5) ? list[j + 1].selectButton : this.closeButton),
					selectOnLeft = list[j].deleteButton,
					selectOnRight = list[j].deleteButton
				};
				selectButton.navigation = navigation;
				Selectable deleteButton = list[j].deleteButton;
				navigation = new Navigation
				{
					mode = Navigation.Mode.Explicit,
					selectOnUp = ((j > 0) ? list[j - 1].deleteButton : null),
					selectOnDown = ((j + 1 < 5) ? list[j + 1].deleteButton : this.closeButton),
					selectOnLeft = list[j].selectButton,
					selectOnRight = list[j].selectButton
				};
				deleteButton.navigation = navigation;
			}
			Selectable selectable = this.closeButton;
			navigation = new Navigation
			{
				mode = Navigation.Mode.Explicit,
				selectOnUp = list[4].selectButton,
				selectOnDown = list[0].selectButton
			};
			selectable.navigation = navigation;
			this.slots = list.ToArray();
			this.templateRow.gameObject.SetActive(false);
		}
	}

	// Token: 0x060015DF RID: 5599 RVA: 0x000B1924 File Offset: 0x000AFB24
	public void ReloadMenu()
	{
		if (this.slots != null)
		{
			SlotRowPanel[] array = this.slots;
			for (int i = 0; i < array.Length; i++)
			{
				Object.Destroy(array[i].gameObject);
			}
			this.slots = null;
		}
		this.wipeConsentWrapper.SetActive(false);
		this.reloadConsentWrapper.SetActive(false);
		this.OnEnable();
	}

	// Token: 0x060015E0 RID: 5600 RVA: 0x000B1980 File Offset: 0x000AFB80
	private void UpdateSlotState(SlotRowPanel targetPanel, SaveSlotMenu.SlotData data)
	{
		bool flag = GameProgressSaver.currentSlot == targetPanel.slotIndex;
		targetPanel.backgroundPanel.color = (flag ? SaveSlotMenu.ActiveColor : Color.black);
		targetPanel.slotNumberLabel.color = (flag ? Color.black : (data.exists ? Color.white : Color.red));
		targetPanel.stateLabel.color = (flag ? Color.black : (data.exists ? Color.white : Color.red));
		targetPanel.selectButton.interactable = !flag;
		targetPanel.selectButton.GetComponentInChildren<TMP_Text>().text = (flag ? "SELECTED" : "SELECT");
		targetPanel.deleteButton.interactable = data.exists;
		targetPanel.slotNumberLabel.text = string.Format("Slot {0}", targetPanel.slotIndex + 1);
		targetPanel.stateLabel.text = data.ToString();
	}

	// Token: 0x060015E1 RID: 5601 RVA: 0x000B1A79 File Offset: 0x000AFC79
	public void ConfirmReload()
	{
		GameProgressSaver.SetSlot(this.queuedSlot);
		SceneHelper.LoadScene("Main Menu", false);
	}

	// Token: 0x060015E2 RID: 5602 RVA: 0x000B1A91 File Offset: 0x000AFC91
	public void ConfirmWipe()
	{
		int currentSlot = GameProgressSaver.currentSlot;
		GameProgressSaver.WipeSlot(this.queuedSlot);
		if (currentSlot == this.queuedSlot)
		{
			SceneHelper.LoadScene("Main Menu", false);
			return;
		}
		this.ReloadMenu();
	}

	// Token: 0x060015E3 RID: 5603 RVA: 0x000B1ABD File Offset: 0x000AFCBD
	public void CancelConsent()
	{
		this.reloadConsentWrapper.SetActive(false);
		this.wipeConsentWrapper.SetActive(false);
	}

	// Token: 0x060015E4 RID: 5604 RVA: 0x000B1AD7 File Offset: 0x000AFCD7
	private void SelectSlot(int slot)
	{
		this.queuedSlot = slot;
		this.reloadConsentWrapper.SetActive(true);
	}

	// Token: 0x060015E5 RID: 5605 RVA: 0x000B1AEC File Offset: 0x000AFCEC
	private void ClearSlot(int slot)
	{
		this.queuedSlot = slot;
		this.wipeConsentContent.text = string.Format("Are you sure you want to <color=red>DELETE SLOT {0}</color>?", slot + 1);
		this.wipeConsentWrapper.SetActive(true);
	}

	// Token: 0x04001E1F RID: 7711
	public const int Slots = 5;

	// Token: 0x04001E20 RID: 7712
	private static readonly Color ActiveColor = new Color(1f, 0.66f, 0f);

	// Token: 0x04001E21 RID: 7713
	[SerializeField]
	private SlotRowPanel templateRow;

	// Token: 0x04001E22 RID: 7714
	[SerializeField]
	private Button closeButton;

	// Token: 0x04001E23 RID: 7715
	[FormerlySerializedAs("consentWrapper")]
	[SerializeField]
	private GameObject reloadConsentWrapper;

	// Token: 0x04001E24 RID: 7716
	[SerializeField]
	private TMP_Text wipeConsentContent;

	// Token: 0x04001E25 RID: 7717
	[SerializeField]
	private GameObject wipeConsentWrapper;

	// Token: 0x04001E26 RID: 7718
	private int queuedSlot;

	// Token: 0x04001E27 RID: 7719
	private SlotRowPanel[] slots;

	// Token: 0x020003C2 RID: 962
	public class SlotData
	{
		// Token: 0x060015E8 RID: 5608 RVA: 0x000B1B3C File Offset: 0x000AFD3C
		public override string ToString()
		{
			if (!this.exists)
			{
				return "EMPTY";
			}
			return GetMissionName.GetMission(this.highestLvlNumber) + " " + ((this.highestLvlNumber <= 0) ? string.Empty : ("(" + MonoSingleton<PresenceController>.Instance.diffNames[this.highestDifficulty] + ")"));
		}

		// Token: 0x04001E28 RID: 7720
		public bool exists;

		// Token: 0x04001E29 RID: 7721
		public int highestLvlNumber;

		// Token: 0x04001E2A RID: 7722
		public int highestDifficulty;
	}
}
