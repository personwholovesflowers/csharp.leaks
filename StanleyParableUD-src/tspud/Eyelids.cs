using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001D3 RID: 467
public class Eyelids : MonoBehaviour
{
	// Token: 0x06000AAD RID: 2733 RVA: 0x00031A98 File Offset: 0x0002FC98
	private void Start()
	{
		this.canvasRect = this.canvas.GetComponent<RectTransform>();
		this.ScaleToScreen();
		this.topLid.anchoredPosition = new Vector2(0f, this.height);
		this.bottomLid.anchoredPosition = new Vector2(0f, -this.height);
		this.canvas.enabled = false;
		GameMaster.OnResume += this.ScaleToScreen;
	}

	// Token: 0x06000AAE RID: 2734 RVA: 0x00031B10 File Offset: 0x0002FD10
	private void OnDestroy()
	{
		GameMaster.OnResume -= this.ScaleToScreen;
	}

	// Token: 0x06000AAF RID: 2735 RVA: 0x00031B23 File Offset: 0x0002FD23
	public void StartClose()
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.Animate(EyelidDir.Close));
	}

	// Token: 0x06000AB0 RID: 2736 RVA: 0x00031B39 File Offset: 0x0002FD39
	public void StartOpen()
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.Animate(EyelidDir.Open));
	}

	// Token: 0x06000AB1 RID: 2737 RVA: 0x00031B50 File Offset: 0x0002FD50
	private void ScaleToScreen()
	{
		this.height = this.canvasRect.rect.size.y;
		this.width = this.canvasRect.rect.size.x;
		this.height += 1f;
		this.width += 150f;
		this.topLid.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.width);
		this.topLid.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.height);
		this.bottomLid.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.width);
		this.bottomLid.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.height);
		this.speed = this.height / this.duration;
	}

	// Token: 0x06000AB2 RID: 2738 RVA: 0x00031C18 File Offset: 0x0002FE18
	private IEnumerator Animate(EyelidDir dir)
	{
		float target = 0f;
		if (dir == EyelidDir.Open)
		{
			target = this.height;
		}
		else
		{
			this.canvas.enabled = base.enabled;
		}
		while (this.topLid.anchoredPosition.y != target)
		{
			float num = Mathf.MoveTowards(this.topLid.anchoredPosition.y, target, this.speed * Singleton<GameMaster>.Instance.GameDeltaTime);
			this.topLid.anchoredPosition = new Vector2(0f, num);
			this.bottomLid.anchoredPosition = new Vector2(0f, -num);
			this.ScaleToScreen();
			yield return null;
		}
		if (dir == EyelidDir.Open)
		{
			this.canvas.enabled = false;
		}
		yield break;
	}

	// Token: 0x06000AB3 RID: 2739 RVA: 0x00031C2E File Offset: 0x0002FE2E
	public void Reset()
	{
		base.StopAllCoroutines();
		this.topLid.anchoredPosition = new Vector2(0f, this.height);
		this.bottomLid.anchoredPosition = new Vector2(0f, -this.height);
	}

	// Token: 0x04000A90 RID: 2704
	public Canvas canvas;

	// Token: 0x04000A91 RID: 2705
	private RectTransform canvasRect;

	// Token: 0x04000A92 RID: 2706
	public RectTransform bottomLid;

	// Token: 0x04000A93 RID: 2707
	public RectTransform topLid;

	// Token: 0x04000A94 RID: 2708
	private float duration = 4f;

	// Token: 0x04000A95 RID: 2709
	private float speed = 200f;

	// Token: 0x04000A96 RID: 2710
	private float height = 400f;

	// Token: 0x04000A97 RID: 2711
	private float width = 800f;
}
