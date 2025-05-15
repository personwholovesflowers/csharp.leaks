using System;
using UnityEngine;

// Token: 0x0200005F RID: 95
public class GraphicsTierEvent : MonoBehaviour
{
	// Token: 0x06000253 RID: 595 RVA: 0x000104E2 File Offset: 0x0000E6E2
	private void Start()
	{
		string[] names = QualitySettings.names;
		QualitySettings.GetQualityLevel();
	}

	// Token: 0x06000254 RID: 596 RVA: 0x00005444 File Offset: 0x00003644
	private void Update()
	{
	}
}
