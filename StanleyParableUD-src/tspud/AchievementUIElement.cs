using System;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200010C RID: 268
public class AchievementUIElement : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
{
	// Token: 0x06000684 RID: 1668 RVA: 0x0002323F File Offset: 0x0002143F
	public void OnSelect(BaseEventData eventData)
	{
		this.animator.SetBool("Selected", true);
	}

	// Token: 0x06000685 RID: 1669 RVA: 0x00023252 File Offset: 0x00021452
	public void OnDeselect(BaseEventData eventData)
	{
		this.animator.SetBool("Selected", false);
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x00023265 File Offset: 0x00021465
	private void Start()
	{
		Singleton<GameMaster>.Instance.AchievementsData.FindAchievement(this.achievementID).dateFoundConfigurable.Init();
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x00023286 File Offset: 0x00021486
	private void UpdateUI()
	{
		this.UpdateUI(false);
	}

	// Token: 0x06000688 RID: 1672 RVA: 0x00023290 File Offset: 0x00021490
	public void UpdateUI(bool calledAchievements)
	{
		AchievementData achievementData = Singleton<GameMaster>.Instance.AchievementsData.FindAchievement(this.achievementID);
		if (achievementData == null)
		{
			return;
		}
		this.foundImage.sprite = achievementData.textureFound;
		this.unfoundImage.sprite = achievementData.textureNotFound;
		this.titleTextLoc.SetTerm(achievementData.TitleTerm(calledAchievements));
		this.descriptionTextLoc.SetTerm(achievementData.DescriptionTerm(calledAchievements));
		if (Application.isPlaying)
		{
			bool flag = PlatformAchievements.IsAchievementUnlocked(this.achievementID);
			this.animator.SetBool("Locked", !flag);
			string text = achievementData.dateFoundConfigurable.GetStringValue();
			if (Debug.isDebugBuild && flag && text == "")
			{
				text = "8/3/2026, 3:15 PM";
			}
			this.dateFoundText.text = text;
		}
	}

	// Token: 0x040006D8 RID: 1752
	[InspectorButton("UpdateUI", null)]
	public AchievementID achievementID;

	// Token: 0x040006D9 RID: 1753
	[Header("Dynamic UI Sub-Elements")]
	public Image foundImage;

	// Token: 0x040006DA RID: 1754
	public Image unfoundImage;

	// Token: 0x040006DB RID: 1755
	public Localize titleTextLoc;

	// Token: 0x040006DC RID: 1756
	public Localize descriptionTextLoc;

	// Token: 0x040006DD RID: 1757
	public TextMeshProUGUI dateFoundText;

	// Token: 0x040006DE RID: 1758
	public Animator animator;
}
