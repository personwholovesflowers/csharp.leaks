using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StanleyUI
{
	// Token: 0x020001F9 RID: 505
	public class SettingsInitiator : MonoBehaviour
	{
		// Token: 0x06000BAD RID: 2989 RVA: 0x000355B2 File Offset: 0x000337B2
		private IEnumerator Start()
		{
			Dictionary<GameObject, bool> origStates = new Dictionary<GameObject, bool>();
			foreach (GameObject gameObject in this.settingsPages)
			{
				origStates[gameObject] = gameObject.activeSelf;
				gameObject.SetActive(true);
			}
			yield return null;
			foreach (GameObject gameObject2 in this.settingsPages)
			{
				gameObject2.SetActive(origStates[gameObject2]);
			}
			yield break;
		}

		// Token: 0x04000B42 RID: 2882
		public GameObject[] settingsPages;
	}
}
