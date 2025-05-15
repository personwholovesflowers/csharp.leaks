using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200016A RID: 362
public class PointerDownButton : Button, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IDeselectHandler
{
	// Token: 0x06000872 RID: 2162 RVA: 0x00005444 File Offset: 0x00003644
	public override void OnPointerClick(PointerEventData eventData)
	{
	}

	// Token: 0x06000873 RID: 2163 RVA: 0x000281A7 File Offset: 0x000263A7
	public override void OnPointerDown(PointerEventData eventData)
	{
		base.OnPointerDown(eventData);
		base.OnPointerClick(eventData);
		this.isHolding = true;
		this.pointerDownEventData = eventData;
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x000281C5 File Offset: 0x000263C5
	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		this.isHolding = false;
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x000281D5 File Offset: 0x000263D5
	public override void OnDeselect(BaseEventData eventData)
	{
		base.OnDeselect(eventData);
		this.isHolding = false;
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x000281E5 File Offset: 0x000263E5
	protected override void OnDisable()
	{
		this.isHolding = false;
		base.OnDisable();
	}

	// Token: 0x06000877 RID: 2167 RVA: 0x000281F4 File Offset: 0x000263F4
	public void Update()
	{
		if (this.isHolding)
		{
			this.timeSinceHold += Time.deltaTime;
		}
		else
		{
			this.timeSinceHold = 0f;
			this.onRepeat = false;
		}
		if (!this.onRepeat)
		{
			if (this.timeSinceHold > this.repeatTimeInit)
			{
				base.OnPointerClick(this.pointerDownEventData);
				this.onRepeat = true;
				this.timeSinceHold -= this.repeatTimeInit;
				return;
			}
		}
		else if (this.timeSinceHold > this.repeatTime)
		{
			base.OnPointerClick(this.pointerDownEventData);
			this.timeSinceHold -= this.repeatTime;
		}
	}

	// Token: 0x04000845 RID: 2117
	public bool repeatClickOnHold;

	// Token: 0x04000846 RID: 2118
	public float repeatTimeInit = 0.8f;

	// Token: 0x04000847 RID: 2119
	public float repeatTime = 0.2f;

	// Token: 0x04000848 RID: 2120
	private float timeSinceHold;

	// Token: 0x04000849 RID: 2121
	private bool onRepeat;

	// Token: 0x0400084A RID: 2122
	private bool isHolding;

	// Token: 0x0400084B RID: 2123
	private PointerEventData pointerDownEventData;
}
