using System;
using System.Collections.Generic;
using UnityEngine;

namespace AmplifyImpostors
{
	// Token: 0x02000308 RID: 776
	[CreateAssetMenu(fileName = "New Impostor", order = 85)]
	public class AmplifyImpostorAsset : ScriptableObject
	{
		// Token: 0x04000FDA RID: 4058
		[SerializeField]
		public Material Material;

		// Token: 0x04000FDB RID: 4059
		[SerializeField]
		public Mesh Mesh;

		// Token: 0x04000FDC RID: 4060
		[HideInInspector]
		[SerializeField]
		public int Version;

		// Token: 0x04000FDD RID: 4061
		[SerializeField]
		public ImpostorType ImpostorType = ImpostorType.Octahedron;

		// Token: 0x04000FDE RID: 4062
		[HideInInspector]
		[SerializeField]
		public bool LockedSizes = true;

		// Token: 0x04000FDF RID: 4063
		[HideInInspector]
		[SerializeField]
		public int SelectedSize = 2048;

		// Token: 0x04000FE0 RID: 4064
		[SerializeField]
		public Vector2 TexSize = new Vector2(2048f, 2048f);

		// Token: 0x04000FE1 RID: 4065
		[HideInInspector]
		[SerializeField]
		public bool DecoupleAxisFrames;

		// Token: 0x04000FE2 RID: 4066
		[SerializeField]
		[Range(1f, 32f)]
		public int HorizontalFrames = 16;

		// Token: 0x04000FE3 RID: 4067
		[SerializeField]
		[Range(1f, 33f)]
		public int VerticalFrames = 16;

		// Token: 0x04000FE4 RID: 4068
		[SerializeField]
		[Range(0f, 64f)]
		public int PixelPadding = 32;

		// Token: 0x04000FE5 RID: 4069
		[SerializeField]
		[Range(4f, 16f)]
		public int MaxVertices = 8;

		// Token: 0x04000FE6 RID: 4070
		[SerializeField]
		[Range(0f, 0.2f)]
		public float Tolerance = 0.15f;

		// Token: 0x04000FE7 RID: 4071
		[SerializeField]
		[Range(0f, 1f)]
		public float NormalScale = 0.01f;

		// Token: 0x04000FE8 RID: 4072
		[SerializeField]
		public Vector2[] ShapePoints = new Vector2[]
		{
			new Vector2(0.15f, 0f),
			new Vector2(0.85f, 0f),
			new Vector2(1f, 0.15f),
			new Vector2(1f, 0.85f),
			new Vector2(0.85f, 1f),
			new Vector2(0.15f, 1f),
			new Vector2(0f, 0.85f),
			new Vector2(0f, 0.15f)
		};

		// Token: 0x04000FE9 RID: 4073
		[SerializeField]
		public AmplifyImpostorBakePreset Preset;

		// Token: 0x04000FEA RID: 4074
		[SerializeField]
		public List<TextureOutput> OverrideOutput = new List<TextureOutput>();
	}
}
