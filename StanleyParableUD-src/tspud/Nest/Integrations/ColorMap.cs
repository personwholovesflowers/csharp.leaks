using System;
using UnityEngine;
using UnityEngine.Events;

namespace Nest.Integrations
{
	// Token: 0x02000242 RID: 578
	[AddComponentMenu("Cast/Integrations/Color Map")]
	public class ColorMap : BaseIntegration
	{
		// Token: 0x1700013D RID: 317
		// (set) Token: 0x06000D9F RID: 3487 RVA: 0x0003D480 File Offset: 0x0003B680
		public override float InputValue
		{
			set
			{
				if (this._colorMode == ColorMap.ColorMode.Gradient)
				{
					this._colorEvent.Invoke(this._gradient.Evaluate(value));
					return;
				}
				int num = this._colorArray.Length;
				int num2 = Mathf.FloorToInt(value * (float)(num - 1));
				num2 = Mathf.Clamp(num2, 0, num - 2);
				float num3 = value * (float)(num - 1) - (float)num2;
				Color color = Color.Lerp(this._colorArray[num2], this._colorArray[num2 + 1], num3);
				this._colorEvent.Invoke(color);
			}
		}

		// Token: 0x04000C25 RID: 3109
		[SerializeField]
		private ColorMap.ColorMode _colorMode;

		// Token: 0x04000C26 RID: 3110
		[SerializeField]
		private Gradient _gradient = new Gradient();

		// Token: 0x04000C27 RID: 3111
		[SerializeField]
		[ColorUsage(true, true, 0f, 16f, 0.125f, 3f)]
		private Color[] _colorArray = new Color[]
		{
			Color.black,
			Color.white
		};

		// Token: 0x04000C28 RID: 3112
		[SerializeField]
		private ColorMap.ColorEvent _colorEvent;

		// Token: 0x02000435 RID: 1077
		public enum ColorMode
		{
			// Token: 0x040015AF RID: 5551
			Gradient,
			// Token: 0x040015B0 RID: 5552
			ColorArray
		}

		// Token: 0x02000436 RID: 1078
		[Serializable]
		public class ColorEvent : UnityEvent<Color>
		{
		}
	}
}
