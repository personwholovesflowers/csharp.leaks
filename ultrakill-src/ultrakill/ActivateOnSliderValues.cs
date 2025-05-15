using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200001D RID: 29
public class ActivateOnSliderValues : MonoBehaviour
{
	// Token: 0x060000E9 RID: 233 RVA: 0x00005DC5 File Offset: 0x00003FC5
	private void Start()
	{
		this.CheckSliders();
	}

	// Token: 0x060000EA RID: 234 RVA: 0x00005DC5 File Offset: 0x00003FC5
	private void Update()
	{
		this.CheckSliders();
	}

	// Token: 0x060000EB RID: 235 RVA: 0x00005DD0 File Offset: 0x00003FD0
	private void CheckSliders()
	{
		int num = 0;
		for (int i = 0; i < this.sliders.Length; i++)
		{
			if (this.sliders[i].value == this.values[i])
			{
				num++;
			}
		}
		GameObject[] array;
		if (num == this.sliders.Length)
		{
			array = this.activateOnValues;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].SetActive(true);
			}
			array = this.deactivateOnValues;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].SetActive(false);
			}
			return;
		}
		array = this.activateOnValues;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].SetActive(false);
		}
		array = this.deactivateOnValues;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].SetActive(true);
		}
	}

	// Token: 0x04000094 RID: 148
	public Slider[] sliders;

	// Token: 0x04000095 RID: 149
	public float[] values;

	// Token: 0x04000096 RID: 150
	public GameObject[] activateOnValues;

	// Token: 0x04000097 RID: 151
	public GameObject[] deactivateOnValues;
}
