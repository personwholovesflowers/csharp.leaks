using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace GameConsole
{
	// Token: 0x020005B2 RID: 1458
	public class ErrorBadge : MonoBehaviour
	{
		// Token: 0x060020B7 RID: 8375 RVA: 0x00107252 File Offset: 0x00105452
		private void OnEnable()
		{
			Console instance = MonoSingleton<Console>.Instance;
			instance.onError = (Action)Delegate.Combine(instance.onError, new Action(this.OnError));
		}

		// Token: 0x060020B8 RID: 8376 RVA: 0x0010727A File Offset: 0x0010547A
		private void OnDisable()
		{
			if (MonoSingleton<Console>.Instance != null)
			{
				Console instance = MonoSingleton<Console>.Instance;
				instance.onError = (Action)Delegate.Remove(instance.onError, new Action(this.OnError));
			}
		}

		// Token: 0x060020B9 RID: 8377 RVA: 0x001072B0 File Offset: 0x001054B0
		private void OnError()
		{
			this.badgeContainer.SetActive(!this.hidden);
			this.Update();
			this.flashGroup.alpha = 0f;
			base.StopAllCoroutines();
			if (!this.hidden)
			{
				base.StartCoroutine(this.FlashBadge());
			}
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x00107302 File Offset: 0x00105502
		private IEnumerator FlashBadge()
		{
			this.flashGroup.alpha = 0f;
			while (this.flashGroup.alpha < 1f)
			{
				this.flashGroup.alpha += 0.2f;
				if (this.alertGroup.alpha < this.flashGroup.alpha)
				{
					this.alertGroup.alpha = (Console.IsOpen ? 0f : this.flashGroup.alpha);
				}
				yield return this.waitTime;
			}
			this.flashGroup.alpha = 1f;
			this.alertGroup.alpha = (Console.IsOpen ? 0f : this.flashGroup.alpha);
			while (this.flashGroup.alpha > 0f)
			{
				this.flashGroup.alpha -= 0.1f;
				yield return this.waitTime;
			}
			this.flashGroup.alpha = 0f;
			yield break;
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x00107311 File Offset: 0x00105511
		public void SetEnabled(bool enabled, bool hide = true)
		{
			this.hidden = !enabled;
			if (enabled)
			{
				if (MonoSingleton<Console>.Instance.errorCount > 0)
				{
					this.badgeContainer.SetActive(true);
					this.Update();
					return;
				}
			}
			else
			{
				this.badgeContainer.SetActive(false);
			}
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x0010734C File Offset: 0x0010554C
		public void Dismiss()
		{
			base.StopAllCoroutines();
			this.alertGroup.alpha = 0f;
		}

		// Token: 0x060020BD RID: 8381 RVA: 0x00107364 File Offset: 0x00105564
		private void Update()
		{
			Console instance = MonoSingleton<Console>.Instance;
			if (instance == null)
			{
				return;
			}
			int errorCount = instance.errorCount;
			if (errorCount != 0)
			{
				this.errorCountText.text = errorCount + ((errorCount == 1) ? " error" : " errors");
			}
		}

		// Token: 0x04002D06 RID: 11526
		[SerializeField]
		private GameObject badgeContainer;

		// Token: 0x04002D07 RID: 11527
		[SerializeField]
		private TMP_Text errorCountText;

		// Token: 0x04002D08 RID: 11528
		[SerializeField]
		private CanvasGroup flashGroup;

		// Token: 0x04002D09 RID: 11529
		[SerializeField]
		private CanvasGroup alertGroup;

		// Token: 0x04002D0A RID: 11530
		public bool hidden;

		// Token: 0x04002D0B RID: 11531
		private readonly CustomYieldInstruction waitTime = new WaitForSecondsRealtime(0.03f);
	}
}
