using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C7 RID: 455
[CreateAssetMenu(fileName = "Data", menuName = "Model List")]
public class ModelList : ScriptableObject
{
	// Token: 0x04000A63 RID: 2659
	public List<GameObject> models = new List<GameObject>();
}
