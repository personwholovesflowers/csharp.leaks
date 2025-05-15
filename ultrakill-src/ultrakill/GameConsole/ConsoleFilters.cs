using System;
using UnityEngine;

namespace GameConsole
{
	// Token: 0x020005B0 RID: 1456
	public class ConsoleFilters : MonoBehaviour
	{
		// Token: 0x060020AC RID: 8364 RVA: 0x00106FB5 File Offset: 0x001051B5
		private void Awake()
		{
			this.errorsFilter.SetOpacity(this.defaultOpacity);
			this.warningsFilter.SetOpacity(this.defaultOpacity);
			this.logsFilter.SetOpacity(this.defaultOpacity);
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x00106FEC File Offset: 0x001051EC
		private void Update()
		{
			this.errorsFilter.text.text = string.Format("errors ({0})", MonoSingleton<Console>.Instance.errorCount);
			this.warningsFilter.text.text = string.Format("warnings ({0})", MonoSingleton<Console>.Instance.warningCount);
			this.logsFilter.text.text = string.Format("logs ({0})", MonoSingleton<Console>.Instance.infoCount);
		}

		// Token: 0x060020AE RID: 8366 RVA: 0x00104E06 File Offset: 0x00103006
		public void TogglePopup()
		{
			base.gameObject.SetActive(!base.gameObject.activeSelf);
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x00107074 File Offset: 0x00105274
		private void UpdateFilters()
		{
			MonoSingleton<Console>.Instance.UpdateFilters(this.errorsFilter.active, this.warningsFilter.active, this.logsFilter.active);
		}

		// Token: 0x060020B0 RID: 8368 RVA: 0x001070A4 File Offset: 0x001052A4
		public void ToggleErrorFiltering()
		{
			this.errorsFilter.active = !this.errorsFilter.active;
			this.errorsFilter.SetOpacity(this.errorsFilter.active ? this.defaultOpacity : this.hiddenOpacity);
			this.errorsFilter.SetCheckmark(this.errorsFilter.active);
			this.UpdateFilters();
		}

		// Token: 0x060020B1 RID: 8369 RVA: 0x0010710C File Offset: 0x0010530C
		public void ToggleWarningFiltering()
		{
			this.warningsFilter.active = !this.warningsFilter.active;
			this.warningsFilter.SetOpacity(this.warningsFilter.active ? this.defaultOpacity : this.hiddenOpacity);
			this.warningsFilter.SetCheckmark(this.warningsFilter.active);
			this.UpdateFilters();
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x00107174 File Offset: 0x00105374
		public void ToggleLogFiltering()
		{
			this.logsFilter.active = !this.logsFilter.active;
			this.logsFilter.SetOpacity(this.logsFilter.active ? this.defaultOpacity : this.hiddenOpacity);
			this.logsFilter.SetCheckmark(this.logsFilter.active);
			this.UpdateFilters();
		}

		// Token: 0x04002CFC RID: 11516
		[SerializeField]
		private float defaultOpacity = 1f;

		// Token: 0x04002CFD RID: 11517
		[SerializeField]
		private float hiddenOpacity = 0.1f;

		// Token: 0x04002CFE RID: 11518
		[Space]
		[SerializeField]
		private FilterButton errorsFilter;

		// Token: 0x04002CFF RID: 11519
		[SerializeField]
		private FilterButton warningsFilter;

		// Token: 0x04002D00 RID: 11520
		[SerializeField]
		private FilterButton logsFilter;
	}
}
