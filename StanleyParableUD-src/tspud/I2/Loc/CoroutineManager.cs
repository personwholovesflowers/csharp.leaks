using System;
using System.Collections;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002C0 RID: 704
	public class CoroutineManager : MonoBehaviour
	{
		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06001240 RID: 4672 RVA: 0x000629A0 File Offset: 0x00060BA0
		private static CoroutineManager pInstance
		{
			get
			{
				if (CoroutineManager.mInstance == null)
				{
					GameObject gameObject = new GameObject("_Coroutiner");
					gameObject.hideFlags = HideFlags.HideAndDontSave;
					CoroutineManager.mInstance = gameObject.AddComponent<CoroutineManager>();
					if (Application.isPlaying)
					{
						Object.DontDestroyOnLoad(gameObject);
					}
				}
				return CoroutineManager.mInstance;
			}
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x000629EA File Offset: 0x00060BEA
		private void Awake()
		{
			if (Application.isPlaying)
			{
				Object.DontDestroyOnLoad(base.gameObject);
			}
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x000629FE File Offset: 0x00060BFE
		public static Coroutine Start(IEnumerator coroutine)
		{
			return CoroutineManager.pInstance.StartCoroutine(coroutine);
		}

		// Token: 0x04000E8D RID: 3725
		private static CoroutineManager mInstance;
	}
}
