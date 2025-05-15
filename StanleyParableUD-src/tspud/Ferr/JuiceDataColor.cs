using System;
using UnityEngine;

namespace Ferr
{
	// Token: 0x020002EB RID: 747
	[Serializable]
	public class JuiceDataColor
	{
		// Token: 0x06001369 RID: 4969 RVA: 0x000676E8 File Offset: 0x000658E8
		public bool Update()
		{
			if (this.renderer == null)
			{
				return true;
			}
			float num = Mathf.Min((Time.time - this.startTime) / this.duration, 1f);
			Color color = Color.Lerp(this.start, this.end, this.curve.Evaluate(num));
			this.renderer.color = color;
			return num >= 1f;
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x00067758 File Offset: 0x00065958
		public void Cancel()
		{
			this.startTime = -10000f;
			this.Update();
		}

		// Token: 0x04000F33 RID: 3891
		public Material renderer;

		// Token: 0x04000F34 RID: 3892
		public Color start;

		// Token: 0x04000F35 RID: 3893
		public Color end;

		// Token: 0x04000F36 RID: 3894
		public float duration;

		// Token: 0x04000F37 RID: 3895
		public float startTime;

		// Token: 0x04000F38 RID: 3896
		public AnimationCurve curve;

		// Token: 0x04000F39 RID: 3897
		public Action callback;
	}
}
