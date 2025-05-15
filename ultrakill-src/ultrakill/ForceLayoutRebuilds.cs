using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001FF RID: 511
public class ForceLayoutRebuilds : MonoBehaviour
{
	// Token: 0x06000A69 RID: 2665 RVA: 0x00049530 File Offset: 0x00047730
	private void Awake()
	{
		if (this.rectTransform == null)
		{
			this.rectTransform = (RectTransform)base.transform;
		}
	}

	// Token: 0x06000A6A RID: 2666 RVA: 0x00049551 File Offset: 0x00047751
	private void OnEnable()
	{
		if (this.onEnable)
		{
			this.ForceRebuild();
			base.StartCoroutine(this.DelayedRebuild());
		}
	}

	// Token: 0x06000A6B RID: 2667 RVA: 0x00049570 File Offset: 0x00047770
	public void ForceRebuild()
	{
		this.Awake();
		List<RectTransform> list = new List<RectTransform> { this.rectTransform };
		if (this.allChildLayoutElements)
		{
			foreach (LayoutGroup layoutGroup in this.rectTransform.GetComponentsInChildren<LayoutGroup>(true))
			{
				list.Add((RectTransform)layoutGroup.transform);
			}
		}
		for (int j = 0; j < this.iterations; j++)
		{
			foreach (RectTransform rectTransform in list)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
			}
		}
		this.scrollRectToReset.verticalNormalizedPosition = 1f;
	}

	// Token: 0x06000A6C RID: 2668 RVA: 0x00049634 File Offset: 0x00047834
	private IEnumerator DelayedRebuild()
	{
		yield return new WaitForEndOfFrame();
		this.ForceRebuild();
		yield break;
	}

	// Token: 0x04000DD8 RID: 3544
	public int iterations = 3;

	// Token: 0x04000DD9 RID: 3545
	public bool onEnable = true;

	// Token: 0x04000DDA RID: 3546
	public bool allChildLayoutElements = true;

	// Token: 0x04000DDB RID: 3547
	public ScrollRect scrollRectToReset;

	// Token: 0x04000DDC RID: 3548
	private RectTransform rectTransform;
}
