using System;
using System.Collections;
using UnityEngine;

namespace ULTRAKILL.Cheats
{
	// Token: 0x02000615 RID: 1557
	public class FullBright : ICheat
	{
		// Token: 0x17000336 RID: 822
		// (get) Token: 0x060022FF RID: 8959 RVA: 0x0010D09F File Offset: 0x0010B29F
		public static bool Enabled
		{
			get
			{
				FullBright instance = FullBright._instance;
				return instance != null && instance.IsActive;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06002300 RID: 8960 RVA: 0x0010D0B1 File Offset: 0x0010B2B1
		public string LongName
		{
			get
			{
				return "Fullbright";
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06002301 RID: 8961 RVA: 0x0010D0B8 File Offset: 0x0010B2B8
		public string Identifier
		{
			get
			{
				return "ultrakill.full-bright";
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06002302 RID: 8962 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string ButtonEnabledOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06002303 RID: 8963 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string ButtonDisabledOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06002304 RID: 8964 RVA: 0x0010D0BF File Offset: 0x0010B2BF
		public string Icon
		{
			get
			{
				return "light";
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06002305 RID: 8965 RVA: 0x0010D0C6 File Offset: 0x0010B2C6
		// (set) Token: 0x06002306 RID: 8966 RVA: 0x0010D0CE File Offset: 0x0010B2CE
		public bool IsActive { get; private set; }

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06002307 RID: 8967 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public bool DefaultState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06002308 RID: 8968 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x06002309 RID: 8969 RVA: 0x0010D0D8 File Offset: 0x0010B2D8
		public void Enable(CheatsManager manager)
		{
			FullBright._instance = this;
			this.IsActive = true;
			this.lightObject = Object.Instantiate<GameObject>(MonoSingleton<CheatsController>.Instance.fullBrightLight);
			this.lastFogEnabled = RenderSettings.fog;
			RenderSettings.fog = false;
			this.lastAmbientColor = RenderSettings.ambientLight;
			RenderSettings.ambientLight = FullBright.brightAmbientColor;
		}

		// Token: 0x0600230A RID: 8970 RVA: 0x0010D12D File Offset: 0x0010B32D
		public void Disable()
		{
			this.IsActive = false;
			Object.Destroy(this.lightObject);
			RenderSettings.fog = this.lastFogEnabled;
			RenderSettings.ambientLight = this.lastAmbientColor;
		}

		// Token: 0x0600230B RID: 8971 RVA: 0x0010D157 File Offset: 0x0010B357
		public IEnumerator Coroutine(CheatsManager manager)
		{
			while (this.IsActive)
			{
				this.Update();
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600230C RID: 8972 RVA: 0x0010D168 File Offset: 0x0010B368
		private void Update()
		{
			if (!this.IsActive)
			{
				return;
			}
			if (RenderSettings.fog)
			{
				this.lastFogEnabled = true;
				RenderSettings.fog = false;
			}
			if (RenderSettings.ambientLight != FullBright.brightAmbientColor)
			{
				this.lastAmbientColor = RenderSettings.ambientLight;
				RenderSettings.ambientLight = FullBright.brightAmbientColor;
			}
		}

		// Token: 0x04002E51 RID: 11857
		private static FullBright _instance;

		// Token: 0x04002E53 RID: 11859
		private bool lastFogEnabled;

		// Token: 0x04002E54 RID: 11860
		private Color lastAmbientColor;

		// Token: 0x04002E55 RID: 11861
		private GameObject lightObject;

		// Token: 0x04002E56 RID: 11862
		private static Color brightAmbientColor = new Color(0.2f, 0.2f, 0.2f);
	}
}
