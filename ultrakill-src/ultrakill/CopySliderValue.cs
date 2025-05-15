using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000EC RID: 236
public class CopySliderValue : MonoBehaviour
{
	// Token: 0x0600049E RID: 1182 RVA: 0x0001FCD6 File Offset: 0x0001DED6
	private void Start()
	{
		this.currentSlider = base.GetComponent<Slider>();
		this.currentSlider.value = this.target.value;
	}

	// Token: 0x04000645 RID: 1605
	public Slider target;

	// Token: 0x04000646 RID: 1606
	private Slider currentSlider;
}
