using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E4 RID: 228
public class HeavenlyButton : MonoBehaviour
{
	// Token: 0x040005CE RID: 1486
	public List<HeavenlyButton.OnOffMaterials> colourSelection = new List<HeavenlyButton.OnOffMaterials>();

	// Token: 0x040005CF RID: 1487
	[InspectorButton("TestOff", null)]
	public Skin offSkin;

	// Token: 0x040005D0 RID: 1488
	[InspectorButton("TestOn", null)]
	public Skin onSkin;

	// Token: 0x040005D1 RID: 1489
	public SkinnedMeshRenderer meshRenderer;

	// Token: 0x040005D2 RID: 1490
	[Header("Colour is selected in editor time! -rax")]
	[InspectorButton("SetSkinMaterials", "Set Skin Materials")]
	[SerializeField]
	private int colourIndex;

	// Token: 0x020003BD RID: 957
	[Serializable]
	public class OnOffMaterials
	{
		// Token: 0x040013D4 RID: 5076
		public Material offMaterial;

		// Token: 0x040013D5 RID: 5077
		public Material onMaterial;
	}
}
