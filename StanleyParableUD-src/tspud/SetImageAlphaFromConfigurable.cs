using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000187 RID: 391
public class SetImageAlphaFromConfigurable : MonoBehaviour
{
	// Token: 0x0600091B RID: 2331 RVA: 0x0002B24E File Offset: 0x0002944E
	private void Start()
	{
		FloatConfigurable floatConfigurable = this.floatConfigurable;
		floatConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(floatConfigurable.OnValueChanged, new Action<LiveData>(this.OnValueChanged));
		this.OnValueChanged(null);
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x0002B27E File Offset: 0x0002947E
	private void OnDestroy()
	{
		FloatConfigurable floatConfigurable = this.floatConfigurable;
		floatConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(floatConfigurable.OnValueChanged, new Action<LiveData>(this.OnValueChanged));
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x0002B2A8 File Offset: 0x000294A8
	private void OnValueChanged(LiveData data)
	{
		Color color = this.targetImage.color;
		color.a = this.floatConfigurable.GetNormalizedFloatValue();
		this.targetImage.color = color;
	}

	// Token: 0x040008F1 RID: 2289
	public FloatConfigurable floatConfigurable;

	// Token: 0x040008F2 RID: 2290
	public Image targetImage;
}
