using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DG.Util
{
	// Token: 0x020002F8 RID: 760
	public class DropdownFix : MonoBehaviour
	{
		// Token: 0x060013CF RID: 5071 RVA: 0x00068F72 File Offset: 0x00067172
		private IEnumerator Start()
		{
			Object.Destroy(base.GetComponent<GraphicRaycaster>());
			yield return null;
			Object.Destroy(base.GetComponent<Canvas>());
			yield break;
		}
	}
}
