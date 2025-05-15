using System;
using System.Collections;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000EB RID: 235
public class InWorldText : MonoBehaviour
{
	// Token: 0x060005B5 RID: 1461 RVA: 0x0001FDFC File Offset: 0x0001DFFC
	public void InitTextLabel(InWorldTextualObject referencedObject)
	{
		string text = LocalizationManager.GetTranslation(referencedObject.localizationTerm, true, 0, true, false, null, null);
		if (text == null)
		{
			text = referencedObject.localizationTerm + "\n<size=14><localization not found></size>";
		}
		this.textMeshPro.text = text;
		this.textMeshPro.color = referencedObject.labelColor;
		this.canvasGroup.alpha = 0f;
		UnityEvent onUpdateText = this.OnUpdateText;
		if (onUpdateText == null)
		{
			return;
		}
		onUpdateText.Invoke();
	}

	// Token: 0x060005B6 RID: 1462 RVA: 0x0001FE6C File Offset: 0x0001E06C
	public void ForceUpdateText()
	{
		base.StartCoroutine(this.WaitOneFrameAndUpdate());
	}

	// Token: 0x060005B7 RID: 1463 RVA: 0x0001FE7B File Offset: 0x0001E07B
	private IEnumerator WaitOneFrameAndUpdate()
	{
		yield return null;
		UnityEvent onUpdateText = this.OnUpdateText;
		if (onUpdateText != null)
		{
			onUpdateText.Invoke();
		}
		yield break;
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x0001FE8A File Offset: 0x0001E08A
	public void UpdateTextLabel(InWorldTextualObject referencedObject, float alpha, float fadeTime)
	{
		this.textMeshPro.horizontalAlignment = referencedObject.horizontalAlignment;
		this.canvasGroup.alpha = Mathf.MoveTowards(this.canvasGroup.alpha, alpha, Time.smoothDeltaTime / fadeTime);
	}

	// Token: 0x040005F4 RID: 1524
	public TextMeshProUGUI textMeshPro;

	// Token: 0x040005F5 RID: 1525
	public CanvasGroup canvasGroup;

	// Token: 0x040005F6 RID: 1526
	public UnityEvent OnUpdateText;
}
