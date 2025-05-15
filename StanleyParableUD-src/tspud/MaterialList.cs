using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C6 RID: 454
[CreateAssetMenu(fileName = "Data", menuName = "Material List")]
public class MaterialList : ScriptableObject
{
	// Token: 0x04000A62 RID: 2658
	public List<Material> materials = new List<Material>();
}
