using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200009D RID: 157
public class ButtonTextColorSetter : MonoBehaviour
{
	// Token: 0x060002FE RID: 766 RVA: 0x000119AC File Offset: 0x0000FBAC
	private void Awake()
	{
		this.button = base.GetComponent<Button>();
		this.texts = base.GetComponentsInChildren<TMP_Text>();
		GameObject gameObject = new GameObject("CrossFadeColorProxy");
		gameObject.SetActive(false);
		gameObject.transform.SetParent(base.gameObject.transform, false);
		gameObject.transform.hideFlags = HideFlags.HideInHierarchy;
		CrossFadeColorProxy crossFadeColorProxy = gameObject.AddComponent<CrossFadeColorProxy>();
		gameObject.GetComponent<RectTransform>().sizeDelta = base.GetComponent<RectTransform>().sizeDelta;
		crossFadeColorProxy.setter = this;
		this.originalGraphic = this.button.targetGraphic;
		this.button.targetGraphic = crossFadeColorProxy;
	}

	// Token: 0x060002FF RID: 767 RVA: 0x00011A48 File Offset: 0x0000FC48
	public void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha, bool useRGB)
	{
		this.originalGraphic.CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha, useRGB);
		TMP_Text[] array;
		if (this.onlyDisabledState)
		{
			targetColor = (this.button.interactable ? this.button.colors.normalColor : this.button.colors.disabledColor);
			array = this.texts;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].color = targetColor;
			}
			return;
		}
		array = this.texts;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha, useRGB);
		}
	}

	// Token: 0x040003A3 RID: 931
	public bool onlyDisabledState;

	// Token: 0x040003A4 RID: 932
	private Button button;

	// Token: 0x040003A5 RID: 933
	private Graphic originalGraphic;

	// Token: 0x040003A6 RID: 934
	private TMP_Text[] texts;
}
