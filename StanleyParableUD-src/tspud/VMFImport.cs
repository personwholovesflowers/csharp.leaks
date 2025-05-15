using System;
using UnityEngine;

// Token: 0x020001CA RID: 458
[ExecuteInEditMode]
public class VMFImport : MonoBehaviour
{
	// Token: 0x04000A67 RID: 2663
	[SerializeField]
	private string VMFPath = "Assets/Resources/Mapsrc/rate_your_experience.vmf";

	// Token: 0x04000A68 RID: 2664
	[SerializeField]
	private GameObject importParent;

	// Token: 0x04000A69 RID: 2665
	[SerializeField]
	private Object VMFasset;

	// Token: 0x04000A6A RID: 2666
	[SerializeField]
	private TextAsset logfile;

	// Token: 0x04000A6B RID: 2667
	[SerializeField]
	private GameObject[] modifiedObjects;
}
