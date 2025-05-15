using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing
{
	// Token: 0x020005DF RID: 1503
	public class FishIconGlow : MonoBehaviour
	{
		// Token: 0x060021AF RID: 8623 RVA: 0x0010A5F0 File Offset: 0x001087F0
		private void Awake()
		{
			this.image = base.GetComponent<Image>();
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x0010A5FE File Offset: 0x001087FE
		public void Blink()
		{
			if (this.image == null || !this.image.isActiveAndEnabled)
			{
				return;
			}
			base.StartCoroutine(this.BlinkCoroutine());
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x0010A629 File Offset: 0x00108829
		private IEnumerator BlinkCoroutine()
		{
			Color color = Color.white;
			color.a = 0.7f;
			this.image.color = color;
			while (color.a < 1f)
			{
				color.a += Time.deltaTime;
				this.image.color = color;
				yield return null;
			}
			while (color.a > 0f)
			{
				color.a -= Time.deltaTime;
				this.image.color = color;
				yield return null;
			}
			yield break;
		}

		// Token: 0x04002D81 RID: 11649
		private Image image;
	}
}
