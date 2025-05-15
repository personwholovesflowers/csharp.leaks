using System;
using I2.Loc;
using TMPro;
using UnityEngine;

// Token: 0x02000183 RID: 387
public class SequelTitleTextFormatter : MonoBehaviour
{
	// Token: 0x0600090D RID: 2317 RVA: 0x0002B068 File Offset: 0x00029268
	private void Start()
	{
		this.UpdateTitleName();
		LocalizationManager.OnLocalizeEvent += this.UpdateTitleName;
		IntConfigurable intConfigurable = this.sequelCountConfigurable;
		intConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(intConfigurable.OnValueChanged, new Action<LiveData>(this.UpdateTitleName));
		IntConfigurable intConfigurable2 = this.prefixIndexConfigurable;
		intConfigurable2.OnValueChanged = (Action<LiveData>)Delegate.Combine(intConfigurable2.OnValueChanged, new Action<LiveData>(this.UpdateTitleName));
		IntConfigurable intConfigurable3 = this.postfixIndexConfigurable;
		intConfigurable3.OnValueChanged = (Action<LiveData>)Delegate.Combine(intConfigurable3.OnValueChanged, new Action<LiveData>(this.UpdateTitleName));
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x0002B104 File Offset: 0x00029304
	private void OnDestroy()
	{
		LocalizationManager.OnLocalizeEvent -= this.UpdateTitleName;
		IntConfigurable intConfigurable = this.sequelCountConfigurable;
		intConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(intConfigurable.OnValueChanged, new Action<LiveData>(this.UpdateTitleName));
		IntConfigurable intConfigurable2 = this.prefixIndexConfigurable;
		intConfigurable2.OnValueChanged = (Action<LiveData>)Delegate.Remove(intConfigurable2.OnValueChanged, new Action<LiveData>(this.UpdateTitleName));
		IntConfigurable intConfigurable3 = this.postfixIndexConfigurable;
		intConfigurable3.OnValueChanged = (Action<LiveData>)Delegate.Remove(intConfigurable3.OnValueChanged, new Action<LiveData>(this.UpdateTitleName));
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x0002B197 File Offset: 0x00029397
	public void ForceUpdateTitleName()
	{
		this.UpdateTitleName();
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x0002B197 File Offset: 0x00029397
	private void UpdateTitleName(LiveData ld)
	{
		this.UpdateTitleName();
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x0002B19F File Offset: 0x0002939F
	private void UpdateTitleName()
	{
		this.titleText.text = SequelTools.DoStandardSequelReplacementStep(this.unformattedString, this.sequelCountConfigurable, this.prefixIndexConfigurable, this.postfixIndexConfigurable).ToUpper();
	}

	// Token: 0x040008E8 RID: 2280
	public TMP_Text titleText;

	// Token: 0x040008E9 RID: 2281
	public IntConfigurable sequelCountConfigurable;

	// Token: 0x040008EA RID: 2282
	public IntConfigurable prefixIndexConfigurable;

	// Token: 0x040008EB RID: 2283
	public IntConfigurable postfixIndexConfigurable;

	// Token: 0x040008EC RID: 2284
	[Multiline(2)]
	public string unformattedString = "The Stanley Parable %!N!%:\n%!P!% %!S!%";
}
