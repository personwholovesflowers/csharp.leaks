using System;
using UnityEngine;

// Token: 0x020000D3 RID: 211
public class vgStylisticFogData : MonoBehaviour
{
	// Token: 0x040004B9 RID: 1209
	public float startDistance = 50f;

	// Token: 0x040004BA RID: 1210
	public float endDistance = 250f;

	// Token: 0x040004BB RID: 1211
	public float intensityScale = 1f;

	// Token: 0x040004BC RID: 1212
	public float offsetFromAToB;

	// Token: 0x040004BD RID: 1213
	public Texture fogColorTextureFromAToB;

	// Token: 0x040004BE RID: 1214
	public Texture fogColorTextureFromBToA;

	// Token: 0x040004BF RID: 1215
	public Transform transformObjectA;

	// Token: 0x040004C0 RID: 1216
	public Transform transformObjectB;
}
