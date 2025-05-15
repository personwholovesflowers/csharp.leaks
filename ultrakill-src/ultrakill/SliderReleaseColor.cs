using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020003EB RID: 1003
public class SliderReleaseColor : MonoBehaviour, IPointerUpHandler, IEventSystemHandler
{
	// Token: 0x0600168F RID: 5775 RVA: 0x000B53B3 File Offset: 0x000B35B3
	private void Awake()
	{
		this.slider = base.GetComponent<Selectable>();
		this.defaultColor = this.slider.targetGraphic.color;
	}

	// Token: 0x06001690 RID: 5776 RVA: 0x000B53D8 File Offset: 0x000B35D8
	public void OnPointerUp(PointerEventData eventData)
	{
		this.fade = this.slider.colors.fadeDuration;
	}

	// Token: 0x06001691 RID: 5777 RVA: 0x000B5400 File Offset: 0x000B3600
	private void Update()
	{
		if (this.fade == 0f)
		{
			return;
		}
		this.fade = Mathf.MoveTowards(this.fade, 0f, Time.unscaledDeltaTime);
		this.slider.targetGraphic.color = Color.Lerp(this.defaultColor, this.releaseColor, this.fade / this.slider.colors.fadeDuration);
	}

	// Token: 0x04001F28 RID: 7976
	[SerializeField]
	private Color releaseColor;

	// Token: 0x04001F29 RID: 7977
	private Color defaultColor;

	// Token: 0x04001F2A RID: 7978
	private Selectable slider;

	// Token: 0x04001F2B RID: 7979
	private float fade;
}
