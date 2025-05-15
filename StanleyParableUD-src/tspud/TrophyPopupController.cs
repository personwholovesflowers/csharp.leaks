using System;
using UnityEngine;

// Token: 0x020001BE RID: 446
public class TrophyPopupController : MonoBehaviour
{
	// Token: 0x170000DD RID: 221
	// (get) Token: 0x06000A4E RID: 2638 RVA: 0x000309A9 File Offset: 0x0002EBA9
	// (set) Token: 0x06000A4F RID: 2639 RVA: 0x000309B0 File Offset: 0x0002EBB0
	public static TrophyPopupController Instance { get; private set; }

	// Token: 0x06000A50 RID: 2640 RVA: 0x000309B8 File Offset: 0x0002EBB8
	private void Awake()
	{
		TrophyPopupController.Instance = this;
	}

	// Token: 0x06000A51 RID: 2641 RVA: 0x000309C0 File Offset: 0x0002EBC0
	private void OnEnable()
	{
		PlatformAchievements.AchievementUnlocked = (Action<AchievementID>)Delegate.Remove(PlatformAchievements.AchievementUnlocked, new Action<AchievementID>(this.ShowTrophyPopup));
		PlatformAchievements.AchievementUnlocked = (Action<AchievementID>)Delegate.Combine(PlatformAchievements.AchievementUnlocked, new Action<AchievementID>(this.ShowTrophyPopup));
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x00030A0D File Offset: 0x0002EC0D
	private void OnDisable()
	{
		PlatformAchievements.AchievementUnlocked = (Action<AchievementID>)Delegate.Remove(PlatformAchievements.AchievementUnlocked, new Action<AchievementID>(this.ShowTrophyPopup));
	}

	// Token: 0x06000A53 RID: 2643 RVA: 0x00005444 File Offset: 0x00003644
	private void OnDestroy()
	{
	}

	// Token: 0x06000A54 RID: 2644 RVA: 0x00030A2F File Offset: 0x0002EC2F
	public void FireTestTrophyUI()
	{
		PlatformAchievements.UnlockAchievement((AchievementID)Random.Range(0, 11));
	}

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x06000A55 RID: 2645 RVA: 0x00030A3E File Offset: 0x0002EC3E
	private bool IsCanvasGroupVisible
	{
		get
		{
			return this.displayCanvasGroup.alpha > 0f;
		}
	}

	// Token: 0x06000A56 RID: 2646 RVA: 0x00030A54 File Offset: 0x0002EC54
	public void ShowTrophyPopup(AchievementID achID)
	{
		if (!this.IsCanvasGroupVisible)
		{
			return;
		}
		int num = 0;
		TrophyPopupElement[] componentsInChildren = base.GetComponentsInChildren<TrophyPopupElement>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i] != null)
			{
				num++;
			}
		}
		TrophyPopupElement component = Object.Instantiate<GameObject>(this.trophyPrefab.gameObject).GetComponent<TrophyPopupElement>();
		component.transform.parent = base.transform;
		component.transform.localScale = Vector3.one;
		component.ID = achID;
		component.GetComponent<RectTransform>().anchoredPosition = Vector3.up * this.elementHeight * (float)num;
	}

	// Token: 0x04000A48 RID: 2632
	[InspectorButton("FireTestTrophyUI", "Fire Test Trophy UI")]
	public TrophyPopupElement trophyPrefab;

	// Token: 0x04000A49 RID: 2633
	[InspectorButton("Clear", "Clear")]
	public float elementHeight = 170f;

	// Token: 0x04000A4A RID: 2634
	[SerializeField]
	private CanvasGroup displayCanvasGroup;
}
