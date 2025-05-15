using System;
using TMPro;
using UnityEngine;

// Token: 0x0200012B RID: 299
public class SubtitlePreview : MonoBehaviour
{
	// Token: 0x0600070E RID: 1806 RVA: 0x00024F85 File Offset: 0x00023185
	private void Start()
	{
		IntConfigurable intConfigurable = this.subtitleIndex;
		intConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(intConfigurable.OnValueChanged, new Action<LiveData>(this.SubtitleSizeChange));
		this.SubtitleSizeChange(null);
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x00024FB8 File Offset: 0x000231B8
	private void SubtitleSizeChange(LiveData liveData)
	{
		SubtitleSizeProfile subtitleSizeProfile = this.subtitleSizeProfiles.sizeProfiles[this.subtitleIndex.GetIntValue()];
		this.subtitelPreviewText.fontSize = this.defaultFontSize / (subtitleSizeProfile.uiReferenceHeight / 1080f);
	}

	// Token: 0x04000747 RID: 1863
	[SerializeField]
	private TMP_Text subtitelPreviewText;

	// Token: 0x04000748 RID: 1864
	[SerializeField]
	private float defaultFontSize = 30f;

	// Token: 0x04000749 RID: 1865
	[SerializeField]
	private SubtitleSizeProfileData subtitleSizeProfiles;

	// Token: 0x0400074A RID: 1866
	[SerializeField]
	private IntConfigurable subtitleIndex;
}
