using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200028C RID: 652
	public class RegisterBundlesManager : MonoBehaviour, IResourceManager_Bundles
	{
		// Token: 0x06001060 RID: 4192 RVA: 0x0005605A File Offset: 0x0005425A
		public void OnEnable()
		{
			if (!ResourceManager.pInstance.mBundleManagers.Contains(this))
			{
				ResourceManager.pInstance.mBundleManagers.Add(this);
			}
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x0005607E File Offset: 0x0005427E
		public void OnDisable()
		{
			ResourceManager.pInstance.mBundleManagers.Remove(this);
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x00008FD9 File Offset: 0x000071D9
		public virtual Object LoadFromBundle(string path, Type assetType)
		{
			return null;
		}
	}
}
