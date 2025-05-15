using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000141 RID: 321
public class MovingScreenshot : MonoBehaviour
{
	// Token: 0x06000783 RID: 1923 RVA: 0x00026743 File Offset: 0x00024943
	private void Awake()
	{
		this.cachedRectTransform = base.GetComponent<RectTransform>();
		this.finalPosition = this.cachedRectTransform.anchoredPosition;
		this.cachedImageMat = base.GetComponent<RawImage>().material;
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x00026773 File Offset: 0x00024973
	public void StartMoveScale()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.moveRoutine != null)
		{
			base.StopCoroutine(this.moveRoutine);
		}
		this.moveRoutine = base.StartCoroutine(this.MoveAndScaleIntoPosition());
	}

	// Token: 0x06000785 RID: 1925 RVA: 0x000267A9 File Offset: 0x000249A9
	private IEnumerator MoveAndScaleIntoPosition()
	{
		float timer = 0f;
		float width = (float)Screen.currentResolution.width;
		float height = (float)Screen.currentResolution.height;
		while (timer < 1f)
		{
			this.SetSize(Mathf.Lerp(width, width / this.reductionFactor, timer / 1f), Mathf.Lerp(height, height / this.reductionFactor, timer / 1f));
			this.cachedRectTransform.anchoredPosition = Vector2.Lerp(Vector2.zero, this.finalPosition, timer / 1f);
			this.cachedImageMat.SetFloat("_Intensity", Mathf.Lerp(0f, 1f, timer / 1f));
			timer += Time.unscaledDeltaTime * 0.666f;
			yield return null;
		}
		this.moveRoutine = null;
		yield break;
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x000267B8 File Offset: 0x000249B8
	private void SetSize(float width, float height)
	{
		base.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
		base.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
	}

	// Token: 0x040007A7 RID: 1959
	[SerializeField]
	private float reductionFactor = 2.5f;

	// Token: 0x040007A8 RID: 1960
	private RectTransform cachedRectTransform;

	// Token: 0x040007A9 RID: 1961
	private Vector2 finalPosition;

	// Token: 0x040007AA RID: 1962
	private Coroutine moveRoutine;

	// Token: 0x040007AB RID: 1963
	private Material cachedImageMat;
}
