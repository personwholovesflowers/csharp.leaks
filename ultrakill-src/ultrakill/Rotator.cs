using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000397 RID: 919
public class Rotator : MonoBehaviour
{
	// Token: 0x0600151C RID: 5404 RVA: 0x000ACF4C File Offset: 0x000AB14C
	public void StartRotate()
	{
		base.StartCoroutine(this.Rotate());
	}

	// Token: 0x0600151D RID: 5405 RVA: 0x000ACF5B File Offset: 0x000AB15B
	private IEnumerator Rotate()
	{
		this.rotationProgress = 0f;
		this.initialRotation = base.transform.rotation;
		this.selectedEasingFunction = EasingFunction.GetEasingFunction(this.easingFunction);
		while (this.rotationProgress < 1f)
		{
			this.rotationProgress += Time.deltaTime / this.rotationTime;
			float num;
			if (this.customCurve == null)
			{
				num = this.selectedEasingFunction(0f, 1f, this.rotationProgress);
			}
			else
			{
				num = this.customCurve.Evaluate(this.rotationProgress);
			}
			num = Mathf.Min(num, 1f);
			base.transform.rotation = Quaternion.LerpUnclamped(this.initialRotation, this.initialRotation * Quaternion.Euler(this.rotation), num);
			yield return null;
		}
		this.doAThing.Invoke("");
		yield break;
	}

	// Token: 0x04001D61 RID: 7521
	public Vector3 rotation;

	// Token: 0x04001D62 RID: 7522
	public float rotationTime = 1f;

	// Token: 0x04001D63 RID: 7523
	private Quaternion initialRotation;

	// Token: 0x04001D64 RID: 7524
	private float rotationProgress;

	// Token: 0x04001D65 RID: 7525
	public AnimationCurve customCurve;

	// Token: 0x04001D66 RID: 7526
	public EasingFunction.Ease easingFunction;

	// Token: 0x04001D67 RID: 7527
	private EasingFunction.Function selectedEasingFunction;

	// Token: 0x04001D68 RID: 7528
	public UltrakillEvent doAThing;
}
