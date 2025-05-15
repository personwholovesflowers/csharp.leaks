using System;
using I2.Loc;
using TMPro;
using UnityEngine;

// Token: 0x020001BC RID: 444
public class TrophiesUnlockedUI : MonoBehaviour
{
	// Token: 0x170000DC RID: 220
	// (get) Token: 0x06000A47 RID: 2631 RVA: 0x00030794 File Offset: 0x0002E994
	public bool useFakes
	{
		get
		{
			return !Application.isPlaying;
		}
	}

	// Token: 0x06000A48 RID: 2632 RVA: 0x0003079E File Offset: 0x0002E99E
	private void Start()
	{
		LocalizationManager.OnLocalizeEvent += this.UpdateUI;
		BooleanConfigurable booleanConfigurable = this.calledAchievementsConfigurable;
		booleanConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(booleanConfigurable.OnValueChanged, new Action<LiveData>(this.UpdateUI));
	}

	// Token: 0x06000A49 RID: 2633 RVA: 0x000307D8 File Offset: 0x0002E9D8
	private void OnDestroy()
	{
		LocalizationManager.OnLocalizeEvent -= this.UpdateUI;
		BooleanConfigurable booleanConfigurable = this.calledAchievementsConfigurable;
		booleanConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(booleanConfigurable.OnValueChanged, new Action<LiveData>(this.UpdateUI));
	}

	// Token: 0x06000A4A RID: 2634 RVA: 0x00030812 File Offset: 0x0002EA12
	public void UpdateUI(LiveData data)
	{
		this.UpdateUI();
	}

	// Token: 0x06000A4B RID: 2635 RVA: 0x0003081C File Offset: 0x0002EA1C
	public void UpdateUI()
	{
		int num = (this.useFakes ? this.fakeCount : PlatformAchievements.AchievementsUnlockedCount);
		int num2 = (this.useFakes ? this.fakeTotal : PlatformAchievements.AchievementsCount);
		bool flag = (this.useFakes ? this.fakeCalledAchievement : this.calledAchievementsConfigurable.GetBooleanValue());
		string text = ((num == num2) ? this.completeTerm : this.progressTerm);
		if (!flag)
		{
			text += this.trophyTermSuffix;
		}
		string text2 = LocalizationManager.GetTranslation(text, true, 0, true, false, null, null);
		if (text2 == null)
		{
			Debug.LogError("Could not find translation");
			return;
		}
		text2 = text2.Replace("%!NUM!%", string.Format("{0}", num));
		text2 = text2.Replace("%!TOTAL!%", string.Format("{0}", num2));
		this.text.text = text2;
		float num3 = (float)num / (float)num2;
		this.percentageText.text = string.Format("{0:0}%", Mathf.FloorToInt(num3 * 100f));
		this.fillImage.anchorMax = new Vector2(num3, 1f);
		AchievementUIElement[] componentsInChildren = this.achievementUIHolder.GetComponentsInChildren<AchievementUIElement>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].UpdateUI(flag);
		}
	}

	// Token: 0x04000A3C RID: 2620
	public TextMeshProUGUI text;

	// Token: 0x04000A3D RID: 2621
	public string progressTerm = "Achievement_Status_Progress";

	// Token: 0x04000A3E RID: 2622
	public string completeTerm = "Achievement_Status_Complete";

	// Token: 0x04000A3F RID: 2623
	public string trophyTermSuffix = "_TROPHY";

	// Token: 0x04000A40 RID: 2624
	public BooleanConfigurable calledAchievementsConfigurable;

	// Token: 0x04000A41 RID: 2625
	public TextMeshProUGUI percentageText;

	// Token: 0x04000A42 RID: 2626
	public RectTransform fillImage;

	// Token: 0x04000A43 RID: 2627
	[Header("Location of the Achievement UI elements")]
	public Transform achievementUIHolder;

	// Token: 0x04000A44 RID: 2628
	[Header("Editor fake values for testing")]
	public int fakeCount = 4;

	// Token: 0x04000A45 RID: 2629
	public int fakeTotal = 12;

	// Token: 0x04000A46 RID: 2630
	[InspectorButton("UpdateUI", null)]
	public bool fakeCalledAchievement = true;
}
